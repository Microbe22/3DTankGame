using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISwitch : MonoBehaviour
{
    public int mode = 0; //0 = combat, 1 = victory screen
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

    private PowerupSpawning powerupSpawner;

    public int liveEnemies = 0;

    private Global tracker;
    [SerializeField] private Vector3 spawnposBlue;
    [SerializeField] private Vector3 spawnposRed;
    void Start()
    {
        tracker = FindFirstObjectByType<Global>();

        powerupSpawner = FindFirstObjectByType<PowerupSpawning>();

        UIs[0].enabled = true;
        if (SceneManager.GetActiveScene().name == "PvP")
        {
            UIs[1].enabled = false;
        }

        Inputsystem = new InputSystem_Actions();
        Inputsystem.PlayerController.Enable();
        Inputsystem.PlayerController.Play.performed += Play;
        Inputsystem.PlayerController.Back.performed += Exit;

        Inputsystem.PlayerKeyboard.Enable();
        Inputsystem.PlayerKeyboard.Play.performed += Play;
        Inputsystem.PlayerKeyboard.Back.performed += Exit;

        CreateTanks();
    }
    public void Switch(int canvas, string loser)
    {
        mode = canvas;
        foreach (Canvas c in UIs)
        {
            c.enabled = false;
        }
        UIs[canvas].enabled = true;

        if (canvas == 1)
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
        if (mode == 1)
        {
            CreateTanks();
            UIs[0].enabled = true;
            UIs[1].enabled = false;
            DeleteBullets();
            powerupSpawner.timer = 0;
        }
    }
    private void Exit(InputAction.CallbackContext context)
    {
        if (mode == 1)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    private void CreateTanks()
    {
        foreach (GameObject tank in tanks)
        {
            Destroy(tank);
        }
        tanks.Clear();
        tanks.Add(Instantiate(Tanks[0], spawnposBlue, Quaternion.identity));
        foreach (Image Heart in blueHearts)
        {
            Heart.enabled = true;
        }
        if (tracker.players == 2 || tracker.PvP == true)
        {
            tanks.Add(Instantiate(Tanks[1], spawnposRed, Quaternion.identity));
            foreach (Image Heart in redHearts)
            {
                Heart.enabled = true;
            }
        }
        mode = 0;
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
    public void NextLevel()
    {
        tracker.currentLevel++;
        SceneManager.LoadScene("Level " + tracker.currentLevel);
    }
    private void OnDestroy()
    {
        Inputsystem.PlayerKeyboard.Disable();
        Inputsystem.PlayerController.Disable();
    }
}
