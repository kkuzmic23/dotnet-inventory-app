using BusinessLogicLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests
{
    public class SupplierTests
    {

        [Fact]
        public void ValidateSupplier_ValidSupplier_ReturnsSuccess()
        {
            SupplierService service = new SupplierService();

            Supplier supplier = new Supplier
            {
                Id = 1,
                Name = "Supplier name",
                Email = "supplier@email.com",
                Phone = "0123456789",
                Address = "Supplieraddress 130",
                CreatedAt = DateTime.Now
            };

            ServiceResult result = service.ValidateSupplier(supplier);

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ValidateSupplier_NullSupplier_ReturnsFailure()
        {
            SupplierService service = new SupplierService();
            
            ServiceResult result = service.ValidateSupplier(null);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Supplier is required", result.ErrorMessage);
        }

        [Fact]
        public void ValidateSupplier_SupplierWithNullName_ReturnsFailure()
        {
            SupplierService service = new SupplierService();

            Supplier supplier = new Supplier
            {
                Id = 1,
                Name = "",
                Email = "supplier@email.com",
                Phone = "0123456789",
                Address = "Supplieraddress 130",
                CreatedAt = DateTime.Now
            };

            ServiceResult result = service.ValidateSupplier(supplier);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Name is required", result.ErrorMessage);
        }
    }
}
