using TMPro;
using UnityEngine;

public class UISwitch : MonoBehaviour
{
    public int mode = 0; //0 = main menu, 1 = combat, 2 = victory screen
    [SerializeField] private Canvas[] UIs;

    [SerializeField] private TextMeshProUGUI txtVictor;
    void Start()
    {
        UIs[1].enabled = false;
        UIs[2].enabled = false;
    }

    void Update()
    {
        switch (mode)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
        }
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
}
