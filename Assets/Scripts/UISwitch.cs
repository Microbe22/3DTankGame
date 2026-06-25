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
    public TextMeshProUGUI txtBlueScore;
    public TextMeshProUGUI txtRedScore;

    private InputSystem_Actions Inputsystem;
    private List<GameObject> tanks = new List<GameObject>();

    private PowerupSpawning powerupSpawner;

    public int liveEnemies = 0;
    public int livePlayers = 0;

    private Global tracker;
    [SerializeField] private Vector3 spawnposBlue;
    [SerializeField] private Vector3 spawnposRed;

    [SerializeField] private GameObject BUI;
    [SerializeField] private GameObject RUI;

    private int levelAmount = 2;
    void Start()
    {
        tracker = FindFirstObjectByType<Global>();

        powerupSpawner = FindFirstObjectByType<PowerupSpawning>();

        //enable ui for health & score
        UIs[0].enabled = true;
        if (SceneManager.GetActiveScene().name == "PvP")
        {
            //disable ui that shows up when a player wins
            UIs[1].enabled = false;
        }
        if (tracker.players == 1)
        {
            //remove UI for player 2
            Destroy(RUI);
        }

        //get inputs
        Inputsystem = new InputSystem_Actions();
        Inputsystem.PlayerController.Enable();
        Inputsystem.PlayerController.Play.performed += Play;
        Inputsystem.PlayerController.Back.performed += Exit;

        Inputsystem.PlayerKeyboard.Enable();
        Inputsystem.PlayerKeyboard.Play.performed += Play;
        Inputsystem.PlayerKeyboard.Back.performed += Exit;

        //instantiate player tanks
        if (SceneManager.GetActiveScene().name != "Results")
        {
            CreateTanks();
        }
    }
    public void Switch(int canvas, string loser)
    {
        mode = canvas;
        //disable all ui
        foreach (Canvas c in UIs)
        {
            c.enabled = false;
        }
        //enable the desired canvas
        UIs[canvas].enabled = true;

        if (canvas == 1)
        {
            if (loser == "Blue")
            {
                //red wins & gets a point
                txtVictor.text = "Red wins!";
                txtVictor.color = Color.red;
                redScore++;
                txtRedScore.text = redScore.ToString();
            }
            else
            {
                //blue wins & gets a point
                txtVictor.text = "Blue wins!";
                txtVictor.color = Color.blue;
                blueScore++;
                txtBlueScore.text = blueScore.ToString();
            }
            //play the win sound
            GetComponent<AudioSource>().Play();
        }
    }
    private void Play(InputAction.CallbackContext context)
    {
        if (mode == 1)
        {
            CreateTanks();
            //enable combat ui
            UIs[0].enabled = true;
            UIs[1].enabled = false;
            //delete bullets flying around
            DeleteBullets();
            //spawn new powerup
            powerupSpawner.timer = 0;
        }
    }
    private void Exit(InputAction.CallbackContext context)
    {
        //when not in combat
        if (mode == 1 || SceneManager.GetActiveScene().name == "Results")
        {
            //go to main menu
            SceneManager.LoadScene("Main Menu");
        }
    }
    private void CreateTanks()
    {
        //remove current tanks
        foreach (GameObject tank in tanks)
        {
            Destroy(tank);
        }
        tanks.Clear();
        //instantiate tanks
        tanks.Add(Instantiate(Tanks[0], spawnposBlue, Quaternion.identity));
        foreach (Image Heart in blueHearts)
        {
            //refill healthbar
            Heart.enabled = true;
        }
        //only add red tank if not in single player
        if (tracker.players == 2 || tracker.PvP == true)
        {
            tanks.Add(Instantiate(Tanks[1], spawnposRed, Quaternion.identity));
            foreach (Image Heart in redHearts)
            {
                Heart.enabled = true;
            }
        }
        //enter combat mode
        mode = 0;
    }
    public void SetHearts(string color, int amount)
    {
        //set the current amount of hearts of a specific color
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
        //delete all bullets flying around
        BulletMove[] bullets = FindObjectsByType<BulletMove>(FindObjectsSortMode.None);
        foreach (BulletMove b in bullets)
        {
            Destroy(b.gameObject);
        }
    }
    public void NextLevel()
    {
        //go to the next level or the results scene if it was the last level
        tracker.currentLevel++;
        if (tracker.currentLevel <= levelAmount)
        {
            SceneManager.LoadScene("Level " + tracker.currentLevel);
        }
        else
        {
            SceneManager.LoadScene("Results");
        }
    }
    private void OnDestroy()
    {
        Inputsystem.PlayerKeyboard.Disable();
        Inputsystem.PlayerController.Disable();
    }
}
