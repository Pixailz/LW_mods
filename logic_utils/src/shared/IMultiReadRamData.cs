using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Shared.CustomData
{
	public interface IMultiReadRamData
	{
		// Put value at the end
		t_width	DataWidth { get; set; }
		t_width	AddressWidth { get; set; }
		int		ReadNumber { get; set; }

		bool	IsDataDirty { get; set; }
		bool	LoadFromSave { get; set; }

		byte[]	Memory { get; set; }
		byte	State { get; set; }
		byte[]	ClientIncomingData { get; set; }
	}

	public static class MultiReadRamDataInit
	{
		public static void Initialize(this IMultiReadRamData Data)
		{
			// Data.DataWidth = CMultiReadRam.DefaultAddressWidth;
			// Data.AddressWidth = CMultiReadRam.DefaultAddressWidth;
			Data.State = 0;
			Data.IsDataDirty = false;
			Data.LoadFromSave = true;
			Data.ClientIncomingData = [];
			Data.Memory = [];
		}
	}
}
