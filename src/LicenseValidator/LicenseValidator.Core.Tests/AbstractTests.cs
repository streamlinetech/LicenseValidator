using System.Linq;
using FlitBit.Wireup;

namespace Validator.Core.Tests
{
    public abstract class AbstractTests
    {
        protected AbstractTests()
        {
            WireupCoordinator.SelfConfigure();
        }
    }
}
