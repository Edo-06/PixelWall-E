using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PixelWall_E.Components;
public partial class CanvasGrid
{
    private ElementReference _numberOfPixels;
    private int Size = 840;
    private bool showGrid { get; set; } = true;
    private string gridColor { get; set; } = "#000000";
    private ElementReference _canvasElement;
    private float pixelSize { get; set;} 
    private string id { get; set;} = "pixelCanvas";
    public int numberOfPixels {get; set;} = 33;
    public List<float> X {get; set;} = new List<float>(); 
    public CanvasGrid(){}

    protected override async Task OnAfterRenderAsync(bool firstRender) 
    {
        if (firstRender) 
        {
            await InitializeCanvas(id);
        }
    }

    public async Task InitializeCanvas(string id) 
    {
        CalculatePixelSize();

        await jsRuntime.InvokeVoidAsync("clearCanvas", id, "#FFFFFF");
        if (showGrid) 
        {
            await jsRuntime.InvokeVoidAsync("drawGrid", id, pixelSize, gridColor);
        }
        PipeLineManager.canvas = this;
    }

    public async Task ChangeGridSize()
    {
        var numberOfPixels = await jsRuntime.InvokeAsync<int>("getNumberOfPixels", _numberOfPixels);
        if (numberOfPixels < 1 || numberOfPixels > 256)
        {
            Console.WriteLine("Error: El número de píxeles debe estar entre 1 y 256.");
            return;
        }
        this.numberOfPixels = numberOfPixels;
        await InitializeCanvas(id);
    }
    private void CalculatePixelSize()
    {
        pixelSize = (float)Size/numberOfPixels;
    }
    public async Task Clear()
    {
        await jsRuntime.InvokeVoidAsync("clearCanvas", id, "#FFFFFF");
    }
    public async Task ChangePixelColor(int x, int y, string color)
    {
        await jsRuntime.InvokeVoidAsync("fillPixel", id, pixelSize, x, y, color);
    }
}
