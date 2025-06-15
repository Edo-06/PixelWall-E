using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp.Processing;

namespace PixelWall_E.Components;
public partial class CanvasGrid
{
    public ElementReference _numberOfPixels { get; set; }
    public int numberOfPixels { get; set; } = 37;
    public Image<Rgba32>? image;
    private string? imageDataUrl { get; set; }
    protected override void OnInitialized()
    {
        base.OnInitialized();
        CreateImage();
    }
    public void CreateImage()
    {
        image?.Dispose();

        image = new Image<Rgba32>(numberOfPixels, numberOfPixels);
        for (int y = 0; y < numberOfPixels; y++)
        {
            for (int x = 0; x < numberOfPixels; x++)
            {
                image[x, y] = new Rgba32(255, 255, 255); // Color blanco
            }
        }
        UpdateImageDisplay();
        PipeLineManager.canvas = this;
    }
    public async Task ChangeNumberOfPixels()
    {
        PipeLineManager.ReStart();
        var numberOfPixels = await jsRuntime.InvokeAsync<int>("getNumberOfPixels", _numberOfPixels);
        if (numberOfPixels < 1 || numberOfPixels > 256)
        {
            Console.WriteLine("Error: El número de píxeles debe estar entre 1 y 256.");
            return;
        }
        this.numberOfPixels = numberOfPixels;
        CreateImage();
    }
    private void UpdateImageDisplay()
    {
        if (image == null) return;

        using var stream = new MemoryStream();
        image.SaveAsPng(stream);
        imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(stream.ToArray())}";
        StateHasChanged();
    }

    public void Dispose()
    {
        image?.Dispose();
    }
    public void ChangePixelColor(int x, int y, Rgba32 color)
    {
        if (image == null) return;

        if (x < 0 || x >= numberOfPixels || y < 0 || y >= numberOfPixels)
        {
            Console.WriteLine($"Error: Coordenadas fuera de rango ({x}, {y})");
            return;
        }
        image[x, y] = color;
        UpdateImageDisplay();
    }
    public Rgba32 GetPixelColor(int x, int y)
    {
        if (image == null) return new Rgba32(0, 0, 0, 0);
        return image[x, y];
    }
}
