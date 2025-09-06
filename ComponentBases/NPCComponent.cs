using Terraria;
using Terraria.ModLoader;

namespace MoeLib.ComponentBases;

/// <summary>
/// A component for NPC entities which stores its own data and runs logic when enabled.
/// </summary>
public abstract class NPCComponent : GlobalNPC, ILocalizedModType
{
    /// <summary>
    /// Whether or not this component is enabled and logic should run.
    /// </summary>
    public bool Enabled { get; set; }

    public sealed override bool InstancePerEntity { get; } = true;

    public virtual string LocalizationCategory => "NPCComponents";

    /// <summary>
    /// Allows you to make one-time actions for when this component is enabled.
    /// </summary>
    /// <param name="npc"></param>
    public virtual void OnEnabled(NPC npc) { }

    /// <summary>
    /// Allows you to make one-time actions for when this component is enabled.
    /// </summary>
    /// <param name="npc"></param>
    public virtual void OnDisabled(NPC npc) { }
}