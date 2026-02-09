using LogicWorld.Rendering.Components;

namespace PixLogicUtils.Client
{
    public class DummyTestClient : ComponentClientCode<DummyTestClient.IData>
    {
        public interface IData
        {
            int testValue { get; set; }
        }

        protected override void SetDataDefaultValues()
        {
            Data.testValue = 0;
        }
    }
}
