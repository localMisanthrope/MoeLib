namespace MoeLib.Systems;

using global::MoeLib.Components;
using global::MoeLib.Extensions;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

internal class RecipeDeprecationSystem : ModSystem
{
    public override void PostAddRecipes()
    {
        int count = 0;
        var watch = Stopwatch.StartNew();

        foreach (var recipe in Main.recipe.Where(x => x.requiredItem.Any(x => x.HasComponent<DeprecatedComponent>()) || x.createItem.HasComponent<DeprecatedComponent>()))
        {
            recipe.DisableRecipe();
            count++;
        }

        watch.Stop();
        Mod.Logger.Info($"Disabled {count} recipes; took {watch.Elapsed.TotalMilliseconds} ms.");

        base.PostAddRecipes();
    }
}