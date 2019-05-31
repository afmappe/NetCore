using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.WebAppi.Data
{
    public interface IAsyncRepository<TEntityType>
   where TEntityType : class
    {
        Task Create(TEntityType record);

        Task Create(IEnumerable<TEntityType> list);

        Task Delete(TEntityType record);

        Task Delete(IEnumerable<TEntityType> list);

        Task<TEntityType> Find(params object[] keys);

        Task Update(TEntityType record);

        Task Update(List<TEntityType> list);
    }
}