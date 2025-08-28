using MoeLib.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace MoeLib.NPCs;

internal class DeprecatedLootClearing : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        foreach (var ID in DeprecationSystem.toBeDeprecated)
            if (shop.TryGetEntry(ID, out NPCShop.Entry entry) && !entry.Disabled)
                entry.Disable();
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        foreach (var ID in DeprecationSystem.toBeDeprecated)
            npcLoot.RemoveWhere(x => x is CommonDrop rule && rule.itemId == ID);
    }
}