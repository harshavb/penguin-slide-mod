using GDWeave;

namespace InfiniteSlide;

public class Mod : IMod
{
    public Mod(IModInterface modInterface)
    {
        modInterface.Logger.Information("InfiniteSlide Mod Loaded!");
        modInterface.RegisterScriptMod(new PlayerPatch());
    }

    public void Dispose() { }
}
