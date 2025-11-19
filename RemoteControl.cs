using Terraria.ModLoader;

namespace RemoteControl;

public class RemoteControl : Mod
{
	public static RemoteControl Instance { get; private set; }

	public override void Load()
	{
		Instance = this;
	}

	public override void Unload()
	{
		Instance = null;
		Common.GlowmaskHelper.Reset();
	}
}
