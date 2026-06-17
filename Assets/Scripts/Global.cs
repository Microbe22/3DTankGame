using System.Linq;
using UnityEngine;

public class Global : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        var globals = FindObjectsByType<Global>(FindObjectsSortMode.None);
        if (globals.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
