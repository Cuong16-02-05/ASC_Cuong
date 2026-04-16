using ASC.Model;
using Xunit;

namespace ASC.Tests
{
    /// <summary>
    /// Unit tests for MasterData models (Lab 3)
    /// </summary>
    public class MasterDataTests
    {
        [Fact]
        public void MasterDataKey_UniqueId_CanBeSet_Test()
        {
            var key = new MasterDataKey();
            var id = Guid.NewGuid().ToString();
            key.UniqueId = id;
            Assert.Equal(id, key.UniqueId);
        }

        [Fact]
        public void MasterDataKey_IsActive_DefaultsFalse_Test()
        {
            var key = new MasterDataKey();
            Assert.False(key.IsActive);
        }

        [Fact]
        public void MasterDataKey_InheritsBaseEntity_Test()
        {
            var key = new MasterDataKey();
            Assert.IsAssignableFrom<BaseEntity>(key);
        }

        [Fact]
        public void MasterDataKey_ImplementsIAuditTracker_Test()
        {
            var key = new MasterDataKey();
            Assert.IsAssignableFrom<IAuditTracker>(key);
        }

        [Fact]
        public void MasterDataValue_MasterDataKeyId_CanBeLinked_Test()
        {
            var keyId = Guid.NewGuid().ToString();
            var value = new MasterDataValue
            {
                UniqueId = Guid.NewGuid().ToString(),
                MasterDataKeyId = keyId,
                Name = "Engine Service",
                IsActive = true
            };
            Assert.Equal(keyId, value.MasterDataKeyId);
        }

        [Fact]
        public void MasterDataValue_Name_CanBeSet_Test()
        {
            var value = new MasterDataValue { Name = "Oil Change" };
            Assert.Equal("Oil Change", value.Name);
        }

        [Theory]
        [InlineData("Engine Repair")]
        [InlineData("Tyre Replacement")]
        [InlineData("AC Service")]
        [InlineData("Battery Check")]
        public void MasterDataValue_Name_AcceptsServiceTypes_Test(string name)
        {
            var value = new MasterDataValue { Name = name };
            Assert.Equal(name, value.Name);
        }
    }
}
