using System.Collections.Generic;
using System.Threading.Tasks;
using MPA.Contracts;
using MPA.Data;

namespace MPA.Repository
{
    public class SupplierRepo : ISupplierRepo
    {
        private readonly ApplicationDbContext _db;

        public SupplierRepo(ApplicationDbContext db)
       {
           _db = db;
       }

        public Task<bool> Create(Supplier entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(Supplier entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<Supplier>> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Supplier> FindById(int id)
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

        public Task<bool> Update(Supplier entity)
        {
            throw new System.NotImplementedException();
        }
    }
    
}
