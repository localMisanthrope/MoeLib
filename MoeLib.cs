using System;
using System.Linq;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace MoeLib;

public class MoeLib : Mod
{
    public static MoeLib Instance => ModContent.GetInstance<MoeLib>();

    public static LocalizedText GetWarn(string warnKey) => Language.GetText($"Mods.MoeLib.Warns.{warnKey}");

    private delegate void orig_Load(Mod self); //Implementation of Detour hook provided by Daviddev. Thank you!

    static MoeLib()
    {
        MonoModHooks.Add(typeof(Mod).GetMethod("Load", BindingFlags.Public | BindingFlags.Instance), On_Load);
    }

    private static void On_Load(orig_Load orig, Mod self)
    {
        if (self.Name == "ModLoader" || self is MoeLib)
            return;

        var moeLib = ModLoader.Mods.First(x => x.Name == "MoeLib").Code;
        var meth = AssemblyManager.GetLoadableTypes(moeLib)
            .FirstOrDefault(x => x.IsClass && x.FullName == "MoeLib.Helpers.JSONHelpers")
            .GetMethod("GetJSONData", BindingFlags.Public | BindingFlags.Static);

        foreach (var type in AssemblyManager.GetLoadableTypes(self.Code))
        {
            if (type.IsClass && type.IsSealed && type.GetProperty("Data") is not null)
            {
                var data = type.GetProperty("Data").PropertyType;
                var path = type.GetField("JSONPath", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                var list = meth.MakeGenericMethod(data).Invoke(null, [self, path]); //Generate and invoke previous function.
                var count = (int)(typeof(Enumerable).GetMember("Count")[0] as MethodInfo).MakeGenericMethod(data).Invoke(null, [list]);

                for (int i = 0; i < count; i++)
                {
                    var element = (typeof(Enumerable).GetMember("ElementAt")[0] as MethodInfo).MakeGenericMethod(data).Invoke(null, [list, i]); //Get the element at the current index.
                    var instance = Activator.CreateInstance(type, element) as ModType; //Create the instance.
                    self.AddContent(instance); //Add the instance.
                }
                self.Logger.Debug($"Finished instantiating type {type.Name} ({count}); ");
            }
        }
    }

    public override void PostSetupContent()
    {
        Instance.Logger.Info($"Mods present made with MoeLib v{Version}!");

        base.PostSetupContent();
    }
}