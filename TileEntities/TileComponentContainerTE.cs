using Microsoft.Xna.Framework;
using MoeLib.ComponentBases;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MoeLib.TileEntities
{
    /// <summary>
    /// A container tile entity which stores <see cref="TileComponent"/> instances on a tile.
    /// </summary>
    internal class TileComponentContainerTE : ModTileEntity
    {
        public List<TileComponent> components = [];

        public override bool IsTileValidForEntity(int x, int y)
        {
            var tile = Main.tile[x, y];
            return tile.HasTile;
        }

        public sealed override void Update()
        {
            if (!IsTileValidForEntity(Position.X, Position.Y))
                Kill(Position.X, Position.Y);

            if (components is null || components.Count <= 0)
                return;

            foreach (var component in components)
                component.Update();
        }

        public override void OnKill() => components.Clear();

        public override void SaveData(TagCompound tag)
        {
            var list = new List<TagCompound>();

            var saveData = new TagCompound();

            foreach (var component in components)
            {
                component.SaveData(saveData);

                var data = new TagCompound()
                {
                    ["ID"] = component.ID,
                    ["name"] = component.GetType().Name,
                    ["X"] = component.Position.X,
                    ["Y"] = component.Position.Y
                };

                if (saveData.Count > 0)
                {
                    data["saveData"] = saveData;
                    saveData = [];
                }

                list.Add(data);
            }

            tag["componentData"] = list;

            base.SaveData(tag);
        }

        public override void LoadData(TagCompound tag)
        {
            foreach (var component in tag.GetList<TagCompound>("componentData"))
            {
                TileComponent instance = TileComponentRegistry.Get(component.GetInt("ID"));
                instance.Position = new Point(component.GetInt("X"), component.GetInt("Y"));

                if (component.ContainsKey("saveData"))
                    instance.LoadData(component.Get<TagCompound>("saveData"));

                components.Add(instance);
            }

            base.LoadData(tag);
        }
    }
}