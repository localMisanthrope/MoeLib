using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoeLib
{
    /// <summary>
    /// Used to create ulterior effects on an entity where a component would be too costly or unnecessary.
    /// <br></br> In JSON, reference and get via the <see cref="ModType.Name"/> of the effect.
    /// <br></br> Do not use for any I/O actions, as this should be handled in the entity or its component.
    /// </summary>
    public abstract class EntityEffect : ModType, ILocalizedModType
    {
        public int Type;

        public LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);

        public LocalizedText Description => this.GetLocalization(nameof(Description), () => "");

        public string LocalizationCategory => "EntityEffects";

        /// <summary>
        /// Used to create special effects on a particular entity.
        /// <br></br> Reminder to use pattern matching if this effect is called on multiple <see cref="Entity"/> types.
        /// </summary>
        /// <param name="actor">The entity to be acting upon.</param>
        public virtual void UpdateEffect<T>(T actor) where T: Entity { }

        protected sealed override void Register()
        {
            ModTypeLookup<EntityEffect>.Register(this);
            Type = EffectRegistry.AddEffect(this);
        }
    }

    internal class EffectRegistry: ILoadable
    {
        private static readonly List<EntityEffect> _registry = [];

        public static int Count => _registry.Count;

        public static int AddEffect(EntityEffect effect)
        {
            int count = Count;
            _registry.Add(effect);
            return count;
        }

        public static EntityEffect GetEffect(int type) => type <= 0 || type > _registry.Count ? null : _registry[type];

        public static EntityEffect GetEffect<T>() where T: EntityEffect => _registry.FirstOrDefault(x => x.GetType() == typeof(T));

        public static EntityEffect GetEffect(string name) => _registry.FirstOrDefault(x => x.Name == name);

        public void Load(Mod mod) { }

        public void Unload() => _registry.Clear();
    }
}