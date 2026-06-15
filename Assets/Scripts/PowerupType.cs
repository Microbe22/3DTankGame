using UnityEngine;

public class PowerupType : MonoBehaviour
{
    public int type; //1 = attack speed, 2 = movement & rotation speed, 3 = healing, 4 = projectile speed
    public GameObject spawner;

    [SerializeField] private Material[] materials;
    private void Start()
    {
        GetComponent<Renderer>().material = materials[type];
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Tank"))
        {
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
            tank.powerupTimer = 10;
            spawner.GetComponent<PowerupSpawning>().spawn = null;
            spawner.GetComponent<PowerupSpawning>().timer = 20;
            Destroy(gameObject);
        }
    }
}
