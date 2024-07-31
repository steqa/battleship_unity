using System;
using UnityEngine;
using Color = UnityEngine.Color;

public class Ship : Entity
{
    private void OnDrawGizmos()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if ((x + y) % 2 == 0) Gizmos.color = new Color(0.08f, 1f, 0f, 0.5f);
                else Gizmos.color = new Color(0.06f, 0.53f, 0f, 0.5f);
                
                Gizmos.DrawCube(transform.position + new Vector3(x , 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
    
    public void Rotate()
    {
        direction += 1;
        if (direction > 4)
        {
            direction = 1;
        }
        
        size = new Vector2Int(size.y, size.x);
        transform.Rotate(Vector3.up, 90);
        
        switch (direction)
        {
            case 1:
                ShiftInternalObjects(0, lenght-1);
                break;
            case 2:
                ShiftInternalObjects(-(width-1), 0);
                break;
            case 3:
                ShiftInternalObjects(0, -(lenght-1));
                break;
            case 4:
                ShiftInternalObjects(width-1, 0);
                break;
            default:
                Debug.LogError("Invalid direction");
                break;
        }
    }
    
    private void ShiftInternalObjects(float shiftX, float shiftY)
    {
        foreach (Transform child in transform)
        {
            child.localPosition += new Vector3(shiftX, 0, shiftY);
        }
    }
}
