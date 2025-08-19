using Terraria.Localization;
using Terraria.ModLoader;

namespace MoeLib
{
	public class MoeLib : Mod
	{
		public static MoeLib Instance => ModContent.GetInstance<MoeLib>();

        public static LocalizedText GetWarn(string warnKey) => Language.GetText($"Mods.MoeLib.Warns.{warnKey}");

        public override void PostSetupContent()
        {
            Instance.Logger.Info("This mod was made with MoeLib!");

            base.PostSetupContent();
        }
	}
}