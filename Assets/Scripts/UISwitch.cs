using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
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

    private InputSystemUIInputModule UIInput;
    void Start()
    {
        UIInput = GetComponent<InputSystemUIInputModule>();

        UIs[1].enabled = false;
    }
    private void Update()
    {
        print(UIInput.cancel);
        if (UIInput.submit == true)
        {
            if (mode == 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        if (UIInput.cancel == true)
        {
            if (mode == 1)
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
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
    public void OnSubmit(InputValue context)
    {
        
    }
    public void OnCancel(InputValue context)
    {
        
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
}
