using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp.Processing;

namespace PixelWall_E.Components;
public partial class CanvasGrid
{
    private int Size { get; set; } = 840;
    private ElementReference _numberOfPixels { get; set; }
    public int numberOfPixels { get; set; } = 200;
    private Image<Rgba32>? image;
    private string? imageDataUrl { get; set; }
    private string dialogueStyle = "display: none;";
    private ElementReference _filenameInput;
    private InputFile? _inputFileElement;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        CreateImage();
    }
    private void CreateImage()
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
    private async Task ChangeNumberOfPixels()
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
    private Rgba32 StringToRgba32(string colorString)
    {
        Rgba32 namedColor = colorString switch
        {
            "Red" => new Rgba32(255, 0, 0),
            "Green" => new Rgba32(0, 255, 0),
            "Blue" => new Rgba32(0, 0, 255),
            "White" => new Rgba32(255, 255, 255),
            "Black" => new Rgba32(0, 0, 0),
            "Yellow" => new Rgba32(255, 255, 0),
            "Orange" => new Rgba32(255, 165, 0),
            "Purple" => new Rgba32(128, 0, 128),
            "NavyBlue" => new Rgba32(0, 0, 128),
            "MediumBlue" => new Rgba32(0, 0, 205),
            "SkyBlue" => new Rgba32(135, 206, 235),

            _ => new Rgba32(0, 0, 0, 0)
        };

        return namedColor;
    }

    private string Rgba32ToString(Rgba32 color)
    {
        string colorString = color switch
        {
            { R: 255, G: 0, B: 0 } => "Red",
            { R: 0, G: 255, B: 0 } => "Green",
            { R: 0, G: 0, B: 255 } => "Blue",
            { R: 255, G: 255, B: 255 } => "White",
            { R: 0, G: 0, B: 0 } => "Black",
            { R: 255, G: 255, B: 0 } => "Yellow",
            { R: 255, G: 165, B: 0 } => "Orange",
            { R: 128, G: 0, B: 128 } => "Purple",
            { R: 0, G: 0, B: 128 } => "NavyBlue",
            { R: 0, G: 0, B: 205 } => "MediumBlue",
            { R: 135, G: 206, B: 235 } => "SkyBlue",
            _ => $"({color.R}, {color.G}, {color.B})"
        };
        return colorString;
    }
    public void ChangePixelColor(int x, int y, string color)
    {
        if (image == null) return;

        Rgba32 newColor = StringToRgba32(color);
        if (x < 0 || x >= numberOfPixels || y < 0 || y >= numberOfPixels)
        {
            Console.WriteLine($"Error: Coordenadas fuera de rango ({x}, {y})");
            return;
        }
        image[x, y] = newColor;
        UpdateImageDisplay();
    }
    public string GetPixelColor(int x, int y)
    {
        if (image == null) return "Error: Imagen no inicializada.";
        return Rgba32ToString(image[x, y]);
    }
    #region Save Image
    private async Task ConfirmSaveImage()
    {
        var name = await jsRuntime.InvokeAsync<string>("getFileName", _filenameInput);

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Nombre de archivo inválido o vacío.");
            return;
        }

        if (!name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        {
            name += ".png";
        }

        using var scaledImage = image?.Clone(x => x.Resize(new ResizeOptions
        {
            Size = new Size(500, 500),
            
            Mode = ResizeMode.Stretch, 
            
            Sampler = KnownResamplers.NearestNeighbor 
        }));

        using var streamForDownload = new MemoryStream();
        scaledImage.SaveAsPng(streamForDownload); 

        var dataUrlForDownload = $"data:image/png;base64,{Convert.ToBase64String(streamForDownload.ToArray())}";
        await jsRuntime.InvokeVoidAsync("saveAsFile", name, dataUrlForDownload);
        dialogueStyle = "display: none;";
        StateHasChanged();
    }
    private void CancelSaveImage()
    {
        dialogueStyle = "display: none;";
        StateHasChanged();
    }
    private async Task ShowDialogueForSave()
    {
        dialogueStyle = "display: block;";
        await Task.Delay(10);
        await jsRuntime.InvokeVoidAsync("focusInput", _filenameInput);
    }
    #endregion
}
