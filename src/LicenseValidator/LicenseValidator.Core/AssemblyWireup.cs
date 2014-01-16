using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using AssemblyWireup = LicenseValidator.Core.AssemblyWireup;

[assembly: WireupDependency(typeof(FlitBit.Wireup.AssemblyWireup))]
[assembly: WireupDependency(typeof(FlitBit.Dto.AssemblyWireup))]
[assembly: WireupDependency(typeof(FlitBit.Represent.AssemblyWireup))]
[assembly: Wireup(typeof(AssemblyWireup))]
namespace LicenseValidator.Core
{
    public class AssemblyWireup : IWireupCommand
    {
        public void Execute(IWireupCoordinator coordinator)
        {
        }
    }
}
