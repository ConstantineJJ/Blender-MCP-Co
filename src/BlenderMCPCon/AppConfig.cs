using System.Text.Json.Serialization;

namespace BlenderMCPCon;

public sealed class AppConfig
{
    public string BlenderExecutable { get; set; } = "";
    public string TunnelClientExecutable { get; set; } = "tunnel-client";
    public string TunnelProfile { get; set; } = "blender-local";
    public string TunnelId { get; set; } = "";
    public string McpCommand { get; set; } = DefaultMcpCommand();
    public string BlenderHost { get; set; } = "127.0.0.1";
    public int BlenderPort { get; set; } = 9876;
    public string RuntimeApiKeyEnvironmentVariable { get; set; } = "CONTROL_PLANE_API_KEY";
    public bool AutoLaunchBlender { get; set; } = true;

    [JsonIgnore]
    public string RuntimeApiKey => string.IsNullOrWhiteSpace(RuntimeApiKeyEnvironmentVariable)
        ? ""
        : Environment.GetEnvironmentVariable(RuntimeApiKeyEnvironmentVariable) ?? "";

    public static AppConfig CreateDefault() => new();

    private static string DefaultMcpCommand()
    {
        var profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(profile, "chatgpt-blender-mcp", "run_blender_mcp_server.cmd");
    }
}
