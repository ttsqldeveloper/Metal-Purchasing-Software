using System.Collections.Generic;
using System.Threading.Tasks;
using MPA.Contracts;
using MPA.Data;

namespace MPA.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _db;

        public OrderRepo(ApplicationDbContext db)
        {
            _db = db;
            
        }

        public Task<bool> Create(Order entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(Order entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<Order>> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Order> FindById(int id)
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

        public Task<bool> Update(Order entity)
        {
            throw new System.NotImplementedException();
        }
    }
    
}
