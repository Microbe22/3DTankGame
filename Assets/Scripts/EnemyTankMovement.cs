using System;
using UnityEngine;

public class EnemyTankMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;

    private UISwitch UIController;
    private Global global;

    [SerializeField] private string color;

    [SerializeField] int enemyType; //0 = stationary, 1 = waypoints

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

    private RaycastHit[] hits;

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypoint = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UIController = FindFirstObjectByType<UISwitch>();
        global = FindFirstObjectByType<Global>();

        //the amount of living enemies increases
        UIController.liveEnemies++;

        switch (enemyType)
        {
            case 0: //stationary
                maxShootCooldown = 2;
                projectileSpeed = 12;
                projectileDamage = 1;
                projectileBounces = 0;
                break;
            case 1: //waypoints
                maxShootCooldown = 1.5f;
                projectileSpeed = 12;
                projectileDamage = 1;
                projectileBounces = 0;
                speed = 5f;
                break;
        }
    }
    
    void Update()
    {
        switch (enemyType)
        {
            case 0:
                target = FindNearestPlayer();

                //shoot raycast forward
                hits = Physics.RaycastAll(transform.position + new Vector3(0, 0.5f, 0), top.transform.forward, 100);
                spotted = false;
                foreach (var hit in hits)
                {
                    if (hit.transform.root.gameObject == target)
                    {
                        //if raycast hits target
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
                    //only rotate if not pointed at target
                    rotationDir = RotateTowards(target);
                }

                break;
            case 1:
                target = FindNearestPlayer();

                //shoot raycast forward
                hits = Physics.RaycastAll(transform.position + new Vector3(0, 0.5f, 0), top.transform.forward, 100);
                spotted = false;
                foreach (var hit in hits)
                {
                    if (hit.transform.root.gameObject == target)
                    {
                        //if raycast hits target
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
                    //only rotate if not pointed at target
                    rotationDir = RotateTowards(target);
                }

                //move to next waypoint
                rb.linearVelocity = (waypoints[currentWaypoint].transform.position - transform.position).normalized * speed;
                //if at next waypoint
                if ((transform.position - waypoints[currentWaypoint].transform.position).magnitude < 0.05f)
                {
                    //set next waypoint to the one after
                    currentWaypoint++;
                    //at last waypoint, loop back to beginning
                    if (currentWaypoint > waypoints.Length - 1)
                    {
                        currentWaypoint = 0;
                    }
                }

                RotateBottom();
                break;
        }

        //rotate top
        top.transform.rotation = Quaternion.Euler(0, top.transform.rotation.eulerAngles.y + (rotationDir * rotateSpeed * Time.deltaTime), 0);

        //reduce shot cooldown
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }
    private void Shoot()
    {
        var pewpew = Instantiate(bullet, top.transform.position + top.transform.forward * 2f, Quaternion.Euler(-90, top.transform.rotation.eulerAngles.y, 0));
        pewpew.GetComponent<Rigidbody>().linearVelocity = top.transform.forward * projectileSpeed;
        pewpew.GetComponent<BulletMove>().damage = projectileDamage;
        pewpew.GetComponent<BulletMove>().lifeTime = 20;
        pewpew.GetComponent<BulletMove>().bounces = projectileBounces;
        pewpew.GetComponent<BulletMove>().shooter = "enemy";
        shootCooldown = maxShootCooldown;
    }
    public void TakeDamage(int damage, int color)
    {
        //reduce health, if dead: reduceamount of living enemies. if amount is 0, go to next level / results scene
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            global.IncreaseScore(color);
            UIController.liveEnemies--;
            if (UIController.liveEnemies <= 0)
            {
                UIController.NextLevel();
            }
        }
    }
    private GameObject FindNearestPlayer()
    {
        TankMovement[] targets = FindObjectsByType<TankMovement>(FindObjectsSortMode.None);
        float distance = 100000;
        GameObject nearest = null;
        foreach (TankMovement tank in targets)
        {
            if ((tank.transform.position - transform.position).magnitude < distance)
            {
                distance = (tank.transform.position - transform.position).magnitude;
                nearest = tank.gameObject;
            }
        }
        return nearest;
    }
    private int RotateTowards(GameObject target)
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
            return 1;
        }
        else
        {
            return -1;
        }
    }
    private void RotateBottom()
    {
        var spin = 0;
        if (rb.linearVelocity.x > 0.1f)
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                spin = 45;
            }
            else if (rb.linearVelocity.y < -0.1f)
            {
                spin = 135;
            }
            else
            {
                spin = 90;
            }
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                spin = 315;
            }
            else if (rb.linearVelocity.y < -0.1f)
            {
                spin = 225;
            }
            else
            {
                spin = 270;
            }
        }
        else
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                spin = 0;
            }
            else if (rb.linearVelocity.y < -0.1f)
            {
                spin = 180;
            }
        }
        if (rb.linearVelocity.magnitude != 0)
        {
            bottom.transform.rotation = Quaternion.Euler(0, spin, 0);
        }
    }
}
