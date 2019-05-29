using NetCore.Library.Identity;
using NetCore.Library.Identity.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Library.Infrastructure.Data.Repositories
{
    public class UserRepository : AsyncRepositoryBase<UserInfo, NetCoreContext>, IUserRepository
    {
        public async Task<UserInfo> Get(string userName)
        {
            UserInfo result = null;
            try
            {
                using (NetCoreContext context = CreateContext())
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