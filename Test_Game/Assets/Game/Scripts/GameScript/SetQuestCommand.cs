using UnityEngine;
using Naninovel;

[CommandAlias("setQuest")]
public class SetQuestCommand : Command
{
    public StringParameter Text;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var uiManager = Engine.GetService<IUIManager>();

        var questUI = uiManager.GetUI<QuestUI>();

        await questUI.ShowQuestAsync(Text, asyncToken);
    }
}
