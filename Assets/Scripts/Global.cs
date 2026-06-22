using System.Linq;
using UnityEngine;

public class Global : MonoBehaviour
{
    public bool PvP = false;
    public int players = 0;
    public int currentLevel = 0;
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
