using System.Linq;
using FlitBit.Wireup;

namespace Validator.Core.Tests
{
	public abstract class AbstractTests
	{
		public bool IsEnabled { get; set; }

		protected AbstractTests()
		{
			WireupCoordinator.SelfConfigure();
			IsEnabled = false;
		}
	}
}
