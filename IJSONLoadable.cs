using MoeLib.Helpers;
using System;
using System.Diagnostics;
using Terraria.ModLoader;

namespace MoeLib;

/// <summary>
/// An interface which allows you to set the JSON file path this entity should be instanced from.
/// </summary>
internal interface IJSONLoadable
{
    public string JSONPath => $"dat/{GetType().Name.ToLower()}";
}

/// <summary>
/// A template JSON Item loader, which automatically scours a given path for JSON data.
/// </summary>
/// <typeparam name="T">The ModItem type.</typeparam>
/// <typeparam name="S">The data struct.</typeparam>
public abstract class JSONItemLoader<T, S>: ILoadable where T: ModItem where S: struct
{
    public void Load(Mod mod)
    {
        int count = 0;
        var watch = Stopwatch.StartNew();

        if (typeof(T) is IJSONLoadable loadable)
        {
            foreach (var data in JSONHelpers.GetJSONData<S>(mod, loadable.JSONPath))
            {
                var instance = Activator.CreateInstance(typeof(T), data) as T;
                mod.AddContent(instance);
                count++;
            }
        }

        watch.Stop();
        mod.Logger.Info($"Finished loading {typeof(T).Name} Instances ({count}); Took {watch.ElapsedMilliseconds} ms.");
    }

    public void Unload() { }
}