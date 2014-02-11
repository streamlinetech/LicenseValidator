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
		ILicenseValidationResponse ValidateLicenseByOrder(IValidateLicenseByOrder request);
	}

	[ContainerRegister(typeof(ILicenseValidator), RegistrationBehaviors.Default)]
	public class LicenseValidator : ILicenseValidator
	{
		Uri LicenseByOrderUrl { get; set; }

		HttpClient Client { get; set; }

		public LicenseValidator()
		{
			WireupCoordinator.SelfConfigure();
			// <add key="api_purchaseorders" value="http://api-purchaseorders.streamlinedb.dev/v1" />
			LicenseByOrderUrl = Validate.LicenseByOrderUrl;
			Client = new HttpClient();
		}

		public LicenseValidator(string url)
		{
			LicenseByOrderUrl = new Uri(url);
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
																						  
																						  throw new NullReferenceException("Response is empty.");
																					  }

																					  throw new LicenseValidationException() { StatusCode = response.StatusCode };
																				  });
			return completion.AwaitValue();
		}

		public ILicenseValidationResponse ValidateLicenseByOrder(IValidateLicenseByOrder request)
		{
			var errors = request.GetValidationErrors().ToList();
			if (errors.Any())
				throw new ObjectValidationException(errors);

			return PostValidationRequest(request, LicenseByOrderUrl);
		}
	}
}
