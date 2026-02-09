using LogicWorld.Server.Circuitry;

namespace PixLogicUtils.Server
{
    public class DummyTestServer : LogicComponent<DummyTestServer.IData>
    {
        public interface IData
        {
            int testValue { get; set; }
        }

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
            Data.testValue = 0;
        }
    }
}
