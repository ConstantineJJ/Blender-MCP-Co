# Blender MCP_Con

**Current version: 0.2.0**

Small Windows launcher/diagnostic UI for the local Blender MCP + OpenAI Secure MCP Tunnel workflow, plus the **Blender Character Agent Pipeline** skill stack for agent-driven character work.

## What v0.2.0 includes

### Blender MCP_Con launcher

- Finds/launches Blender (including common Steam locations).
- Checks the Blender MCP addon TCP bridge on `127.0.0.1:9876`.
- Initializes a `tunnel-client` stdio profile using your MCP wrapper command.
- Runs `tunnel-client doctor --explain`.
- Starts/stops `tunnel-client run --profile ...` and captures logs.
- Never stores the runtime API key on disk; it only reads the environment variable you configure (default `CONTROL_PLANE_API_KEY`).
- Does **not** kill Blender when you stop/close the launcher, so unsaved `.blend` work is protected.

### Blender Character Agent Pipeline

The repository now includes an eight-skill character-production stack under `skills/`:

```text
Blender_Character_Pipeline_Core
Blender_Reference_Reconstruction_SKILL
Blender_Character_Modeling_SKILL
Blender_Organic_Sculpting_SKILL
Blender_Retopology_Deformation_SKILL
Blender_Character_QA_SKILL
Blender_Iterative_Refinement_SKILL
Blender_Character_Rigging_Animation_Godot_SKILL
```

The stack adds:
- task routing and Scope Lock / Surgical Mode;
- reference-contract reconstruction;
- primary-form stage gates;
- deterministic-first sculpting;
- retopology and deformation validation;
- fixed-view QA;
- iterative refinement;
- anti-degradation checks with `KEEP / CORRECT / REVERT`;
- rigging, animation and Godot/GLB rules;
- a contained Stickmans Duel production profile instead of contaminating the universal base.

See `skills/README_Blender_Character_Agent_Pipeline.md` and `skills/PIPELINE_MANIFEST.json`.

## Architecture

```text
ChatGPT
  ↕
OpenAI Secure MCP Tunnel
  ↕
tunnel-client
  ↕ stdio
blender MCP server / wrapper
  ↕ TCP :9876
Blender MCP addon
  ↕
Blender
```

The official Blender MCP implementation uses TCP port `9876` between the MCP server and the Blender addon. `6262/6263` belong to the separate Godot setup and are intentionally not reused here.

## First build

From the repository root:

```powershell
dotnet build
```

Optional: remove the unused WinForms template `Form1` files:

```powershell
.\scripts\cleanup-template.ps1
```

Run:

```powershell
dotnet run --project .\src\BlenderMCPCon\BlenderMCPCon.csproj
```

## One-time setup

1. Install/enable the Blender MCP addon in Blender.
2. Keep the working MCP wrapper command. The app defaults to:
   `%USERPROFILE%\chatgpt-blender-mcp\run_blender_mcp_server.cmd`
3. Install `tunnel-client` and make it available on `PATH`, or select the executable in **Settings**.
4. Create/reuse a valid Secure MCP Tunnel and copy its `tunnel_...` ID into **Settings**.
5. Set the runtime API key in Windows under the environment variable `CONTROL_PLANE_API_KEY` (or change the variable name in Settings). The app intentionally has no API-key textbox.
6. Restart Blender MCP_Con after changing Windows environment variables.
7. Click **Setup Profile** once.
8. Click **Doctor**. Fix any reported tunnel/profile issue before starting.
9. In Blender, open `N` panel → BlenderMCP and click **Connect** so port `9876` is listening.
10. Click **Start All**.

## Buttons

- **Start All** — launches Blender when needed and starts the configured Secure MCP Tunnel profile.
- **Stop Tunnel** — stops only the tunnel process started by this app. Blender stays open.
- **Start Blender** — only launches Blender.
- **Setup Profile** — runs `tunnel-client init --sample sample_mcp_stdio_local ...` using the configured tunnel ID and MCP command.
- **Doctor** — runs `tunnel-client doctor --profile <name> --explain`.
- **Open Tunnels** — opens OpenAI Platform tunnel management.
- **Runtime Keys** — opens OpenAI Platform runtime API key management.
- **Settings** — edits paths, tunnel ID/profile and Blender bridge host/port.

## Security

Do not commit runtime/admin API keys. `CONTROL_PLANE_API_KEY` is inherited from the Windows environment by the child `tunnel-client` process. The config stores only non-secret paths, profile name, tunnel ID and port.

## Current known limitation

Blender's addon-side **Connect** action is still manual in v0.2.0. This is deliberate: the launcher keeps the connection chain observable and reliable before automating Blender UI/addon internals.
