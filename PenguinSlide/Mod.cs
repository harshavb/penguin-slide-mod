using GDWeave;

namespace PenguinSlide;

public class Mod : IMod
{
    public Config Config;

    public Mod(IModInterface modInterface)
    {
        modInterface.Logger.Information("InfiniteSlide Mod Loaded!");

        this.Config = modInterface.ReadConfig<Config>();

        modInterface.RegisterScriptMod(new PlayerPatch(Config.Speed, Config.TurnSpeed, Config.EnableRoll));
    }

    public void Dispose() { }
}
