using NetCore.WebAppi.Identity;
using NetCore.WebAppi.Identity.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.WebAppi.Data.Repositories
{
    public class UserRepository : AsyncRepositoryBase<UserInfo, ApplicationDbContext>, IUserRepository
    {
        public async Task<UserInfo> Get(string userName)
        {
            UserInfo result = null;
            try
            {
                using (ApplicationDbContext context = CreateContext())
                {
                    result = context.Users.SingleOrDefault(x => x.UserName == userName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
    }
}