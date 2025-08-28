using MoeLib.ComponentBases;
using System;
using Terraria;

namespace MoeLib.Extensions;

public static class ProjectileExtensions
{
    /// <summary>
    /// Attempts to enable a component instance on the Projectile.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projectile"></param>
    /// <param name="init"></param>
    /// <returns>True if the component exists, false otherwise.</returns>
    public static bool TryEnableComponent<T>(this Projectile projectile, Action<T> init = null) where T : ProjectileComponent
    {
        if (!projectile.TryGetGlobalProjectile(out T result))
        {
            MoeLib.Instance.Logger.Warn(MoeLib.GetWarn("ComponentNotFound").Format(typeof(T).Name));
            return false;
        }

        result.Enabled = true;
        init?.Invoke(result);
        result.OnEnabled(projectile);
        return true;
    }

    /// <summary>
    /// Attempts to safely grab the component instance on this Projectile.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projectile"></param>
    /// <param name="component"></param>
    /// <returns>True if the instance exists and is enabled, false otherwise.</returns>
    public static bool TryGetComponent<T>(this Projectile projectile, out T component) where T : ProjectileComponent
    {
        if (projectile.TryGetGlobalProjectile(out T result))
        {
            component = result;
            return result.Enabled;
        }

        MoeLib.Instance.Logger.Warn(MoeLib.GetWarn("ComponentNotFound").Format(typeof(T).Name));
        component = default;
        return false;
    }

    /// <summary>
    /// Checks whether or not the component instance exists and is enabled.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projectile"></param>
    /// <returns></returns>
    public static bool HasComponent<T>(this Projectile projectile) where T : ProjectileComponent => projectile.TryGetGlobalProjectile(out T result) && result.Enabled;

    /// <summary>
    /// Attempts to safely disable the component instance on this Projectile.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projectile"></param>
    /// <returns>True if the component exists, false otherwise.</returns>
    public static bool TryDisableComponent<T>(this Projectile projectile) where T : ProjectileComponent
    {
        if (!projectile.TryGetGlobalProjectile(out T result))
        {
            MoeLib.Instance.Logger.Warn(MoeLib.GetWarn("ComponentNotFound").Format(typeof(T).Name));
            return false;
        }

        result.Enabled = false;
        result.OnDisabled(projectile);
        return true;
    }
}