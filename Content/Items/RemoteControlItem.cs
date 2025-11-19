using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RemoteControl.Content.Items;

public class RemoteControlItem : ModItem
{
	private static short glowMaskID = GlowMaskID.None;
	private static Asset<Texture2D> glowmaskTextureOn;
	private static Asset<Texture2D> glowmaskTextureOff;
	private static bool On = false;
	private static SoundStyle BeepSoundStyle = new SoundStyle("RemoteControl/Assets/Sounds/RemoteControlBeep") with { PitchVariance = 0.015625f };

	public override void Load()
	{
		glowmaskTextureOn  = ModContent.Request<Texture2D>(Texture + "_GlowmaskOn",  AssetRequestMode.ImmediateLoad);
		glowmaskTextureOff = ModContent.Request<Texture2D>(Texture + "_GlowmaskOff", AssetRequestMode.ImmediateLoad);

		if (Main.netMode != NetmodeID.Server && !ModLoader.HasMod("TerrariaOverhaul"))
		{
			glowMaskID = Common.GlowmaskHelper.AddGlowMask(glowmaskTextureOff);
		}
	}

	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 1;

		ItemID.Sets.DuplicationMenuToolsFilter[Type] = true;
		ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
	}

	public override void SetDefaults()
	{
		Item.maxStack = 1;
		Item.width = 30;
		Item.height = 30;
		Item.value = 25000;
		Item.rare = ItemRarityID.Orange;
		Item.mech = true;

		if (!ModLoader.HasMod("TerrariaOverhaul"))
		{
			Item.glowMask = glowMaskID;
            Item.holdStyle = ItemHoldStyleID.HoldHeavy;
        }

        Item.useTime = 10;
        Item.useAnimation = Item.useTime;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = BeepSoundStyle;
	}

	public override bool AltFunctionUse(Player player) => true;

	public override bool? UseItem(Player player)
	{
		if (player.JustDroppedAnItem) return true;

		if (Main.netMode != NetmodeID.Server && !ModLoader.HasMod("TerrariaOverhaul"))
		{
			On = On || Common.GlowmaskHelper.EditGlowMask(glowMaskID, glowmaskTextureOn);
		}

		if (player.whoAmI != Main.myPlayer) return true;

		try
		{
			Point pos = Main.MouseWorld.ToTileCoordinates();
			if (!WorldGen.InWorld(pos.X, pos.Y)) return true;

			if (player.altFunctionUse == 2) Common.WiringHelper.HitWireSingle(pos.X, pos.Y);
			else Wiring.TripWire(pos.X, pos.Y, 1, 1);

			return true;
		}
		catch
		{
			return true;
		}
	}
	
	public override void UseStyle(Player player, Rectangle heldItemFrame)
	{
		if (ModLoader.HasMod("TerrariaOverhaul")) return;

		if (player.itemTime == 0)
		{
			player.ApplyItemTime(Item);
		}
		else if (player.itemTime == 1)
		{
			if (On && Main.netMode != NetmodeID.Server)
			{
				Common.GlowmaskHelper.EditGlowMask(glowMaskID, glowmaskTextureOff);
				On = false;
			}
		}
	}
}

public class RemoteControlShop : GlobalNPC
{
	public override void ModifyShop(NPCShop shop)
	{
		if (shop.NpcType == NPCID.Mechanic) shop.Add(ModContent.ItemType<RemoteControlItem>());
	}
}