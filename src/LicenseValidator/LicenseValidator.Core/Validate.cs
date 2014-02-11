using System;
using System.Configuration;
using System.Security.Policy;
using FlitBit.IoC;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using LicenseValidator.Core.Dtos;
using LicenseValidator.Core.Exceptions;

[assembly: WireupDependency(typeof(FlitBit.Wireup.AssemblyWireup))]
[assembly: WireupDependency(typeof(FlitBit.Dto.AssemblyWireup))]

namespace LicenseValidator.Core
{
	public static class Validate
	{
		/// <summary>
		/// Machine Configuration Key
		/// </summary>
		const string ConfigurationKey = "api_locations";

		public static Uri LicenseByOrderUrl
		{
			get
			{
				return new Uri(LocationsBaseUrl + "/validation/orders");
			}
		}

		static Validate()
		{
			WireupCoordinator.SelfConfigure();
			var baseUrl = ConfigurationManager.AppSettings[ConfigurationKey];
			LocationsBaseUrl = baseUrl;
		}

		public static string LocationsBaseUrl { get; set; }

		public static ILicenseValidationResponse LicenseByOrder(int shipFromLocationId, int orderId)
		{
			var request = SetupLicenseByOrder(shipFromLocationId, orderId);
			var licenseValidator = SetupLicenseValidator();
			try
			{
				return licenseValidator.ValidateLicenseByOrder(request);
			}
			catch (LicenseValidationException ex)
			{
				throw new BasicHttpException(ex);
			}
			catch (NullReferenceException ex)
			{
				throw new BasicHttpException(ex);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException.Message, ex);
			}
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