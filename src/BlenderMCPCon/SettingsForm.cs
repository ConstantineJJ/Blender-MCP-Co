namespace BlenderMCPCon;

public sealed class SettingsForm : Form
{
    private readonly TextBox _blender = new();
    private readonly TextBox _tunnelClient = new();
    private readonly TextBox _profile = new();
    private readonly TextBox _tunnelId = new();
    private readonly TextBox _mcpCommand = new();
    private readonly TextBox _host = new();
    private readonly NumericUpDown _port = new();
    private readonly TextBox _keyEnv = new();
    private readonly CheckBox _autoLaunch = new();

    public AppConfig Result { get; private set; }

    public SettingsForm(AppConfig source)
    {
        Result = Clone(source);
        Text = "Blender MCP_Con — Settings";
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(780, 500);
        Size = new Size(860, 560);
        BackColor = Color.FromArgb(24, 26, 31);
        ForeColor = Color.Gainsboro;
        Font = new Font("Segoe UI", 10F);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(18),
            ColumnCount = 3,
            RowCount = 10,
            AutoScroll = true
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92));
        Controls.Add(root);

        AddPathRow(root, 0, "Blender executable", _blender, source.BlenderExecutable, "Executable (*.exe)|*.exe|All files (*.*)|*.*");
        AddPathRow(root, 1, "tunnel-client", _tunnelClient, source.TunnelClientExecutable, "Executable (*.exe)|*.exe|All files (*.*)|*.*");
        AddTextRow(root, 2, "Tunnel profile", _profile, source.TunnelProfile);
        AddTextRow(root, 3, "Tunnel ID", _tunnelId, source.TunnelId);
        AddPathRow(root, 4, "MCP command / wrapper", _mcpCommand, source.McpCommand, "Scripts (*.cmd;*.bat;*.py)|*.cmd;*.bat;*.py|All files (*.*)|*.*");
        AddTextRow(root, 5, "Blender host", _host, source.BlenderHost);

        root.Controls.Add(MakeLabel("Blender port"), 0, 6);
        _port.Minimum = 1;
        _port.Maximum = 65535;
        _port.Value = Math.Clamp(source.BlenderPort, 1, 65535);
        _port.Dock = DockStyle.Fill;
        root.Controls.Add(_port, 1, 6);

        AddTextRow(root, 7, "Runtime key env variable", _keyEnv, source.RuntimeApiKeyEnvironmentVariable);

        root.Controls.Add(MakeLabel("Launch Blender with Start All"), 0, 8);
        _autoLaunch.Checked = source.AutoLaunchBlender;
        _autoLaunch.AutoSize = true;
        root.Controls.Add(_autoLaunch, 1, 8);

        var note = new Label
        {
            Text = "API keys are never stored by this app. Set the runtime key in Windows as an environment variable.",
            AutoSize = true,
            ForeColor = Color.Silver,
            Padding = new Padding(0, 10, 0, 4)
        };
        root.Controls.Add(note, 0, 9);
        root.SetColumnSpan(note, 3);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 56,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(10)
        };
        Controls.Add(buttons);

        var save = new Button { Text = "Save", Width = 100, Height = 32 };
        var cancel = new Button { Text = "Cancel", Width = 100, Height = 32, DialogResult = DialogResult.Cancel };
        save.Click += (_, _) => SaveAndClose();
        buttons.Controls.Add(save);
        buttons.Controls.Add(cancel);
        AcceptButton = save;
        CancelButton = cancel;
    }

    private void SaveAndClose()
    {
        Result = new AppConfig
        {
            BlenderExecutable = _blender.Text.Trim(),
            TunnelClientExecutable = _tunnelClient.Text.Trim(),
            TunnelProfile = _profile.Text.Trim(),
            TunnelId = _tunnelId.Text.Trim(),
            McpCommand = _mcpCommand.Text.Trim(),
            BlenderHost = _host.Text.Trim(),
            BlenderPort = (int)_port.Value,
            RuntimeApiKeyEnvironmentVariable = _keyEnv.Text.Trim(),
            AutoLaunchBlender = _autoLaunch.Checked
        };
        DialogResult = DialogResult.OK;
        Close();
    }

    private static AppConfig Clone(AppConfig c) => new()
    {
        BlenderExecutable = c.BlenderExecutable,
        TunnelClientExecutable = c.TunnelClientExecutable,
        TunnelProfile = c.TunnelProfile,
        TunnelId = c.TunnelId,
        McpCommand = c.McpCommand,
        BlenderHost = c.BlenderHost,
        BlenderPort = c.BlenderPort,
        RuntimeApiKeyEnvironmentVariable = c.RuntimeApiKeyEnvironmentVariable,
        AutoLaunchBlender = c.AutoLaunchBlender
    };

    private static Label MakeLabel(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleLeft,
        ForeColor = Color.Gainsboro
    };

    private static void AddTextRow(TableLayoutPanel root, int row, string label, TextBox box, string value)
    {
        root.Controls.Add(MakeLabel(label), 0, row);
        box.Text = value;
        box.Dock = DockStyle.Fill;
        root.Controls.Add(box, 1, row);
        root.SetColumnSpan(box, 2);
    }

    private static void AddPathRow(TableLayoutPanel root, int row, string label, TextBox box, string value, string filter)
    {
        root.Controls.Add(MakeLabel(label), 0, row);
        box.Text = value;
        box.Dock = DockStyle.Fill;
        root.Controls.Add(box, 1, row);

        var browse = new Button { Text = "Browse…", Dock = DockStyle.Fill };
        browse.Click += (_, _) =>
        {
            using var dialog = new OpenFileDialog { Filter = filter, CheckFileExists = true };
            if (dialog.ShowDialog() == DialogResult.OK)
                box.Text = dialog.FileName;
        };
        root.Controls.Add(browse, 2, row);
    }
}
