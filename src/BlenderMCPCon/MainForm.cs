using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BlenderMCPCon;

public sealed class MainForm : Form
{
    private AppConfig _config;
    private readonly ManagedProcess _tunnel;
    private readonly System.Windows.Forms.Timer _statusTimer = new() { Interval = 1200 };
    private bool _statusUpdateRunning;

    private readonly Label _blenderStatus = new();
    private readonly Label _bridgeStatus = new();
    private readonly Label _tunnelStatus = new();
    private readonly Label _keyStatus = new();
    private readonly Label _profileLabel = new();
    private readonly RichTextBox _log = new();

    private static readonly Color Good = Color.FromArgb(92, 201, 120);
    private static readonly Color Bad = Color.FromArgb(232, 104, 104);
    private static readonly Color Warn = Color.FromArgb(232, 188, 104);
    private static readonly Color Muted = Color.FromArgb(170, 174, 184);

    public MainForm()
    {
        _config = ConfigStore.Load();
        _tunnel = new ManagedProcess(LogFromAnyThread);

        Text = "Blender MCP_Con";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(860, 660);
        Size = new Size(980, 740);
        BackColor = Color.FromArgb(22, 24, 29);
        ForeColor = Color.Gainsboro;
        Font = new Font("Segoe UI", 10F);

        BuildUi();

        _statusTimer.Tick += async (_, _) => await RefreshStatusAsync();
        _statusTimer.Start();
        Shown += async (_, _) =>
        {
            Log("Blender MCP_Con ready.");
            Log($"Config: {ConfigStore.ConfigPath}");
            Log("Blender bridge target: 127.0.0.1:9876 (configurable).");
            Log("The app does not store API keys.");
            await RefreshStatusAsync();
        };

        FormClosing += (_, _) => _tunnel.Stop();
    }

