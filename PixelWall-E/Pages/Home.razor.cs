using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BlazorMonaco;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;


namespace PixelWall_E.Pages;

public partial class Home
{
    private string _valueToSet = "";

    [AllowNull]
    private StandaloneCodeEditor _editor;

    private static StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            Language = "paint",
            GlyphMargin = true,
            AutomaticLayout = true,
            Value = ""
        };
    }

    private async Task EditorOnDidInit()
    {
        await _editor.Layout();
        await _editor.AddCommand((int)KeyMod.CtrlCmd | (int)KeyCode.KeyH, (args) =>
        {
            Console.WriteLine("Ctrl+H : Initial editor command is triggered.");
        });

    }

    private void OnContextMenu(EditorMouseEvent eventArg)
    {
        Console.WriteLine("OnContextMenu : " + JsonSerializer.Serialize(eventArg));
    }

    private async Task ChangeTheme(ChangeEventArgs e)
    {
        Console.WriteLine($"setting theme to: {e.Value?.ToString()}");
        await BlazorMonaco.Editor.Global.SetTheme(jsRuntime, e.Value?.ToString());
    }

    private async Task SetValue()
    {
        Console.WriteLine($"setting value to: {_valueToSet}");
        await _editor.SetValue(_valueToSet);
    }

    private async Task GetValue()
    {
        var val = await _editor.GetValue();
        Console.WriteLine($"value is: {val}");
    }

}
