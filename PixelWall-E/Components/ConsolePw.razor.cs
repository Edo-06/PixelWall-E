using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BlazorMonaco;
using BlazorMonaco.Editor;

namespace PixelWall_E.Components;
public partial class ConsolePw
{
    [AllowNull]
    private StandaloneCodeEditor _editor;
    private static StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            Language = "plaintext",
            GlyphMargin = true,
            AutomaticLayout = true,
            Value = "",   
            LineNumbers = "off",
            Folding = false,    
            Minimap = new EditorMinimapOptions { Enabled = false }, 
            Scrollbar = new EditorScrollbarOptions 
            {
                Vertical = "visible",   
                Horizontal = "auto",    
                UseShadows = false      
            },
            CursorStyle = "block",
            CursorBlinking = "slow",
            RenderLineHighlight = "none",
            MatchBrackets = "never",
            RenderWhitespace = "none",
            WordWrap = "on",
            WrappingIndent = "none",
            ScrollBeyondLastLine = false,

            FontFamily = "Consolas, 'Courier New', monospace",
            FontSize = 14,
            FontWeight = "normal",

            QuickSuggestions = new BlazorMonaco.Editor.QuickSuggestionsOptions 
            {
                Other = "off",
                Comments = "off",
                Strings = "off"
            },
            AcceptSuggestionOnCommitCharacter = false,
            AcceptSuggestionOnEnter = "off",
            SnippetSuggestions = "none",
            WordBasedSuggestions = false,
            TabCompletion = "off",
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
}