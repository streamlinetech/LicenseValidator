using System;
using System.Diagnostics;
using FlitBit.Core;
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
			if (IsEnabled)
			{
				using (Create.SharedOrNewContainer())
				{
					Validate.LocationsBaseUrl = "http://api-locations.npc.stage/v1";
					var response = Validate.LicenseByOrder(8, 1154918);
					Assert.NotNull(response);
					Assert.True(response.IsValidAndShippable);
					Assert.False(response.DoesLicenseExpireWithinThreshold);
					Trace.WriteLine(response.ToJson());
				}
			}
		}
	}
}