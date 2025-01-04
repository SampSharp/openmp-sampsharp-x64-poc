namespace SampSharp.OpenMp.Core.Api;

public static class BodyPartExtensions
{
    private static readonly string[] _names =
    [
        "invalid",
        "invalid",
        "invalid",
        "torso",
        "groin",
        "left arm",
        "right arm",
        "left leg",
        "right leg",
        "head"
    ];

    public static string GetName(this BodyPart bodyPart)
    {
        var number = (int)bodyPart;

        if (number < 0 || number >= _names.Length)
        {
            return "invalid";
        }

        return _names[number];
    }
}