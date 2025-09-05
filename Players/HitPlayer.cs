using Terraria;
using Terraria.ModLoader;

namespace MoeLib.Players;

public class HitPlayer : ModPlayer
{
    /// <summary>
    /// Whether the player's just been hit.
    /// </summary>
    internal bool JustHit => _hitByNPC || _hitByProjectile;

    /// <summary>
    /// Whether the player's just been hit by an NPC.
    /// </summary>
    internal bool HitByNPC => _hitByNPC;

    private bool _hitByNPC;

    /// <summary>
    /// Whether the player's just been hit by a Projectile.
    /// </summary>
    internal bool HitByProjectile => _hitByProjectile;

    private bool _hitByProjectile;

    /// <summary>
    /// How long it's been (in ticks) since the player was last hit by anything.
    /// </summary>
    internal int TimeSinceLastHit => _timeSinceLastHit;

    private int _timeSinceLastHit;

    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        _hitByNPC = true;
        _timeSinceLastHit = 0;
    }

    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
    {
        _hitByProjectile = true;
        _timeSinceLastHit = 0;
    }

    public override void PreUpdate()
    {
        _hitByNPC = false;
        _hitByProjectile = false;
        _timeSinceLastHit++;
    }

    public override void Load()
    {
        _hitByNPC = false;
        _hitByProjectile = false;
        _timeSinceLastHit = 0;

        base.Load();
    }
}