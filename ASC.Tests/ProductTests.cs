using ASC.Model;
using Xunit;

namespace ASC.Tests
{
    /// <summary>
    /// Unit tests cho Product model - Lab 2 Step 6
    /// </summary>
    public class ProductTests
    {
        [Fact]
        public void Product_InheritsBaseEntity_Test()
        {
            var product = new Product();
            Assert.IsAssignableFrom<BaseEntity>(product);
        }

        [Fact]
        public void Product_ImplementsIAuditTracker_Test()
        {
            var product = new Product();
            Assert.IsAssignableFrom<IAuditTracker>(product);
        }

        [Fact]
        public void Product_IsActive_DefaultsTrue_Test()
        {
            var product = new Product();
            Assert.True(product.IsActive);
        }

        [Fact]
        public void Product_UniqueId_CanBeSet_Test()
        {
            var id = Guid.NewGuid().ToString();
            var product = new Product { UniqueId = id };
            Assert.Equal(id, product.UniqueId);
        }

        [Fact]
        public void Product_Name_CanBeSet_Test()
        {
            var product = new Product { Name = "Engine Oil" };
            Assert.Equal("Engine Oil", product.Name);
        }

        [Fact]
        public void Product_Price_CanBeSet_Test()
        {
            var product = new Product { Price = 150000m };
            Assert.Equal(150000m, product.Price);
        }

        [Theory]
        [InlineData("Engine Parts")]
        [InlineData("Tyres")]
        [InlineData("Accessories")]
        public void Product_Category_AcceptsValidValues_Test(string category)
        {
            var product = new Product { Category = category };
            Assert.Equal(category, product.Category);
        }
    }
}
