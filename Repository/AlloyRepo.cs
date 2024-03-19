using System.Collections.Generic;
using System.Threading.Tasks;
using MPA.Contracts;
using MPA.Data;

namespace MPA.Repository
{
    public class AlloyRepo : IAlloyRepo
    {
        private readonly ApplicationDbContext _db;

        public AlloyRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<bool> Create(Alloy entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(Alloy entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<Alloy>> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Alloy> FindById(int id)
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

        public Task<bool> Update(Alloy entity)
        {
            throw new System.NotImplementedException();
        }
    }
    
}
