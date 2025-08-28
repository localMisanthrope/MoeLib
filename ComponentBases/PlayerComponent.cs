using Terraria.ModLoader;

namespace MoeLib.ComponentBases;

/// <summary>
/// A component for Player entities which stores its own data and runs logic when enabled.
/// </summary>
public abstract class PlayerComponent : ModPlayer, ILocalizedModType
{
    /// <summary>
    /// Whether or not this component is enabled and logic should run.
    /// </summary>
    public bool Enabled { get; set; }

    public string LocalizationCategory => "PlayerComponents";

    /// <summary>
    /// Allows you to make one-time actions for when this component is enabled.
    /// </summary>
    public virtual void OnEnabled() { }

    /// <summary>
    /// Allows you to make one-time actions for when this component is disabled.
    /// </summary>
    public virtual void OnDisabled() { }
}