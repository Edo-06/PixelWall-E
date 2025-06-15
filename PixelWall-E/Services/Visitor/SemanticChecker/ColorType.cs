using System.Text.RegularExpressions;
using System.Drawing; 
using SixLabors.ImageSharp.PixelFormats;
public static class ColorTypes
{
    public static readonly Dictionary<string, string> colorHexCodes = new Dictionary<string, string>
    {
        { "Red", "#FF0000" },
        { "Blue", "#0000FF" },
        { "Green", "#008000" },
        { "Yellow", "#FFFF00" },
        { "Orange", "#FFA500" },
        { "Purple", "#800080" },
        { "Black", "#000000" },
        { "White", "#FFFFFF" },
        { "MediumBlue", "#0000CD" },
        { "NavyBlue", "#000080" },
        { "SkyBlue", "#87CEEB" },
        { "Brown", "#A52A2A" },
        { "OrangeRed", "#FF4500" },
        { "MediumVioletRed", "#C71585" },
        { "HotPink", "#FF69B4" },
        { "LimeGreen", "#32CD32" },
        { "Teal", "#008080" },
        { "Maroon", "#800000" },
        { "Olive", "#808000" },
        { "Fuchsia", "#FF00FF" },
        { "Silver", "#C0C0C0" },
        { "Gray", "#808080" },
        { "Aqua", "#00FFFF" },
        { "DarkGreen", "#006400" },
        { "Indigo", "#4B0082" },
        { "Crimson", "#DC143C" },
        { "Turquoise", "#40E0D0" },
        { "Lavender", "#E6E6FA" },
        { "Gold", "#FFD700" },
        { "Coral", "#FF7F50" }
    };
    public static bool IsValidHexColor(string hexColorString)
    {
        string pattern = @"^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$";

        return Regex.IsMatch(hexColorString, pattern);
    }
    public static Rgba32 HexagToRgba32(string hexColor)
    {
        
        if (string.IsNullOrWhiteSpace(hexColor))
        {
            throw new ArgumentException("El string de color hexadecimal no puede ser nulo o vacío.", nameof(hexColor));
        }

        string pattern = @"^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$";
        if (!Regex.IsMatch(hexColor, pattern))
        {
            throw new ArgumentException("Formato de color hexadecimal inválido. Debe ser '#RGB' o '#RRGGBB'.", nameof(hexColor));
        }

        string hex = hexColor.Substring(1); // Remover el '#'

        byte r, g, b;

        if (hex.Length == 3) 
        {
            r = Convert.ToByte(hex.Substring(0, 1) + hex.Substring(0, 1), 16);
            g = Convert.ToByte(hex.Substring(1, 1) + hex.Substring(1, 1), 16);
            b = Convert.ToByte(hex.Substring(2, 1) + hex.Substring(2, 1), 16);
        }
        else 
        {
            r = Convert.ToByte(hex.Substring(0, 2), 16);
            g = Convert.ToByte(hex.Substring(2, 2), 16);
            b = Convert.ToByte(hex.Substring(4, 2), 16);
        }

        return new Rgba32(r, g, b);
    }
}
