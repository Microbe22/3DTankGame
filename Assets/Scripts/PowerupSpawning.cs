using UnityEngine;

public class PowerupSpawning : MonoBehaviour
{
    [SerializeField] private GameObject Powerup;

    public GameObject spawn;
    public float timer = 0;
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if ((spawn == null || ReferenceEquals(spawn, null) == true) && timer <= 0)
        {
            spawn = Instantiate(Powerup, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            spawn.GetComponent<PowerupType>().type = Random.Range(1, 5);
            spawn.GetComponent<PowerupType>().spawner = gameObject;
        }
    }
}
