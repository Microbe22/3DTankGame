using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{
    [SerializeField] private string input;
    private InputSystem_Actions Inputsystem;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;

    private Vector2 moveDir;
    public float speed = 5f;

    private Vector2 rotationDir;

    public float rotateSpeed = 100;
    void Start()
    {
        Inputsystem = new InputSystem_Actions();
        if (input == "Keyboard")
        {
            Inputsystem.PlayerKeyboard.Enable();
            Inputsystem.PlayerKeyboard.Move.performed += Move;
            Inputsystem.PlayerKeyboard.Move.canceled += Move;
            Inputsystem.PlayerKeyboard.Rotate.performed += Rotate;
            Inputsystem.PlayerKeyboard.Rotate.canceled += Rotate;
        }
        else if (input == "Controller")
        {
            Inputsystem.PlayerController.Enable();
            Inputsystem.PlayerController.Move.performed += Move;
            Inputsystem.PlayerController.Move.canceled += Move;
            Inputsystem.PlayerController.Rotate.performed += Rotate;
            Inputsystem.PlayerController.Rotate.canceled += Rotate;
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

    }
    private void Move(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
    private void Rotate(InputAction.CallbackContext context)
    {
        rotationDir = context.ReadValue<Vector2>();
    }
}
