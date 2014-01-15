using FlitBit.Dto;

namespace LicenseValidator.Core.Dtos
{
    [DTO]
    public interface IState
    {
        string CountryCode { get; set; }
        int StateId { get; set; }
        string StateCode { get; set; }
        string StateName { get; set; }
    }
}