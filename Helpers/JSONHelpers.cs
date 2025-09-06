using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoeLib.Helpers;

/// <summary>
/// A helper class containing various JSON functions.
/// </summary>
public class JSONHelpers
{
    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> of JSON data elements, if the file exists.
    /// </summary>
    /// <typeparam name="T">The struct the data will parse to.</typeparam>
    /// <param name="mod"></param>
    /// <param name="path"></param>
    /// <returns>The <see cref="IEnumerable{T}"/> of the input JSON file; otherwise null.</returns>
    public static IEnumerable<T>? GetJSONData<T>(Mod mod, string path) where T : struct
    {
        var file = mod.GetFileBytes(path);

        if (file is null)
        {
            MoeLib.Instance?.Logger.Warn(Language.GetText("Mods.MoeLib.Warns.JSONNotFound").Format(path));
            return null;
        }

        return JsonConvert.DeserializeObject<IEnumerable<T>>(Encoding.UTF8.GetString(file))!;
    }

    /// <summary>
    /// Grabs a specific instance of a data struct via the instance's provided <paramref name="internalName"/>.
    /// <br></br> Only use if your data has a public "Name" string property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mod"></param>
    /// <param name="path"></param>
    /// <param name="internalName"></param>
    /// <returns></returns>
    public static T? GetJSONData<T>(Mod mod, string path, string internalName)
        where T : struct
        => GetJSONData<T>(mod, path)?.First(x => (string)x.GetType().GetProperty("Name", BindingFlags.Public)?.GetValue(x)! == internalName);

    /// <summary>
    /// Creates an instance of the provided type <typeparamref name="C"/> using the provided struct <typeparamref name="T"/> and from the provided <paramref name="internalName"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="C"></typeparam>
    /// <param name="mod"></param>
    /// <param name="path"></param>
    /// <param name="internalName"></param>
    /// <returns></returns>
    public static C? GetJSONContent<T, C>(Mod mod, string path, string internalName)
        where T : struct
        where C : ModType
        => typeof(C).GetConstructor([typeof(T)])?.Invoke([GetJSONData<T>(mod, path, internalName)]) as C;

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