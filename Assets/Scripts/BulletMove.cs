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
    }
    private void FixedUpdate()
    {
        if (rb.linearVelocity.x != 0)
        {
            speedX = rb.linearVelocity.x;
        }
        if (rb.linearVelocity.z != 0)
        {
            speedZ = rb.linearVelocity.z;
        }
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
        else
        {
            if (bounces > 0)
            {
                bounces -= 1;
                //bounce
                if (collision.GetContact(0).normal.x == 0)
                {
                    rb.linearVelocity = new Vector3(speedX, rb.linearVelocity.y, speedZ * -1);
                }
                else
                {
                    rb.linearVelocity = new Vector3(speedX * -1, rb.linearVelocity.y, speedZ);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
