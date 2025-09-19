using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace MoeLib;

/// <summary>
/// A simple attribute to attach to any object loaded by JSON via a data struct.
/// <br></br> MoeLib requires your class have a public static readonly string entitled "JSONPath" and a primary constructor with your data struct as the parameter.
/// <br></br> If you'd like to use custom instantiation, create a custom <see cref="ILoadable"/> in your object's class and add the content that way.
/// <br></br> For more information on how JSON instantiation works, please refer to the documentation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class JSONAutoloadAttribute : Attribute { }

public class MoeLib : Mod
{
    /// <summary>
    /// Convenience static accessor.
    /// </summary>
    public static MoeLib? Instance { get; set; }

    /// <summary>
    /// Grabs a particular warn message from the localization files.
    /// </summary>
    /// <param name="warnKey"></param>
    /// <returns></returns>
    public static LocalizedText GetWarn(string warnKey) => Language.GetText($"Mods.MoeLib.Warns.{warnKey}");

    private delegate void orig_Load(Mod self); //Load detour credit to @davidfdev. Thank you!

    public MoeLib()
    {
        MonoModHooks.Add(typeof(Mod).GetMethod(nameof(Load), BindingFlags.Instance | BindingFlags.Public), On_Load);
        Instance = this;
    }

    private void On_Load(orig_Load orig, Mod self)
    {
        orig.Invoke(self);

        var moeLib = ModLoader.Mods.First(x => x.Name == "MoeLib").Code;
        var meth = AssemblyManager.GetLoadableTypes(moeLib)
            .FirstOrDefault(x => x.IsClass && x.FullName == "MoeLib.Helpers.JSONHelpers")?
            .GetMethod("GetJSONData", BindingFlags.Public | BindingFlags.Static, [typeof(Mod), typeof(string)]);

        Type[] loadables = AssemblyManager.GetLoadableTypes(self.Code);

        foreach (var type in loadables.Where(x => x.IsClass && x.CustomAttributes.Any(x => x.AttributeType.FullName == "MoeLib.JSONAutoloadAttribute")))
        {
            self.Logger.Debug(Language.GetText("Mods.MoeLib.Misc.BeginTypeInstantiation").Format(type.Name, type.BaseType?.Name));

            var watch = Stopwatch.StartNew();
            var data = type.GetConstructors()[0].GetParameters()[0].ParameterType;
            string path = (string)type.GetField("JSONPath", BindingFlags.Public | BindingFlags.Static)?.GetValue(null)!;
            if (path is null)
            {
                self.Logger.Warn(GetWarn("JSONPathNotFound").Format(type.Name));
                watch.Stop();
                continue;
            }

            var list = meth?.MakeGenericMethod(data).Invoke(null, [self, path]);
            if (list is null)
            {
                self.Logger.Warn(GetWarn("JSONNotFoundAuto").Format(type.Name, path));
                watch.Stop();
                continue;
            }

            int count = (int)(typeof(Enumerable).GetMember("Count")[0] as MethodInfo)?.MakeGenericMethod(data).Invoke(null, [list])!;
            if (count <= 0)
            {
                self.Logger.Debug(Language.GetText("Mods.MoeLib.Misc.NoJSONDataPresent").Format(type.Name, path));
                watch.Stop();
                continue;
            }

            for (int i = 0; i < count; i++)
            {
                var element = (typeof(Enumerable).GetMember("ElementAt")[0] as MethodInfo)?.MakeGenericMethod(data).Invoke(null, [list, i]);
                var instance = type.GetConstructor([data])?.Invoke([element]) as ModType;

                var entityData = data.GetProperty("EntityData", BindingFlags.Public | BindingFlags.Instance)?.GetValue(data)!;
                if (entityData is not null)
                    instance = TagRegistry.EnableTags(self, (EntityData)entityData, instance!);

                self.AddContent(instance);
                instance = null;
                element = null;
            }

            list = null;
            watch.Stop();
            self.Logger.Debug(Language.GetText("Mods.MoeLib.Misc.EndTypeInstantiation").Format(type.Name, count, watch.Elapsed.TotalMilliseconds));
        }

        moeLib = null;
        meth = null;
    }

    public override void Unload()
    {
        TagRegistry.Unload();
        Instance ??= null;
    }
}