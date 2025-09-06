using Terraria;
using Terraria.ModLoader;

namespace MoeLib.ComponentBases;

/// <summary>
/// A component for Item entities which stores its own data and runs logic when enabled.
/// </summary>
public abstract class ItemComponent : GlobalItem, ILocalizedModType
{
    /// <summary>
    /// Whether or not this component is enabled and logic should run.
    /// </summary>
    public bool Enabled { get; set; }

    public sealed override bool InstancePerEntity { get; } = true;

    public virtual string LocalizationCategory => "ItemComponents";

    /// <summary>
    /// Allows you to make one-time actions for when this component is enabled.
    /// </summary>
    /// <param name="item"></param>
    public virtual void OnEnabled(Item item) { }

    /// <summary>
    /// Allows you to make one-time actions for when this component is disabled.
    /// </summary>
    /// <param name="item"></param>
    public virtual void OnDisabled(Item item) { }
}