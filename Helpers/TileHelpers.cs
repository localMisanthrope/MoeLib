using MoeLib.ComponentBases;
using MoeLib.TileEntities;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;

namespace MoeLib.Helpers
{
    public class TileHelpers
    {
        /// <summary>
        /// Attempts to add a component instance to the tile at the given position.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i">The x position in the world of this tile.</param>
        /// <param name="j">The y position in the world of this tile.</param>
        /// <param name="component">The component.</param>
        /// <returns>True if the component exists, doesn't already exist on the tile, and the tile is valid for components; false otherwise.</returns>
        public static bool TryAddComponent<T>(int i, int j, T component) where T : TileComponent
        {
            if (!TileEntity.TryGet(i, j, out TileComponentContainerTE entity))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.TileEntityNotFound").Format(typeof(T).Name));
                return false;
            }

            if (HasComponent<T>(i, j))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentDuplicate").Format(typeof(T).Name));
                return false;
            }

            component.Position = entity.Position.ToPoint(); //Perhaps handle within Init()?
            component.Init();
            entity.components.Add(component);
            return true;
        }

        /// <summary>
        /// Attempts to get the component instance from the tile at the given coordinates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i">The x coordinate.</param>
        /// <param name="j">The y coordinate.</param>
        /// <param name="result"></param>
        /// <returns>True if the component exists and is present on the tile; otherwise false.</returns>
        public static bool TryGetComponent<T>(int i, int j, out T result) where T : TileComponent
        {
            if (!TileEntity.TryGet(i, j, out TileComponentContainerTE entity))
            {
                result = null;
                return false;
            }

            var component = entity.components.First(x => x.GetType() == typeof(T));

            if (component is null)
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                result = null;
                return false;
            }

            result = (T)component;
            return true;
        }

        public static bool HasComponent<T>(int i, int j) where T : TileComponent
            => TileEntity.TryGet(i, j, out TileComponentContainerTE entity) && entity.components.FirstOrDefault(x => x.GetType() == typeof(T)) is not null;

        public static bool RemoveComponent<T>(int i, int j) where T : TileComponent
        {
            if (!TileEntity.TryGet(i, j, out TileComponentContainerTE entity))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.TileEntityNotFound").Format(typeof(T).Name));
                return false;
            }

            if (!TryGetComponent(i, j, out T result))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                return false;
            }

            entity.components.Remove(result);
            return true;
        }
    }
}