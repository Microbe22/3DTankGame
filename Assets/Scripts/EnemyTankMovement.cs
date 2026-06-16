using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class EnemyTankMovement : MonoBehaviour
{
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;

    private UISwitch UIController;

    [SerializeField] string enemyType; //0 = stationary, 1 = basic, 2 = mines, 3 = projectile speed

    private Vector2 moveDir;
    public float speed = 5f;

    private Vector2 rotationDir;

    public float rotateSpeed = 100;

    public int health = 3;

    [SerializeField] private GameObject bullet;
    private float shootCooldown = 0;
    public float maxShootCooldown = 0.5f;
    public float projectileSpeed = 18f;
    private int projectileDamage = 1;

    public float powerupTimer = 0;
    void Start()
    {
        UIController = FindFirstObjectByType<UISwitch>();
        UIController.liveEnemies++;
    }
    
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * new Vector3(moveDir.x, 0, moveDir.y), Space.Self);
        if (moveDir.magnitude == 0)
        {
            top.transform.rotation = Quaternion.Euler(0, top.transform.rotation.eulerAngles.y + (rotationDir.x * rotateSpeed * Time.deltaTime), 0);
        }

        var spin = 0;
        if (moveDir.x > 0)
        {
            spin = 90;
        }
        else if (moveDir.x < 0)
        {
            spin = 270;
        }

        if (moveDir.x != 0)
        {
            if (moveDir.y > 0)
            {
                spin = (0 + spin) / 2;
            }
            else if (moveDir.y < 0)
            {
                spin = (180 + spin) / 2;
            }
        }
        else
        {
            if (moveDir.y > 0)
            {
                spin = 0;
            }
            else if (moveDir.y < 0)
            {
                spin = 180;
            }
        }

        if (moveDir.magnitude != 0)
        {
            bottom.transform.rotation = Quaternion.Euler(0, spin, 0);
        }

        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }

        if (powerupTimer > 0)
        {
            powerupTimer -= Time.deltaTime;
            if (powerupTimer <= 0)
            {
                speed = 5f;
                rotateSpeed = 100;
                maxShootCooldown = 0.5f;
                projectileSpeed = 18f;
            }
        }
    }
    private void Shoot(InputAction.CallbackContext context)
    {
        if (shootCooldown <= 0)
        {
            var pewpew = Instantiate(bullet, top.transform.position + top.transform.forward * 1.6f, Quaternion.identity);
            pewpew.GetComponent<Rigidbody>().linearVelocity = top.transform.forward * projectileSpeed;
            pewpew.GetComponent<BulletMove>().damage = projectileDamage;
            pewpew.GetComponent<BulletMove>().lifeTime = 20;
            shootCooldown = maxShootCooldown;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            UIController.liveEnemies--;
            if (UIController.liveEnemies <= 0)
            {
                UIController.NextLevel();
            }
        }
    }
}
