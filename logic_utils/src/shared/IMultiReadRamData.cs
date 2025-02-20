namespace PixLogicUtils.Shared.CustomData
{
	public interface IMultiReadRamData
	{
		// Put value at the end
		int dataWidth { get; set; }
		int addressWidth { get; set; }

		byte[] Data { get; set; }
		byte State { get; set; }
		byte[] ClientIncomingData { get; set; }
	}

	public static class MultiReadRamDataInit
	{
		public static void initialize(this IMultiReadRamData data)
		{
			data.dataWidth = 1;
			data.addressWidth = 4;
			data.State = 0;
			data.ClientIncomingData = new byte[0];
			data.Data = new byte[0];
		}
	}
}
