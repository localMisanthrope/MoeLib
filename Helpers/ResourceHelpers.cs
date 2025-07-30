using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace MoeLib.Helpers
{
    public class ResourceHelpers
    {
        /// <summary>
        /// A path to a placeholder texture for you to use in place of real textures.
        /// </summary>
        public const string PLACEHOLDER_PATH = "MoeLib/res/textures/placeholder";

        /// <summary>
        /// A placeholder texture for you to use in place of real textures.
        /// </summary>
        public static Texture2D Placeholder => ModContent.Request<Texture2D>(PLACEHOLDER_PATH).Value;

        /// <summary>
        /// Assesses whether a texture exists at the given path and yields the path or a placeholder.
        /// <br>Useful for <see cref="ModTexturedType.Texture"/> Property overrides.</br>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetTexturePath(string path) =>
            ModContent.Request<Texture2D>(path).Value is null ? PLACEHOLDER_PATH : path;

        /// <summary>
        /// Requests the texture at the given path and returns it or a placeholder.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(string path)
        {
            var tex = ModContent.Request<Texture2D>(path).Value;
            return tex is null ? Placeholder : tex;
        }
    }
}