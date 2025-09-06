using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoeLib.Helpers;

public class CrossModHelpers
{
    /// <summary>
    /// Grabs an instance of a particular entity by name via the provided <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="modName"></param>
    /// <param name="entityName"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool GetCrossModEntity<T>(string modName, string entityName, out T? result) where T: ModType
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

    /// <summary>
    /// Allows you to directly modify the properties of a particular entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="modName"></param>
    /// <param name="entityName"></param>
    /// <param name="init"></param>
    /// <returns></returns>
    public static bool ModifyEntity<T>(string modName, string entityName, Action<T>? init = null) where T: ModType
    {
        if (!GetCrossModEntity(modName, entityName, out T? item))
            return false;

        init?.Invoke(item!);
        MoeLib.Instance?.Logger.Info(Language.GetText("Mods.MoeLib.Misc.EntityModified").Format(entityName, modName));
        return true;
    }
}