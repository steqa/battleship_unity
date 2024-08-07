using System;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Holder;
    [NonSerialized] public string IP;

    private void Awake()
    {
        if (Holder == null)
        {
            Holder = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}