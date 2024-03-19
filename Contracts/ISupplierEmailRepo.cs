using System.Collections.Generic;
using System.Threading.Tasks;
using MPA.Data;

namespace MPA.Contracts
{
    interface ISupplierEmailRepo : IBaseRepo<SupplierEmail>
    {
        Task<ICollection<SupplierEmail>> GetEmailBySupplierId(int supplierid);
    }
    
}
