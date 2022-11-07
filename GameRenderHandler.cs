using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ClientSideBoard
{
    public class GameRenderHandler
    {
        private GameModel.Board _currentTab { get; set; }
        public long height { get; private set; } = 2000;
        public long width { get; private set; } = 1000;
        public EventHandler GameRenderRequired;
        private Canvas2DContext _canvasContext;
        private GameModel.Game _gameModel;
        private bool _initialized = false;

        private int _zoomGrade = 100;
        public int ZoomGrade
        {
            get
            {
                return _zoomGrade;
            }
            set
            {
                _zoomGrade = value;
                RenderBoardAsync();
            }
        }
        public async Task InitializeAsync(GameModel.Game game, Canvas2DContext canvas2DContext)
        {
            _canvasContext = canvas2DContext;
            _gameModel = game;
            _initialized = true;
            await RenderBoardAsync();
        }
        public async Task ChangeTabAsync(int tabIndex)
        {
            if (tabIndex <= 0)
                return;
            _currentTab = _gameModel.Tabs[tabIndex - 1];
            await RenderBoardAsync();
        }
        public async Task RenderBoardAsync()
        {
            if (!_initialized)
                throw new InvalidOperationException("Call InitializeAsync to start render handler");
            
            height = (_currentTab.Height) * ZoomGrade / 100;
            width = (_currentTab.Width) * ZoomGrade / 100;

            await RenderGridAsync();
            await RenderTokensAsync();
        }

        
        private async Task RenderGridAsync()
        {
            var blackspaceSize = height / (50 + 1);
            await _canvasContext.SetFillStyleAsync("black");
            await _canvasContext.FillRectAsync(0, 0, width, height);
            await _canvasContext.SetFillStyleAsync("white");
            await _canvasContext.FillRectAsync(blackspaceSize, blackspaceSize, width - blackspaceSize * 2, height - blackspaceSize * 2);
            await _canvasContext.SetFillStyleAsync("black");
            await _canvasContext.BeginPathAsync();
            for (float i = blackspaceSize; i <= height - blackspaceSize; i += blackspaceSize)
            {
                //await _canvasContext.FillRectAsync(i, blackspaceSize, lineWidth, height-blackspaceSize*2);
                await _canvasContext.MoveToAsync(i, blackspaceSize);
                await _canvasContext.LineToAsync(i, height - blackspaceSize);
            }
            for (float i = blackspaceSize; i <= height - blackspaceSize; i += blackspaceSize)
            {
                //await _canvasContext.FillRectAsync(blackspaceSize, i, height-blackspaceSize*2, lineWidth);    
                await _canvasContext.MoveToAsync(blackspaceSize, i);
                await _canvasContext.LineToAsync(height - blackspaceSize, i);
            }
            await _canvasContext.StrokeAsync();

        }

        private async Task RenderTokensAsync()
        {

            /*foreach (var t in _currentTab.Tokens)
            {
                ImageBufferUrl = t.ImageUrl;
                await _canvasContext.DrawImageAsync(ImageBuffer, (t.X - 0.5 * t.Width) * ZoomGrade / 100, (t.Y - 0.5 * t.Height) * ZoomGrade / 100, t.Width * ZoomGrade / 100, t.Height * ZoomGrade / 100);

            }

            foreach (var t in _currentTab._currentTabImages)
            {
                ImageBufferUrl = t.ImageUrl;
                await _canvasContext.DrawImageAsync(ImageBuffer, (t.X - 0.5 * t.Width) * ZoomGrade / 100, (t.Y - 0.5 * t.Height) * ZoomGrade / 100, t.Width * ZoomGrade / 100, t.Height * ZoomGrade / 100);

            }*/
        }
    }
}
