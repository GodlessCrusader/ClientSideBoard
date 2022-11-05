using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ClientSideBoard
{
    public class GameRenderHandler
    {
        public EventHandler GameRenderRequired;
        private Canvas2DContext _canvasContext;
        private GameModel.Game _gameModel;
        private bool _initialized = false;
        public async Task InitializeAsync(GameModel.Game game, Canvas2DContext canvas2DContext)
        {
            _canvasContext = canvas2DContext;
            _gameModel = game;
            _initialized = true;
            await RenderBoardAsync();
        }
        private async Task RenderBoardAsync()
        {
            if (!_initialized)
                throw new InvalidOperationException("Call InitializeAsync to start render handler");

        }
    }
}
