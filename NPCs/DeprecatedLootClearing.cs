namespace MoeLib.NPCs;

using global::MoeLib.Components;
using global::MoeLib.Extensions;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

internal class DeprecatedLootClearing : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.TryGetEntry(ItemID.CopperShortsword, out NPCShop.Entry entry))
            entry.Disable();
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        npcLoot.RemoveWhere(x => (x is CommonDrop rule && new Item(rule.itemId).HasComponent<DeprecatedComponent>()));
    }
}