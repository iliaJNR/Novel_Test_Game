using Naninovel;

[CommandAlias("NaniActive")]
public class NaniActive : Command
{
    public StringParameter LocationName;
    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var player = Engine.GetService<IScriptPlayer>();
        await player.PreloadAndPlayAsync(LocationName);
    }
}
