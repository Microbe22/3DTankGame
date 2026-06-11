using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UISwitch : MonoBehaviour
{
    public int mode = 0; //0 = main menu, 1 = combat, 2 = victory screen
    [SerializeField] private Canvas[] UIs;

    [SerializeField] private TextMeshProUGUI txtVictor;

    [SerializeField] private GameObject[] Tanks;


    private InputSystem_Actions Inputsystem;
    void Start()
    {
        UIs[1].enabled = false;
        UIs[2].enabled = false;

        Inputsystem = new InputSystem_Actions();
        Inputsystem.PlayerController.Enable();
        Inputsystem.PlayerController.Attack.performed += Play;
        Inputsystem.PlayerKeyboard.Enable();
        Inputsystem.PlayerKeyboard.Attack.performed += Play;
    }

    void Update()
    {
        
    }

    public void Switch(int canvas, string victor)
    {
        mode = canvas;
        foreach (Canvas c in UIs)
        {
            c.enabled = false;
        }
        UIs[canvas].enabled = true;

        if (canvas == 2)
        {
            if (victor == "Red")
            {
                txtVictor.text = "Red wins!";
                txtVictor.color = Color.red;
            }
            else
            {
                txtVictor.text = "Blue wins!";
                txtVictor.color = Color.blue;
            }
        }
    }
    private void Play(InputAction.CallbackContext context)
    {
        switch (mode)
        {
            case 0:
                Instantiate(Tanks[0], new Vector3(-15, 0, -5), Quaternion.identity);
                Instantiate(Tanks[1], new Vector3(15, 0, -5), Quaternion.identity);
                mode = 1;
                UIs[0].enabled = false;
                UIs[1].enabled = true;
                break;
            case 1:

                break;
            case 2:

                break;
        }
    }
}