    private void BuildUi()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            RowCount = 5,
            ColumnCount = 1
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 174));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(root);

        var header = new Panel { Dock = DockStyle.Fill };
        var title = new Label
        {
            Text = "Blender MCP_Con",
            Font = new Font("Segoe UI Semibold", 20F),
            AutoSize = true,
            Location = new Point(0, 2)
        };
        var subtitle = new Label
        {
            Text = "Blender ↔ MCP server ↔ Secure MCP Tunnel",
            ForeColor = Muted,
            AutoSize = true,
            Location = new Point(3, 42)
        };
        header.Controls.Add(title);
        header.Controls.Add(subtitle);
        root.Controls.Add(header, 0, 0);

        var status = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(30, 33, 40),
            Padding = new Padding(16),
            RowCount = 4,
            ColumnCount = 2
        };
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (var i = 0; i < 4; i++)
            status.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

        AddStatusRow(status, 0, "Blender", _blenderStatus);
        AddStatusRow(status, 1, "Blender bridge", _bridgeStatus);
        AddStatusRow(status, 2, "Secure MCP Tunnel", _tunnelStatus);
        AddStatusRow(status, 3, "Runtime API key", _keyStatus);
        root.Controls.Add(status, 0, 1);

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 14, 0, 8),
            WrapContents = true
        };

        actions.Controls.Add(MakeButton("Start All", async (_, _) => await StartAllAsync(), primary: true));
        actions.Controls.Add(MakeButton("Stop Tunnel", (_, _) => _tunnel.Stop()));
        actions.Controls.Add(MakeButton("Start Blender", (_, _) => StartBlender()));
        actions.Controls.Add(MakeButton("Setup Profile", async (_, _) => await SetupProfileAsync()));
        actions.Controls.Add(MakeButton("Doctor", async (_, _) => await DoctorAsync()));
        actions.Controls.Add(MakeButton("Open Tunnels", (_, _) => OpenUrl("https://platform.openai.com/settings/organization/tunnels")));
        actions.Controls.Add(MakeButton("Runtime Keys", (_, _) => OpenUrl("https://platform.openai.com/settings/organization/api-keys")));
        actions.Controls.Add(MakeButton("Settings", (_, _) => OpenSettings()));
        root.Controls.Add(actions, 0, 2);

        _profileLabel.Dock = DockStyle.Fill;
        _profileLabel.TextAlign = ContentAlignment.MiddleLeft;
        _profileLabel.ForeColor = Muted;
        root.Controls.Add(_profileLabel, 0, 3);

        _log.Dock = DockStyle.Fill;
        _log.ReadOnly = true;
        _log.BackColor = Color.FromArgb(15, 17, 21);
        _log.ForeColor = Color.Gainsboro;
        _log.BorderStyle = BorderStyle.FixedSingle;
        _log.Font = new Font("Cascadia Mono", 9.5F);
        root.Controls.Add(_log, 0, 4);
    }

    private static void AddStatusRow(TableLayoutPanel table, int row, string name, Label value)
    {
        var key = new Label
        {
            Text = name,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.Silver
        };
        value.Dock = DockStyle.Fill;
        value.TextAlign = ContentAlignment.MiddleLeft;
        value.Text = "● Checking…";
        table.Controls.Add(key, 0, row);
        table.Controls.Add(value, 1, row);
    }

    private static Button MakeButton(string text, EventHandler handler, bool primary = false)
    {
        var button = new Button
        {
            Text = text,
            Width = primary ? 125 : 120,
            Height = 38,
            Margin = new Padding(0, 0, 10, 8),
            FlatStyle = FlatStyle.Flat,
            BackColor = primary ? Color.FromArgb(55, 106, 175) : Color.FromArgb(42, 46, 55),
            ForeColor = Color.White
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(70, 74, 85);
        button.Click += handler;
        return button;
    }

    private async Task RefreshStatusAsync()
    {
        if (_statusUpdateRunning)
            return;

        _statusUpdateRunning = true;
        try
        {
            var blenderRunning = Process.GetProcessesByName("blender").Any(p => !p.HasExited);
            SetStatus(_blenderStatus, blenderRunning, blenderRunning ? "Running" : "Not running");

            var bridge = await PortProbe.CanConnectAsync(_config.BlenderHost, _config.BlenderPort);
            SetStatus(_bridgeStatus, bridge,
                bridge ? $"Connected {_config.BlenderHost}:{_config.BlenderPort}" : $"Waiting on {_config.BlenderHost}:{_config.BlenderPort}");

            SetStatus(_tunnelStatus, _tunnel.IsRunning, _tunnel.IsRunning ? "Running" : "Stopped");

            var keyPresent = !string.IsNullOrWhiteSpace(_config.RuntimeApiKey);
            SetStatus(_keyStatus, keyPresent,
                keyPresent ? $"Present in {_config.RuntimeApiKeyEnvironmentVariable}" : $"Missing: {_config.RuntimeApiKeyEnvironmentVariable}");

            var id = string.IsNullOrWhiteSpace(_config.TunnelId) ? "<not set>" : _config.TunnelId;
            _profileLabel.Text = $"Profile: {_config.TunnelProfile}   |   Tunnel: {id}   |   Config: {ConfigStore.ConfigPath}";
        }
        finally
        {
            _statusUpdateRunning = false;
        }
    }

    private static void SetStatus(Label label, bool ok, string text)
    {
        label.Text = "● " + text;
        label.ForeColor = ok ? Good : Bad;
    }

    private async Task StartAllAsync()
    {
        Log("Start All requested.");
        if (_config.AutoLaunchBlender)
            StartBlender();

        if (string.IsNullOrWhiteSpace(_config.RuntimeApiKey))
        {
            Log($"Runtime API key is missing. Set {_config.RuntimeApiKeyEnvironmentVariable} in Windows, then restart Blender MCP_Con.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_config.TunnelProfile))
        {
            Log("Tunnel profile is empty. Open Settings.");
            return;
        }

        StartTunnel();
        await Task.Delay(500);
        await RefreshStatusAsync();

        if (!await PortProbe.CanConnectAsync(_config.BlenderHost, _config.BlenderPort))
            Log("Blender bridge is not listening yet. In Blender open N-panel → BlenderMCP and click Connect.");
    }

    private void StartBlender()
    {
        if (Process.GetProcessesByName("blender").Any(p => !p.HasExited))
        {
            Log("Blender is already running.");
            return;
        }

        var blender = ExecutableLocator.FindBlender(_config.BlenderExecutable);
        if (string.IsNullOrWhiteSpace(blender))
        {
            Log("Blender executable was not found. Set it in Settings.");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = blender,
                UseShellExecute = true
            });
            Log("Blender launched: " + blender);
        }
        catch (Exception ex)
        {
            Log("Failed to launch Blender: " + ex.Message);
        }
    }

    private void StartTunnel()
    {
        if (_tunnel.IsRunning)
        {
            Log("Tunnel is already running.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_config.RuntimeApiKey))
        {
            Log($"Cannot start tunnel: {_config.RuntimeApiKeyEnvironmentVariable} is missing.");
            return;
        }

        _tunnel.Start(_config.TunnelClientExecutable, new[]
        {
            "run",
            "--profile",
            _config.TunnelProfile
        });
    }

    private async Task SetupProfileAsync()
    {
        if (!Regex.IsMatch(_config.TunnelId ?? "", @"^tunnel_[0-9a-f]{32}$"))
        {
            Log("Tunnel ID is missing or invalid. Expected: tunnel_ + 32 lowercase hex characters. Open Settings.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_config.McpCommand) || !File.Exists(_config.McpCommand))
        {
            Log("MCP command/wrapper was not found. Expected the run_blender_mcp_server.cmd path in Settings.");
            return;
        }

        Log("Initializing tunnel-client profile…");
        try
        {
            var result = await ProcessRunner.RunCaptureAsync(_config.TunnelClientExecutable, new[]
            {
                "init",
                "--sample", "sample_mcp_stdio_local",
                "--profile", _config.TunnelProfile,
                "--tunnel-id", _config.TunnelId,
                "--mcp-command", _config.McpCommand
            });
            Log(result.Output);
            Log(result.ExitCode == 0 ? "Profile initialization completed." : $"Profile initialization failed with code {result.ExitCode}.");
        }
        catch (Exception ex)
        {
            Log("Profile initialization failed: " + ex.Message);
        }
    }

    private async Task DoctorAsync()
    {
        if (string.IsNullOrWhiteSpace(_config.RuntimeApiKey))
        {
            Log($"Doctor needs {_config.RuntimeApiKeyEnvironmentVariable}. Set the environment variable and restart the app.");
            return;
        }

        Log("Running tunnel-client doctor…");
        try
        {
            var result = await ProcessRunner.RunCaptureAsync(_config.TunnelClientExecutable, new[]
            {
                "doctor",
                "--profile", _config.TunnelProfile,
                "--explain"
            });
            Log(result.Output);
            Log(result.ExitCode == 0 ? "Doctor PASS." : $"Doctor returned code {result.ExitCode}.");
        }
        catch (Exception ex)
        {
            Log("Doctor failed: " + ex.Message);
        }
    }


    private void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Log("Failed to open browser: " + ex.Message);
        }
    }

    private void OpenSettings()
    {
        using var settings = new SettingsForm(_config);
        if (settings.ShowDialog(this) != DialogResult.OK)
            return;

        _config = settings.Result;
        ConfigStore.Save(_config);
        Log("Settings saved.");
        _ = RefreshStatusAsync();
    }

    private void Log(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        var lines = message.Replace("\r\n", "\n").Split('\n');
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            _log.AppendText($"[{DateTime.Now:HH:mm:ss}] {line}{Environment.NewLine}");
        }
        _log.SelectionStart = _log.TextLength;
        _log.ScrollToCaret();
    }

    private void LogFromAnyThread(string message)
    {
        if (IsDisposed)
            return;
        if (InvokeRequired)
        {
            try { BeginInvoke(new Action(() => Log(message))); }
            catch { }
            return;
        }
        Log(message);
    }
}
