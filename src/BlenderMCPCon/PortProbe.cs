using System.Net.Sockets;

namespace BlenderMCPCon;

public static class PortProbe
{
    public static async Task<bool> CanConnectAsync(string host, int port, int timeoutMs = 300)
    {
        if (string.IsNullOrWhiteSpace(host) || port is < 1 or > 65535)
            return false;

        try
        {
            using var client = new TcpClient();
            using var cts = new CancellationTokenSource(timeoutMs);
            await client.ConnectAsync(host, port, cts.Token);
            return client.Connected;
        }
        catch
        {
            return false;
        }
    }
}
