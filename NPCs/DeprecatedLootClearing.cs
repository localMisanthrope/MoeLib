namespace MoeLib.NPCs;

using Terraria;
using Terraria.ModLoader;

internal class DeprecatedLootClearing : GlobalNPC
{
    //To-Do: Resolve item drops and shop items for deprecated items.

    public override void ModifyShop(NPCShop shop)
    {
        base.ModifyShop(shop);
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {

        base.ModifyNPCLoot(npc, npcLoot);
    }
}