using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{
    [SerializeField] private string input;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;

    private UISwitch UIController;

    public string color;

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
    }
    
    void Update()
    {
        //move
        transform.Translate(speed * Time.deltaTime * new Vector3(moveDir.x, 0, moveDir.y), Space.Self);

        //rotate top (only when standing still)
        if (moveDir.magnitude == 0)
        {
            top.transform.rotation = Quaternion.Euler(0, top.transform.rotation.eulerAngles.y + (rotationDir.x * rotateSpeed * Time.deltaTime), 0);
        }

        //rotate bottom
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

        //if the player has a powerup
        if (powerupTimer > 0)
        {
            //reduce remaining powerup time
            powerupTimer -= Time.deltaTime;
            //if time up
            if (powerupTimer <= 0)
            {
                //reset stats to default
                speed = 5f;
                rotateSpeed = 100;
                maxShootCooldown = 0.5f;
                projectileSpeed = 18f;
            }
        }
    }
    public void OnMove(InputValue context)
    {
        moveDir = context.Get<Vector2>();
    }
    public void OnRotate(InputValue context)
    {
        rotationDir = context.Get<Vector2>();
    }
    public void OnShoot(InputValue context)
    {
        if (shootCooldown <= 0)
        {
            var pewpew = Instantiate(bullet, top.transform.position + top.transform.forward * 1.6f, Quaternion.identity);
            pewpew.GetComponent<Rigidbody>().linearVelocity = top.transform.forward * projectileSpeed;
            pewpew.GetComponent<BulletMove>().damage = projectileDamage;
            pewpew.GetComponent<BulletMove>().lifeTime = 10;
            shootCooldown = maxShootCooldown;
        }
        TakeDamage(1);
    }
    public void TakeDamage(int damage)
    {
        if (UIController.mode == 0)
        {
            health -= damage;
            UIController.SetHearts(color, health);
            if (health <= 0)
            {
                Destroy(gameObject);
                UIController.Switch(1, color);
            }
        }
    }
}
