using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using FlitBit.IoC;
using FlitBit.IoC.Meta;
using FlitBit.Represent.Json;
using FlitBit.Wireup;
using LicenseValidator.Core.Dtos;
using LicenseValidator.Core.Exceptions;
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

        HttpClient Client { get; set; }

        IJsonRepresentation<ILicenseValidationResponse> Representation { get; set; }

        public LicenseValidator()
        {
            WireupCoordinator.SelfConfigure();
            // <add key="api_purchaseorders" value="http://api-purchaseorders.streamlinedb.dev/v1" />
            PurchaseOrderUrl = ConfigurationManager.AppSettings["api_purchaseorders"];
            Client = new HttpClient();
            Representation = Create.New<IJsonRepresentation<ILicenseValidationResponse>>();
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

            var httpResponse = Client.PostAsJsonAsync(url, request).Result;

            if (httpResponse == null)
                throw new NullReferenceException("HttpResponse is null");

            if (httpResponse.IsSuccessStatusCode)
                return Representation.RestoreItem(httpResponse.Content.ReadAsStringAsync().Result);


            throw new LicenseValidationException()
                  {
                      StatusCode = httpResponse.StatusCode,
                      HttpResponse = httpResponse
                  };
        }

        public ILicenseValidationResponse ValidateLicenseByOrder(IValidateLicenseByOrder request)
        {
            var url = new Uri(PurchaseOrderUrl + "/{0}".P(OrderValidationPath));
            var errors = request.GetValidationErrors().ToList();
            if (errors.Any())
                throw new ObjectValidationException(errors);

            var httpResponse = Client.PostAsJsonAsync(url, request).Result;

            if (httpResponse == null)
                throw new NullReferenceException("HttpResponse is null");

            if (httpResponse.IsSuccessStatusCode)
                return Representation.RestoreItem(httpResponse.Content.ReadAsStringAsync().Result);

            throw new LicenseValidationException()
                  {
                      StatusCode = httpResponse.StatusCode,
                      HttpResponse = httpResponse
                  };
        }
    }
}
