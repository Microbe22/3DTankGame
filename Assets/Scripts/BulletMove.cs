using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public int damage;
    public float lifeTime;
    public int bounces;
    void Start()
    {
        
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<TankMovement>().TakeDamage(damage);
        }
        else
        {
            Destroy(gameObject);
            //bounce
        }
    }
}
