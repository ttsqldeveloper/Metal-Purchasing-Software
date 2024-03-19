using MPA.Contracts;
using MPA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPA.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepo<Order> _Orders;
        private IGenericRepo<Supplier> _Suppliers;
        private IGenericRepo<Alloy> _Alloys;
        private IGenericRepo<ReceivedOrder> _ReceivedOrders;
        private IGenericRepo<RCPMonthly> _RCPMonthly;
        private IGenericRepo<AlloyOrder> _AlloyOrders;
        private IGenericRepo<SupplierEmail> _SupplierEmails;
        private IGenericRepo<ReceivedAlloyOrder> _ReceivedAlloyOrders;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepo<Order> Orders
              => _Orders ??= new GenericRepo<Order>(_context);
        public IGenericRepo<Supplier> Suppliers
              => _Suppliers ??= new GenericRepo<Supplier>(_context);
        public IGenericRepo<Alloy> Alloys
              => _Alloys ??= new GenericRepo<Alloy>(_context);
        public IGenericRepo<ReceivedOrder> ReceivedOrders
              => _ReceivedOrders ??= new GenericRepo<ReceivedOrder>(_context);
        public IGenericRepo<RCPMonthly> RCPMonthly
              => _RCPMonthly ??= new GenericRepo<RCPMonthly>(_context);
        public IGenericRepo<AlloyOrder> AlloyOrders
              => _AlloyOrders ??= new GenericRepo<AlloyOrder>(_context);
        public IGenericRepo<SupplierEmail> SupplierEmails
            => _SupplierEmails ??= new GenericRepo<SupplierEmail>(_context);
        public IGenericRepo<ReceivedAlloyOrder> ReceivedAlloyOrder
            => _ReceivedAlloyOrders ??= new GenericRepo<ReceivedAlloyOrder>(_context);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                _context.Dispose();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
