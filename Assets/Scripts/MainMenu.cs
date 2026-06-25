using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private int choice = 1;
    private int maxChoice = 3;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image[] arrows;

    private InputSystem_Actions Inputsystem;

    private Global tracker;
    void Start()
    {
        tracker = FindFirstObjectByType<Global>();

        //get inputs
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
        //configurations for gamemodes
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
        //arrow movement
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
                arrows[0].enabled = true;
                arrows[1].enabled = false;
                arrows[2].enabled = false;
                break;
            case 2:
                arrows[0].enabled = false;
                arrows[1].enabled = true;
                arrows[2].enabled = false;
                break;
            case 3:
                arrows[0].enabled = false;
                arrows[1].enabled = false;
                arrows[2].enabled = true;
                break;
        }
    }
    private void OnDestroy()
    {
        Inputsystem.PlayerKeyboard.Disable();
        Inputsystem.PlayerController.Disable();
    }
}
