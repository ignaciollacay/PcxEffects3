using Pcx;
using UnityEngine;

public class PointCloud : MonoBehaviour
{
    public BakedPointCloud bakedPointCloud;
    public bool automaticSettings = true;
    private string Name { get => GetPointCloudName(); set => SetPointCloudName(); }

    private string GetPointCloudName()
    {
        if (bakedPointCloud != null)
            return bakedPointCloud.name;
        else
            return name;
    }

    private void OnValidate()
    {
        if (automaticSettings)
        {
            SetPointCloudName();
            SetProperties();
        }
    }

    [ContextMenu("Set Name")]
    public void SetPointCloudName()
    {
        if (bakedPointCloud != null)
        {
            name = Name;
        }
    }

    [ContextMenu("Set VFX Properties")]
    public void SetProperties()
    {
        if (TryGetComponent(out PointCloudBinder pointCloudBinder))
        {
            pointCloudBinder.SetVFXProperties();
        }
    }
}