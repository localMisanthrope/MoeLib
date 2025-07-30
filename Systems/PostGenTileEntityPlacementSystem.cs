using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace MoeLib.Systems
{
    internal class PostGenTileEntityPlacementSystem : ModSystem
    {
        public static List<ModTileEntity> postGenTileEntities;

        public static LocalizedText PostGenTileEntitiesPassMessage { get; set; }

        public override void SetStaticDefaults() => Language.GetText(Mod.GetLocalizationKey($"GenPass.{nameof(PostGenTileEntitiesPassMessage)}"));

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            if (postGenTileEntities is not null)
                tasks.Add(new PostGenerationTileEntitiesGenPass("Post Generation Tile Entities", 1f));

            base.ModifyWorldGenTasks(tasks, ref totalWeight);
        }

        public override void Load() => postGenTileEntities = [];

        public override void Unload() => postGenTileEntities = null;
    }

    internal class PostGenerationTileEntitiesGenPass(string name, float loadWeight) : GenPass(name, loadWeight)
    {
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = PostGenTileEntityPlacementSystem.PostGenTileEntitiesPassMessage.Value;

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    foreach (var entity in PostGenTileEntityPlacementSystem.postGenTileEntities)
                    {
                        if (entity.IsTileValidForEntity(x, y) && TileObjectData.IsTopLeft(Main.tile[x, y]))
                            entity.Place(x, y);
                    }
                }
            }
        }
    }
}