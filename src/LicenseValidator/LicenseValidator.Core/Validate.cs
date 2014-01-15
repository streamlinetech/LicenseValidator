using FlitBit.IoC;
using LicenseValidator.Core.Dtos;

namespace LicenseValidator.Core
{
    public static class Validate
    {
        public static ILicenseValidationResponse LicenseByState(int shipFromLocationId, string countryCode, string stateCodeOrName)
        {
            var request = SetupLicenseByStateRequest(shipFromLocationId, countryCode, stateCodeOrName);
            var licenseValidator = SetupLicenseValidator();
            var response = licenseValidator.ValidateLicenseByState(request);
            return response;
        }

        public static ILicenseValidationResponse LicenseByState(int shipFromLocationId, string countryCode, string stateCodeOrName, string purchaseOrderApiUrl)
        {
            var request = SetupLicenseByStateRequest(shipFromLocationId, countryCode, stateCodeOrName);
            var licenseValidator = SetupLicenseValidator(purchaseOrderApiUrl);
            var response = licenseValidator.ValidateLicenseByState(request);
            return response;
        }

        public static ILicenseValidationResponse LicenseByOrder(int shipFromLocationId, int orderId)
        {
            var request = SetupLicenseByOrder(shipFromLocationId, orderId);
            var licenseValidator = SetupLicenseValidator();
            var response = licenseValidator.ValidateLicenseByOrder(request);
            return response;
        }

        public static ILicenseValidationResponse LicenseByOrder(int shipFromLocationId, int orderId, string purchaseOrderApiUrl)
        {
            var request = SetupLicenseByOrder(shipFromLocationId, orderId);
            var licenseValidator = SetupLicenseValidator(purchaseOrderApiUrl);
            var response = licenseValidator.ValidateLicenseByOrder(request);
            return response;
        }

        static IValidateLicenseByState SetupLicenseByStateRequest(int shipFromLocationId, string countryCode, string stateCodeOrName)
        {
            var request = Create.NewInit<IValidateLicenseByState>().Init(new
                                                                         {
                                                                             Country = countryCode,
                                                                             State = stateCodeOrName,
                                                                             LocationId = shipFromLocationId
                                                                         });
            return request;
        }

        static IValidateLicenseByOrder SetupLicenseByOrder(int shipFromLocationId, int orderId)
        {
            var request = Create.NewInit<IValidateLicenseByOrder>().Init(new
                                                                         {
                                                                             LocationId = shipFromLocationId,
                                                                             OrderId = orderId
                                                                         });

            return request;
        }

        static ILicenseValidator SetupLicenseValidator(string url = "")
        {
            if (!string.IsNullOrEmpty(url))
                return Create.NewWithParams<ILicenseValidator>(LifespanTracking.Automatic, Param.FromValue(url));
            return Create.New<ILicenseValidator>();
        }
    }
}