param(
    [string]$Tag
)
# Run the C# generator
$repoRoot = $PSScriptRoot | Split-Path -Parent
dotnet script ./scripts/generate-mascot.csx -- $Tag $repoRoot
$png = "mascots/$Tag.png"
Write-Host "Generated mascot: $png"

# Update README
$readme = "README.md"
$content = Get-Content $readme -Raw
$md = "![Release Mascot](mascots/$Tag.png)"
$updated = $content -replace "(?s)<!-- MASCOT -->.*<!-- /MASCOT -->", "<!-- MASCOT -->`n$md`n<!-- /MASCOT -->"
Set-Content -Path $readme -Value $updated -NoNewline
Write-Host "README updated."
