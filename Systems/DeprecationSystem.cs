using MoeLib.Extensions;
using MoeLib.Components;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace MoeLib.Systems;

internal sealed class DeprecationSystem : ModSystem
{
    public static HashSet<int> toBeDeprecated = [];

    public override void PostAddRecipes()
    {
        int count = 0;
        var watch = Stopwatch.StartNew();

        foreach (var recipe in Main.recipe.Where(x => x.requiredItem.Any(x => x.IsDeprecated()) || x.createItem.IsDeprecated()))
        {
            recipe.DisableRecipe();
            count++;
        }

        watch.Stop();
        Mod.Logger.Info($"Disabled {count} recipes; took {watch.Elapsed.TotalMilliseconds} ms.");

        base.PostAddRecipes();
    }

    public override void Unload() => toBeDeprecated.Clear();
}

internal sealed class ItemDeprecationManager: GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        if (DeprecationSystem.toBeDeprecated.Contains(entity.type))
            entity.TryEnableComponent<DeprecatedComponent>();

        base.SetDefaults(entity);
    }
}