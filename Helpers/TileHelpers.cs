using MoeLib.ComponentBases;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;

namespace MoeLib.Helpers;

public class TileHelpers
{
    /// <summary>
    /// Unused. Do not call. Does nothing.
    /// </summary>
    /// <param name="tileType"></param>
    public static void EnableTileComponents(int tileType)
    {
        //Automatically enable the tile to use TileComponentContainerTE when created.
        //Perhaps condense this to a general "EnableTileEntity" function?
    }

    /// <summary>
    /// Attempts to add a component instance to the tile at the given position.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="i">The x position in the world of this tile.</param>
    /// <param name="j">The y position in the world of this tile.</param>
    /// <param name="component">The component.</param>
    /// <returns>True if the component exists, doesn't already exist on the tile, and the tile is valid for components; false otherwise.</returns>
    public static bool TryAddComponent<T>(int i, int j, T component) where T : TileComponent
    {
        if (!TileEntity.TryGet(i, j, out TileComponentContainerTE entity))
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("TileEntityNotFound").Format(typeof(TileComponentContainerTE).Name));
            return false;
        }

        if (HasComponent<T>(i, j))
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("ComponentDuplicate").Format(typeof(T).Name));
            return false;
        }

        component.Position = entity.Position.ToPoint(); //Perhaps handle within Init()?
        component.Init();
        entity.components?.Add(component);
        return true;
    }

    /// <summary>
    /// Attempts to add a component instance to the tile at the given position.
    /// </summary>
    /// <param name="i">The x position in the world of this tile.</param>
    /// <param name="j">The y position in the world of this tile.</param>
    /// <param name="componentType">The ID of the component.</param>
    /// <param name="init">An optional initializer for extraneous data. Normally you should use <see cref="TileComponent.Init()"/>.</param>
    /// <returns>True if the component exists, doesn't already exist on the tile, and the tile is valid for components; false otherwise.</returns>
    public static bool TryAddComponent(int i, int j, int componentType, Action<TileComponent>? init = null)
    {
        if (!TileEntity.TryGet(i, j, out TileComponentContainerTE entity))
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("TileEntityNotFound").Format(typeof(TileComponentContainerTE).Name));
            return false;
        }

        if (HasComponent(i, j, componentType))
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("ComponentDuplicate").Format(TileComponentRegistry.GetTileComponent(componentType)?.Name));
            return false;
        }

        var component = TileComponentRegistry.GetTileComponent(componentType);
        component?.Init();
        init?.Invoke(component!);
        entity.components?.Add(component!);
        return true;
    }

    /// <summary>
    /// Attempts to get the component instance from the tile at the given coordinates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="i">The x coordinate.</param>
    /// <param name="j">The y coordinate.</param>
    /// <param name="result"></param>
    /// <returns>True if the component exists and is present on the tile; otherwise false.</returns>
    public static bool TryGetComponent<T>(int i, int j, out T? result) where T : TileComponent
    {
        if (!TileEntity.TryGet(i, j, out TileComponentContainerTE entity))
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("TileEntityNotFound").Format(typeof(TileComponentContainerTE).Name));
            result = null;
            return false;
        }

        var component = entity.components?.First(x => x.GetType() == typeof(T));

        if (component is null)
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("ComponentNotFound").Format(typeof(T).Name));
            result = null;
            return false;
        }

        result = (T)component;
        return true;
    }

    /// <summary>
    /// Attempts to get the component instance from the tile at the given coordinates.
    /// </summary>
    /// <param name="i">The x coordinate.</param>
    /// <param name="j">The y coordinate.</param>
    /// <param name="componentType">The ID of the component.</param>
    /// <param name="result"></param>
    /// <returns>True if the component exists and is present on the tile; otherwise false.</returns>
    public static bool TryGetComponent(int i, int j, int componentType, out TileComponent? result)
    {
        if (!TileEntity.TryGet(i, j, out TileComponentContainerTE entity))
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("TileEntityNotFound").Format(typeof(TileComponentContainerTE).Name));
            result = null;
            return false;
        }

        var component = entity.components?.First(x => x.ID == componentType);

        if (component is null)
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("ComponentNotFound").Format(TileComponentRegistry.GetTileComponent(componentType)?.Name));
            result = null;
            return false;
        }

        result = component;
        return true;
    }

    /// <summary>
    /// Checks whether or not the component exists and is enabled at the given tile coordinates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public static bool HasComponent<T>(int i, int j) where T : TileComponent
        => TileEntity.TryGet(i, j, out TileComponentContainerTE entity) && entity.components?.FirstOrDefault(x => x.GetType() == typeof(T)) is not null;

    /// <summary>
    /// Checks whether or not the component exists and is enabled at the given tile coordinates.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="componentType"></param>
    /// <returns></returns>
    public static bool HasComponent(int i, int j, int componentType)
        => TileEntity.TryGet(i, j, out TileComponentContainerTE entity) && entity.components?.FirstOrDefault(x => x.ID == componentType) is not null;

    /// <summary>
    /// Attempts to remove a component at the given tile coordinates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public static bool RemoveComponent<T>(int i, int j) where T : TileComponent
    {
        if (!TileEntity.TryGet(i, j, out TileComponentContainerTE entity))
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("TileEntityNotFound").Format(typeof(TileComponentContainerTE).Name));
            return false;
        }

        if (!TryGetComponent(i, j, out T? result))
        {
            MoeLib.Instance?.Logger.Warn(MoeLib.GetWarn("ComponentNotFound").Format(typeof(T).Name));
            return false;
        }

        entity.components?.Remove(result!);
        return true;
    }
}