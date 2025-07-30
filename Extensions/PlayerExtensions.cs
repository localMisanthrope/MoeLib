using Microsoft.Xna.Framework;
using MoeLib.ComponentBases;
using MoeLib.Players;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;

namespace MoeLib.Extensions
{
    public static class PlayerExtensions
    {
        /// <summary>
        /// Checks whether or not the player is still.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsStill(this Player player) => player.velocity == Vector2.Zero;

        /// <summary>
        /// Checks whether or not the player is in any liquid. (Does not account for modded liquids, should they exist.)
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool InLiquid(this Player player) => player.wet || player.lavaWet || player.honeyWet || player.shimmerWet;

        /// <summary>
        /// A shorthand to check whether this player has just been hit.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool JustHit(this Player player) => player.GetHitManager().JustHit;

        /// <summary>
        /// A shorthand to get the player's respective hit manager for hit data.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static HitManager GetHitManager(this Player player) => player.TryGetModPlayer(out HitManager manager) ? manager : null;

        /// <summary>
        /// Consumes a set amount of an item from the player's inventory.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        public static void ConsumeFromInventory(this Player player, int type, int amount)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (amount <= 0)
                    break;

                if (player.inventory[i].type == type)
                {
                    if (amount > player.inventory[i].stack)
                    {
                        amount -= player.inventory[i].stack;
                        player.inventory[i].stack = 0;
                        player.inventory[i].TurnToAir();
                    }
                    else
                    {
                        player.inventory[i].stack -= amount;
                        amount = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of surrounding Players within a specified range.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="range"></param>
        /// <param name="count">Caps the search to an amount of Players.</param>
        /// <returns></returns>
        public static List<Player> GetSurroundingPlayers(this Player player, float range, int count = -1)
        {
            List<Player> enumer = [];
            int counter = 0;
            bool doCount = count > -1;

            foreach (var other in Main.ActivePlayers)
            {
                if (other.WithinRange(player.Center, range))
                {
                    if (doCount)
                        counter++;

                    enumer.Add(other);
                }

                if (doCount && counter >= count)
                    break;
            }

            return enumer;
        }

        /// <summary>
        /// Gets a list of surrounding NPCs within a specified range.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="range"></param>
        /// <param name="count">Caps the search to an amount of NPCs.</param>
        /// <returns></returns>
        public static List<NPC> GetSurroundingNPCs(this Player player, float range, int count = -1)
        {
            List<NPC> enumer = [];
            int counter = 0;
            bool doCount = count > -1;

            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.WithinRange(player.Center, range))
                {
                    if (doCount)
                        counter++;

                    enumer.Add(npc);
                }

                if (doCount && counter >= count)
                    break;
            }

            return enumer;
        }

        /// <summary>
        /// Gets the closest Player within the world.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Player GetClosestPlayer(this Player player)
        {
            Player closest = null;
            float closestDist = float.PositiveInfinity;

            foreach (var other in Main.ActivePlayers)
            {
                if (other == player)
                    continue;

                var newDist = other.DistanceSQ(player.Center);
                if (newDist < closestDist)
                {
                    closestDist = newDist;
                    closest = other;
                }
            }

            return closest;
        }

        /// <summary>
        /// Gets the closest NPC within the world.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="excludeFriendlies">Whether or not to exclude friendly (Town NPCs, Critters, etc.) from the search. True by default.</param>
        /// <returns></returns>
        public static NPC GetClosestNPC(this Player player, bool excludeFriendlies = true)
        {
            NPC closest = null;
            float closestDist = float.PositiveInfinity;

            foreach (var npc in Main.ActiveNPCs)
            {
                if (excludeFriendlies && (npc.friendly || npc.CountsAsACritter))
                    continue;

                var newDist = npc.DistanceSQ(player.Center);
                if (newDist < closestDist)
                {
                    closestDist = newDist;
                    closest = npc;
                }
            }

            return closest;
        }

        /// <summary>
        /// Attempts to enable a component instance on the Player.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="player"></param>
        /// <param name="init"></param>
        /// <returns>True if the component exists, false otherwise.</returns>
        public static bool TryEnableComponent<T>(this Player player, Action<T> init = null) where T : PlayerComponent
        {
            if (!player.TryGetModPlayer(out T result))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                return false;
            }

            result.Enabled = true;
            result.OnEnabled();
            init?.Invoke(result);
            return true;
        }

        /// <summary>
        /// Attempts to safely grab the component instance on this Player.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="player"></param>
        /// <param name="component"></param>
        /// <returns>True if the instance exists and is enabled, false otherwise.</returns>
        public static bool TryGetComponent<T>(this Player player, out T component) where T : PlayerComponent
        {
            if (!player.TryGetModPlayer(out T result) && result.Enabled)
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
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool HasComponent<T>(this Player player) where T : PlayerComponent => player.TryGetModPlayer(out T result) && result.Enabled;

        /// <summary>
        /// Attempts to safely disable the component instance on this Player.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="player"></param>
        /// <returns>True if the component exists, false otherwise.</returns>
        public static bool TryDisableComponent<T>(this Player player) where T : PlayerComponent
        {
            if (!player.TryGetModPlayer(out T result))
            {
                MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.ComponentNotFound").Format(typeof(T).Name));
                return false;
            }

            result.Enabled = false;
            result.OnDisabled();
            return true;
        }
    }
}