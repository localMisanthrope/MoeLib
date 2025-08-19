using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MoeLib
{
    internal class MoeLibConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("DeveloperMode")]
        [DefaultValue(false)]
        public bool EnableDevMode { get; set; }
    }
}
