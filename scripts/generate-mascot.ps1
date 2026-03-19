param(
    [string]$Tag
)

# Run the C# generator
dotnet script ./scripts/generate-mascot.csx -- $Tag

$png = "mascots/$Tag.png"
Write-Host "Generated mascot: $png"

# Update README
$readme = "README.md"
$content = Get-Content $readme -Raw

$base64 = [Convert]::ToBase64String([IO.File]::ReadAllBytes($png))
$md = "![Release Mascot](data:image/png;base64,$base64)"

$updated = $content -replace "<!-- MASCOT -->.*<!-- /MASCOT -->", "<!-- MASCOT -->`n$md`n<!-- /MASCOT -->"

Set-Content -Path $readme -Value $updated
Write-Host "README updated."
