using UnityEngine;
using UnityEngine.UI;

public class MapActivator : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;
    private Button button;
    private bool active;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (!active)
        {
            mapPanel.SetActive(true);
            active = true;
        }

        else
        {
            HideMap();
        }
    }

    public void HideMap()
    {
        mapPanel.SetActive(false);
        active = false;
    }
}
