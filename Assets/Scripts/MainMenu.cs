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
    void Start()
    {

    }
    public void OnSelect(InputValue context)
    {
        switch (choice)
        {
            case 1:
                SceneManager.LoadScene("PvP");
                break;
            case 2:
                //singleplayer
                break;
            case 3:
                //multiplayer
                break;
        }
    }
    public void OnCancel(InputValue context)
    {
        Application.Quit();
    }
    public void OnNavigate(InputValue context)
    {
        if (context.Get<Vector2>().x < 0)
        {
            choice--;
        }
        else if (context.Get<Vector2>().x > 0)
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
                arrow.transform.position = new Vector2(109, arrow.transform.position.y);
                break;
            case 2:
                arrow.transform.position = new Vector2(326, arrow.transform.position.y);
                break;
            case 3:
                arrow.transform.position = new Vector2(572, arrow.transform.position.y);
                break;
        }
    }
}
