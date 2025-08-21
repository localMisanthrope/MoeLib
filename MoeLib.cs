namespace MoeLib;

using Terraria.Localization;
using Terraria.ModLoader;

public class MoeLib : Mod
{
    public static MoeLib Instance => ModContent.GetInstance<MoeLib>();

    public static LocalizedText GetWarn(string warnKey) => Language.GetText($"Mods.MoeLib.Warns.{warnKey}");

    public override void PostSetupContent()
    {
        Instance.Logger.Info($"This mod was made with MoeLib v{Version}!");

        base.PostSetupContent();
    }
}