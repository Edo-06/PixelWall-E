using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace PixelWall_E.Pages;
public partial class Home
{
    private ElementReference splitContainerRef;
    private ElementReference splitterRef;
    private ElementReference topPanelRef;
    private ElementReference bottomPanelRef;
    [AllowNull] private Components.CodeEditor codeEditorRef;
    [AllowNull] private Components.ConsolePw consoleRef;
    private string _currentMonacoTheme = "vs-light"; 
    private string currentMonacoTheme => $"monaco-{_currentMonacoTheme}";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync(
                    "splitterJsInterop.initialize",
                    splitterRef,
                    topPanelRef,
                    bottomPanelRef,
                    splitContainerRef
                );
                Console.WriteLine("JS Interop call successful!");
            }
            catch (JSException ex)
            {
                Console.WriteLine($"JS Interop error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled error in OnAfterRenderAsync: {ex.Message}");
                throw;
            }
        }
    }
    public void HandleMonacoThemeChanged(string newMonacoTheme)
    {
        if (_currentMonacoTheme != newMonacoTheme)
        {
            _currentMonacoTheme = newMonacoTheme;
            StateHasChanged(); 
            Console.WriteLine($"Home: Monaco theme changed to {_currentMonacoTheme}. Splitter class updated.");
        }
    }
    private async Task HandleConsoleKeyDown(KeyboardEventArgs e)
    {
        Console.WriteLine($"Home: Key '{e.Key}' pressed in ConsolePw.");
        await consoleRef.HandleEnter();
        if (codeEditorRef != null )
        {
            string userCommand = await consoleRef.HandleEnter();
            switch (userCommand)
            {
                case "run":
                    // Lógica para el comando "run"
                    if (codeEditorRef != null)
                    {
                        await codeEditorRef.Run();
                        await consoleRef.AppendOutput("Running");
                    }
                    else
                    {
                        await consoleRef.AppendOutput("Error: Editor no disponible para ejecutar.");
                    }
                    break;

                case "clear":

                    await consoleRef.ClearConsole();
                    //await consoleRef.AppendOutput("Consola limpiada.\n"); // Opcional: un mensaje de confirmación
                    break;

                case "help":
                    await consoleRef.AppendOutput("Available commands:"); 
                    await consoleRef.AppendOutput("  run   - Executes the code in the editor."); 
                    await consoleRef.AppendOutput("  clear - Clears the console."); 
                    await consoleRef.AppendOutput("  help  - Displays this help message.");
                    break;

                case "":
                    break;

                default:
                    await consoleRef.AppendOutput($"Unrecognized command: '{userCommand}'. Type 'help' for commands.");
                    break;
            }
            Console.WriteLine("Home: ConsolePw key down event handled by CodeEditor.");
        }
        else
        {
            Console.WriteLine("Home: CodeEditor reference is null, cannot handle key down event.");
        }
    }
    private async Task HandleClearConsole()
    {
        if (consoleRef != null)
        {
            await consoleRef.ClearConsole();
            Console.WriteLine("Home: Console cleared.");
        }
        else
        {
            Console.WriteLine("Home: ConsolePw reference is null, cannot clear console.");
        }
    }
}