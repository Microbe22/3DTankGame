using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private int choice = 1;
    private int maxChoice = 3;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image arrow;

    private InputSystem_Actions Inputsystem;

    private Global tracker;
    void Start()
    {
        tracker = FindFirstObjectByType<Global>();

        Inputsystem = new InputSystem_Actions();
        Inputsystem.PlayerController.Enable();
        Inputsystem.PlayerController.Play.performed += Play;
        Inputsystem.PlayerController.Back.performed += Exit;
        Inputsystem.PlayerController.Move.performed += Move;

        Inputsystem.PlayerKeyboard.Enable();
        Inputsystem.PlayerKeyboard.Play.performed += Play;
        Inputsystem.PlayerKeyboard.Back.performed += Exit;
        Inputsystem.PlayerKeyboard.Move.performed += Move;
    }
    public void Play(InputAction.CallbackContext context)
    {
        switch (choice)
        {
            case 1:
                tracker.PvP = true;
                SceneManager.LoadScene("PvP");
                break;
            case 2:
                tracker.PvP = false;
                tracker.players = 1;
                tracker.currentLevel = 1;
                SceneManager.LoadScene("Level 1");
                break;
            case 3:
                tracker.PvP = false;
                tracker.players = 2;
                tracker.currentLevel = 1;
                SceneManager.LoadScene("Level 1");
                break;
        }
    }
    public void Exit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
    public void Move(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x < 0)
        {
            choice--;
        }
        else if (context.ReadValue<Vector2>().x > 0)
        {
            choice++;
        }
        if (choice < 1)
        {
            choice += maxChoice;
        }
        else if (choice > maxChoice)
        {
            choice -= maxChoice;
        }
        switch (choice)
        {
            case 1:
                arrow.transform.position = new Vector2(107, arrow.transform.position.y);
                break;
            case 2:
                arrow.transform.position = new Vector2(316, arrow.transform.position.y);
                break;
            case 3:
                arrow.transform.position = new Vector2(556, arrow.transform.position.y);
                break;
        }
    }
    private void OnDestroy()
    {
        Inputsystem.PlayerKeyboard.Disable();
        Inputsystem.PlayerController.Disable();
    }
}
