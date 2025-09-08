using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;

namespace MoeLib.Helpers;

public class WorldGenHelpers
{
    /// <summary>
    /// Checks whether or not the tile at the given coordinates (<paramref name="i"/>, <paramref name="j"/>) is a MultiTile.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns>True if the given coordinates is a MultiTile; false otherwise.</returns>
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
    /// <returns>A <see cref="Vector2"/> containing the coordinates to the surface of the given X.</returns>
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

    /// <summary>
    /// Counts the number of tiles in the world from the input <paramref name="tileIDs"/>.
    /// </summary>
    /// <param name="tileIDs">An array of <see cref="TileID"/>s.</param>
    /// <returns>The amount of tiles of each ID present; -1 otherwise.</returns>
    public static int TileCounter(ushort[] tileIDs)
    {
        //Run tile scanner.
        //Gather tile counts.
        //Summate tile counts.

        return 0;
    }

    /// <summary>
    /// Counts the number of tiles in the world from the input <paramref name="tileIDs"/> within a given <paramref name="area"/>.
    /// </summary>
    /// <param name="tileIDs">An array of <see cref="TileID"/>s.</param>
    /// <param name="area">The area from which to search within.</param>
    /// <returns>The amount of tiles of each ID present within the given area; -1 otherwise.</returns>
    public static int TileCounter(ushort[] tileIDs, Rectangle area)
    {
        return 0;
    }
}