using System.Diagnostics;

namespace BlenderMCPCon;

public sealed class ManagedProcess : IDisposable
{
    private Process? _process;
    private readonly Action<string> _log;

    public ManagedProcess(Action<string> log)
    {
        _log = log;
    }

    public bool IsRunning => _process is { HasExited: false };

    public bool Start(string executable, IEnumerable<string> arguments)
    {
        if (IsRunning)
            return true;

        var resolved = ExecutableLocator.ResolveFromPath(executable) ?? executable;
        var psi = new ProcessStartInfo
        {
            FileName = resolved,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        foreach (var arg in arguments)
            psi.ArgumentList.Add(arg);

        try
        {
            _process = new Process { StartInfo = psi, EnableRaisingEvents = true };
            _process.OutputDataReceived += (_, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) _log(e.Data); };
            _process.ErrorDataReceived += (_, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) _log("ERR: " + e.Data); };
            _process.Exited += (_, _) => _log($"Tunnel process exited with code {_process?.ExitCode}.");

            if (!_process.Start())
                return false;

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            _log($"Started: {psi.FileName} {string.Join(" ", psi.ArgumentList)}");
            return true;
        }
        catch (Exception ex)
        {
            _log("Failed to start process: " + ex.Message);
            _process?.Dispose();
            _process = null;
            return false;
        }
    }

    public void Stop()
    {
        if (!IsRunning)
            return;

        try
        {
            _process!.Kill(entireProcessTree: true);
            _process.WaitForExit(3000);
            _log("Tunnel process stopped.");
        }
        catch (Exception ex)
        {
            _log("Failed to stop tunnel process: " + ex.Message);
        }
        finally
        {
            _process?.Dispose();
            _process = null;
        }
    }

    public void Dispose() => Stop();
}

public static class ProcessRunner
{
    public static async Task<(int ExitCode, string Output)> RunCaptureAsync(
        string executable,
        IEnumerable<string> arguments,
        CancellationToken cancellationToken = default)
    {
        var resolved = ExecutableLocator.ResolveFromPath(executable) ?? executable;
        var psi = new ProcessStartInfo
        {
            FileName = resolved,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        foreach (var arg in arguments)
            psi.ArgumentList.Add(arg);

        using var process = new Process { StartInfo = psi };
        process.Start();

        var stdout = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stderr = process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);

        var output = (await stdout) + (await stderr);
        return (process.ExitCode, output.Trim());
    }
}
