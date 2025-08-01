﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoeLib.Helpers
{
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
    }
}