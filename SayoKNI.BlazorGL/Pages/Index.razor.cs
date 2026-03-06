using Microsoft.JSInterop;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace SayoKNI.Pages
{
    public partial class Index
    {
        Game _game;
        DotNetObjectReference<Index> _objRef;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                // init game
                if (_game == null)
                {
                    //float dpr = (float)await JsRuntime.InvokeAsync<double>("getDevicePixelRatio");
                    _game = new SayoKNIGame();
                    _objRef ??= DotNetObjectReference.Create(this);
                    await JsRuntime.InvokeAsync<object>("initRenderJS", _objRef);
                    _game.Run();
                }
            }
        }

        [JSInvokable]
        public void TickDotNet()
        {
            // run gameloop
            _game?.Tick();
        }

    }
}
