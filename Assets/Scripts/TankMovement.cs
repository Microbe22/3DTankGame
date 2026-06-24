using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TankMovement : MonoBehaviour
{
    [SerializeField] private string input;
    private InputSystem_Actions Inputsystem;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;

    private UISwitch UIController;

    [SerializeField] private string color;

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
    private int projectileBounces = 3;

    public float powerupTimer = 0;
    void Start()
    {
        UIController = FindFirstObjectByType<UISwitch>();
        Inputsystem = new InputSystem_Actions();
        if (input == "Keyboard")
        {
            Inputsystem.PlayerKeyboard.Enable();
            Inputsystem.PlayerKeyboard.Move.performed += Move;
            Inputsystem.PlayerKeyboard.Move.canceled += Move;
            Inputsystem.PlayerKeyboard.Rotate.performed += Rotate;
            Inputsystem.PlayerKeyboard.Rotate.canceled += Rotate;
            Inputsystem.PlayerKeyboard.Attack.performed += Shoot;
        }
        else if (input == "Controller")
        {
            Inputsystem.PlayerController.Enable();
            Inputsystem.PlayerController.Move.performed += Move;
            Inputsystem.PlayerController.Move.canceled += Move;
            Inputsystem.PlayerController.Rotate.performed += Rotate;
            Inputsystem.PlayerController.Rotate.canceled += Rotate;
            Inputsystem.PlayerController.Attack.performed += Shoot;
        }
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
    private void Move(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
    private void Rotate(InputAction.CallbackContext context)
    {
        rotationDir = context.ReadValue<Vector2>();
    }
    private void Shoot(InputAction.CallbackContext context)
    {
        if (shootCooldown <= 0)
        {
            GetComponent<AudioSource>().Play();

            //blue tank was modelled backwards
            var invert = 1;
            if (color == "Blue")
            {
                invert = -1;
            }

            var pewpew = Instantiate(bullet, top.transform.position + new Vector3(0, 0.8f, 0) + (top.transform.forward * invert * 1.6f), Quaternion.Euler(-90, 0 + top.transform.rotation.eulerAngles.y, 0));
            pewpew.GetComponent<Rigidbody>().linearVelocity = top.transform.forward * invert * projectileSpeed;
            pewpew.GetComponent<BulletMove>().damage = projectileDamage;
            pewpew.GetComponent<BulletMove>().lifeTime = 20;
            pewpew.GetComponent<BulletMove>().bounces = projectileBounces;
            pewpew.GetComponent<BulletMove>().shooter = color;
            shootCooldown = maxShootCooldown;
        }
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
                if (SceneManager.GetActiveScene().name == "PvP")
                {
                    UIController.Switch(1, color);
                }
            }
        }
    }
    private void OnDestroy()
    {
        Inputsystem.PlayerKeyboard.Disable();
        Inputsystem.PlayerController.Disable();
    }
}
