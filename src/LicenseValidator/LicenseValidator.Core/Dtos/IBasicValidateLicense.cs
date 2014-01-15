using System.ComponentModel.DataAnnotations;
using FlitBit.Dto;

namespace LicenseValidator.Core.Dtos
{
    [DTO]
    public interface IBasicValidateLicense
    {
        [Required(ErrorMessage = "Location Id is required"), Range(0, int.MaxValue, ErrorMessage = "Location Id must be a positive integer number")]
        int LocationId { get; set; }
    }
}