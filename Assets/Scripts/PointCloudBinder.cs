using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using Pcx;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(PointCloud), typeof(VisualEffect))]
public class PointCloudBinder : MonoBehaviour
{
    #region Required Components
    private VisualEffect visualEffect => GetComponent<VisualEffect>();
    private BakedPointCloud PointCloud => GetComponent<PointCloud>().bakedPointCloud;
    #endregion

    #region VFXProperies
    [Header("VFX Bindings")]
    public ExposedProperty positionMapProperty = "PositionMap";
    public ExposedProperty colorMapProperty = "ColorMap";
    public ExposedProperty spawnRateProperty = "SpawnRate";
    public ExposedProperty capacityProperty = "Capacity";
    #endregion

    #region Binded Properies
    private int Capacity { get => GetPointCloudCount(); set => SetVFXCapacity(Capacity); }
    public int SpawnRate { get => GetSpawnRate(); private set => SetVFXSpawnRate(SpawnRate); }
    private Texture2D PositionMap { get => GetPointCloudPositionMap(); set => SetVFXPositionMap(PositionMap); }
    private Texture2D ColorMap { get => GetPointCloudColorMap(); set => SetVFXColorMap(ColorMap); }
    #endregion

    private void OnValidate()
    {
        if (IsValid())
            SetVFXProperties();
        else
            Debug.Log("VFX asset is not valid");
    }

    private void Awake()
    {
        UpdateSpawnRate();
    }

    private bool IsValid()
    {
        if (visualEffect.HasInt(spawnRateProperty))
            return true;
        else if (!visualEffect.HasInt(spawnRateProperty))
        {
            Debug.Log("Missing Spawn Rate");
            return false;
        }

        if (visualEffect.HasInt(capacityProperty))
        {
            return true;
        }
        else if (!(visualEffect.HasInt(capacityProperty)))
        {
            Debug.Log("Missing Capacity");
            return false;
        }

        if (visualEffect.HasTexture(positionMapProperty))
            return true;
        else if(!visualEffect.HasTexture(positionMapProperty))
        {
            Debug.Log("Missing Position Map");
            return false;
        }

        if (visualEffect.HasTexture(colorMapProperty))
            return true;
        else if(visualEffect.HasTexture(colorMapProperty))
        {
            Debug.Log("Missing Color Map");
            return false;
        }

        if (PointCloud != null)
            return true;
        else if (PointCloud == null)
        {
            Debug.Log("Missing Point Cloud Component");
            return false;
        }
        else
        {
            return false;
        }
    }

    public void SetVFXProperties()
    {
        SetVFXCapacity(Capacity);
        SetVFXSpawnRate(SpawnRate);
        SetVFXPositionMap(PositionMap);
        SetVFXColorMap(ColorMap);
    }

    #region Capacity
    private int GetPointCloudCount()
    {
        if (PointCloud != null)
            return PointCloud.pointCount;
        else
            return 0;
    }
    private void SetVFXCapacity(int capacity)
    {
        visualEffect.SetInt(capacityProperty, capacity);
    }
    #endregion

    #region SpawnRate
    public void UpdateSpawnRate(VFXOutputEventArgs args)
    {
        GetSpawnRate();
        SetVFXSpawnRate(SpawnRate);
        visualEffect.SendEvent("ConstantSpawn");
    }
    public void UpdateSpawnRate()
    {
        GetSpawnRate();
        SetVFXSpawnRate(SpawnRate);
    }

    private int GetSpawnRate()
    {
        int spawnRate = (int)(Capacity * 0.405f);
        return spawnRate > 0 ? spawnRate : 0;
    }
    public void SetVFXSpawnRate(int spawnRate)
    {
        visualEffect.SetInt(spawnRateProperty, spawnRate);
        //Debug.Log("Max   " + visualEffect.GetInt("Capacity") + "  Alive   " + visualEffect.aliveParticleCount + " / Rate   " + spawnRate);
    }
    #endregion

    #region Textures
    private Texture2D GetPointCloudPositionMap()
    {
        if (PointCloud != null)
            return PointCloud.positionMap;
        else
        {
            Debug.Log("Could not get position map");
            return null;
        }
    }
    private Texture2D GetPointCloudColorMap()
    {
        if (PointCloud != null)
            return PointCloud.colorMap;
        else
        {
            Debug.Log("Could not get color map");
            return null;
        }
    }
    private void SetVFXPositionMap(Texture2D positionMap)
    {
        visualEffect.SetTexture(positionMapProperty, positionMap);
    }
    private void SetVFXColorMap(Texture2D colorMap)
    {
        visualEffect.SetTexture(colorMapProperty, colorMap);
    }
    #endregion   
}