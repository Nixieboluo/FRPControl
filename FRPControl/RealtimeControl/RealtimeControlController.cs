using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using FRPControl.RealtimeControl.Actions;
using FRPControl.RealtimeControl.Connection;
using Microsoft.AspNetCore.Mvc;

namespace FRPControl.RealtimeControl;

public class RealtimeControlController(ILogger<RealtimeControlController> logger, SessionManager sessionManager)
    : ControllerBase
{
    /// <summary>
    ///     Realtime control endpoint
    /// </summary>
    [Route("/v1/control")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task HandleHandshakeAsync()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();

            var ctx = new RealtimeControlContext { Connection = ws };
            // [TODO] Identify clients based on some actual properties.
            sessionManager.AddSession(ws.GetHashCode().ToString(), ctx);

            await HandleConnectionAsync(ctx);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task HandleConnectionAsync(RealtimeControlContext ctx)
    {
        var buffer = new byte[1024 * 4];
        var data = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        try
        {
            while (ctx.Connection.State == WebSocketState.Open)
            {
                var result = await ctx.Connection.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.MessageType != WebSocketMessageType.Text)
                    continue;

                data.Write(buffer, 0, result.Count);

                if (result.EndOfMessage)
                {
                    data.Seek(0, SeekOrigin.Begin);
                    var json = Encoding.UTF8.GetString(data.ToArray());
                    data.SetLength(0);
                    await HandleMessageAsync(ctx, json, CancellationToken.None);
                }
            }
        }
        catch (WebSocketException e)
        {
            logger.LogWarning("WebSocket connection closed unexpectedly: " + e.Message);
        }
        finally
        {
            if (ctx.Connection.State != WebSocketState.Open)
            {
                await ctx.Connection.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "WebSocket connection gracefully closed.",
                    CancellationToken.None
                );

                sessionManager.RemoveSession(ctx.Connection.GetHashCode().ToString());
            }
        }
    }

    private async Task HandleMessageAsync(
        RealtimeControlContext ctx,
        string messageString,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // Deserialize message
            var payload = ActionMessageDecoder.Decode(messageString);

            // Execute handler
            await payload.Handle(ctx);
        }
        catch (JsonException e)
        {
            logger.LogWarning($"Cannot deserialize WebSocket message: {e.Message}");
        }
    }
}
