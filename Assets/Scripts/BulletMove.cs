using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public int damage;
    public float lifeTime;
    public int bounces;

    private bool hit = false;
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
        if (collision.gameObject.CompareTag("Tank") && hit == false)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<TankMovement>().TakeDamage(damage);
            hit = true;
        }
        else
        {
            Destroy(gameObject);
            //bounce
        }
    }
}
