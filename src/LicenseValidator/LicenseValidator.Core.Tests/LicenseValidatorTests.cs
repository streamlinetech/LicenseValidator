using FlitBit.IoC;
using LicenseValidator.Core;
using Xunit;

namespace Validator.Core.Tests
{
    public class LicenseValidatorTests : AbstractTests
    {
        [Fact]
        public void ValidateValidLicense()
        {
            using (Create.SharedOrNewContainer())
            {
                var licenseValidator = Create.New<ILicenseValidator>();
                var response = Validate.LicenseByOrder(8, 1154918);
                Assert.NotNull(response);
                Assert.False(response.IsValidAndShippable);
            }
        }
    }
}