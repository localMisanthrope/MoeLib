namespace MoeLib.ComponentBases;

using Terraria;
using Terraria.ModLoader;

/// <summary>
/// A component for Projectiles entities which stores its own data and runs logic when enabled.
/// </summary>
public abstract class ProjectileComponent : GlobalProjectile, ILocalizedModType
{
    /// <summary>
    /// Whether or not this component is enabled and logic should run.
    /// </summary>
    public bool Enabled { get; set; }

    public sealed override bool InstancePerEntity { get; } = true;

    public string LocalizationCategory => "ProjectileComponents";

    /// <summary>
    /// Allows you to make one-time actions for when this component is enabled.
    /// </summary>
    public virtual void OnEnabled(Projectile projectile) { }

    /// <summary>
    /// Allows you to make one-time actions for when this component is disabled.
    /// </summary>
    public virtual void OnDisabled(Projectile projectile) { }
}