using Microsoft.Win32;

namespace BlenderMCPCon;

public static class ExecutableLocator
{
    public static string? FindBlender(string configured)
    {
        if (File.Exists(configured))
            return configured;

        var candidates = new List<string>();

        var steamPath = Registry.GetValue(
            @"HKEY_CURRENT_USER\Software\Valve\Steam",
            "SteamPath",
            null) as string;

        if (!string.IsNullOrWhiteSpace(steamPath))
            candidates.Add(Path.Combine(steamPath.Replace('/', Path.DirectorySeparatorChar), "steamapps", "common", "Blender", "blender.exe"));

        var pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        var pfx86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

        candidates.Add(Path.Combine(pf, "Steam", "steamapps", "common", "Blender", "blender.exe"));
        candidates.Add(Path.Combine(pfx86, "Steam", "steamapps", "common", "Blender", "blender.exe"));

        var foundation = Path.Combine(pf, "Blender Foundation");
        if (Directory.Exists(foundation))
        {
            candidates.AddRange(
                Directory.GetDirectories(foundation, "Blender *")
                    .OrderByDescending(x => x)
                    .Select(x => Path.Combine(x, "blender.exe")));
        }

        return candidates.FirstOrDefault(File.Exists) ?? ResolveFromPath("blender.exe");
    }

    public static string? ResolveFromPath(string executable)
    {
        if (File.Exists(executable))
            return Path.GetFullPath(executable);

        if (executable.Contains(Path.DirectorySeparatorChar) || executable.Contains(Path.AltDirectorySeparatorChar))
            return null;

        var path = Environment.GetEnvironmentVariable("PATH") ?? "";
        foreach (var raw in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            try
            {
                var candidate = Path.Combine(raw.Trim('"'), executable);
                if (File.Exists(candidate))
                    return candidate;
            }
            catch
            {
                // Ignore malformed PATH entries.
            }
        }

        return null;
    }
}
