using FlitBit.Dto;

namespace LicenseValidator.Core.Dtos
{
    [DTO]
    public interface ILocation
    {
        int LocationId { get; set; }
        string Name { get; set; }
        bool IsShippableLocation { get; set; }
    }
}