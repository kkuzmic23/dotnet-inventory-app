using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class SupplierService
    {
        public List<Supplier> GetSuppliers()
        {
            using (var repo = new SupplierRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public bool AddSupplier(Supplier supplier)
        {
            bool isSuccessful = false;
            if (supplier == null)
            {
                return false;
            }

            using (var repo = new SupplierRepository())
            {
                int affectedRows = repo.Add(supplier);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool RemoveSupplier(Supplier supplier)
        {
            bool isSuccessful = false;

            bool canRemove = CheckIfSupplierCanBeRemoved(supplier);
            if (canRemove == true)
            {
                using (var repo = new SupplierRepository())
                {
                    int affectedRows = repo.Remove(supplier);
                    isSuccessful = affectedRows > 0;
                }
            }

            return isSuccessful;
        }

        private bool CheckIfSupplierCanBeRemoved(Supplier supplier)
        {
            if (supplier == null) return false;

            using (var repo = new SupplierRepository())
            {
                int productCount = repo.GetProductCount(supplier).Single();
                if (productCount > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            bool isSuccessful = false;
            if (supplier == null)
            {
                return false;
            }

            using (var repo = new SupplierRepository())
            {
                int affectedRows = repo.Update(supplier);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public List<Supplier> GetSuppliersByName(string phrase)
        {
            using (var repo = new SupplierRepository())
            {
                return repo.GetSuppliersByName(phrase).ToList();
            }
        }
    }
}
