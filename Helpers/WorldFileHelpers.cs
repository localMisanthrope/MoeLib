using Terraria;
using Terraria.IO;
using Terraria.Localization;

namespace MoeLib.Helpers;

public class WorldFileHelpers
{
    /// <summary>
    /// Allows you to manually send the player to a provided world.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cloud"></param>
    /// <param name="fromWorld">If the player is already in a world, this should be true. False by default.</param>
    /// <returns>True if the world is valid, otherwise false.</returns>
    public static bool ToWorld(string path, bool cloud = false, bool fromWorld = false)
    {
        var data = new WorldFileData(path, cloud);

        if (!data.IsValid)
        {
            MoeLib.Instance.Logger.Warn(MoeLib.GetWarn("WorldFileInvalid").Format(data.Name));
            return false;
        }

        if (fromWorld)
            WorldGen.SaveAndQuit();

        data.SetAsActive();
        Main.ActiveWorldFileData = data;
        WorldGen.playWorld();
        Main.MenuUI.SetState(null);

        MoeLib.Instance.Logger.Info(Language.GetText("Mods.MoeLib.Misc.ToWorldMessage").Format(Main.LocalPlayer.name, data.Name));

        return true;
    }
}