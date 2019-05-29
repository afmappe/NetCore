using NetCore.Library.Common;
using System.Threading.Tasks;

namespace NetCore.Library.Identity.Repositories
{
    public interface IUserRepository : IAsyncRepository<UserInfo>
    {
        Task<UserInfo> Get(string userName);
    }
}