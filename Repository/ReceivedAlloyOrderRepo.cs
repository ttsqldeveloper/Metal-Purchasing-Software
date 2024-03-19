using System.Collections.Generic;
using System.Threading.Tasks;
using MPA.Contracts;
using MPA.Data;

namespace MPA.Repository
{
    public class ReceivedAlloyOrderRepo : IReceivedAlloyOrderRepo
    {
        private readonly ApplicationDbContext _db;

        public ReceivedAlloyOrderRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<bool> Create(ReceivedAlloyOrder entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(ReceivedAlloyOrder entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<ReceivedAlloyOrder>> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<ReceivedAlloyOrder> FindById(int id)
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

        public Task<bool> Update(ReceivedAlloyOrder entity)
        {
            throw new System.NotImplementedException();
        }
    }
    
}
