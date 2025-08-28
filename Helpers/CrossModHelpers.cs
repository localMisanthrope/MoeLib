using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoeLib.Helpers;

public class CrossModHelpers
{
    public static bool GetCrossModEntity<T>(string modName, string entityName, out T result) where T: ModType
    {
        if (!ModLoader.TryGetMod(modName, out var mod))
        {
            result = null;
            return false;
        }

        if (!mod.TryFind(entityName, out T item))
         {
            result = null;
            return false;
        }

        result = item;
        return true;
    }

    public static bool ModifyEntity<T>(string modName, string entityName, Action<T> init = null) where T: ModType
    {
        if (!GetCrossModEntity(modName, entityName, out T item))
            return false;

        init?.Invoke(item);
        MoeLib.Instance.Logger.Info(Language.GetText("Mods.MoeLib.Misc.EntityModified").Format(entityName, modName));
        return true;
    }
}