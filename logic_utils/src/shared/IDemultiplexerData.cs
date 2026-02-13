using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Shared.CustomData
{
	public interface IDemultiplexerData
	{
		t_width	DataWidth { get; set; }
		t_width SelectorWidth {get; set; }
	}

	public static class DemultiplexerDataInit
	{
		public static void Initialize(this IDemultiplexerData data)
		{
			data.DataWidth = CDemultiplexer.DefaultDataWidth;
			data.SelectorWidth = CDemultiplexer.DefaultSelectorWidth;
		}
	}
}
