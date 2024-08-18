using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;
    public new string name;
    public bool isRotatable = true;
    [NonSerialized] public int Direction = 1;
    [NonSerialized] public int Length;

    [NonSerialized] public string Uuid;
    [NonSerialized] public int Width;

    [NonSerialized] public int X;
    [NonSerialized] public int Y;

    public void Awake()
    {
        Length = size.y;
        Width = size.x;

        Uuid = Guid.NewGuid().ToString();
    }

    public void SetColorStatus(bool available)
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in childRenderers)
            if (available)
                rend.material.color = EntityController.SuccessColor;
            else
                rend.material.color = EntityController.WarningColor;
    }

    public void SetColorNormal()
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in childRenderers) rend.material.color = EntityController.NormalColor;
    }

    public void Rotate()
    {
        Direction += 1;
        if (Direction > 4) Direction = 1;

        size = new Vector2Int(size.y, size.x);
        transform.Rotate(Vector3.up, 90);

        switch (Direction)
        {
            case 1:
                ShiftInternalObjects(0, Length - 1);
                break;
            case 2:
                ShiftInternalObjects(-(Width - 1), 0);
                break;
            case 3:
                ShiftInternalObjects(0, -(Length - 1));
                break;
            case 4:
                ShiftInternalObjects(Width - 1, 0);
                break;
            default:
                Debug.LogError("Invalid direction");
                break;
        }
    }

    private void ShiftInternalObjects(float shiftX, float shiftY)
    {
        foreach (Transform child in transform) child.localPosition += new Vector3(shiftX, 0, shiftY);
    }
}