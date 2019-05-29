using MediatR;
using NetCore.Library.Identity.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Library.Identity.Commands
{
    public static class CreateUserCommand
    {
        public class Handler : IRequestHandler<Request, int>
        {
            private readonly IUserRepository UserRepository;

            public Handler(IUserRepository userRepository)
            {
                UserRepository = userRepository;
            }

            public async Task<int> Handle(Request request, CancellationToken cancellationToken)
            {
                int result = 0;

                try
                {
                    UserInfo user = await UserRepository.Get(request.UserName);

                    if (user == null)
                    {
                        user = new UserInfo
                        {
                            CreationDate = DateTime.Now,
                            FullName = request.FullName,
                            UserName = request.UserName
                        };
                        await UserRepository.Create(user);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                return result;
            }
        }

        public class Request : IRequest<int>
        {
            public string FullName { get; set; }

            public string UserName { get; set; }
        }
    }
}