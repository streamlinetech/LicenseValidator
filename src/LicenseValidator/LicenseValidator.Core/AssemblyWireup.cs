using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using AssemblyWireup = LicenseValidator.Core.AssemblyWireup;


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
