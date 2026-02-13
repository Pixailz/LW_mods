using LICC;

namespace PixLogicUtils.Client
{
	public interface FileLoadable
	{
		void Load(byte[] filedata, LineWriter writer, bool force);
	}
}
