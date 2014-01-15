using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using FlitBit.Core;
using FlitBit.Core.Net;
using FlitBit.IoC;
using FlitBit.IoC.Meta;
using FlitBit.Represent.Json;
using LicenseValidator.Core.Dtos;
using RedRocket;
using RedRocket.Utilities.Core.Validation;

namespace LicenseValidator.Core
{
    public interface ILicenseValidator
    {
        ILicenseValidationResponse ValidateLicenseByState(IValidateLicenseByState request);
        ILicenseValidationResponse ValidateLicenseByOrder(IValidateLicenseByOrder request);
    }

    [ContainerRegister(typeof(ILicenseValidator), RegistrationBehaviors.Default)]
    public class LicenseValidator : ILicenseValidator
    {
        const string StateValidationPath = "licenses/validate/location";
        const string OrderValidationPath = "licenses/validate/order";
        protected string PurchaseOrderUrl { get; private set; }

        public LicenseValidator()
        {
            // <add key="api_purchaseorders" value="http://api-purchaseorders.streamlinedb.dev/v1" />
            PurchaseOrderUrl = ConfigurationManager.AppSettings["api_purchaseorders"];

        }

        public LicenseValidator(string url)
        {
            PurchaseOrderUrl = url;
        }

        public ILicenseValidationResponse ValidateLicenseByState(IValidateLicenseByState request)
        {
            var url = new Uri(PurchaseOrderUrl + "/{0}".P(StateValidationPath));
            var errors = request.GetValidationErrors().ToList();
            if (errors.Any())
                throw new ObjectValidationException(errors);
            var resp = url.MakeResourceRequest().ParallelPost(Encoding.UTF8.GetBytes(request.ToJson()), "application/json", response =>
                                                                                                                            {
                                                                                                                                if (response.StatusCode == HttpStatusCode.OK)
                                                                                                                                    return Create.New<IJsonRepresentation<ILicenseValidationResponse>>().RestoreItem(response.GetResponseBodyAsString());
                                                                                                                                throw new HttpException((int)response.StatusCode, response.GetResponseBodyAsString());
                                                                                                                            }).AwaitValue();

            return resp;

        }

        public ILicenseValidationResponse ValidateLicenseByOrder(IValidateLicenseByOrder request)
        {
            var url = new Uri(PurchaseOrderUrl + "/{0}".P(OrderValidationPath));
            var errors = request.GetValidationErrors().ToList();
            if (errors.Any())
                throw new ObjectValidationException(errors);

            var resp = url.MakeResourceRequest().ParallelPost(Encoding.UTF8.GetBytes(request.ToJson()), "application/json", response =>
                                                                                                                            {
                                                                                                                                if (response.StatusCode == HttpStatusCode.OK)
                                                                                                                                    return Create.New<IJsonRepresentation<ILicenseValidationResponse>>().RestoreItem(response.GetResponseBodyAsString());
                                                                                                                                throw new HttpException((int)response.StatusCode, response.GetResponseBodyAsString());
                                                                                                                            }).AwaitValue();

            return resp;
        }
    }
}
