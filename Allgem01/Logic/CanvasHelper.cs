using Microsoft.JSInterop;
using System.Threading.Tasks;

public static class CanvasHelper
{
    public class Canvas2DContext
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _canvasId;

        public Canvas2DContext(IJSRuntime jsRuntime, string canvasId)
        {
            _jsRuntime = jsRuntime;
            _canvasId = canvasId;
        }

        public Task BeginPathAsync() =>
            _jsRuntime.InvokeVoidAsync("eval", $"document.getElementById('{_canvasId}').getContext('2d').beginPath()").AsTask();

        public Task ArcAsync(double x, double y, double radius, double startAngle, double endAngle) =>
            _jsRuntime.InvokeVoidAsync("eval", $"document.getElementById('{_canvasId}').getContext('2d').arc({x}, {y}, {radius}, {startAngle}, {endAngle})").AsTask();

        public Task SetFillStyleAsync(string color) =>
            _jsRuntime.InvokeVoidAsync("eval", $"document.getElementById('{_canvasId}').getContext('2d').fillStyle = '{color}'").AsTask();

        public Task FillAsync() =>
            _jsRuntime.InvokeVoidAsync("eval", $"document.getElementById('{_canvasId}').getContext('2d').fill()").AsTask();

        public Task ClosePathAsync() =>
            _jsRuntime.InvokeVoidAsync("eval", $"document.getElementById('{_canvasId}').getContext('2d').closePath()").AsTask();

        public Task ClearRectAsync(double x, double y, double width, double height) =>
            _jsRuntime.InvokeVoidAsync("eval", $"document.getElementById('{_canvasId}').getContext('2d').clearRect({x}, {y}, {width}, {height})").AsTask();
    }

    public static Task<Canvas2DContext> InitializeCanvasAsync(string canvasId)
    {
        var jsRuntime = AppDomain.CurrentDomain.GetData("JSRuntime") as IJSRuntime 
            ?? throw new InvalidOperationException("JSRuntime is not available in the current AppDomain.");
        return Task.FromResult(new Canvas2DContext(jsRuntime, canvasId));
    }
}