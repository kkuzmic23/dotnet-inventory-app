using DataAccessLayer;
using EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class SupplierService
    {
        ISupplierCRUDRepository repo;

        public SupplierService() : this(new SupplierRepository())
        {
        }

        public SupplierService(ISupplierCRUDRepository supplierRepository)
        {
            repo = supplierRepository;
        }

        public List<Supplier> GetSuppliers()
        {
            return repo.GetAll().ToList();
        }

        public bool AddSupplier(Supplier supplier)
        {
            if (supplier == null)
            {
                return false;
            }

            int affectedRows = repo.Add(supplier);
            return affectedRows > 0;
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            if (supplier == null)
            {
                return false;
            }

            int affectedRows = repo.Update(supplier);
            return affectedRows > 0;
        }

        public bool RemoveSupplier(Supplier supplier)
        {
            if (supplier == null)
            {
                return false;
            }

            if (!CanRemoveSupplier(supplier))
            {
                return false;
            }

            int affectedRows = repo.Remove(supplier);
            return affectedRows > 0;
        }

        private bool CanRemoveSupplier(Supplier supplier)
        {
            if (supplier.Id == 0)
            {
                return false;
            }

            int productCount = repo.GetProductCount(supplier);
            return productCount == 0;
        }
    }
}