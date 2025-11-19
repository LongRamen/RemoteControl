using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RemoteControl.Common;

public static class WiringHelper
{
	private static void GeyserTrap(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileType != TileID.GeyserTrap)
			return;

		int num = tile.TileFrameX / 36;
		int num2 = i - (tile.TileFrameX - num * 36) / 18;
		if (Wiring.CheckMech(num2, j, 200))
		{
			Vector2 zero;
			Vector2 zero2;
			int num3 = 654;
			int damage = 20;
			if (num < 2)
			{
				zero = new Vector2(num2 + 1, j) * 16f;
				zero2 = new Vector2(0f, -8f);
			}
			else
			{
				zero = new Vector2(num2 + 1, j + 1) * 16f;
				zero2 = new Vector2(0f, 8f);
			}

			if (num3 != 0)
				Projectile.NewProjectile(Wiring.GetProjectileSource(num2, j), (int)zero.X, (int)zero.Y, zero2.X, zero2.Y, num3, damage, 2f, Main.myPlayer);
		}
	}

	public static void HitWireSingle(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		bool? forcedStateWhereTrueIsOn = null;
		bool doSkipWires = false;//true;
		int tileType = tile.TileType;
		if (tile.HasActuator)
			Wiring.ActuateForced(i, j);

		if (!tile.HasTile)
			return;

		if (!TileLoader.PreHitWire(i, j, tileType))
			return;

		switch (tileType)
		{
			case TileID.Timers:
				Wiring.HitSwitch(i, j);
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
				break;
			case TileID.ConveyorBeltLeft:
				if (!tile.HasActuator)
				{
					tile.TileType = TileID.ConveyorBeltRight;
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j);
				}
				break;
			case TileID.ConveyorBeltRight:
				if (!tile.HasActuator)
				{
					tile.TileType = TileID.ConveyorBeltLeft;
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j);
				}
				break;
		}

		if (tileType >= 255 && tileType <= 268)
		{
			if (!tile.HasActuator)
			{
				if (tileType >= 262)
					tile.TileType -= 7;
				else
					tile.TileType += 7;

				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}

			return;
		}

		if (tileType == 419)
		{
			int num = 18;
			if (tile.TileFrameX >= num)
				num = -num;

			if (tile.TileFrameX == 36)
				num = 0;

			//Wiring.SkipWire(i, j);
			tile.TileFrameX = (short)(tile.TileFrameX + num);
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			Wiring._LampsToCheck.Enqueue(new Point16(i, j));
			return;
		}

		if (tileType == 406)
		{
			int num2 = tile.TileFrameX % 54 / 18;
			int num3 = tile.TileFrameY % 54 / 18;
			int num4 = i - num2;
			int num5 = j - num3;
			int num6 = 54;
			if (Main.tile[num4, num5].TileFrameY >= 108)
				num6 = -108;

			for (int k = num4; k < num4 + 3; k++)
			{
				for (int l = num5; l < num5 + 3; l++)
				{
					//Wiring.SkipWire(k, l);
					Main.tile[k, l].TileFrameY = (short)(Main.tile[k, l].TileFrameY + num6);
				}
			}

			NetMessage.SendTileSquare(-1, num4 + 1, num5 + 1, 3);
			return;
		}

		if (tileType == 452)
		{
			int num7 = tile.TileFrameX % 54 / 18;
			int num8 = tile.TileFrameY % 54 / 18;
			int num9 = i - num7;
			int num10 = j - num8;
			int num11 = 54;
			if (Main.tile[num9, num10].TileFrameX >= 54)
				num11 = -54;

			for (int m = num9; m < num9 + 3; m++)
			{
				for (int n = num10; n < num10 + 3; n++)
				{
					//Wiring.SkipWire(m, n);
					Main.tile[m, n].TileFrameX = (short)(Main.tile[m, n].TileFrameX + num11);
				}
			}

			NetMessage.SendTileSquare(-1, num9 + 1, num10 + 1, 3);
			return;
		}

		if (tileType == 411)
		{
			int num12 = tile.TileFrameX % 36 / 18;
			int num13 = tile.TileFrameY % 36 / 18;
			int num14 = i - num12;
			int num15 = j - num13;
			int num16 = 36;
			if (Main.tile[num14, num15].TileFrameX >= 36)
				num16 = -36;

			for (int num17 = num14; num17 < num14 + 2; num17++)
			{
				for (int num18 = num15; num18 < num15 + 2; num18++)
				{
					//Wiring.SkipWire(num17, num18);
					Main.tile[num17, num18].TileFrameX = (short)(Main.tile[num17, num18].TileFrameX + num16);
				}
			}

			NetMessage.SendTileSquare(-1, num14, num15, 2, 2);
			return;
		}

		if (tileType == 356)
		{
			int num19 = tile.TileFrameX % 36 / 18;
			int num20 = tile.TileFrameY % 54 / 18;
			int num21 = i - num19;
			int num22 = j - num20;
			for (int num23 = num21; num23 < num21 + 2; num23++)
			{
				for (int num24 = num22; num24 < num22 + 3; num24++)
				{
					//Wiring.SkipWire(num23, num24);
				}
			}

			if (!Main.fastForwardTimeToDawn && Main.sundialCooldown == 0)
				Main.Sundialing();

			NetMessage.SendTileSquare(-1, num21, num22, 2, 2);
			return;
		}

		if (tileType == 663)
		{
			int num25 = tile.TileFrameX % 36 / 18;
			int num26 = tile.TileFrameY % 54 / 18;
			int num27 = i - num25;
			int num28 = j - num26;
			for (int num29 = num27; num29 < num27 + 2; num29++)
			{
				for (int num30 = num28; num30 < num28 + 3; num30++)
				{
					//Wiring.SkipWire(num29, num30);
				}
			}

			if (!Main.fastForwardTimeToDusk && Main.moondialCooldown == 0)
				Main.Moondialing();

			NetMessage.SendTileSquare(-1, num27, num28, 2, 2);
			return;
		}

		if (tileType == 425)
		{
			int num31 = tile.TileFrameX % 36 / 18;
			int num32 = tile.TileFrameY % 36 / 18;
			int num33 = i - num31;
			int num34 = j - num32;
			for (int num35 = num33; num35 < num33 + 2; num35++)
			{
				for (int num36 = num34; num36 < num34 + 2; num36++)
				{
					//Wiring.SkipWire(num35, num36);
				}
			}

			if (Main.AnnouncementBoxDisabled)
				return;

			Color pink = Color.Pink;
			int num37 = Sign.ReadSign(num33, num34, CreateIfMissing: false);
			if (num37 == -1 || Main.sign[num37] == null || string.IsNullOrWhiteSpace(Main.sign[num37].text))
				return;

			if (Main.AnnouncementBoxRange == -1)
			{
				if (Main.netMode == NetmodeID.SinglePlayer)
					Main.NewTextMultiline(Main.sign[num37].text, force: false, pink, 460);
				else if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.SmartTextMessage, -1, -1, NetworkText.FromLiteral(Main.sign[num37].text), 255, (int)pink.R, (int)pink.G, (int)pink.B, 460);
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
			{
				if (Main.player[Main.myPlayer].Distance(new Vector2(num33 * 16 + 16, num34 * 16 + 16)) <= (float)Main.AnnouncementBoxRange)
					Main.NewTextMultiline(Main.sign[num37].text, force: false, pink, 460);
			}
			else
			{
				if (Main.netMode != NetmodeID.Server)
					return;

				for (int num38 = 0; num38 < 255; num38++)
				{
					if (Main.player[num38].active && Main.player[num38].Distance(new Vector2(num33 * 16 + 16, num34 * 16 + 16)) <= (float)Main.AnnouncementBoxRange)
						NetMessage.SendData(MessageID.SmartTextMessage, num38, -1, NetworkText.FromLiteral(Main.sign[num37].text), 255, (int)pink.R, (int)pink.G, (int)pink.B, 460);
				}
			}

			return;
		}

		if (tileType == 405)
		{
			Wiring.ToggleFirePlace(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			return;
		}

		if (tileType == 209)
		{
			int num39 = tile.TileFrameX % 72 / 18;
			int num40 = tile.TileFrameY % 54 / 18;
			int num41 = i - num39;
			int num42 = j - num40;
			int num43 = tile.TileFrameY / 54;
			int num44 = tile.TileFrameX / 72;
			int num45 = -1;
			if (num39 == 1 || num39 == 2)
				num45 = num40;

			int num46 = 0;
			if (num39 == 3)
				num46 = -54;

			if (num39 == 0)
				num46 = 54;

			if (num43 >= 8 && num46 > 0)
				num46 = 0;

			if (num43 == 0 && num46 < 0)
				num46 = 0;

			bool flag = false;
			if (num46 != 0)
			{
				for (int num47 = num41; num47 < num41 + 4; num47++)
				{
					for (int num48 = num42; num48 < num42 + 3; num48++)
					{
						//Wiring.SkipWire(num47, num48);
						Main.tile[num47, num48].TileFrameY = (short)(Main.tile[num47, num48].TileFrameY + num46);
					}
				}

				flag = true;
			}

			if ((num44 == 3 || num44 == 4) && (num45 == 0 || num45 == 1))
			{
				num46 = ((num44 == 3) ? 72 : (-72));
				for (int num49 = num41; num49 < num41 + 4; num49++)
				{
					for (int num50 = num42; num50 < num42 + 3; num50++)
					{
						//Wiring.SkipWire(num49, num50);
						Main.tile[num49, num50].TileFrameX = (short)(Main.tile[num49, num50].TileFrameX + num46);
					}
				}

				flag = true;
			}

			if (flag)
				NetMessage.SendTileSquare(-1, num41, num42, 4, 3);

			if (num45 != -1)
			{
				bool flag2 = true;
				if ((num44 == 3 || num44 == 4) && num45 < 2)
					flag2 = false;

				if (Wiring.CheckMech(num41, num42, 30) && flag2)
					WorldGen.ShootFromCannon(num41, num42, num43, num44 + 1, 0, 0f, Main.myPlayer, fromWire: true);
			}

			return;
		}

		if (tileType == 212)
		{
			int num51 = tile.TileFrameX % 54 / 18;
			int num52 = tile.TileFrameY % 54 / 18;
			int num53 = i - num51;
			int num54 = j - num52;
			int num55 = tile.TileFrameX / 54;
			int num56 = -1;
			if (num51 == 1)
				num56 = num52;

			int num57 = 0;
			if (num51 == 0)
				num57 = -54;

			if (num51 == 2)
				num57 = 54;

			if (num55 >= 1 && num57 > 0)
				num57 = 0;

			if (num55 == 0 && num57 < 0)
				num57 = 0;

			bool flag3 = false;
			if (num57 != 0)
			{
				for (int num58 = num53; num58 < num53 + 3; num58++)
				{
					for (int num59 = num54; num59 < num54 + 3; num59++)
					{
						//Wiring.SkipWire(num58, num59);
						Main.tile[num58, num59].TileFrameX = (short)(Main.tile[num58, num59].TileFrameX + num57);
					}
				}

				flag3 = true;
			}

			if (flag3)
				NetMessage.SendTileSquare(-1, num53, num54, 3, 3);

			if (num56 != -1 && Wiring.CheckMech(num53, num54, 10))
			{
				float num60 = 12f + (float)Main.rand.Next(450) * 0.01f;
				float num61 = Main.rand.Next(85, 105);
				float num62 = Main.rand.Next(-35, 11);
				int type2 = 166;
				int damage = 0;
				float knockBack = 0f;
				Vector2 vector = new((num53 + 2) * 16 - 8, (num54 + 2) * 16 - 8);
				if (tile.TileFrameX / 54 == 0)
				{
					num61 *= -1f;
					vector.X -= 12f;
				}
				else
				{
					vector.X += 12f;
				}

				float num63 = num61;
				float num64 = num62;
				float num65 = (float)Math.Sqrt(num63 * num63 + num64 * num64);
				num65 = num60 / num65;
				num63 *= num65;
				num64 *= num65;
				Projectile.NewProjectile(Wiring.GetProjectileSource(num53, num54), vector.X, vector.Y, num63, num64, type2, damage, knockBack, Main.myPlayer);
			}

			return;
		}

		if (tileType == 215)
		{
			Wiring.ToggleCampFire(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			return;
		}

		if (tileType == 130)
		{
			if (Main.tile[i, j - 1] == null || !Main.tile[i, j - 1].HasTile || (!TileID.Sets.BasicChest[Main.tile[i, j - 1].TileType] && !TileID.Sets.BasicChestFake[Main.tile[i, j - 1].TileType] && Main.tile[i, j - 1].TileType != 88))
			{
				tile.TileType = 131;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}

			return;
		}

		if (tileType == 131)
		{
			tile.TileType = 130;
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			return;
		}

		if (tileType == 387 || tileType == 386)
		{
			bool value = tileType == 387;
			int num66 = WorldGen.ShiftTrapdoor(i, j, playerAbove: true).ToInt();
			if (num66 == 0)
				num66 = -WorldGen.ShiftTrapdoor(i, j, playerAbove: false).ToInt();

			if (num66 != 0)
				NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 3 - value.ToInt(), i, j, num66);

			return;
		}

		if (tileType == 389 || tileType == 388)
		{
			bool flag4 = tileType == 389;
			WorldGen.ShiftTallGate(i, j, flag4);
			NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4 + flag4.ToInt(), i, j);
			return;
		}

		if (TileLoader.CloseDoorID(Main.tile[i, j]) >= 0)
		{
			if (WorldGen.CloseDoor(i, j, forced: true))
				NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 1, i, j);

			return;
		}

		if (TileLoader.OpenDoorID(Main.tile[i, j]) >= 0)
		{
			int num67 = 1;
			if (Main.rand.NextBool(2))
				num67 = -1;

			if (!WorldGen.OpenDoor(i, j, num67))
			{
				if (WorldGen.OpenDoor(i, j, -num67))
					NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, i, j, -num67);
			}
			else
			{
				NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, i, j, num67);
			}

			return;
		}

		if (tileType == 216)
		{
			WorldGen.LaunchRocket(i, j, fromWiring: true);
			//Wiring.SkipWire(i, j);
			return;
		}

		if (tileType == 497 || (tileType == 15 && tile.TileFrameY / 40 == 1) || (tileType == 15 && tile.TileFrameY / 40 == 20))
		{
			int num68 = j - tile.TileFrameY % 40 / 18;
			//Wiring.SkipWire(i, num68);
			//Wiring.SkipWire(i, num68 + 1);
			if (Wiring.CheckMech(i, num68, 60))
				Projectile.NewProjectile(Wiring.GetProjectileSource(i, num68), i * 16 + 8, num68 * 16 + 12, 0f, 0f, 733, 0, 0f, Main.myPlayer);

			return;
		}

		switch (tileType)
		{
			case 335:
				{
					int num156 = j - tile.TileFrameY / 18;
					int num157 = i - tile.TileFrameX / 18;
					//Wiring.SkipWire(num157, num156);
					//Wiring.SkipWire(num157, num156 + 1);
					//Wiring.SkipWire(num157 + 1, num156);
					//Wiring.SkipWire(num157 + 1, num156 + 1);
					if (Wiring.CheckMech(num157, num156, 30))
						WorldGen.LaunchRocketSmall(num157, num156, fromWiring: true);

					break;
				}
			case 338:
				{
					int num75 = j - tile.TileFrameY / 18;
					int num76 = i - tile.TileFrameX / 18;
					//Wiring.SkipWire(num76, num75);
					//Wiring.SkipWire(num76, num75 + 1);
					if (!Wiring.CheckMech(num76, num75, 30))
						break;

					bool flag5 = false;
					for (int num77 = 0; num77 < 1000; num77++)
					{
						if (Main.projectile[num77].active && Main.projectile[num77].aiStyle == 73 && Main.projectile[num77].ai[0] == (float)num76 && Main.projectile[num77].ai[1] == (float)num75)
						{
							flag5 = true;
							break;
						}
					}

					if (!flag5)
					{
						int type3 = 419 + Main.rand.Next(4);
						Projectile.NewProjectile(Wiring.GetProjectileSource(num76, num75), num76 * 16 + 8, num75 * 16 + 2, 0f, 0f, type3, 0, 0f, Main.myPlayer, num76, num75);
					}

					break;
				}
			case 235:
				{
					int num107 = i - tile.TileFrameX / 18;
					if (tile.WallType == 87 && (double)j > Main.worldSurface && !NPC.downedPlantBoss)
						break;

					if (Wiring._teleport[0].X == -1f)
					{
						Wiring._teleport[0].X = num107;
						Wiring._teleport[0].Y = j;
						if (tile.IsHalfBlock)
							Wiring._teleport[0].Y += 0.5f;
					}
					else if (Wiring._teleport[0].X != (float)num107 || Wiring._teleport[0].Y != (float)j)
					{
						Wiring._teleport[1].X = num107;
						Wiring._teleport[1].Y = j;
						if (tile.IsHalfBlock)
							Wiring._teleport[1].Y += 0.5f;
					}

					break;
				}
			case int _ when TileID.Sets.Torch[tileType]:
				Wiring.ToggleTorch(i, j, tile, forcedStateWhereTrueIsOn);
				break;
			/*
			case TileID.WireBulb:
				{
					int num78 = Main.tile[i, j].TileFrameX / 18;
					bool flag6 = num78 % 2 >= 1;
					bool flag7 = num78 % 4 >= 2;
					bool flag8 = num78 % 8 >= 4;
					bool flag9 = num78 % 16 >= 8;
					bool flag10 = false;
					short num79 = 0;
					switch (Wiring._currentWireColor)
					{
						case 1:
							num79 = 18;
							flag10 = !flag6;
							break;
						case 2:
							num79 = 72;
							flag10 = !flag8;
							break;
						case 3:
							num79 = 36;
							flag10 = !flag7;
							break;
						case 4:
							num79 = 144;
							flag10 = !flag9;
							break;
					}

					if (flag10)
						tile.TileFrameX += num79;
					else
						tile.TileFrameX -= num79;

					NetMessage.SendTileSquare(-1, i, j);
					break;
				}
			*/
			case TileID.HolidayLights:
				Wiring.ToggleHolidayLight(i, j, tile, forcedStateWhereTrueIsOn);
				break;
			case 244:
				{
					int num131;
					for (num131 = tile.TileFrameX / 18; num131 >= 3; num131 -= 3)
					{
					}

					int num132;
					for (num132 = tile.TileFrameY / 18; num132 >= 3; num132 -= 3)
					{
					}

					int num133 = i - num131;
					int num134 = j - num132;
					int num135 = 54;
					if (Main.tile[num133, num134].TileFrameX >= 54)
						num135 = -54;

					for (int num136 = num133; num136 < num133 + 3; num136++)
					{
						for (int num137 = num134; num137 < num134 + 2; num137++)
						{
							//Wiring.SkipWire(num136, num137);
							Main.tile[num136, num137].TileFrameX = (short)(Main.tile[num136, num137].TileFrameX + num135);
						}
					}

					NetMessage.SendTileSquare(-1, num133, num134, 3, 2);
					break;
				}
			case 565:
				{
					int num98;
					for (num98 = tile.TileFrameX / 18; num98 >= 2; num98 -= 2)
					{
					}

					int num99;
					for (num99 = tile.TileFrameY / 18; num99 >= 2; num99 -= 2)
					{
					}

					int num100 = i - num98;
					int num101 = j - num99;
					int num102 = 36;
					if (Main.tile[num100, num101].TileFrameX >= 36)
						num102 = -36;

					for (int num103 = num100; num103 < num100 + 2; num103++)
					{
						for (int num104 = num101; num104 < num101 + 2; num104++)
						{
							//Wiring.SkipWire(num103, num104);
							Main.tile[num103, num104].TileFrameX = (short)(Main.tile[num103, num104].TileFrameX + num102);
						}
					}

					NetMessage.SendTileSquare(-1, num100, num101, 2, 2);
					break;
				}
			case 42:
				Wiring.ToggleHangingLantern(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 93:
				Wiring.ToggleLamp(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 95:
			case 100:
			case 126:
			case 173:
			case 564:
				Wiring.Toggle2x2Light(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 593:
				{
					//Wiring.SkipWire(i, j);
					short num105 = (short)((Main.tile[i, j].TileFrameX != 0) ? (-18) : 18);
					Main.tile[i, j].TileFrameX += num105;
					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendTileSquare(-1, i, j, 1, 1);

					int num106 = ((num105 > 0) ? 4 : 3);
					Animation.NewTemporaryAnimation(num106, 593, i, j);
					NetMessage.SendTemporaryAnimation(-1, num106, 593, i, j);
					break;
				}
			case 594:
				{
					int num80;
					for (num80 = tile.TileFrameY / 18; num80 >= 2; num80 -= 2)
					{
					}

					num80 = j - num80;
					int num81 = tile.TileFrameX / 18;
					if (num81 > 1)
						num81 -= 2;

					num81 = i - num81;
					//Wiring.SkipWire(num81, num80);
					//Wiring.SkipWire(num81, num80 + 1);
					//Wiring.SkipWire(num81 + 1, num80);
					//Wiring.SkipWire(num81 + 1, num80 + 1);
					short num82 = (short)((Main.tile[num81, num80].TileFrameX != 0) ? (-36) : 36);
					for (int num83 = 0; num83 < 2; num83++)
					{
						for (int num84 = 0; num84 < 2; num84++)
						{
							Main.tile[num81 + num83, num80 + num84].TileFrameX += num82;
						}
					}

					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendTileSquare(-1, num81, num80, 2, 2);

					int num85 = ((num82 > 0) ? 4 : 3);
					Animation.NewTemporaryAnimation(num85, 594, num81, num80);
					NetMessage.SendTemporaryAnimation(-1, num85, 594, num81, num80);
					break;
				}
			case 34:
				Wiring.ToggleChandelier(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 314:
				if (Wiring.CheckMech(i, j, 5))
					Minecart.FlipSwitchTrack(i, j);
				break;
			case 33:
			case 49:
			case 174:
			case 372:
			case 646:
				Wiring.ToggleCandle(i, j, tile, forcedStateWhereTrueIsOn);
				break;
			case 92:
				Wiring.ToggleLampPost(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 137:
				{
					int num138 = tile.TileFrameY / 18;
					Vector2 vector3 = Vector2.Zero;
					float speedX = 0f;
					float speedY = 0f;
					int num139 = 0;
					int damage3 = 0;
					switch (num138)
					{
						case 0:
						case 1:
						case 2:
						case 5:
							if (Wiring.CheckMech(i, j, 200))
							{
								int num147 = ((tile.TileFrameX == 0) ? (-1) : ((tile.TileFrameX == 18) ? 1 : 0));
								int num148 = ((tile.TileFrameX >= 36) ? ((tile.TileFrameX >= 72) ? 1 : (-1)) : 0);
								vector3 = new Vector2(i * 16 + 8 + 10 * num147, j * 16 + 8 + 10 * num148);
								float num149 = 3f;
								if (num138 == 0)
								{
									num139 = 98;
									damage3 = 20;
									num149 = 12f;
								}

								if (num138 == 1)
								{
									num139 = 184;
									damage3 = 40;
									num149 = 12f;
								}

								if (num138 == 2)
								{
									num139 = 187;
									damage3 = 40;
									num149 = 5f;
								}

								if (num138 == 5)
								{
									num139 = 980;
									damage3 = 30;
									num149 = 12f;
								}

								speedX = (float)num147 * num149;
								speedY = (float)num148 * num149;
							}
							break;
						case 3:
							{
								if (!Wiring.CheckMech(i, j, 300))
									break;

								int num142 = 200;
								for (int num143 = 0; num143 < 1000; num143++)
								{
									if (Main.projectile[num143].active && Main.projectile[num143].type == num139)
									{
										float num144 = (new Vector2(i * 16 + 8, j * 18 + 8) - Main.projectile[num143].Center).Length();
										num142 = ((!(num144 < 50f)) ? ((!(num144 < 100f)) ? ((!(num144 < 200f)) ? ((!(num144 < 300f)) ? ((!(num144 < 400f)) ? ((!(num144 < 500f)) ? ((!(num144 < 700f)) ? ((!(num144 < 900f)) ? ((!(num144 < 1200f)) ? (num142 - 1) : (num142 - 2)) : (num142 - 3)) : (num142 - 4)) : (num142 - 5)) : (num142 - 6)) : (num142 - 8)) : (num142 - 10)) : (num142 - 15)) : (num142 - 50));
									}
								}

								if (num142 > 0)
								{
									num139 = 185;
									damage3 = 40;
									int num145 = 0;
									int num146 = 0;
									switch (tile.TileFrameX / 18)
									{
										case 0:
										case 1:
											num145 = 0;
											num146 = 1;
											break;
										case 2:
											num145 = 0;
											num146 = -1;
											break;
										case 3:
											num145 = -1;
											num146 = 0;
											break;
										case 4:
											num145 = 1;
											num146 = 0;
											break;
									}

									speedX = (float)(4 * num145) + (float)Main.rand.Next(-20 + ((num145 == 1) ? 20 : 0), 21 - ((num145 == -1) ? 20 : 0)) * 0.05f;
									speedY = (float)(4 * num146) + (float)Main.rand.Next(-20 + ((num146 == 1) ? 20 : 0), 21 - ((num146 == -1) ? 20 : 0)) * 0.05f;
									vector3 = new Vector2(i * 16 + 8 + 14 * num145, j * 16 + 8 + 14 * num146);
								}

								break;
							}
						case 4:
							if (Wiring.CheckMech(i, j, 90))
							{
								int num140 = 0;
								int num141 = 0;
								switch (tile.TileFrameX / 18)
								{
									case 0:
									case 1:
										num140 = 0;
										num141 = 1;
										break;
									case 2:
										num140 = 0;
										num141 = -1;
										break;
									case 3:
										num140 = -1;
										num141 = 0;
										break;
									case 4:
										num140 = 1;
										num141 = 0;
										break;
								}

								speedX = 8 * num140;
								speedY = 8 * num141;
								damage3 = 60;
								num139 = 186;
								vector3 = new Vector2(i * 16 + 8 + 18 * num140, j * 16 + 8 + 18 * num141);
							}
							break;
					}

					switch (num138)
					{
						case -10:
							if (Wiring.CheckMech(i, j, 200))
							{
								int num154 = -1;
								if (tile.TileFrameX != 0)
									num154 = 1;

								speedX = 12 * num154;
								damage3 = 20;
								num139 = 98;
								vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
								vector3.X += 10 * num154;
								vector3.Y += 2f;
							}
							break;
						case -9:
							if (Wiring.CheckMech(i, j, 200))
							{
								int num150 = -1;
								if (tile.TileFrameX != 0)
									num150 = 1;

								speedX = 12 * num150;
								damage3 = 40;
								num139 = 184;
								vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
								vector3.X += 10 * num150;
								vector3.Y += 2f;
							}
							break;
						case -8:
							if (Wiring.CheckMech(i, j, 200))
							{
								int num155 = -1;
								if (tile.TileFrameX != 0)
									num155 = 1;

								speedX = 5 * num155;
								damage3 = 40;
								num139 = 187;
								vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
								vector3.X += 10 * num155;
								vector3.Y += 2f;
							}
							break;
						case -7:
							{
								if (!Wiring.CheckMech(i, j, 300))
									break;

								num139 = 185;
								int num151 = 200;
								for (int num152 = 0; num152 < 1000; num152++)
								{
									if (Main.projectile[num152].active && Main.projectile[num152].type == num139)
									{
										float num153 = (new Vector2(i * 16 + 8, j * 18 + 8) - Main.projectile[num152].Center).Length();
										num151 = ((!(num153 < 50f)) ? ((!(num153 < 100f)) ? ((!(num153 < 200f)) ? ((!(num153 < 300f)) ? ((!(num153 < 400f)) ? ((!(num153 < 500f)) ? ((!(num153 < 700f)) ? ((!(num153 < 900f)) ? ((!(num153 < 1200f)) ? (num151 - 1) : (num151 - 2)) : (num151 - 3)) : (num151 - 4)) : (num151 - 5)) : (num151 - 6)) : (num151 - 8)) : (num151 - 10)) : (num151 - 15)) : (num151 - 50));
									}
								}

								if (num151 > 0)
								{
									speedX = (float)Main.rand.Next(-20, 21) * 0.05f;
									speedY = 4f + (float)Main.rand.Next(0, 21) * 0.05f;
									damage3 = 40;
									vector3 = new Vector2(i * 16 + 8, j * 16 + 16);
									vector3.Y += 6f;
									Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), (int)vector3.X, (int)vector3.Y, speedX, speedY, num139, damage3, 2f, Main.myPlayer);
								}

								break;
							}
						case -6:
							if (Wiring.CheckMech(i, j, 90))
							{
								speedX = 0f;
								speedY = 8f;
								damage3 = 60;
								num139 = 186;
								vector3 = new Vector2(i * 16 + 8, j * 16 + 16);
								vector3.Y += 10f;
							}
							break;
					}

					if (num139 != 0)
						Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), (int)vector3.X, (int)vector3.Y, speedX, speedY, num139, damage3, 2f, Main.myPlayer);

					break;
				}
			case 443:
				GeyserTrap(i, j);
				break;
			case 531:
				{
					int num126 = tile.TileFrameX / 36;
					int num127 = tile.TileFrameY / 54;
					int num128 = i - (tile.TileFrameX - num126 * 36) / 18;
					int num129 = j - (tile.TileFrameY - num127 * 54) / 18;
					if (Wiring.CheckMech(num128, num129, 900))
					{
						Vector2 vector2 = new Vector2(num128 + 1, num129) * 16f;
						vector2.Y += 28f;
						int num130 = 99;
						int damage2 = 70;
						float knockBack2 = 10f;
						if (num130 != 0)
							Projectile.NewProjectile(Wiring.GetProjectileSource(num128, num129), (int)vector2.X, (int)vector2.Y, 0f, 0f, num130, damage2, knockBack2, Main.myPlayer);
					}

					break;
				}
			case 35:
			case 139:
			case int _ when TileLoader.IsModMusicBox(tile):
				WorldGen.SwitchMB(i, j);
				break;
			case 207:
				WorldGen.SwitchFountain(i, j);
				break;
			case 410:
			case 480:
			case 509:
			case 657:
			case 658:
				WorldGen.SwitchMonolith(i, j);
				break;
			case 455:
				BirthdayParty.ToggleManualParty();
				break;
			case 141:
				WorldGen.KillTile(i, j, fail: false, effectOnly: false, noItem: true);
				NetMessage.SendTileSquare(-1, i, j);
				Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), i * 16 + 8, j * 16 + 8, 0f, 0f, 108, 500, 10f, Main.myPlayer);
				break;
			case 210:
				WorldGen.ExplodeMine(i, j, fromWiring: true);
				break;
			case 142:
			case 143:
				{
					int num92 = j - tile.TileFrameY / 18;
					int num93 = tile.TileFrameX / 18;
					if (num93 > 1)
						num93 -= 2;

					num93 = i - num93;
					//Wiring.SkipWire(num93, num92);
					//Wiring.SkipWire(num93, num92 + 1);
					//Wiring.SkipWire(num93 + 1, num92);
					//Wiring.SkipWire(num93 + 1, num92 + 1);
					if (tileType == 142)
					{
						for (int num94 = 0; num94 < 4; num94++)
						{
							if (Wiring._numInPump >= 19)
								break;

							int num95;
							int num96;
							switch (num94)
							{
								case 0:
									num95 = num93;
									num96 = num92 + 1;
									break;
								case 1:
									num95 = num93 + 1;
									num96 = num92 + 1;
									break;
								case 2:
									num95 = num93;
									num96 = num92;
									break;
								default:
									num95 = num93 + 1;
									num96 = num92;
									break;
							}

							Wiring._inPumpX[Wiring._numInPump] = num95;
							Wiring._inPumpY[Wiring._numInPump] = num96;
							Wiring._numInPump++;
						}

						break;
					}

					for (int num97 = 0; num97 < 4; num97++)
					{
						if (Wiring._numOutPump >= 19)
							break;

						int num95;
						int num96;
						switch (num97)
						{
							case 0:
								num95 = num93;
								num96 = num92 + 1;
								break;
							case 1:
								num95 = num93 + 1;
								num96 = num92 + 1;
								break;
							case 2:
								num95 = num93;
								num96 = num92;
								break;
							default:
								num95 = num93 + 1;
								num96 = num92;
								break;
						}

						Wiring._outPumpX[Wiring._numOutPump] = num95;
						Wiring._outPumpY[Wiring._numOutPump] = num96;
						Wiring._numOutPump++;
					}

					break;
				}
			case 105:
				{
					int num108 = j - tile.TileFrameY / 18;
					int num109 = tile.TileFrameX / 18;
					int num110 = 0;
					while (num109 >= 2)
					{
						num109 -= 2;
						num110++;
					}

					num109 = i - num109;
					num109 = i - tile.TileFrameX % 36 / 18;
					num108 = j - tile.TileFrameY % 54 / 18;
					int num111 = tile.TileFrameY / 54;
					num111 %= 3;
					num110 = tile.TileFrameX / 36 + num111 * 55;
					//Wiring.SkipWire(num109, num108);
					//Wiring.SkipWire(num109, num108 + 1);
					//Wiring.SkipWire(num109, num108 + 2);
					//Wiring.SkipWire(num109 + 1, num108);
					//Wiring.SkipWire(num109 + 1, num108 + 1);
					//Wiring.SkipWire(num109 + 1, num108 + 2);
					int num112 = num109 * 16 + 16;
					int num113 = (num108 + 3) * 16;
					int num114 = -1;
					int num115 = -1;
					bool flag11 = true;
					bool flag12 = false;
					switch (num110)
					{
						case 5:
							num115 = 73;
							break;
						case 13:
							num115 = 24;
							break;
						case 30:
							num115 = 6;
							break;
						case 35:
							num115 = 2;
							break;
						case 51:
							num115 = Utils.SelectRandom(Main.rand, new short[2] {
							299,
							538
						});
							break;
						case 52:
							num115 = 356;
							break;
						case 53:
							num115 = 357;
							break;
						case 54:
							num115 = Utils.SelectRandom(Main.rand, new short[2] {
							355,
							358
						});
							break;
						case 55:
							num115 = Utils.SelectRandom(Main.rand, new short[2] {
							367,
							366
						});
							break;
						case 56:
							num115 = Utils.SelectRandom(Main.rand, new short[5] {
							359,
							359,
							359,
							359,
							360
						});
							break;
						case 57:
							num115 = 377;
							break;
						case 58:
							num115 = 300;
							break;
						case 59:
							num115 = Utils.SelectRandom(Main.rand, new short[2] {
							364,
							362
						});
							break;
						case 60:
							num115 = 148;
							break;
						case 61:
							num115 = 361;
							break;
						case 62:
							num115 = Utils.SelectRandom(Main.rand, new short[3] {
							487,
							486,
							485
						});
							break;
						case 63:
							num115 = 164;
							flag11 &= NPC.MechSpawn(num112, num113, 165);
							break;
						case 64:
							num115 = 86;
							flag12 = true;
							break;
						case 65:
							num115 = 490;
							break;
						case 66:
							num115 = 82;
							break;
						case 67:
							num115 = 449;
							break;
						case 68:
							num115 = 167;
							break;
						case 69:
							num115 = 480;
							break;
						case 70:
							num115 = 48;
							break;
						case 71:
							num115 = Utils.SelectRandom(Main.rand, new short[3] {
							170,
							180,
							171
						});
							flag12 = true;
							break;
						case 72:
							num115 = 481;
							break;
						case 73:
							num115 = 482;
							break;
						case 74:
							num115 = 430;
							break;
						case 75:
							num115 = 489;
							break;
						case 76:
							num115 = 611;
							break;
						case 77:
							num115 = 602;
							break;
						case 78:
							num115 = Utils.SelectRandom(Main.rand, new short[6] {
							595,
							596,
							599,
							597,
							600,
							598
						});
							break;
						case 79:
							num115 = Utils.SelectRandom(Main.rand, new short[2] {
							616,
							617
						});
							break;
						case 80:
							num115 = Utils.SelectRandom(Main.rand, new short[2] {
							671,
							672
						});
							break;
						case 81:
							num115 = 673;
							break;
						case 82:
							num115 = Utils.SelectRandom(Main.rand, new short[2] {
							674,
							675
						});
							break;
					}

					if (num115 != -1 && Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, num115) && flag11)
					{
						if (!flag12 || !Collision.SolidTiles(num109 - 2, num109 + 3, num108, num108 + 2))
						{
							num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113, num115);
						}
						else
						{
							Vector2 position = new Vector2(num112 - 4, num113 - 22) - new Vector2(10f);
							Utils.PoofOfSmoke(position);
							NetMessage.SendData(MessageID.PoofOfSmoke, -1, -1, null, (int)position.X, position.Y);
						}
					}

					if (num114 <= -1)
					{
						switch (num110)
						{
							case 4:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 1))
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, 1);
								break;
							case 7:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 49))
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112 - 4, num113 - 6, 49);
								break;
							case 8:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 55))
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, 55);
								break;
							case 9:
								{
									int type4 = 46;
									if (BirthdayParty.PartyIsUp)
										type4 = 540;

									if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, type4))
										num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, type4);

									break;
								}
							case 10:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 21))
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113, 21);
								break;
							case 16:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 42))
								{
									if (!Collision.SolidTiles(num109 - 1, num109 + 1, num108, num108 + 1))
									{
										num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, 42);
										break;
									}

									Vector2 position3 = new Vector2(num112 - 4, num113 - 22) - new Vector2(10f);
									Utils.PoofOfSmoke(position3);
									NetMessage.SendData(MessageID.PoofOfSmoke, -1, -1, null, (int)position3.X, position3.Y);
								}
								break;
							case 18:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 67))
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, 67);
								break;
							case 23:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 63))
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, 63);
								break;
							case 27:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 85))
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112 - 9, num113, 85);
								break;
							case 28:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 74))
								{
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, Utils.SelectRandom(Main.rand, new short[3] {
									74,
									297,
									298
								}));
								}
								break;
							case 34:
								{
									for (int num124 = 0; num124 < 2; num124++)
									{
										for (int num125 = 0; num125 < 3; num125++)
										{
											Tile tile2 = Main.tile[num109 + num124, num108 + num125];
											tile2.TileType = 349;
											tile2.TileFrameX = (short)(num124 * 18 + 216);
											tile2.TileFrameY = (short)(num125 * 18);
										}
									}

									Animation.NewTemporaryAnimation(0, 349, num109, num108);
									if (Main.netMode == NetmodeID.Server)
										NetMessage.SendTileSquare(-1, num109, num108, 2, 3);

									break;
								}
							case 42:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 58))
									num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, 58);
								break;
							case 37:
								if (Wiring.CheckMech(num109, num108, 600) && Item.MechSpawn(num112, num113, 58) && Item.MechSpawn(num112, num113, 1734) && Item.MechSpawn(num112, num113, 1867))
									Item.NewItem(Wiring.GetItemSource(num112, num113), num112, num113 - 16, 0, 0, 58);
								break;
							case 50:
								if (Wiring.CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 65))
								{
									if (!Collision.SolidTiles(num109 - 2, num109 + 3, num108, num108 + 2))
									{
										num114 = NPC.NewNPC(Wiring.GetNPCSource(num109, num108), num112, num113 - 12, 65);
										break;
									}

									Vector2 position2 = new Vector2(num112 - 4, num113 - 22) - new Vector2(10f);
									Utils.PoofOfSmoke(position2);
									NetMessage.SendData(MessageID.PoofOfSmoke, -1, -1, null, (int)position2.X, position2.Y);
								}
								break;
							case 2:
								if (Wiring.CheckMech(num109, num108, 600) && Item.MechSpawn(num112, num113, 184) && Item.MechSpawn(num112, num113, 1735) && Item.MechSpawn(num112, num113, 1868))
									Item.NewItem(Wiring.GetItemSource(num112, num113), num112, num113 - 16, 0, 0, 184);
								break;
							case 17:
								if (Wiring.CheckMech(num109, num108, 600) && Item.MechSpawn(num112, num113, 166))
									Item.NewItem(Wiring.GetItemSource(num112, num113), num112, num113 - 20, 0, 0, 166);
								break;
							case 40:
								{
									if (!Wiring.CheckMech(num109, num108, 300))
										break;

									int num120 = 50;

									/*
									int[] array2 = new int[num120];
									*/
									var array2 = new List<int>(num120);

									int num121 = 0;
									for (int num122 = 0; num122 < 200; num122++)
									{
										bool vanillaCanGo = false;
										if (Main.npc[num122].active && (Main.npc[num122].type == NPCID.Merchant || Main.npc[num122].type == NPCID.ArmsDealer || Main.npc[num122].type == NPCID.Guide || Main.npc[num122].type == NPCID.Demolitionist || Main.npc[num122].type == NPCID.Clothier || Main.npc[num122].type == NPCID.GoblinTinkerer || Main.npc[num122].type == NPCID.Wizard || Main.npc[num122].type == NPCID.SantaClaus || Main.npc[num122].type == NPCID.Truffle || Main.npc[num122].type == NPCID.DyeTrader || Main.npc[num122].type == NPCID.Cyborg || Main.npc[num122].type == NPCID.Painter || Main.npc[num122].type == NPCID.WitchDoctor || Main.npc[num122].type == NPCID.Pirate || Main.npc[num122].type == NPCID.TravellingMerchant || Main.npc[num122].type == NPCID.Angler || Main.npc[num122].type == NPCID.DD2Bartender || Main.npc[num122].type == NPCID.TaxCollector || Main.npc[num122].type == NPCID.Golfer))
										{
											vanillaCanGo = true;
										}

										if (Main.npc[num122].active && (NPCLoader.CanGoToStatue(Main.npc[num122], toKingStatue: true) ?? vanillaCanGo))
										{
											/*
											array2[num121] = num122;
											*/
											array2.Add(num122);
											num121++;

											/*
											if (num121 >= num120)
												break;
											*/
										}
									}

									if (num121 > 0)
									{
										int num123 = array2[Main.rand.Next(num121)];
										Main.npc[num123].position.X = num112 - Main.npc[num123].width / 2;
										Main.npc[num123].position.Y = num113 - Main.npc[num123].height - 1;
										NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num123);

										NPCLoader.OnGoToStatue(Main.npc[num123], toKingStatue: true);
									}

									break;
								}
							case 41:
								{
									if (!Wiring.CheckMech(num109, num108, 300))
										break;

									int num116 = 50;

									/*
									int[] array = new int[num116];
									*/
									var array = new List<int>(num116);

									int num117 = 0;
									for (int num118 = 0; num118 < 200; num118++)
									{
										bool vanillaCanGo = false;
										if (Main.npc[num118].active && (Main.npc[num118].type == NPCID.Nurse || Main.npc[num118].type == NPCID.Dryad || Main.npc[num118].type == NPCID.Mechanic || Main.npc[num118].type == NPCID.Steampunker || Main.npc[num118].type == NPCID.PartyGirl || Main.npc[num118].type == NPCID.Stylist || Main.npc[num118].type == NPCID.BestiaryGirl || Main.npc[num118].type == NPCID.Princess))
										{
											vanillaCanGo = true;
										}

										if (Main.npc[num118].active && (NPCLoader.CanGoToStatue(Main.npc[num118], toKingStatue: false) ?? vanillaCanGo))
										{
											/*
											array[num117] = num118;
											*/
											array.Add(num118);
											num117++;

											/*
											if (num117 >= num116)
												break;
											*/
										}
									}

									if (num117 > 0)
									{
										int num119 = array[Main.rand.Next(num117)];
										Main.npc[num119].position.X = num112 - Main.npc[num119].width / 2;
										Main.npc[num119].position.Y = num113 - Main.npc[num119].height - 1;
										NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num119);

										NPCLoader.OnGoToStatue(Main.npc[num119], toKingStatue: false);
									}

									break;
								}
						}
					}

					if (num114 >= 0)
					{
						Main.npc[num114].value = 0f;
						Main.npc[num114].npcSlots = 0f;
						Main.npc[num114].SpawnedFromStatue = true;
						Main.npc[num114].CanBeReplacedByOtherNPCs = true;
					}

					break;
				}
			case 349:
				{
					int num86 = tile.TileFrameY / 18;
					num86 %= 3;
					int num87 = j - num86;
					int num88;
					for (num88 = tile.TileFrameX / 18; num88 >= 2; num88 -= 2)
					{
					}

					num88 = i - num88;
					//Wiring.SkipWire(num88, num87);
					//Wiring.SkipWire(num88, num87 + 1);
					//Wiring.SkipWire(num88, num87 + 2);
					//Wiring.SkipWire(num88 + 1, num87);
					//Wiring.SkipWire(num88 + 1, num87 + 1);
					//Wiring.SkipWire(num88 + 1, num87 + 2);
					short num89 = (short)((Main.tile[num88, num87].TileFrameX != 0) ? (-216) : 216);
					for (int num90 = 0; num90 < 2; num90++)
					{
						for (int num91 = 0; num91 < 3; num91++)
						{
							Main.tile[num88 + num90, num87 + num91].TileFrameX += num89;
						}
					}

					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendTileSquare(-1, num88, num87, 2, 3);

					Animation.NewTemporaryAnimation((num89 <= 0) ? 1 : 0, 349, num88, num87);
					break;
				}
			case 506:
				{
					int num69 = tile.TileFrameY / 18;
					num69 %= 3;
					int num70 = j - num69;
					int num71;
					for (num71 = tile.TileFrameX / 18; num71 >= 2; num71 -= 2)
					{
					}

					num71 = i - num71;
					//Wiring.SkipWire(num71, num70);
					//Wiring.SkipWire(num71, num70 + 1);
					//Wiring.SkipWire(num71, num70 + 2);
					//Wiring.SkipWire(num71 + 1, num70);
					//Wiring.SkipWire(num71 + 1, num70 + 1);
					//Wiring.SkipWire(num71 + 1, num70 + 2);
					short num72 = (short)((Main.tile[num71, num70].TileFrameX >= 72) ? (-72) : 72);
					for (int num73 = 0; num73 < 2; num73++)
					{
						for (int num74 = 0; num74 < 3; num74++)
						{
							Main.tile[num71 + num73, num70 + num74].TileFrameX += num72;
						}
					}

					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendTileSquare(-1, num71, num70, 2, 3);

					break;
				}
			case 546:
				tile.TileType = 557;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
				break;
			case 557:
				tile.TileType = 546;
				// Extra patch context.
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
				break;
		}

		// End of HitWireSingle.
		TileLoader.HitWire(i, j, tileType);
	}
}
