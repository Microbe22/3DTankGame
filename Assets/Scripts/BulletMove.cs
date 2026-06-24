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
        if (collision.gameObject.CompareTag("Tank") && hit == false)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<TankMovement>().TakeDamage(damage);
            hit = true;
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyTank") && hit == false)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<EnemyTankMovement>().TakeDamage(damage);
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
                    rb.linearVelocity = new Vector3(speedX * -1, rb.linearVelocity.y, rb.linearVelocity.z);
                    xBounce = true;
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
                    rb.linearVelocity = new Vector3(speedX, rb.linearVelocity.y, speedZ * -1);
                    zBounce = true;
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
                Destroy(gameObject);
            }
        }
    }
    private void OnDestroy()
    {
        MakeSound();
    }
    void MakeSound()
    {
        Instantiate(explosionObj, transform.position, Quaternion.identity);
    }
}
