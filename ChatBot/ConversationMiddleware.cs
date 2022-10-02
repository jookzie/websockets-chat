using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using ChatBot.Models;
using System.Collections.Generic;
using ChatBot.Models.DTOs;
using System.Linq;

namespace ChatBot
{
    public class ConversationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private static readonly List<WebSocketClient> _clients = new();
        
        public ConversationMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ConversationMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path != "/ws")
            {
                await _next(context);
                return;
            }
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 404;
                return;
            }
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var wsClient = new WebSocketClient(new Anonymous(), webSocket);
            try
            {
                await Handle(wsClient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Echo websocket client {wsClient.Participant.ID} error.");
                await context.Response.WriteAsync("closed");
            }
        }

        private async Task Handle(WebSocketClient webSocket)
        {
            _clients.Add(webSocket);
            _logger.LogInformation($"Websocket client added.");

            WebSocketReceiveResult result;
            do
            {
                var buffer = new byte[1024 * 1];
                result = await webSocket.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text && !result.CloseStatus.HasValue)
                {
                    var msgString = Encoding.UTF8.GetString(buffer);
                    _logger.LogInformation($"Websocket client ReceiveAsync message {msgString}.");
                    var message = JsonConvert.DeserializeObject<MessageDTO>(msgString);
                    message.AuthorID = webSocket.Participant.ID;
                    Broadcast(message);
                }
            }
            while (!result.CloseStatus.HasValue);
            _clients.Remove(webSocket);
            _logger.LogInformation($"Websocket client disconnected.");
        }

        private void Broadcast(MessageDTO message)
        {
            var client = _clients.First(c => c.Participant.ID == message.AuthorID);
            _clients.ForEach(c =>
            {
                c.SendMessageAsync(message.Content);
            });
            _logger.LogInformation($"Websocket client ID: '{message.AuthorID}' sent a message: '{message.Content}'.");
        }
    }
}
