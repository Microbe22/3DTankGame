using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public int damage;
    public int lifeTime;
    public int bounces;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            collision.gameObject.GetComponent<TankMovement>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            //bounce
        }
    }
}
