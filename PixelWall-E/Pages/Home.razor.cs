using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PixelWall_E.Pages;
public partial class Home
{
    private ElementReference splitContainerRef;
    private ElementReference splitterRef;
    private ElementReference topPanelRef;
    private ElementReference bottomPanelRef;

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
}