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
                var response = Validate.LicenseByState(8, "USA", "GA");
                Assert.NotNull(response);
            }
        }
    }
}