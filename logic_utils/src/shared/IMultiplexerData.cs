using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Shared.CustomData
{
	public interface IMultiplexerData
	{
		t_width	DataWidth { get; set; }
		t_width SelectorWidth {get; set; }
	}

	public static class MultiplexerDataInit
	{
		public static void Initialize(this IMultiplexerData data)
		{
			data.DataWidth = CMultiplexer.DefaultDataWidth;
			data.SelectorWidth = CMultiplexer.DefaultSelectorWidth;
		}
	}
}
