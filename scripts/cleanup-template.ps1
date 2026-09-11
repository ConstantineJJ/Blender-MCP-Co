$project = Join-Path $PSScriptRoot "..\src\BlenderMCPCon"
Get-ChildItem $project -Filter "Form1*" -ErrorAction SilentlyContinue | Remove-Item -Force
Write-Host "Removed unused WinForms template Form1 files, if present."
