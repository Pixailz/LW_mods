using LogicWorld.Server.Circuitry;
using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Server
{
	public class DummyTestServer : LogicComponent<IDummyData>
	{
		protected override void Initialize()
		{
			// Nothing to do
		}

		protected override void DoLogicUpdate()
		{
			// Nothing to do - this is a dummy component
		}

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
