using System.ComponentModel.DataAnnotations;
using FlitBit.Dto;

namespace LicenseValidator.Core.Dtos
{
    [DTO]
    public interface IValidateLicenseByState : IBasicValidateLicense
    {


        /// <summary>
        /// Country Code
        /// </summary>
        [Required(ErrorMessage = "Country is required"), StringLength(3, MinimumLength = 2, ErrorMessage = "Country must be 2 or 3 characters")]
        string Country { get; set; }

        /// <summary>
        /// State Name or Code
        /// </summary>
        [Required(ErrorMessage = "State is required"), StringLength(128, MinimumLength = 2, ErrorMessage = "State must be at least 2 characters and less than 128 characters in length")]
        string State { get; set; }
    }
}