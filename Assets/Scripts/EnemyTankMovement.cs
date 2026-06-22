using UnityEngine;

public class EnemyTankMovement : MonoBehaviour
{
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;

    private UISwitch UIController;

    [SerializeField] int enemyType; //0 = stationary, 1 = basic, 2 = mines, 3 = projectile speed

    private Vector2 moveDir;
    public float speed = 5f;

    private float rotationDir;

    public float rotateSpeed = 100;

    public int health = 1;

    [SerializeField] private GameObject bullet;
    private float shootCooldown = 0;
    public float maxShootCooldown;
    public float projectileSpeed;
    private int projectileDamage;
    private int projectileBounces;

    public float powerupTimer = 0;

    private GameObject target;
    private bool spotted = false;
    void Start()
    {
        UIController = FindFirstObjectByType<UISwitch>();
        UIController.liveEnemies++;
        switch (enemyType)
        {
            case 0:
                maxShootCooldown = 1;
                projectileSpeed = 12;
                projectileDamage = 1;
                projectileBounces = 0;
                break;
        }
    }
    
    void Update()
    {
        switch (enemyType)
        {
            case 0:
                //find nearest player
                TankMovement[] targets = FindObjectsByType<TankMovement>(FindObjectsSortMode.None);
                float distance = 100000;
                foreach (TankMovement tank in targets)
                {
                    if ((tank.transform.position - transform.position).magnitude < distance)
                    {
                        distance = (tank.transform.position - transform.position).magnitude;
                        target = tank.gameObject;
                    }
                }
                var hits = Physics.RaycastAll(transform.position + new Vector3(0, 0.5f, 0), top.transform.forward, 100);
                spotted = false;
                foreach (var hit in hits)
                {
                    if (hit.transform.root.gameObject == target)
                    {
                        if (shootCooldown <= 0)
                        {
                            Shoot();
                        }
                        spotted = true;
                        rotationDir = 0;
                    }
                }

                if (spotted == false)
                {
                    //calculating rotation to target
                    Vector3 direction = transform.position - target.transform.position;
                    float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                    float targetDeg = Quaternion.AngleAxis(angle, Vector3.down).eulerAngles.y - 90;

                    //correction with negative values to create consistency with own rotation
                    if (targetDeg < 0)
                    {
                        targetDeg += 360;
                    }

                    //calculate own rotation
                    float currentDeg = top.transform.rotation.eulerAngles.y;

                    //correction for passing the jump from 359 to 0 degrees
                    if (targetDeg > 270)
                    {
                        if (currentDeg < 90)
                        {
                            currentDeg += 360;
                        }
                    }
                    else if (targetDeg < 90)
                    {
                        if (currentDeg > 270)
                        {
                            targetDeg += 360;
                        }
                    }

                    //determining the direction that should be rotated in
                    if (targetDeg > currentDeg)
                    {
                        rotationDir = 1;
                    }
                    else
                    {
                        rotationDir = -1;
                    }
                }
                
                break;
        }

        transform.Translate(speed * Time.deltaTime * new Vector3(moveDir.x, 0, moveDir.y), Space.Self);
        top.transform.rotation = Quaternion.Euler(0, top.transform.rotation.eulerAngles.y + (rotationDir * rotateSpeed * Time.deltaTime), 0);

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
    }
    private void Shoot()
    {
        var pewpew = Instantiate(bullet, top.transform.position + top.transform.forward * 1.6f, Quaternion.identity);
        pewpew.GetComponent<Rigidbody>().linearVelocity = top.transform.forward * projectileSpeed;
        pewpew.GetComponent<BulletMove>().damage = projectileDamage;
        pewpew.GetComponent<BulletMove>().lifeTime = 20;
        pewpew.GetComponent<BulletMove>().bounces = projectileBounces;
        pewpew.GetComponent<BulletMove>().shooter = "enemy";
        shootCooldown = maxShootCooldown;
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
