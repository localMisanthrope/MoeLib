﻿using MoeLib.ComponentBases;
using System;
using Terraria;
using Terraria.Localization;

namespace MoeLib.Extensions
{
    public static class ItemExtensions
    {
        /// <summary>
        /// A shorthand which converts the item into a text icon when viewed in chat or tooltips.
        /// </summary>
        /// <param name="item">The item in question.</param>
        /// <param name="stack">The stack count.</param>
        /// <param name="prefix">The prefix this item has.</param>
        /// <returns></returns>
        public static string ToTextIcon(this Item item, int stack = 0, int prefix = -1)
            => $"[i{(prefix != -1 ? $"/p{prefix}" : "")}{(stack > 0 ? $"/s{stack}" : "")}:{(item.ModItem is null ? $"{item.type}" : $"{item.ModItem.FullName}")}]";

        /// <summary>
        /// Attempts to enable a component instance on the Item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="init"></param>
        /// <returns>True if the component exists, false otherwise.</returns>
        public static bool TryEnableComponent<T>(this Item item, Action<T> init = null) where T : ItemComponent
        {
            if (!item.TryGetGlobalItem(out T result))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                return false;
            }

            result.Enabled = true;
            result.OnEnabled(item);
            init?.Invoke(result);
            return true;
        }

        /// <summary>
        /// Allows you to manually enable a component by its name at runtime.
        /// <br></br> <b>DO NOT USE THIS WHILE LOADING;</b> Globals are not fully instantiated at load time!
        /// </summary>
        /// <param name="item"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public static bool TryEnableComponent(this Item item, string componentName)
        {
            foreach (var global in item.Globals)
            {
                if (global is not ItemComponent)
                    continue;

                if (global.Name == componentName)
                {
                    var component = global as ItemComponent;
                    component.Enabled = true;
                    component.OnEnabled(item);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to safely grab the component instance on this Item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="component"></param>
        /// <returns>True if the instance exists and is enabled, false otherwise.</returns>
        public static bool TryGetComponent<T>(this Item item, out T component) where T : ItemComponent
        {
            if (!item.TryGetGlobalItem(out T result) && result.Enabled)
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                component = default;
                return false;
            }

            component = result;
            return true;
        }

        /// <summary>
        /// Checks whether or not the component instance exists and is enabled.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool HasComponent<T>(this Item item) where T : ItemComponent => item.TryGetGlobalItem(out T result) && result.Enabled;

        /// <summary>
        /// Attempts to safely disable the component instance on this Item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>True if the component exists, false otherwise.</returns>
        public static bool TryDisableComponent<T>(this Item item) where T : ItemComponent
        {
            if (!item.TryGetGlobalItem(out T result))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                return false;
            }

            result.Enabled = false;
            result.OnDisabled(item);
            return true;
        }
    }
}