using System.Collections.Generic;
using System.Threading.Tasks;
using MPA.Contracts;
using MPA.Data;

namespace MPA.Repository
{
    public class AlloyOrderRepo : IAlloyOrderRepo
    {
        private readonly ApplicationDbContext _db;
        public AlloyOrderRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<bool> Create(AlloyOrder entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(AlloyOrder entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<AlloyOrder>> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<AlloyOrder> FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> isExists(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Save()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(AlloyOrder entity)
        {
            throw new System.NotImplementedException();
        }
    }
    
}
