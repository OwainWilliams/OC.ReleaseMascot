param(
    [string]$Tag
)

Write-Host "Running Generate-Mascot.ps1 for tag: $Tag"

# 1. Define adjectives and animals
$adjectives = @(
    'Jubilant', 'Stoic', 'Curious', 'Determined', 'Sleepy',
    'Radiant', 'Mischievous', 'Gallant', 'Whimsical', 'Bold'
)

$animals = @(
    'Pangolin', 'Capybara', 'Otter', 'Red Panda', 'Lynx',
    'Hedgehog', 'Raccoon', 'Badger', 'Fox', 'Alpaca'
)

# 2. Pick random items
$rand = New-Object System.Random
$adj = $adjectives[$rand.Next(0, $adjectives.Count)]
$animal = $animals[$rand.Next(0, $animals.Count)]
$mascotName = "The $adj $animal"

Write-Host "Mascot name: $mascotName"

# 3. Ensure mascots folder exists
$mascotsDir = Join-Path $PSScriptRoot "..\mascots"
$mascotsDir = (Resolve-Path $mascotsDir).Path 2>$null -or (New-Item -ItemType Directory -Path $mascotsDir).FullName

if (-not (Test-Path $mascotsDir)) {
    New-Item -ItemType Directory -Path $mascotsDir | Out-Null
}

# 4. Build SVG content
$colors = @('#FF6B6B', '#4ECDC4', '#FFD93D', '#6C5CE7', '#00B894')
$bg = $colors[$rand.Next(0, $colors.Count)]

do {
    $circle = $colors[$rand.Next(0, $colors.Count)]
} while ($circle -eq $bg)

$svg = @"
<svg width="600" height="300" viewBox="0 0 600 300" xmlns="http://www.w3.org/2000/svg">
  <rect width="600" height="300" fill="$bg" rx="24" />
  <circle cx="120" cy="150" r="70" fill="$circle" opacity="0.9" />
  <text x="220" y="140" font-family="system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif"
        font-size="28" fill="#ffffff" font-weight="600">
    $mascotName
  </text>
  <text x="220" y="180" font-family="system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif"
        font-size="16" fill="#f0f0f0">
    Release: $Tag
  </text>
</svg>
"@

# 5. Save SVG file
$svgFileName = "$Tag.svg"
$svgPath = Join-Path $mascotsDir $svgFileName
$svg | Out-File -FilePath $svgPath -Encoding utf8

Write-Host "SVG written to $svgPath"

# 6. Update README.md
$repoRoot = Join-Path $PSScriptRoot ".."
$readmePath = Join-Path $repoRoot "README.md"

if (-not (Test-Path $readmePath)) {
    throw "README.md not found at $readmePath"
}

$readme = Get-Content $readmePath -Raw

$startMarker = "<!-- mascot-start -->"
$endMarker = "<!-- mascot-end -->"

$startIndex = $readme.IndexOf($startMarker)
$endIndex = $readme.IndexOf($endMarker)

if ($startIndex -lt 0 -or $endIndex -lt 0) {
    throw "Mascot markers not found in README.md"
}

$before = $readme.Substring(0, $startIndex + $startMarker.Length)
$after = $readme.Substring($endIndex)

# Make path relative for README
$relativePath = "mascots/$svgFileName"

$middle = @"

## 🐾 Release Mascot: $mascotName

![$mascotName]($relativePath)

"@

$updated = $before + $middle + $after

$updated | Out-File -FilePath $readmePath -Encoding utf8

Write-Host "README.md updated with new mascot."
