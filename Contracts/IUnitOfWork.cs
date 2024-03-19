using MPA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPA.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepo<Alloy> Alloys { get; }
        IGenericRepo<AlloyOrder> AlloyOrders { get; }
        IGenericRepo<Order> Orders { get; }
        // IGenericRepo<RCPMonthly> RCPMonthly { get; }
        IGenericRepo<ReceivedAlloyOrder> ReceivedAlloyOrder { get; }
        IGenericRepo<ReceivedOrder> ReceivedOrders { get; }
        IGenericRepo<Supplier> Suppliers { get; }
        IGenericRepo<SupplierEmail> SupplierEmails { get; }

        Task Save();
    }
}
