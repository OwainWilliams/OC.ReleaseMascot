# UpdateReadme.ps1
# Prepends a new mascot entry to the Mascot Hall of Fame section in README.md.
#
# Usage:
#   ./UpdateReadme.ps1 -Tag "1.0.0" -MascotName "Admiral Fluffy Pawsworth" -RepoRoot "."
#
# The README must contain the following marker comment exactly:
#   <!-- MASCOT_HALL_OF_FAME -->
# All new entries are injected immediately after this line.

param(
    [Parameter(Mandatory = $true)]
    [string]$Tag,

    [Parameter(Mandatory = $true)]
    [string]$MascotName,

    [Parameter(Mandatory = $false)]
    [string]$RepoRoot = $PSScriptRoot
)

$readmePath  = Join-Path $RepoRoot "README.md"
$mascotImage = "mascots/$Tag.png"
$marker      = "<!-- MASCOT_HALL_OF_FAME -->"

if (-not (Test-Path $readmePath)) {
    Write-Error "README.md not found at: $readmePath"
    exit 1
}

$readmeContent = Get-Content $readmePath -Raw

if (-not $readmeContent.Contains($marker)) {
    Write-Error "Marker '$marker' not found in README.md. Please add it where you want the hall of fame to appear."
    exit 1
}

# Build the new entry block
$newEntry = @"

### ``$Tag`` — $MascotName

![${MascotName} (${Tag})]($mascotImage)

---
"@

# Inject the new entry immediately after the marker
$updatedContent = $readmeContent.Replace($marker, "$marker$newEntry")

Set-Content -Path $readmePath -Value $updatedContent -NoNewline

Write-Host "README.md updated with mascot '$MascotName' for release $Tag"
