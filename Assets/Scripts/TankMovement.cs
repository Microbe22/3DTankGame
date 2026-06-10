using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{
    private InputSystem_Actions Inputsystem;
    [SerializeField] private GameObject top;

    private Vector2 moveDir;
    public float speed = 5f;

    private Vector2 rotationDir;

    public float rotateSpeed = 100;
    void Start()
    {
        Inputsystem = new InputSystem_Actions();
        Inputsystem.Player.Enable();
        Inputsystem.Player.Move.performed += Move;
        Inputsystem.Player.Move.canceled += Move;
        Inputsystem.Player.Rotate.performed += Rotate;
        Inputsystem.Player.Rotate.canceled += Rotate;
    }
    
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * new Vector3(moveDir.x, 0, moveDir.y), Space.Self);
        if (moveDir.magnitude == 0)
        {
            top.transform.rotation = Quaternion.Euler(top.transform.rotation.eulerAngles.x, top.transform.rotation.eulerAngles.y + (rotationDir.x * rotateSpeed * Time.deltaTime), top.transform.rotation.eulerAngles.z);
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
