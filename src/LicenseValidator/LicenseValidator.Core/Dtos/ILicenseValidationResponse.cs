using System.Collections.Generic;
using FlitBit.Dto;

namespace LicenseValidator.Core.Dtos
{
    [DTO]
    public interface ILicenseValidationResponse
    {
        string FriendlyMessage { get; set; }
        IEnumerable<ILicense> Licenses { get; set; }
        bool IsValidAndShippable { get; set; }
        bool DoesLicenseExpireWithinThreshold { get; set; }
    }
}