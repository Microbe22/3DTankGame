using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UISwitch : MonoBehaviour
{
    public int mode = 0; //0 = main menu, 1 = combat, 2 = victory screen
    [SerializeField] private Canvas[] UIs;

    [SerializeField] private TextMeshProUGUI txtVictor;

    [SerializeField] private GameObject[] Tanks;

    [SerializeField] private Image[] blueHearts;
    [SerializeField] private Image[] redHearts;

    private int blueScore = 0;
    private int redScore = 0;
    [SerializeField] private TextMeshProUGUI txtBlueScore;
    [SerializeField] private TextMeshProUGUI txtRedScore;

    private InputSystem_Actions Inputsystem;
    private List<GameObject> tanks = new List<GameObject>();
    void Start()
    {
        UIs[1].enabled = false;
        UIs[2].enabled = false;

        Inputsystem = new InputSystem_Actions();
        Inputsystem.PlayerController.Enable();
        Inputsystem.PlayerController.Play.performed += Play;
        Inputsystem.PlayerController.Back.performed += Exit;

        Inputsystem.PlayerKeyboard.Enable();
        Inputsystem.PlayerKeyboard.Play.performed += Play;
        Inputsystem.PlayerKeyboard.Back.performed += Exit;
    }

    void Update()
    {
        
    }

    public void Switch(int canvas, string loser)
    {
        mode = canvas;
        foreach (Canvas c in UIs)
        {
            c.enabled = false;
        }
        UIs[canvas].enabled = true;

        if (canvas == 2)
        {
            if (loser == "Blue")
            {
                txtVictor.text = "Red wins!";
                txtVictor.color = Color.red;
                redScore++;
                txtRedScore.text = redScore.ToString();
            }
            else
            {
                txtVictor.text = "Blue wins!";
                txtVictor.color = Color.blue;
                blueScore++;
                txtBlueScore.text = blueScore.ToString();
            }
        }
    }
    private void Play(InputAction.CallbackContext context)
    {
        switch (mode)
        {
            case 0:
                CreateTanks();
                UIs[0].enabled = false;
                UIs[1].enabled = true;
                break;
            case 1:

                break;
            case 2:
                CreateTanks();
                UIs[2].enabled = false;
                UIs[1].enabled = true;
                DeleteBullets();
                break;
        }
    }
    private void Exit(InputAction.CallbackContext context)
    { 
        Application.Quit();
    }
    private void CreateTanks()
    {
        foreach (GameObject tank in tanks)
        {
            Destroy(tank);
        }
        tanks.Clear();
        tanks.Add(Instantiate(Tanks[0], new Vector3(-15, 0, -5), Quaternion.identity));
        tanks.Add(Instantiate(Tanks[1], new Vector3(15, 0, -5), Quaternion.identity));
        mode = 1;
        foreach (Image Heart in blueHearts)
        {
            Heart.enabled = true;
        }
        foreach (Image Heart in redHearts)
        {
            Heart.enabled = true;
        }
    }
    public void SetHearts(string color, int amount)
    {
        if (color == "Red")
        {
            foreach (Image Heart in redHearts)
            {
                Heart.enabled = false;
            }
            for (int i = 0; i < amount; i++)
            {
                redHearts[i].enabled = true;
            }
        }
        else
        {
            foreach (Image Heart in blueHearts)
            {
                Heart.enabled = false;
            }
            for (int i = 0; i < amount; i++)
            {
                blueHearts[i].enabled = true;
            }
        }
    }
    private void DeleteBullets()
    {
        BulletMove[] bullets = FindObjectsByType<BulletMove>(FindObjectsSortMode.None);
        foreach (BulletMove b in bullets)
        {
            Destroy(b.gameObject);
        }
    }
}
