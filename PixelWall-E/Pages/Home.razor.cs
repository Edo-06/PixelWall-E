using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BlazorMonaco;
using BlazorMonaco.Editor;
using BlazorMonaco.Languages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using PixelWall_E.Components;

namespace PixelWall_E.Pages;
public partial class Home
{
    public string code {get; private set;} = "";
    private string _valueToSet = "";
    private InputFile? _inputFileElement;
    private ElementReference _name;
    private string dialogueStyle = "display: none;";
    private string temporaryContent = "";
    private EventCallback<int> OnGridChange {get; set;}
    
    [AllowNull]
    private StandaloneCodeEditor _editor;
    private static StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
{
    return new StandaloneEditorConstructionOptions
    {
        Language = "pixelwalle",
        GlyphMargin = true,
        AutomaticLayout = true,
        Value = "", 

        WordBasedSuggestions = false,
        SuggestOnTriggerCharacters = false,
        QuickSuggestions = new BlazorMonaco.Editor.QuickSuggestionsOptions 
        {
            Other = "off",
            Comments = "off",
            Strings = "off"
        },
        AcceptSuggestionOnEnter = "off",
        AcceptSuggestionOnCommitCharacter = false,
        TabCompletion = "off",
        SnippetSuggestions = "none", 
        ParameterHints = new EditorParameterHintOptions { Enabled = false },
        Hover = new EditorHoverOptions { Enabled = false },
        SuggestSelection = "first", 
        Links = false,       
        CodeLens = false,       
        Folding = false,        
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

    public async Task Run()
    {
        code = await _editor.GetValue();
        Console.WriteLine(code);
        PipeLineManager pip = new PipeLineManager();
        pip.Start(code);
    }

    
#region LoadFile
    private async Task LoadFile()
    {
        await jsRuntime.InvokeVoidAsync("clickFileInput", "fileInput");
    }
    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        try
        {
            var file = e.File;
            if (Path.GetExtension(file.Name) != ".pw")
            {
                Console.WriteLine("Error: Solo se permiten archivos .pw");
                return;
            }

            var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            string fileContent = await reader.ReadToEndAsync();

            await _editor.SetValue(fileContent);
            Console.WriteLine("Archivo cargado exitosamente!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
#endregion

#region SaveFile
    private async Task ShowDialogueForSave()
    {
        temporaryContent = await _editor.GetValue();
        dialogueStyle = "display: block;";
        await Task.Delay(10); 
        await jsRuntime.InvokeVoidAsync("focusInput", _name);
    }
    private async Task ConfirmSave()
    {
        var name = await jsRuntime.InvokeAsync<string>("getFileName", _name);
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Nombre inválido");
            return;
        }

        if (!name.EndsWith(".pw")) name += ".pw";

        await jsRuntime.InvokeVoidAsync("downloadFile", name, temporaryContent);
        dialogueStyle = "display: none;";
    }
    private void CancelSave()
    {
        dialogueStyle = "display: none;";
    }
#endregion
}