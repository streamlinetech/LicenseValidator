using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using FlitBit.Core;
using FlitBit.Core.Net;
using FlitBit.IoC.Meta;
using FlitBit.Wireup;
using LicenseValidator.Core.Dtos;
using LicenseValidator.Core.Exceptions;
using Newtonsoft.Json;
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

        public LicenseValidator()
        {
            WireupCoordinator.SelfConfigure();
            // <add key="api_purchaseorders" value="http://api-purchaseorders.streamlinedb.dev/v1" />
            PurchaseOrderUrl = ConfigurationManager.AppSettings["api_purchaseorders"];
            Client = new HttpClient();
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

            return PostValidationRequest(request, url);


        }

        ILicenseValidationResponse PostValidationRequest<T>(T request, Uri url)
        {
            var json = request.ToJson();
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var completion = url.MakeResourceRequest().ParallelPost(jsonBytes, "application/json", response =>
                                                                                  {
                                                                                      if (response.StatusCode == HttpStatusCode.OK)
                                                                                      {
                                                                                          var responseJson = response.GetResponseBodyAsString();
                                                                                          if (!string.IsNullOrEmpty(responseJson))
                                                                                              return JsonConvert.DeserializeObject<ILicenseValidationResponse>(responseJson, DefaultJsonSerializerSettings.Current);
                                                                                      }

                                                                                      throw new LicenseValidationException()
                                                                                            {
                                                                                                StatusCode = response.StatusCode
                                                                                            };
                                                                                  });
            return completion.AwaitValue();
        }

        public ILicenseValidationResponse ValidateLicenseByOrder(IValidateLicenseByOrder request)
        {
            var url = new Uri(PurchaseOrderUrl + "/{0}".P(OrderValidationPath));
            var errors = request.GetValidationErrors().ToList();
            if (errors.Any())
                throw new ObjectValidationException(errors);

            return PostValidationRequest(request, url);
        }
    }
}
