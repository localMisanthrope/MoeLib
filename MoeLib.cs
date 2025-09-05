using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace MoeLib;

[AttributeUsage(AttributeTargets.Class)]
public sealed class JSONAutoloadAttribute : Attribute { }

public class MoeLib : Mod
{
    public static MoeLib Instance { get; set; }

    public static LocalizedText GetWarn(string warnKey) => Language.GetText($"Mods.MoeLib.Warns.{warnKey}");

    private delegate void orig_Load(Mod self); //Load detour credit to @davidfdev. Thank you!

    public MoeLib()
    {
        MonoModHooks.Add(typeof(Mod).GetMethod(nameof(Load), BindingFlags.Instance | BindingFlags.Public), On_Load);
        Instance = this;
    }

    private static void On_Load(orig_Load orig, Mod self)
    {
        orig.Invoke(self);

        var moeLib = ModLoader.Mods.First(x => x.Name == "MoeLib").Code;
        var meth = AssemblyManager.GetLoadableTypes(moeLib)
            .FirstOrDefault(x => x.IsClass && x.FullName == "MoeLib.Helpers.JSONHelpers")
            .GetMethod("GetJSONData", BindingFlags.Public | BindingFlags.Static);

        foreach (var type in AssemblyManager.GetLoadableTypes(self.Code).Where(x => x.IsClass && x.IsSealed && x.CustomAttributes.Any(x => x.AttributeType.FullName == "MoeLib.JSONAutoloadAttribute")))
        {
            self.Logger.Debug($"Begin instantiating type {type.Name} (type of {type.BaseType.Name}).");

            var watch = Stopwatch.StartNew();

            var data = type.GetConstructors()[0].GetParameters()[0].ParameterType;
            string path = (string)type.GetField("JSONPath", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            var list = meth.MakeGenericMethod(data).Invoke(null, [self, path]); //Generate and invoke previous function.
            int count = (int)(typeof(Enumerable).GetMember("Count")[0] as MethodInfo).MakeGenericMethod(data).Invoke(null, [list]);

            for (int i = 0; i < count; i++)
            {
                var element = (typeof(Enumerable).GetMember("ElementAt")[0] as MethodInfo).MakeGenericMethod(data).Invoke(null, [list, i]); //Get the element at the current index.
                var instance = type.GetConstructor([data]).Invoke([element]) as ModType; //Create the instance.
                self.AddContent(instance); //Add the instance.

                instance = null;
            }

            list = null;
            watch.Stop();
            self.Logger.Debug($"Finished instantiating type {type.Name} ({count}); Took {watch.Elapsed.TotalMilliseconds} ms.");
        }

        moeLib = null;
        meth = null;
    }

    public override void Unload()
    {
        Instance ??= null;
    }
}