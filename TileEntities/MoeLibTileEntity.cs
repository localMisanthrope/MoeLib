using Terraria.ModLoader;

namespace MoeLib.TileEntities
{
    /// <summary>
    /// A custom tile entity which automatically eliminates itself if the tile is no longer valid for a TE.
    /// </summary>
    public abstract class MoeLibTileEntity : ModTileEntity
    {
        public virtual void SafeUpdate() { }

        public sealed override void Update()
        {
            if (!IsTileValidForEntity(Position.X, Position.Y))
                Kill(Position.X, Position.Y);

            SafeUpdate();

            base.Update();
        }
    }
}