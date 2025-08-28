using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoeLib.Helpers;

public class JSONHelpers
{
    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> of JSON data elements, if the file exists.
    /// </summary>
    /// <typeparam name="T">The struct the data will parse to.</typeparam>
    /// <param name="mod"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetJSONData<T>(Mod mod, string path) where T : struct
    {
        var file = mod.GetFileBytes(path);

        if (file is null)
        {
            MoeLib.Instance.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.JSONNotFound").Format(path));
            return null;
        }

        return JsonConvert.DeserializeObject<IEnumerable<T>>(Encoding.UTF8.GetString(file));
    }

    /// <summary>
    /// Gets a JSON Item reference by its name.
    /// </summary>
    /// <param name="mod"></param>
    /// <param name="name"></param>
    /// <returns>The given type of the Item.</returns>
    public static int GetJSONItemType(Mod mod, string name) => mod.GetContent<ModItem>().First(x => x.Name == name).Type;

    /// <summary>
    /// Gets a JSON Item reference by its name.
    /// </summary>
    /// <param name="mod"></param>
    /// <param name="name"></param>
    /// <returns>The given <see cref="Item"/> of the Item.</returns>
    public static Item GetJSONItem(Mod mod, string name) => mod.GetContent<ModItem>().First(x => x.Name == name).Item;
}