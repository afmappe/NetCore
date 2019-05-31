using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCore.WebAppi.Helpers;
using NetCore.WebAppi.Identity.Repositories;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.WebAppi.Identity.Commands
{
    public static class UserAuthenticateCommand
    {
        /// <summary>
        /// Implementation of <see cref="IRequestHandler{TRequest, TResponse}"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly AppSettings AppSettings;
            private readonly IUserRepository UserRepository;

            /// <summary>
            /// Default constructor
            /// </summary>
            public Handler(
                IOptions<AppSettings> appSettings,
                IUserRepository userRepository)
            {
                AppSettings = appSettings.Value;
                UserRepository = userRepository;
            }

            /// <summary>
            /// Implementation of <see cref="IRequestHandler{TRequest, TResponse}.Handle(TRequest, CancellationToken)"/>
            /// </summary>
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Response result = null;

                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    UserInfo user = await UserRepository.Get(request.UserName);

                    result = GenerateUserToken(user);
                }
                catch (Exception ex)
                {
                    throw;
                }

                return result;
            }

            /// <summary>
            /// Authentication successful so generate jwt token
            /// </summary>
            /// <param name="user">User information</param>
            /// <returns>Jwt Token</returns>
            private Response GenerateUserToken(UserInfo user)
            {
                Response result = new Response();

                if (user != null)
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    byte[] key = Encoding.ASCII.GetBytes(AppSettings.Secret);
                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, user.Id.ToString()),
                                new Claim(ClaimTypes.Role, "Admin")
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                    result.Token = tokenHandler.WriteToken(token);
                }
                return result;
            }
        }

        [JsonObject]
        public class Request : IRequest<Response>
        {
            /// <summary>
            /// User Password
            /// </summary>
            [JsonProperty("password")]
            public string Password { get; set; }

            /// <summary>
            /// UserName
            /// </summary>
            [JsonProperty("userName")]
            public string UserName { get; set; }
        }

        [JsonObject]
        public class Response
        {
            /// <summary>
            /// User JWT Token
            /// </summary>
            [JsonProperty("token")]
            public string Token { get; set; }
        }
    }
}