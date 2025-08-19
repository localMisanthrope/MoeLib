namespace MoeLib.ComponentBases;

using Terraria;
using Terraria.ModLoader;

public abstract class ProjectileComponent : GlobalProjectile, ILocalizedModType
{
    public bool Enabled { get; set; }

    public sealed override bool InstancePerEntity { get; } = true;

    public string LocalizationCategory => "ProjectileComponents";

    public virtual void OnEnabled(Projectile projectile) { }

    public virtual void OnDisabled(Projectile projectile) { }
}