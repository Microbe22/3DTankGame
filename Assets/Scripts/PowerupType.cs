using UnityEngine;

public class PowerupType : MonoBehaviour
{
    public int type; //1 = attack speed, 2 = movement & rotation speed, 3 = healing, 4 = projectile speed
    public GameObject spawner;

    [SerializeField] private Material[] materials;

    private bool triggered = false;
    private void Start()
    {
        GetComponent<Renderer>().material = materials[type];
    }
    private void OnCollisionEnter(Collision other)
    {
        //if this hits a tank ("triggered" exists to prevent triggering the OnCollisionEnter twice in 1 frame)
        if (other.gameObject.CompareTag("Tank") && triggered == false)
        {
            //the tank gains a bonus based on the type of powerup
            var tank = other.gameObject.GetComponent<TankMovement>();
            switch (type)
            {
                case 1:
                    tank.maxShootCooldown /= 2;
                    break;
                case 2:
                    tank.speed *= 2;
                    tank.rotateSpeed *= 2;
                    break;
                case 3:
                    if (tank.health < 3)
                    {
                        tank.TakeDamage(-1);
                    }
                    break;
                case 4:
                    tank.projectileSpeed *= 2;
                    break;
            }
            //powerups last 10 seconds
            tank.powerupTimer = 10;
            //the time for a new powerup to spawn is 20 seconds
            spawner.GetComponent<PowerupSpawning>().timer = 20;
            spawner.GetComponent<PowerupSpawning>().spawned = false;
            triggered = true;
            Destroy(gameObject);
        }
    }
}
