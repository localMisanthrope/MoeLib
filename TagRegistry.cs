using MoeLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace MoeLib;

/// <summary>
/// Basic struct that contains <see cref="Name"/> and <see cref="Tags"/> properties.
/// <br></br> Any tags entered in <see cref="Tags"/> need to be registered to a component with <see cref="TagComponentAttribute"/>.
/// </summary>
public struct EntityData
{
    public string Name { get; set; }

    public string[] Tags { get; set; }
}

/// <summary>
/// Recognized a component as a Tag-based Component.
/// </summary>
/// <param name="name">The Tag this component corresponds to.</param>
[AttributeUsage(AttributeTargets.Class)]
public class TagComponentAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

public static class TagRegistry
{
    private static readonly Dictionary<string, Type> tagComponentRegistry = [];

    private static readonly Dictionary<string, bool[]> tagSetFactoryRegistry = [];

    /// <summary>
    /// Automatically enables tag components for the given type.
    /// <br></br> You should not be using this for vanilla entities, as that can be handled with a GlobalX!
    /// </summary>
    /// <param name="mod">The mod the entity hails from.</param>
    /// <param name="data">The <see cref="EntityData"/> of this entity.</param>
    /// <param name="entity">The instance of the entity.</param>
    /// <returns>The newly constructed entity; otherwise null.</returns>
    public static ModType? EnableTags(Mod mod, EntityData data, ModType entity)
    {
        if (entity is null)
            return null;

        var arr = data.Tags;
        if (arr.Length == 0 || arr is null) {
            if (ModContent.GetInstance<MoeLibConfig>().EnableDevMode)
                mod.Logger.Debug($"Skipped loading Entity Tags for {entity.Name}; No tags present in array.");
            return entity;
        }

        foreach (var tag in arr)
        {
            var component = 
                AssemblyManager.GetLoadableTypes(mod.Code)
                .First(x => x.IsClass && x.CustomAttributes
                    .Any(x => x.AttributeType.FullName == "MoeLib.TagComponentAttribute" && 
                    (string)x.AttributeType.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance)?.GetValue(x)! == tag));

            if (component is null)
                continue;

            MethodInfo? info = null;

            if (entity.GetType() == typeof(ModItem))
            {
                tagSetFactoryRegistry?.TryAdd(tag, ItemID.Sets.Factory.CreateNamedSet(tag).RegisterCustomSet(false));
                info = typeof(ItemExtensions).GetMethod("TryEnableComponent", BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod([component]);
            }

            if (entity.GetType() == typeof(ModNPC))
            {
                tagSetFactoryRegistry?.TryAdd(tag, NPCID.Sets.Factory.CreateNamedSet(tag).RegisterCustomSet(false));
                info = typeof(NPCExtensions).GetMethod("TryEnableComponent", BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod([component]);
            }

            if (entity.GetType() == typeof(ModProjectile))
            {
                tagSetFactoryRegistry?.TryAdd(tag, ProjectileID.Sets.Factory.CreateNamedSet(tag).RegisterCustomSet(false));
                info = typeof(ProjectileExtensions).GetMethod("TryEnableComponent", BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod([component]);
            }

            else
            {
                mod.Logger.Debug($"Failed to instantiate tags on {entity.Name}; Unknown type {entity.GetType()} (only acceptable types are ModItem, ModNPC and ModProjectile!).");
                return entity;
            }

            tagComponentRegistry?.TryAdd(tag, component);
            info?.Invoke(null, [entity]);
        }

        return entity;
    }

    public static bool[] GetSet(string tag) => tagSetFactoryRegistry[tag];

    public static void AddToTagSet(this Item item, string tag)
    {
        if (!tagSetFactoryRegistry.TryGetValue(tag, out var tagSet))
            return;

        if (!tagComponentRegistry.TryGetValue(tag, out var tagComponent))
            return;

        tagSet[item.type] = true;
        var meth = typeof(ItemExtensions).GetMethod("TryEnableComponent", BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod([tagComponent]);
        meth?.Invoke(null, [item]);
    }

    public static void AddToTagSet(this NPC npc, string tag)
    {
        if (!tagSetFactoryRegistry.TryGetValue(tag, out var tagSet))
            return;

        if (!tagComponentRegistry.TryGetValue(tag, out var tagComponent))
            return;

        tagSet[npc.type] = true;
        var meth = typeof(NPCExtensions).GetMethod("TryEnableComponent", BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod([tagComponent]);
        meth?.Invoke(null, [npc]);
    }

    public static void AddToTagSet(this Projectile projectile, string tag)
    {
        if (!tagSetFactoryRegistry.TryGetValue(tag, out var tagSet))
            return;

        if (!tagComponentRegistry.TryGetValue(tag, out var tagComponent))
            return;

        tagSet[projectile.type] = true;
        var meth = typeof(ProjectileExtensions).GetMethod("TryEnableComponent", BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod([tagComponent]);
        meth?.Invoke(null, [projectile]);
    }

    public static bool IsInTagSet(this Item item, string tag) => tagSetFactoryRegistry.TryGetValue(tag, out var arr) && arr[item.type];

    public static bool IsInTagSet(this NPC npc, string tag) => tagSetFactoryRegistry.TryGetValue(tag, out var arr) && arr[npc.type];

    public static bool IsInTagSet(this Projectile projectile, string tag) => tagSetFactoryRegistry.TryGetValue(tag, out var arr) && arr[projectile.type];

    public static void Unload()
    {
        tagComponentRegistry.Clear();
        tagSetFactoryRegistry.Clear();
    }
}