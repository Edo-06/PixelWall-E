using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BlazorMonaco;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace PixelWall_E.Components;
public partial class CodeEditor
{
    public delegate Task OnRunEnd(string message);
    public event OnRunEnd OnRunEndEvent = delegate { return Task.CompletedTask; };
    public string code {get; private set;} = "";
    private string _selectedTheme = "vs-light";
    public StandaloneCodeEditor editor
    {
        get => _editor;
        set
        {
            if (_editor != value)
            {
                _editor = value; 
            }
        }
    }
    [AllowNull]
    private StandaloneCodeEditor _editor;
    private static StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            Language = "pixelwalle",
            Theme = "pixelwalle-light-theme",
            GlyphMargin = true,
            AutomaticLayout = true,
            Value = _value,   
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
    public async Task ChangeTheme(ChangeEventArgs e)
    {
        string? newTheme = e.Value?.ToString();
        if (newTheme != null && newTheme != _selectedTheme)
        {
            string codeThemeName = $"pixelwalle-{newTheme}";
            Console.WriteLine($"setting theme to: code");
            await Global.SetTheme(jsRuntime, codeThemeName);
            _selectedTheme = newTheme;
        }
    }
    public async Task Run()
    {
        if(PipeLineManager.isRunning)
        {
            Console.WriteLine("Ya hay un proceso en ejecución.");
            PipeLineManager.isRunning = true;
            return;
        }
        PipeLineManager.ReStart();
        code = await _editor.GetValue();
        Console.WriteLine(code);
        await PipeLineManager.Start(code);
        if(PipeLineManager.isRunning)
        {
            await OnRunEndEvent.Invoke("Code executed successfully.");
            Console.WriteLine("Código ejecutado exitosamente.");
            PipeLineManager.isRunning = false;
        }
        else
        {
            await OnRunEndEvent.Invoke("Code execution has finished");
            Console.WriteLine("La ejecución del código ha finalizado.");
        }
    }
    public async Task LoadFile()
    {
        await jsRuntime.InvokeVoidAsync("clickFileInput", "fileInput");
    }
    public async Task HandleFileSelected(InputFileChangeEventArgs e)
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
    
#region InitialCode
    private static string _value = @"Spawn(25,21)
Color(""NavyBlue"")

x <- GetActualX()
y <- GetActualY()

DrawLine(0,-1,3)
DrawLine(-1,0,3)
DrawLine(1,1,3)
MoveTo(x-1 , y-2)
Fill()
x <- GetActualX()
y <- GetActualY()

Color(""MediumBlue"")
MoveTo(x+2, y+2)
x <- GetActualX()
y <- GetActualY()
DrawLine(1,-1,6)
DrawLine(-1,0,6)
DrawLine(0,1,6)
MoveTo(x+1,y-2)
Fill()
x <- GetActualX()
y <- GetActualY()

Color(""Blue"")
MoveTo(x+6, y-5)
x <- GetActualX()
y <- GetActualY()
DrawLine(-1,-1,12)
DrawLine(0,1,12)
DrawLine(1,0,12)
MoveTo(x-2, y-1)
Fill()

x <- GetActualX()
y <- GetActualY()

Color(""SkyBlue"")
MoveTo(x-10, y-10)
x <- GetActualX()
y <- GetActualY()
MoveTo(x ,y-1)
DrawLine(0,0,1)
MoveTo(x,y)
DrawLine(-1,1,18)
DrawLine(1,0,18)
DrawLine(0,-1,18)
MoveTo(x-3, y+5)
Fill()
x <- GetActualX()
y <- GetActualY()

Color(""Black"")
MoveTo(x-16,y+24)
DrawLine(0,-1,4)
DrawLine(1,1,2)
DrawLine(1,-1,2)
DrawLine(0,1,4)
x <- GetActualX()
y <- GetActualY()

MoveTo(x+2,y)
DrawLine(0,-1,3)
DrawLine(1,-1,1)
DrawLine(1,0,1)
DrawLine(1,1,1)
DrawLine(0,1,3)
DrawLine(0,-1,2)
DrawLine(-1,0,2)
x <- GetActualX()
y <- GetActualY()

MoveTo(x+4,y-2)
DrawLine(1,0,4)
DrawLine(-1,0,2)
DrawLine(0,1,4)
x <- GetActualX()
y <- GetActualY()

MoveTo(x+7,y-4)
DrawLine(-1,0,3)
DrawLine(0,1,4)
DrawLine(1,0,3)
x <- GetActualX()
y <- GetActualY()

MoveTo(x+2,y-4)
DrawLine(0,1,4)
DrawLine(1,0,4)
DrawLine(0,-1,4)
DrawLine(-1,0,4)
x <- GetActualX()
y <- GetActualY()

MoveTo(x+6,y+4)
DrawLine(0,-1,4)
DrawLine(1,1,2)
DrawLine(1,-1,2)
DrawLine(0,1,4)";
#endregion
}