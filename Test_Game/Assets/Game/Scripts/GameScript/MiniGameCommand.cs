using Naninovel;

[CommandAlias("MiniGame")]
public class MiniGameCommand : Command
{
    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        await Engine.GetService<MiniGameService>().PlayCardsAsync();
    }
}
