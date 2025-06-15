using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BlazorMonaco;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


namespace PixelWall_E.Components;
public partial class ConsolePw
{
    [Parameter] public EventCallback<KeyboardEventArgs> OnConsoleKeyDown { get; set; }
    [Parameter] public EventCallback<KeyboardEventArgs> OnDidChangeModelContent { get; set; }
    [AllowNull] private StandaloneCodeEditor _console;
    private static string _value = ">";
    private Position _readOnlyPosition = new Position { LineNumber = 1, Column = 2 };
    private string _currentOfficialContent = ">";
    private bool _ignoreNextContentChangedEvent = false;

    private static StandaloneEditorConstructionOptions ConsoleConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            Language = "plaintext",
            GlyphMargin = true,
            AutomaticLayout = true,
            Value = _value,   
            Dimension = new Dimension{Width = 800, Height = 2},
            LineNumbers = "off",
            Folding = false,    
            Minimap = new EditorMinimapOptions { Enabled = false }, 
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

            QuickSuggestions = new QuickSuggestionsOptions 
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
    private async Task ConsoleOnDidInit()
    {
        await _console.Layout();
        await _console.AddAction(new ActionDescriptor
        {
            Id = "no-newline-on-enter", 
            Label = "Prevent Newline on Enter", 
            Keybindings = new int[] { (int)KeyCode.Enter },

            Run = async (console) => 
            {
                Console.WriteLine("Enter key pressed on THIS editor (via action). Newline prevented.");
                await OnConsoleKeyDown.InvokeAsync( new KeyboardEventArgs
                {
                    Code = "Enter",
                    Key = "Enter",
                    CtrlKey = false,
                    ShiftKey = false,
                    AltKey = false,
                    MetaKey = false
                });
            }
        });
        await _console.AddAction(new ActionDescriptor
        {
            Id = "no-newline-on-shift-enter",
            Label = "Prevent Newline on Shift + Enter",
            Keybindings = new int[] { (int)KeyMod.Shift | (int)KeyCode.Enter },
            Run = async (monacoEditor) =>
            {
                Console.WriteLine("Shift+Enter key pressed on THIS editor (via action). Newline prevented.");
                await Task.CompletedTask;
            }
        });
    }
    private void OnContextMenu(EditorMouseEvent eventArg)
    {
        Console.WriteLine("OnContextMenu : " + JsonSerializer.Serialize(eventArg));
    }
    private async Task HandleConsoleKeyDown(KeyboardEvent e)
    {
        if(e.Code == "KeyC" && e.CtrlKey)
        {
            await AppendOutput("^C");
            PipeLineManager.isRunning = false;
            return; 
        }
    }
    public async Task<string> HandleEnter()
    {
        Console.WriteLine("DEBUG: HandleEnter called in ConsolePw.");
        string fullEditorContent = await _console.GetValue();

        string[] lines = fullEditorContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        if (_readOnlyPosition.LineNumber < 1 || _readOnlyPosition.LineNumber > lines.Length)
        {
            Console.WriteLine($"ConsolePw: ReadOnlyPosition LineNumber {_readOnlyPosition.LineNumber} is out of bounds.");
            return string.Empty;
        }

        string inputLine = lines[_readOnlyPosition.LineNumber - 1];

        int startIndex = _readOnlyPosition.Column - 1;

        if (startIndex < 0 || startIndex >= inputLine.Length)
        {
            Console.WriteLine($"ConsolePw: Calculated startIndex {startIndex} is out of bounds for line '{inputLine}'. Returning empty.");
            return string.Empty;
        }
        string userInputValue = inputLine.Substring(startIndex).Trim();

        Console.WriteLine($"ConsolePw: Detected user input: '{userInputValue}'");

        return userInputValue;
    }
    public async Task AppendOutput(string text)
    {
            TextModel model = await _console.GetModel();
            int lineCount = await model.GetLineCount();
            int lastCharInLastLine = await model.GetLineMaxColumn(lineCount);
            string newText = "\n" + text + "\n> ";
            _currentOfficialContent = await _console.GetValue() + newText;

            Selection endOfDocumentSelection = new Selection
            {
                StartLineNumber = lineCount,
                StartColumn = lastCharInLastLine,
                EndLineNumber = lineCount,
                EndColumn = lastCharInLastLine,

                SelectionStartLineNumber = lineCount + 2,
                SelectionStartColumn = 2,
                PositionLineNumber = lineCount + 2,
                PositionColumn = 2,
            };

            await _console.ExecuteEdits("", 
                new List<IdentifiedSingleEditOperation>
                {
                    new IdentifiedSingleEditOperation
                    {
                        Range = new BlazorMonaco.Range(lineCount, lastCharInLastLine, lineCount, lastCharInLastLine +1),
                        Text = newText,
                        ForceMoveMarkers = true
                    }
                },
                new List<Selection> {endOfDocumentSelection}
            );

            _readOnlyPosition = new Position{LineNumber = lineCount + 2, Column = 2};
            Console.WriteLine($"ConsolePw: Appended output '{text}'. New read-only position is {_readOnlyPosition.LineNumber}:{_readOnlyPosition.Column}.");
    }
    private async Task HandleContentChanged(ModelContentChangedEvent eventArgs)
    {
        if(_console == null) return;
        if (_ignoreNextContentChangedEvent)
        {
            _ignoreNextContentChangedEvent = false; 
            return; 
        }

        foreach (var change in eventArgs.Changes)
        {
            Position changeStart = new Position{LineNumber = change.Range.StartLineNumber, Column  = change.Range.StartColumn};

            bool isTryingToModifyReadOnlyZone = false;

            // Empieza dentro de la zona de solo lectura (_readOnlyPosition)
            if (changeStart.LineNumber < _readOnlyPosition.LineNumber ||
                (changeStart.LineNumber == _readOnlyPosition.LineNumber && changeStart.Column < _readOnlyPosition.Column))
            {
                isTryingToModifyReadOnlyZone = true;
            }
            // Empieza exactamente en el límite (_readOnlyEndPosition)
            else if (changeStart.LineNumber == _readOnlyPosition.LineNumber && changeStart.Column == _readOnlyPosition.Column)
            {
                // Eliminacion o remplazo
                if (change.Text == "" || (change.Text.Length < (change.Range.EndColumn - change.Range.StartColumn)))
                {
                    isTryingToModifyReadOnlyZone = false;
                }
            }

            if (isTryingToModifyReadOnlyZone)
            {
                Console.WriteLine("DEBUG: ¡Intento de modificar zona de solo lectura detectado! Revirtiendo.");

                _ignoreNextContentChangedEvent = true;
                await _console.SetValue(_currentOfficialContent);

                Selection newSelection = new Selection
                {
                    StartLineNumber = _readOnlyPosition.LineNumber,
                    StartColumn = _readOnlyPosition.Column,
                    EndLineNumber = _readOnlyPosition.LineNumber,
                    EndColumn = _readOnlyPosition.Column,
                    SelectionStartLineNumber = _readOnlyPosition.LineNumber,
                    SelectionStartColumn = _readOnlyPosition.Column,
                    PositionLineNumber = _readOnlyPosition.LineNumber,
                    PositionColumn = _readOnlyPosition.Column
                };
                await _console.SetSelection(newSelection, "");
                await _console.RevealPositionInCenterIfOutsideViewport(_readOnlyPosition);
                return;
            }
        }
    }
    public async Task ClearConsole()
    {
        if (_console == null) return;

        Console.WriteLine("DEBUG: Limpiando consola...");

        _currentOfficialContent = _value;
        _readOnlyPosition = new Position { LineNumber = 1, Column = 2 };
        _ignoreNextContentChangedEvent = false;
        await _console.SetValue(_currentOfficialContent);


        Selection initialPosition = new Selection
        {
            StartLineNumber = _readOnlyPosition.LineNumber,
            StartColumn = _readOnlyPosition.Column,
            EndLineNumber = _readOnlyPosition.LineNumber,
            EndColumn = _readOnlyPosition.Column,
            PositionLineNumber = _readOnlyPosition.LineNumber,
            PositionColumn = _readOnlyPosition.Column,
            SelectionStartLineNumber = _readOnlyPosition.LineNumber, 
            SelectionStartColumn = _readOnlyPosition.Column
        };
        await _console.SetSelection(initialPosition, "");
        await _console.RevealPositionInCenterIfOutsideViewport(_readOnlyPosition);

        Console.WriteLine("DEBUG: Consola limpia y cursor restablecido.");
    }

}