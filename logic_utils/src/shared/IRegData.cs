using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Shared.CustomData
{
	public interface IRegData
	{
		t_data	Value { get; set; }
		t_width	DataWidth { get; set; }
		bool	LoadFromSave { get; set; }
	}

	public static class RegDataInit
	{
		public static void Initialize(this IRegData data)
		{
			data.Value = 0;
			data.DataWidth = CRegister.DefaultDataWidth;
			data.LoadFromSave = true;
		}
	}
}
