using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;
    public string name;
    
    [NonSerialized] public int lenght;
    [NonSerialized] public int width;
    [NonSerialized] public int direction = 1;

    [NonSerialized] public int x;
    [NonSerialized] public int y;
    
    public void Awake()
    {
        lenght = size.y;
        width = size.x;
    }
    
    public void SetColorStatus(bool available)
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in childRenderers)
        {
            if (available)
            {
                rend.material.color = Color.green;
            }
            else
            {
                rend.material.color = Color.red;
            }
        }
    }

    public void SetColorNormal()
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in childRenderers)
        {
            rend.material.color = Color.white;
        }
    }
}
