using System.Collections.Generic;
using LogicAPI.Data;
using LogicWorld.Interfaces;
using LogicWorld.Interfaces.Building;

namespace PixLogicUtils.Client.Menus
{
    /// <summary>
    /// Wrapper minimal pour simuler un composant avec des entrées virtuelles.
    /// </summary>
    internal sealed class ComponentClientCodeWrapper : IComponentClientCode
    {
        private readonly IComponentClientCode wrapped;
        private readonly int overrideInputCount;

        public ComponentClientCodeWrapper(IComponentClientCode original, int bpp)
        {
            wrapped = original;
            overrideInputCount = bpp;
        }

        public int InputCount => overrideInputCount;

        public int OutputCount => wrapped.OutputCount;

        public bool GetInputState(int index)
        {
            // Return false for all virtual inputs (we don't have real inputs)
            if (index >= 0 && index < overrideInputCount)
                return false;
            return wrapped.GetInputState(index);
        }
        public bool GetOutputState(int index) => wrapped.GetOutputState(index);
        public void QueueDataUpdate() => wrapped.QueueDataUpdate();
        public void QueueFrameUpdate() => wrapped.QueueFrameUpdate();
        public byte[] SerializeCustomData() => wrapped.SerializeCustomData();
        public void ReloadCustomDataClientSide() => wrapped.ReloadCustomDataClientSide();
        public void ForceDataRefreshClientSide() => wrapped.ForceDataRefreshClientSide();
        public ChildPlacementInfo GetChildPlacementInfo() => wrapped.GetChildPlacementInfo();
        public bool HasCustomPlacementInfo() => wrapped.HasCustomPlacementInfo();

        public ComponentAddress Address => wrapped.Address;
        public IComponentInWorld Component => null!;
        public IReadOnlyList<IDecoration> Decorations => wrapped.Decorations;
        public object CustomDataObject => wrapped.CustomDataObject;
    }
}
