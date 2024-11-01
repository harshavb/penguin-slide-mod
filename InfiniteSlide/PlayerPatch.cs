using GDWeave.Godot;
using GDWeave.Modding;

namespace InfiniteSlide;

public class PlayerPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
        {
            yield return token;
        }
    }
}
