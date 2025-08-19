namespace MoeLib.Helpers;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ObjectData;

internal class WorldGenHelpers
{
    public static bool IsMultiTile(int i, int j)
    {
        var tile = Main.tile[i, j];

        if (!tile.HasTile)
            return false;

        return TileObjectData.GetTileData(tile) != null;
    }

    /// <summary>
    /// Incrementally attempts to find a coordinate on the surface from a given x value.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Vector2 GetSurfaceCoord(int x)
    {
        int y = 0;
        while (y < Main.worldSurface)
        {
            if (WorldGen.SolidTile(x, y))
                break;

            y++;
        }

        return new(x, y);
    }

    public static int TileCounter(ushort[] tileIDs)
    {
        //Run tile scanner.
        //Gather tile counts.
        //Summate tile counts.

        return 0;
    }
}