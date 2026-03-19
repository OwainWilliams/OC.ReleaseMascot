param(
    [string]$Tag
)

# Ensure output folder exists
$mascotDir = "mascots"
if (-not (Test-Path $mascotDir)) {
    New-Item -ItemType Directory -Path $mascotDir | Out-Null
}

# Output file path
$outFile = "$mascotDir/$Tag.png"

# --- Generate the creature ---
# (Paste the cute symmetrical creature generator here)
# Replace the final save line with:
$bmp.Save($outFile, [System.Drawing.Imaging.ImageFormat]::Png)

Write-Host "Generated mascot: $outFile"

# --- Embed into README ---
$readme = "README.md"
$readmeContent = Get-Content $readme -Raw

# Convert PNG to base64
$imgBase64 = [Convert]::ToBase64String([IO.File]::ReadAllBytes($outFile))
$mdImage = "![Release Mascot](data:image/png;base64,$imgBase64)"

# Replace placeholder in README
$updated = $readmeContent -replace "<!-- MASCOT -->.*<!-- /MASCOT -->", "<!-- MASCOT -->`n$mdImage`n<!-- /MASCOT -->"

Set-Content -Path $readme -Value $updated

Write-Host "README updated with new mascot."
