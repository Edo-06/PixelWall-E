using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using PixelWall_E.Components;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;

using SixLabors.ImageSharp.Processing;
namespace PixelWall_E.Pages;
public partial class Home
{
    private ElementReference splitContainerRef;
    private ElementReference splitterRef;
    private ElementReference topPanelRef;
    private ElementReference bottomPanelRef;
    private ElementReference numberOfPixels;
    private InputFile ?inputFileElement;
    private ElementReference filenameInput;
    private ElementReference imagenameInput;
    private string dialogueIStyle = "display: none;";
    private string dialogueFStyle = "display: none;";
    [AllowNull] private CodeEditor codeEditorRef;
    [AllowNull] private ConsolePw consoleRef;
    [AllowNull] private CanvasGrid canvasGridRef;
    private string _currentMonacoTheme = "vs-light"; 
    private string currentMonacoTheme => $"monaco-{_currentMonacoTheme}";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            PipeLineManager.OnErrorsDetected += HandleErrors;
            if(codeEditorRef!=null)
            {
                codeEditorRef.OnRunEndEvent += HandleConsoleLog;
            }
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
    public async Task HandleMonacoThemeChanged(ChangeEventArgs e)
    {
        string newMonacoThemeBase = e.Value?.ToString() ?? "vs-light";
        if (_currentMonacoTheme != newMonacoThemeBase)
        {
            _currentMonacoTheme = newMonacoThemeBase;
            StateHasChanged(); 
            Console.WriteLine($"Home: Monaco theme changed to {_currentMonacoTheme}. Splitter class updated.");
            await codeEditorRef.ChangeTheme(e);
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
                        await consoleRef.AppendOutput("Running");

                        if(PipeLineManager.isRunning)
                        {
                            await consoleRef.AppendOutput("Pipeline is already running.");
                            return;
                        }
                        await codeEditorRef.Run();
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
    private async Task HandleConsoleLog(string message)
    {
        await consoleRef.AppendOutput(message);
    }
    private async Task HandleErrors(Exception error)
    {
        if (consoleRef != null)
        {
            await consoleRef.AppendOutput(error.ToString());
        }
        else
        {
            Console.WriteLine("Home: ConsolePw reference is null, cannot handle pipeline errors.");
        }
    }
    private async Task HandleExecute()
    {
        Console.WriteLine("Home: HandleExecute called.");
        if (codeEditorRef != null)
        {
            await codeEditorRef.Run();
        }
        else
        {
            Console.WriteLine("Home: CodeEditor reference is null, cannot execute code.");
        }
    }
    private async Task HandleResize()
    {
        canvasGridRef._numberOfPixels = numberOfPixels;
        await canvasGridRef.ChangeNumberOfPixels();
    }
    private async Task HandleLoadFile()
    {
        Console.WriteLine("Home: HandleLoad called.");
        if (codeEditorRef != null)
        {
            await codeEditorRef.LoadFile();
        }
        else
        {
            Console.WriteLine("Home: CodeEditor reference is null, cannot load file.");
        }
    }
    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        await codeEditorRef.HandleFileSelected(e);
    }
    private async Task HandleSaveImage()
    {
        dialogueIStyle = "display: block;";
        await Task.Delay(10);
        await jsRuntime.InvokeVoidAsync("focusInput", filenameInput);
    }
    private async Task HandleSaveFile()
    {
        dialogueFStyle = "display: block;";
        await Task.Delay(10);
        await jsRuntime.InvokeVoidAsync("focusInput", filenameInput);
    }
    private async Task HandleConfirmSaveImage()
    {
        var name = await jsRuntime.InvokeAsync<string>("getFileName", filenameInput);

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Nombre de archivo inválido o vacío.");
            return;
        }

        if (!name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        {
            name += ".png";
        }

        using var scaledImage = canvasGridRef.image?.Clone(x => x.Resize(new ResizeOptions
        {
            Size = new Size(500, 500),
            
            Mode = ResizeMode.Stretch, 
            
            Sampler = KnownResamplers.NearestNeighbor 
        }));

        using var streamForDownload = new MemoryStream();
        scaledImage.SaveAsPng(streamForDownload); 

        var dataUrlForDownload = $"data:image/png;base64,{Convert.ToBase64String(streamForDownload.ToArray())}";
        await jsRuntime.InvokeVoidAsync("saveAsFile", name, dataUrlForDownload);
        dialogueIStyle = "display: none;";
        StateHasChanged();
    }
    private void HandleCancelSaveImage()
    {
        dialogueIStyle = "display: none;";
        StateHasChanged();
    }
    private void HandleCancelSaveFile()
    {
        dialogueFStyle = "display: none;";
        StateHasChanged();
    }
    private async Task HandleConfirmSaveFile()
    {
        string temporaryContent = await codeEditorRef.editor.GetValue();
        var name = await jsRuntime.InvokeAsync<string>("getFileName", imagenameInput);
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Nombre inválido");
            return;
        }

        if (!name.EndsWith(".pw")) name += ".pw";

        await jsRuntime.InvokeVoidAsync("downloadFile", name, temporaryContent);
        dialogueFStyle = "display: none;";
    }
}