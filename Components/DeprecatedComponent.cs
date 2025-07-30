using Microsoft.Xna.Framework;
using MoeLib.ComponentBases;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoeLib.Components
{
    /// <summary>
    /// Allows you to manually deprecate Items and remove their sources from the game. 
    /// <br>Use this <b>only</b> for Items that no longer serve a purpose.</br>
    /// </summary>
    internal class DeprecatedComponent : ItemComponent
    {
        public override void SetDefaults(Item entity)
        {
            if (Enabled) //To-Do: Fill out the remaining properties.
            {
                entity.accessory = false;
                entity.autoReuse = false;

                entity.axe = 0;
                entity.damage = -1;
                entity.knockBack = 0f;
                entity.pick = 0;

                entity.color = Color.Gray;
            }

            base.SetDefaults(entity);
        }

        public override bool CanUseItem(Item item, Player player) => !Enabled;

        public override bool CanPickup(Item item, Player player) => !Enabled;

        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded) => !Enabled;

        public override bool CanRightClick(Item item) => !Enabled;

        public override bool CanReforge(Item item) => !Enabled;

        public override bool CanResearch(Item item) => !Enabled;

        public override bool CanShoot(Item item, Player player) => !Enabled;

        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player) => !Enabled;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Enabled)
            {
                tooltips[0].Text += $" {Language.GetTextValue("Mods.MoeLib.ItemComponents.DeprecatedSuffix")}";
                tooltips[0].OverrideColor = Color.Gray;
                tooltips.Add(new(Mod, "DeprecatedLine", Language.GetTextValue("Mods.MoeLib.ItemComponents.DeprecatedLine")));
            }

            base.ModifyTooltips(item, tooltips);
        }
    }
}