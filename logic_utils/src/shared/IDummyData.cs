namespace PixLogicUtils.Shared.CustomData
{
	public interface IDummyData
	{
		t_data	Value { get; set; }
	}

	public static class DummyDataInit
	{
		public static void Initialize(this IDummyData data)
		{
			data.Value = 0;
		}
	}
}
