﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebsocktChatRoom;

public class WebsocketClient
{
    public WebSocket WebSocket { get; set; }

    public string Id { get; set; }

    public string RoomNumber { get; set; }

    public Task SendMessageAsync(string message)
    {
        byte[] msg = Encoding.UTF8.GetBytes(message);
        return WebSocket.SendAsync(
            new ArraySegment<byte>(msg, 0, msg.Length), 
            WebSocketMessageType.Text, 
            true, 
            CancellationToken.None);
    }
}
