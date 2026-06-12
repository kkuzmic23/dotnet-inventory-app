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
            if (!ValidateSupplier(supplier).IsSuccessful)
            {
                return false;
            }

            int affectedRows = repo.Add(supplier);
            return affectedRows > 0;
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            if (!ValidateSupplier(supplier).IsSuccessful)
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

        public ServiceResult ValidateSupplier(Supplier supplier)
        {
            if (supplier == null)
            {
                return ServiceResult.Failure("Supplier is required");
            }

            if (string.IsNullOrWhiteSpace(supplier.Name))
            {
                return ServiceResult.Failure("Name is required");
            }

            return ServiceResult.Success();
        }
    }
}
