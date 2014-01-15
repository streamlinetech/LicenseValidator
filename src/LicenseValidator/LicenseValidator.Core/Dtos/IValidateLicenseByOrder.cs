using System.ComponentModel.DataAnnotations;
using FlitBit.Dto;

namespace LicenseValidator.Core.Dtos
{
    [DTO]
    public interface IValidateLicenseByOrder : IBasicValidateLicense
    {
        [Required(ErrorMessage = "Order Id is required"), Range(0, int.MaxValue, ErrorMessage = "Order Id must be a positive integer")]
        int OrderId { get; set; }
    }
}