using MoeLib.ComponentBases;
using System;
using Terraria;
using Terraria.Localization;

namespace MoeLib.Extensions
{
    public static class NPCExtensions
    {
        /// <summary>
        /// Attempts to enable a component instance on the NPC.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="npc"></param>
        /// <param name="init"></param>
        /// <returns>True if the component exists, false otherwise.</returns>
        public static bool TryEnableComponent<T>(this NPC npc, Action<T> init = null) where T : NPCComponent
        {
            if (!npc.TryGetGlobalNPC(out T result))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                return false;
            }

            result.Enabled = true;
            result.OnEnabled(npc);
            init?.Invoke(result);
            return true;
        }

        /// <summary>
        /// Attempts to safely grab the component instance on this NPC.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="npc"></param>
        /// <param name="component"></param>
        /// <returns>True if the instance exists and is enabled, false otherwise.</returns>
        public static bool TryGetComponent<T>(this NPC npc, out T component) where T : NPCComponent
        {
            if (!npc.TryGetGlobalNPC(out T result) || !result.Enabled)
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
        /// <param name="npc"></param>
        /// <returns></returns>
        public static bool HasComponent<T>(this NPC npc) where T : NPCComponent => npc.TryGetGlobalNPC(out T result) && result.Enabled;

        /// <summary>
        /// Attempts to safely disable the component instance on this NPC.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="npc"></param>
        /// <returns>True if the component exists, false otherwise.</returns>
        public static bool TryDisableComponent<T>(this NPC npc) where T : NPCComponent
        {
            if (!npc.TryGetGlobalNPC(out T result))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                return false;
            }

            result.Enabled = false;
            result.OnDisabled(npc);
            return true;
        }
    }
}