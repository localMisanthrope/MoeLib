namespace MoeLib;

using System.ComponentModel;
using Terraria.ModLoader.Config;

internal class MoeLibConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("DeveloperMode")]
    [DefaultValue(false)]
    public bool EnableDevMode { get; set; }
}