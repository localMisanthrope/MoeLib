using MoeLib.Components;
using MoeLib.Extensions;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace MoeLib.NPCs
{
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
}