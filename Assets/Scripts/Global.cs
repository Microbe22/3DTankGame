using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public bool PvP = false;
    public int players = 0;
    public int currentLevel = 0;

    public int ScoreB = 0;
    public int ScoreR = 0;

    private UISwitch tracker;
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        //prevent multiple of this appearing when entering the scene multiple times
        var globals = FindObjectsByType<Global>(FindObjectsSortMode.None);
        if (globals.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //do script whenever a new scene is loaded
            SceneManager.sceneLoaded += StartScene;
        }
    }
    private void StartScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu")
        {
            //when on main menu, reset scores
            ScoreB = 0;
            ScoreR = 0;
            print("reset");
        }
        else
        {
            //otherwise, put current scores on screen
            tracker = FindFirstObjectByType<UISwitch>();
            tracker.txtBlueScore.text = ScoreB.ToString();
            tracker.txtRedScore.text = ScoreR.ToString();
        }
    }
    public void IncreaseScore(int player)
    {
        //increase the score of the player
        switch (player)
        {
            case 0:
                //I wanted to track npc kills, maybe I will add this some time.
                break;
            case 1:
                ScoreB++;
                tracker.txtBlueScore.text = ScoreB.ToString();
                break;
            case 2:
                ScoreR++;
                tracker.txtRedScore.text = ScoreR.ToString();
                break;
        }
    }
}
