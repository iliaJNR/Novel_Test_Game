using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button button_1;
    public Button button_2;
    public Button button_3;

    private void Start()
    {
        button_2.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        button_3.interactable = true;
    }

    public void RemoveLisener()
    {
        button_2.onClick.RemoveListener(OnButtonClick);
    }
}
