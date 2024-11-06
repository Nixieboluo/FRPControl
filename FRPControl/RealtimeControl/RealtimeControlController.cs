using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using FRPControl.RealtimeControl.Actions;
using FRPControl.RealtimeControl.Actions.Handlers;
using FRPControl.RealtimeControl.Actions.Payloads;
using Microsoft.AspNetCore.Mvc;

namespace FRPControl.RealtimeControl;

public class RealtimeControlController(ILogger<RealtimeControlController> logger) : ControllerBase
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
            var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await HandleConnection(ws);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task HandleConnection(WebSocket ws)
    {
        var buffer = new byte[1024 * 4];
        var data = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        try
        {
            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                data.Write(buffer, 0, result.Count);

                if (result.EndOfMessage)
                {
                    data.Seek(0, SeekOrigin.Begin);
                    var json = Encoding.UTF8.GetString(data.ToArray());
                    data.SetLength(0);

                    await HandleMessageAsync(ws, json, CancellationToken.None);
                }
            }
        }
        catch (WebSocketException e)
        {
            logger.LogWarning("WebSocket connection closed unexpectedly:" + e.Message);
        }
        finally
        {
            if (ws.State != WebSocketState.Open)
                await ws.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "WebSocket connection gracefully closed",
                    CancellationToken.None
                );
        }
    }

    private async Task HandleMessageAsync(WebSocket ws, string messageString, CancellationToken cancellationToken)
    {
        try
        {
            // Deserialize message
            var deserializeOptions = new JsonSerializerOptions
            {
                Converters = { new RequestConverter() },
                WriteIndented = true,
            };

            var message = JsonSerializer.Deserialize<RequestMessage<IActionPayload>>(messageString, deserializeOptions);
            if (message == null)
            {
                logger.LogWarning("Received unexpected message: " + messageString);
                return;
            }

            logger.LogDebug("Received message: " + message);

            // Do handling
            var handler = message.ActionType switch
            {
                ActionType.GetServerConfig => new GetServerConfigHandler(),
                _ => null,
            };
            if (handler == null)
            {
                logger.LogWarning("No handler found for: " + message.ActionType);
                return;
            }

            var response = await handler.HandleMessageAsync((GetServerConfigPayload)message.Payload, cancellationToken);
            await ws.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response))),
                WebSocketMessageType.Text,
                true,
                cancellationToken
            );
        }
        catch (JsonException e)
        {
            logger.LogWarning($"Cannot deserialize WebSocket message ${e.Message}");
        }
    }
}
