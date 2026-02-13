
using LogicAPI.Server.Components;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Server
{
	// LogicWorld.LogicCode.FastBuffer
	public class PulserServer : LogicComponent
	{
		protected override void Initialize()
		{
			this.link();
		}

		private void link()
		{
			base.Inputs[CPulser.Pin.In].AddOneWayPhasicLinkTo(base.Inputs[CPulser.Pin.Out]);
		}

		private void unlink()
		{
			base.Inputs[CPulser.Pin.In].RemoveOneWayPhasicLinkTo(base.Inputs[CPulser.Pin.Out]);
		}

		public override bool InputAtIndexShouldTriggerComponentLogicUpdates(int inputIndex)
		{
			if (inputIndex == CPulser.Pin.In)
				return true;
			return false;
		}

		private bool have_pulsed = false;

		protected override void DoLogicUpdate()
		{
			if (Inputs[CPulser.Pin.In].On)
			{
				if (!this.have_pulsed)
				{
					unlink();
					this.have_pulsed = true;
					return ;
				}
			}
			else
			{
				if (this.have_pulsed)
				{
					link();
					this.have_pulsed = false;
				}
			}
			QueueLogicUpdate();
		}
	}
}
