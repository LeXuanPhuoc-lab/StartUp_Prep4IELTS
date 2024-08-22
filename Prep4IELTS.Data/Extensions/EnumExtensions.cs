using System.ComponentModel;
using System.Reflection;

namespace Prep4IELTS.Data.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this System.Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value.ToString();
    }
}