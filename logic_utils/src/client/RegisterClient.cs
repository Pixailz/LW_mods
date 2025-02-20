using LogicWorld.Rendering.Components;
using System.IO;
using System.IO.Compression;

using PixLogicUtils.Shared.CustomData;
using LICC;

namespace PixLogicUtils.Client
{
	public class RegisterClient : ComponentClientCode<IRegData>
	{
		protected override void SetDataDefaultValues()
		{
			Data.initialize();
		}
	}
}
