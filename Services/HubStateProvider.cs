﻿using ClientSideBoard.Components;
using GameModel;
using Microsoft.AspNetCore.SignalR.Client;
using System.Data;

namespace ClientSideBoard.Services
{
    public class HubStateProvider
    {
        
        private const string SERVER_URI = "https://www.localhost:5001";
        private HubConnection _connection { get; set; } = null;
        public HubConnection Connection => _connection;

        private Chat? _chat = null;
        public Chat? Chat
        {
            get
            {
                if (IsConnected)
                    return _chat;
                else
                    throw new InvalidOperationException("Hub connection isn't established");
            }
            private set
            {
                _chat = value;
            }

        }

        private Game? _game { get; set; } = null;
        public Game? Game {
            get
            {
                if (IsConnected)
                    return _game;
                else
                    throw new InvalidOperationException("Hub connection isn't established");
            }
            private set
            {
                _game = value;
            }

        }

        public event Action StateChangeRequired;

        public bool IsConnected { get; set; } = false;

        public async Task ConnectAsync()
        {
            _connection = new HubConnectionBuilder()
            .WithUrl($"{SERVER_URI}/gamehub")
            .Build();

            _connection.On<Game>("UpdateGameState", (game) => {
                Game = game;
                StateChangeRequired.Invoke();
            });
            _connection.On<Chat>("UpdateChatState", (chat) => {
                Chat = chat;
                StateChangeRequired.Invoke();
            });



            await _connection.StartAsync();

            IsConnected = true;


        }
    }
}
