using UnityEngine;
using Naninovel;

public class InteractiveObject : MonoBehaviour
{
    private ICustomVariableManager variable;

    [SerializeField] private QuestUI questUI;

    private void Awake()
    {
        variable = Engine.GetService<ICustomVariableManager>();

        if (questUI == null)
            questUI = FindObjectOfType<QuestUI>();

    }

    private async void OnMouseDown()
    {
        if (!enabled || !gameObject.activeInHierarchy) return;

        var button = FindObjectOfType<ButtonController>();
        button.button_3.interactable = false;
        button.RemoveLisener();
        gameObject.SetActive(false);
        variable.SetVariableValue("Take_Item", "true");

        await questUI.ShowQuestAsync("Go to personal office and deliver the item");
    }
}
