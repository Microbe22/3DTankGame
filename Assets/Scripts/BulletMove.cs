using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public int damage;
    public float lifeTime;
    public int bounces;

    private bool hit = false;

    private Rigidbody rb;

    private float speedX;
    private float speedZ;

    private bool xBounce;
    private bool zBounce;

    public string shooter;

    [SerializeField] private GameObject explosionObj;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }

        if (rb.linearVelocity.x != 0)
        {
            speedX = rb.linearVelocity.x;
        }
        if (rb.linearVelocity.z != 0)
        {
            speedZ = rb.linearVelocity.z;
        }
        xBounce = false;
        zBounce = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //allways play the explosion sound and destroy this, unless the collision was with a wall, then this bounces
        if (collision.gameObject.CompareTag("Tank") && hit == false)
        {
            MakeSound();
            Destroy(gameObject);
            //damage the tank
            collision.gameObject.GetComponent<TankMovement>().TakeDamage(damage);
            //prevent hitting twice in 1 frame
            hit = true;
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            MakeSound();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyTank") && hit == false)
        {
            MakeSound();
            Destroy(gameObject);
            //damage the tank, keeping track of who dealt the damage for the scores
            if (shooter == "Blue")
            {
                collision.gameObject.GetComponent<EnemyTankMovement>().TakeDamage(damage, 1);
            }
            else if (shooter == "Red")
            {
                collision.gameObject.GetComponent<EnemyTankMovement>().TakeDamage(damage, 2);
            }
            //prevent hitting twice in 1 frame
            hit = true;
        }
        else
        {
            if (bounces > 0)
            {
                bounces -= 1;
                //bounce
                if (Mathf.Abs(collision.GetContact(0).normal.x) > 0.1f && xBounce == false)
                {
                    //flip speed
                    rb.linearVelocity = new Vector3(speedX * -1, rb.linearVelocity.y, speedZ);
                    xBounce = true;
                    //flip rotation
                    if (transform.rotation.y < 0)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 360, transform.rotation.eulerAngles.z);
                    }
                    if (transform.rotation.y > 90 && transform.rotation.y < 270)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180 + (180 - transform.rotation.eulerAngles.y), transform.rotation.eulerAngles.z);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 360 - transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }
                if (Mathf.Abs(collision.GetContact(0).normal.z) > 0.1f && zBounce == false)
                {
                    //flip speed
                    rb.linearVelocity = new Vector3(speedX, rb.linearVelocity.y, speedZ * -1);
                    zBounce = true;
                    //flip rotation
                    if (transform.rotation.y < 0)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 360, transform.rotation.eulerAngles.z);
                    }
                    if (transform.rotation.y < 180)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180 - transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, (180 - (transform.rotation.eulerAngles.y - 180)) + 180, transform.rotation.eulerAngles.z);
                    }
                }
            }
            else
            {
                MakeSound();
                Destroy(gameObject);
            }
        }
    }
    void MakeSound()
    {
        Instantiate(explosionObj, transform.position, Quaternion.identity);
    }
}
