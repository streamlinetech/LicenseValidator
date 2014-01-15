using System.Collections.Generic;
using FlitBit.Dto;

namespace LicenseValidator.Core.Dtos
{
    [DTO]
    public interface ILicenseValidationResponse
    {
        IEnumerable<ILicense> Licenses { get; set; }
        bool IsValidAndShippable { get; set; }
    }
}