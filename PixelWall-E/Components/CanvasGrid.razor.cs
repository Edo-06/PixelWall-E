using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PixelWall_E.Components;
public partial class CanvasGrid
{
    private ElementReference _numberOfPixels;
    private int Size { get; set; } = 840;
    private bool ShowGrid { get; set; } = true;
    private string GridColor { get; set; } = "#000000";
    private ElementReference _canvasElement;
    private float pixelSize { get; set;} 
    public int numberOfPixels {get; set;} = 33;
    public CanvasGrid(){}

    protected override async Task OnAfterRenderAsync(bool firstRender) 
    {
        if (firstRender) 
        {
            await InitializeCanvas();
        }
    }

    public async Task InitializeCanvas() 
    {
        CalculatePixelSize();
        await jsRuntime.InvokeVoidAsync("clearCanvas", "pixelCanvas", "#FFFFFF");
        if (ShowGrid) 
        {
            await jsRuntime.InvokeVoidAsync("drawGrid", "pixelCanvas", pixelSize, GridColor);
        }
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
        await InitializeCanvas();
    }

    private void CalculatePixelSize()
    {
        pixelSize = (float)Size/numberOfPixels;
    }
}
