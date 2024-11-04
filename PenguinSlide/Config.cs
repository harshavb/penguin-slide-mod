using System.Text.Json.Serialization;

namespace PenguinSlide;

public class Config {
    [JsonInclude] public float Speed = 12;
    [JsonInclude] public float TurnSpeed = 2;
    [JsonInclude] public bool EnableRoll = true;
}
