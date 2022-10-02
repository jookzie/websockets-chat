using System.Collections.Generic;
using System.Linq;

namespace ChatBot.Example;

public class WebsocketClientCollection
{
    private static readonly List<WebsocketClient> _clients = new();

    public static void Add(WebsocketClient client)
    {
        _clients.Add(client);
    }

    public static void Remove(WebsocketClient client)
    {
        _clients.Remove(client);
    }

    public static WebsocketClient Get(string clientId)
    {
        return _clients.FirstOrDefault(c => c.Id == clientId);
    }

    public static List<WebsocketClient> GetRoomClients(string roomNo)
    {
        return _clients.Where(c => c.RoomNumber == roomNo).ToList();
    }
}
