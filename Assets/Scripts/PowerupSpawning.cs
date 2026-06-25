using UnityEngine;

public class PowerupSpawning : MonoBehaviour
{
    [SerializeField] private GameObject Powerup;

    public float timer = 0;
    void Update()
    {
        //reduce timer
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        //when time is up
        if (timer <= 0)
        {
            //spawn a random powerup
            var spawn = Instantiate(Powerup, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            spawn.GetComponent<PowerupType>().type = Random.Range(1, 5);
            spawn.GetComponent<PowerupType>().spawner = gameObject;
        }
    }
}
