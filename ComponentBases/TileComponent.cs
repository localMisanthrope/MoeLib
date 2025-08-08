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
    public abstract class TileComponent: ModType
    {
        public int ID { get; private set; }

        public Tile Owner => Main.tile[Position];

        public Point Position;

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

    public class TileComponentRegistry: ILoadable
    {
        private static readonly List<TileComponent> _registry = [];

        public static int Count => _registry.Count;

        public static int Add(TileComponent component)
        {
            int count = Count;
            _registry.Add(component);
            return count;
        }

        public static TileComponent Get(int ID) => ID < 0 || ID > _registry.Count ? null : _registry[ID];

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
}