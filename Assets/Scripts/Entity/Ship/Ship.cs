using UnityEngine;
using Color = UnityEngine.Color;

public class Ship : Entity
{
    private void OnDrawGizmos()
    {
        for (var x = 0; x < size.x; x++)
        for (var y = 0; y < size.y; y++)
        {
            if ((x + y) % 2 == 0) Gizmos.color = new Color(0.08f, 1f, 0f, 0.5f);
            else Gizmos.color = new Color(0.06f, 0.53f, 0f, 0.5f);

            Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
        }
    }
}