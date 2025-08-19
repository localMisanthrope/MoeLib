using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MoeLib.ComponentBases
{
    /// <summary>
    /// A custom component for tiles which have multiple functions.
    /// </summary>
    public abstract class TileComponent: ModType, ILocalizedModType
    {
        public int ID { get; private set; }

        public Tile Owner => Main.tile[Position];

        public Point Position;

        public string LocalizationCategory => "TileComponents";

        public virtual void Init() { }

        public virtual void Update() { }

        public virtual void LoadData(TagCompound tag) { }

        public virtual void SaveData(TagCompound tag) { }

        protected sealed override void Register()
        {
            ModTypeLookup<TileComponent>.Register(this);
            ID = TileComponentRegistry.Add(this);
        }
    }

    public sealed class TileComponentRegistry: ILoadable
    {
        private static readonly List<TileComponent> _registry = [];

        public static int Count => _registry.Count;

        public static int Add(TileComponent component)
        {
            int count = Count;
            _registry.Add(component);
            return count;
        }

        public static TileComponent GetTileComponent(int ID) => ID < 0 || ID > _registry.Count ? null : _registry[ID];

        public static TileComponent GetTileComponent(string name) => _registry.FirstOrDefault(x => x.Name == name);

        public static int GetType<T>() where T : TileComponent
        {
            var component = _registry.FirstOrDefault(x => x.GetType() == typeof(T));

            if (component is null)
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                return -1;
            }

            return component.ID;
        }

        public void Load(Mod mod) { }

        public void Unload() => _registry.Clear();
    }

    /// <summary>
    /// A container tile entity which stores <see cref="TileComponent"/> instances on a tile.
    /// </summary>
    public sealed class TileComponentContainerTE : ModTileEntity
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
                TileComponent instance = TileComponentRegistry.GetTileComponent(component.GetInt("ID"));
                instance.Position = new Point(component.GetInt("X"), component.GetInt("Y"));

                if (component.ContainsKey("saveData"))
                    instance.LoadData(component.Get<TagCompound>("saveData"));

                components.Add(instance);
            }

            base.LoadData(tag);
        }
    }
}