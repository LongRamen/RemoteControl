using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ID;

namespace RemoteControl.Common;

public static class GlowmaskHelper
{
	public static short AddGlowMask(Asset<Texture2D> glowmaskTexture)
	{
		if (TextureAssets.GlowMask.Length > Int16.MaxValue) return GlowMaskID.None;

		Array.Resize(ref TextureAssets.GlowMask, TextureAssets.GlowMask.Length + 1);
		short glowMask = (short)(TextureAssets.GlowMask.Length - 1);
		TextureAssets.GlowMask[glowMask] = glowmaskTexture;
		return glowMask;
	}

	public static bool EditGlowMask(short glowMask, Asset<Texture2D> glowmaskTexture)
	{
		if (glowMask >= TextureAssets.GlowMask.Length || glowMask == GlowMaskID.None) return false;

		TextureAssets.GlowMask[glowMask] = glowmaskTexture;
		return true;
	}

	public static void Reset()
	{
		if (TextureAssets.GlowMask.Length != GlowMaskID.Count)
		{
			Array.Resize(ref TextureAssets.GlowMask, GlowMaskID.Count);
		}
	}
}
