using Naninovel;
using UnityEngine;

[InitializeAtRuntime]
public class MiniGameService : IEngineService
{
    public bool IsMiniGameCompleted;

    private IUIManager uiManager;

    public UniTask InitializeServiceAsync()
    {
        IsMiniGameCompleted = false;
        uiManager = Engine.GetService<IUIManager>();
        return UniTask.CompletedTask;
    }

    public async UniTask PlayCardsAsync()
    {
        var cardGameUI = uiManager.GetUI("CardGameUI");
        cardGameUI.Show();

        var memoryGame = Object.FindObjectOfType<CardManager>();
        memoryGame.StartGame();
        var tcs = new UniTaskCompletionSource();

        memoryGame.OnGameCompleted += () =>
        {
            Debug.Log("Мини-игра окончена!");
            tcs.TrySetResult();
        };

        await tcs.Task;

        cardGameUI.Hide();

        IsMiniGameCompleted = true;
    }

    public void ResetService() { }

    public void DestroyService() { }
}
