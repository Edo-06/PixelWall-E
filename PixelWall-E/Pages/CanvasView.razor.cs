using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PixelWall_E.Pages;

public partial class CanvasView
{
    private int Size { get; set; } = 845;
    private bool ShowGrid { get; set; } = true;
    private string GridColor { get; set; } = "#dc3545";
    
    private ElementReference _canvasElement;
    private string[,]? _pixels;

    protected override async Task OnAfterRenderAsync(bool firstRender) 
    {
        if (firstRender) 
        {
            await InitializeCanvas();
        }
    }

    private async Task InitializeCanvas() 
    {
        await jsRuntime.InvokeVoidAsync("clearCanvas", "pixelCanvas", "#FFFFFF");
        if (ShowGrid) 
        {
            await jsRuntime.InvokeVoidAsync("drawGrid", "pixelCanvas", 845/20, GridColor);
        }
    }

}
