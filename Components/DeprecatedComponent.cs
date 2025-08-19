using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoeLib.ComponentBases;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace MoeLib.Components
{
    /// <summary>
    /// Allows you to manually deprecate Items and remove their sources from the game. 
    /// <br>Use this <b>only</b> for Items that no longer serve a purpose.</br>
    /// </summary>
    internal sealed class DeprecatedComponent : ItemComponent
    {
        public override void SetDefaults(Item entity)
        {
            if (!Enabled) 
                return;

            entity.accessory = false;
            entity.autoReuse = false;

            entity.axe = 0;
            entity.crit = 0;
            entity.damage = 0;
            entity.DamageType = DamageClass.Default;
            entity.knockBack = 0f;
            entity.pick = 0;
        }

        public override bool CanUseItem(Item item, Player player) => !Enabled;

        public override bool CanPickup(Item item, Player player) => !Enabled;

        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded) => !Enabled;

        public override bool CanRightClick(Item item) => !Enabled;

        public override bool CanReforge(Item item) => !Enabled;

        public override bool CanResearch(Item item) => !Enabled;

        public override bool CanShoot(Item item, Player player) => !Enabled;

        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player) => !Enabled;

        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!Enabled)
                return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);

            item.color = Color.Gray;
            drawColor = Color.Gray;
            return true;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!Enabled)
                return;

            tooltips[0].Text += $" {this.GetLocalization("DeprecatedSuffix").Value}";
            tooltips[0].OverrideColor = Color.Gray;
            tooltips.Add(new(Mod, "DeprecatedLine", this.GetLocalization("DeprecatedLine").Value));
        }
    }
}