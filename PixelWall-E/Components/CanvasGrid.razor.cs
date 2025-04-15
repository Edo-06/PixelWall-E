using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PixelWall_E.Components;

public partial class CanvasGrid
{
    private int Size { get; set; } = 840;
    private bool ShowGrid { get; set; } = true;
    private string GridColor { get; set; } = "#000000";
    
    private ElementReference _canvasElement;

    private float pixelSize { get; set;} 

    private int numberOfPixels = 33;

    protected override async Task OnAfterRenderAsync(bool firstRender) 
    {
        if (firstRender) 
        {
            await InitializeCanvas();
        }
    }

    private async Task InitializeCanvas() 
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
        await InitializeCanvas();
    }

    private void CalculatePixelSize()
    {
        pixelSize = (float)Size/numberOfPixels;
    }
}
