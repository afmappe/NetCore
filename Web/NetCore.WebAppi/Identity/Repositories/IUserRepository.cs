using NetCore.WebAppi.Data;
using System.Threading.Tasks;

namespace NetCore.WebAppi.Identity.Repositories
{
    public interface IUserRepository : IAsyncRepository<UserInfo>
    {
        Task<UserInfo> Get(string userName);
    }
}