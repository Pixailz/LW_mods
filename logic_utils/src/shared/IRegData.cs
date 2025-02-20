namespace PixLogicUtils.Shared.CustomData
{
    public interface IRegData
    {
        int value { get; set; }
        int dataWidth { get; set; }
    }

    public static class RegDataInit
    {
        public static void initialize(this IRegData data)
        {
            data.value = 0;
            data.dataWidth = 1;
        }
    }
}
