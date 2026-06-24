using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer = 1;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
