using System;
using System.Configuration;
using System.Linq;
using System.Net;
using FlitBit.Core.Net;
using FlitBit.IoC.Meta;
using LicenseValidator.Core.Dtos;
using RedRocket;
using RedRocket.Utilities.Core.Validation;

namespace LicenseValidator.Core
{

    public interface ILicenseValidator
    {
        ILicenseValidationResponse ValidateLicenseByState(IValidateLicenseByState request);
        ILicenseValidationResponse ValidateLicenseByOrder(IValidateLicenseByState request);
    }

    [ContainerRegister(typeof(ILicenseValidator), RegistrationBehaviors.Default)]
    public class LicenseValidator : ILicenseValidator
    {
        const string StateValidationPath = "validate/location";
        const string OrderValidationPath = "validate/order";
        protected string PurchaseOrderUrl { get; private set; }

        public LicenseValidator()
        {
            // <add key="api_purchaseorders" value="http://api-purchaseorders.streamlinedb.dev/v1" />
            PurchaseOrderUrl = ConfigurationManager.AppSettings["api_purchaseorders"];

        }

        public LicenseValidator(string url)
        {
            PurchaseOrderUrl = ConfigurationManager.AppSettings["api_purchaseorders"];

        }

        public ILicenseValidationResponse ValidateLicenseByState(IValidateLicenseByState request)
        {
            var url = new Uri(PurchaseOrderUrl + "/{0}".P(StateValidationPath));
            var errors = request.GetValidationErrors().ToList();
            if (errors.Any())
                throw new ObjectValidationException(errors);
            ILicenseValidationResponse licenseValidationResponse = null;
            url.MakeResourceRequest().HttpPostJson(request, (exception, response) =>
                                                                         {
                                                                             if (response.StatusCode == HttpStatusCode.OK)
                                                                             {
                                                                                 licenseValidationResponse = response.DeserializeResponse<ILicenseValidationResponse>();
                                                                             }

                                                                         });

            return licenseValidationResponse;

        }

        public ILicenseValidationResponse ValidateLicenseByOrder(IValidateLicenseByState request)
        {
            var url = new Uri(PurchaseOrderUrl + "/{0}".P(OrderValidationPath));
            var errors = request.GetValidationErrors().ToList();
            if (errors.Any())
                throw new ObjectValidationException(errors);

            ILicenseValidationResponse licenseValidationResponse = null;
            url.MakeResourceRequest().HttpPostJson(request, (exception, response) =>
                                                            {
                                                                if (response.StatusCode == HttpStatusCode.OK)
                                                                {
                                                                    licenseValidationResponse = response.DeserializeResponse<ILicenseValidationResponse>();
                                                                }

                                                            });

            return licenseValidationResponse;
        }
    }
}
