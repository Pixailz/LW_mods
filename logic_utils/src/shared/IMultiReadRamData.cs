namespace PixLogicUtils.Shared.CustomData
{
	public interface IMultiReadRamData
	{
		t_width	DataWidth { get; set; }
		t_width	AddressWidth { get; set; }
		int		ReadNumber { get; set; }

		byte[]	Memory { get; set; }
		byte	State { get; set; }
		byte[]	ClientIncomingData { get; set; }
	}

	public static class MultiReadRamDataInit
	{
		public static void Initialize(this IMultiReadRamData Data)
		{
			Data.State = 0;
			Data.ClientIncomingData = [];
			Data.Memory = [];
		}
	}
}
