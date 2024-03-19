using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MPA.Contracts;
using MPA.Data;

namespace MPA.Repository
{
    public class SupplierEmailRepo : ISupplierEmailRepo
    {
        private readonly ApplicationDbContext _db;

        public SupplierEmailRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<bool> Create(SupplierEmail entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(SupplierEmail entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<SupplierEmail>> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<SupplierEmail> FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<SupplierEmail>> GetEmailBySupplierId(int supplierid)
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

        public Task<bool> Update(SupplierEmail entity)
        {
            throw new System.NotImplementedException();
        }
    }

}
