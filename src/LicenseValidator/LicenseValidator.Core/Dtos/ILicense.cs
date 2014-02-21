using System;
using FlitBit.Dto;

namespace LicenseValidator.Core.Dtos
{
    [DTO]
    public interface ILicense
    {
        ILocation Location { get; set; }
        string LicenseNumber { get; set; }
        string LicenseTypeCode { get; set; }
        string LicenseTypeDescription { get; set; }
        IState State { get; set; }
        DateTime ExpireDate { get; set; }
		DateTime IssueDate { get; set; }
    }
}