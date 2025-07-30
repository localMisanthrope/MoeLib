using MoeLib.Helpers;
using MoeLib.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MoeLib.Tiles
{
    internal class VanillaTileEntityKillGlobalTile : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (PostGenTileEntityPlacementSystem.postGenTileEntities is not null)
            {
                var topLeft = TileObjectData.TopLeft(i, j);

                foreach (var entity in PostGenTileEntityPlacementSystem.postGenTileEntities)
                {
                    if (TileEntity.ByPosition.TryGetValue(topLeft, out var result) && result == entity)
                    {
                        entity.Kill(topLeft.X, topLeft.Y);
                    }
                        
                }
            }

            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }
    }
}
