using UnityEngine;
using TMPro;
using Naninovel.UI;
using Naninovel;

public class QuestUI : CustomUI
{
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private GameObject questGameObject;
    [SerializeField] private float showDuration = 1f;

    private bool firstActive;

    public void SetQuest(string description)
    {
        questText.text = description;
    }

    public async UniTask ShowQuestAsync(string description, AsyncToken asyncToken = default)
    {
        if (firstActive == true)
        {
            questGameObject.SetActive(true);
        }

        await UniTask.Delay(System.TimeSpan.FromSeconds(showDuration), cancellationToken: asyncToken.CancellationToken);

        questGameObject.SetActive(false);
        questText.text = description;
        firstActive = true;
    }

    public void HideAfterCompletion()
    {
        Hide();
    }
}
