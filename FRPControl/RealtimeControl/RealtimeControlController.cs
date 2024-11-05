using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace FRPControl.RealtimeControl;

public class RealtimeControlController : ControllerBase
{
    /// <summary>
    ///     Realtime control endpoint
    /// </summary>
    [Route("/v1/control")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await HandleMessage(ws);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async Task HandleMessage(WebSocket ws)
    {
        byte[] buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            await ws.SendAsync(
                new ArraySegment<byte>(buffer, 0, result.Count),
                result.MessageType,
                result.EndOfMessage,
                CancellationToken.None
            );

            result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}
