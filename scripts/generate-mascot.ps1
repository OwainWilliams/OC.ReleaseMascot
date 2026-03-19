param(
    [string]$Tag
)

# Install ImageSharp if not already present
if (-not (Test-Path "./packages")) {
    dotnet new console -n tempapp | Out-Null
    cd tempapp
    dotnet add package SixLabors.ImageSharp
    cd ..
}

Add-Type -Path "./tempapp/obj/Debug/net8.0/tempapp.AssemblyInfo.cs" -ErrorAction SilentlyContinue

# Load ImageSharp
$assemblyPath = Get-ChildItem -Recurse -Filter "SixLabors.ImageSharp.dll" | Select-Object -First 1
Add-Type -Path $assemblyPath.FullName

# Aliases for convenience
$Image = [SixLabors.ImageSharp.Image]
$Color = [SixLabors.ImageSharp.Color]
$Rgba32 = [SixLabors.ImageSharp.PixelFormats.Rgba32]
$Rect = [SixLabors.ImageSharp.Rectangle]
$Mutate = [SixLabors.ImageSharp.Processing.ProcessingExtensions]

$size = 16
$img = $Image::Create($size, $size, $Color::White)

$rand = New-Object System.Random

function Get-Pastel {
    return [SixLabors.ImageSharp.Color]::FromRgb(
        [byte]$rand.Next(150,255),
        [byte]$rand.Next(150,255),
        [byte]$rand.Next(150,255)
    )
}

$body = Get-Pastel
$eye  = $Color::Black
$blush = [SixLabors.ImageSharp.Color]::FromRgb(255,120,120)

# Fill background
$img.Mutate({ param($ctx) $ctx.Fill($body) })

# Eyes
$img.Mutate({ param($ctx)
    $ctx.Fill($eye, (New-Object SixLabors.ImageSharp.Rectangle 6,7,1,1))
    $ctx.Fill($eye, (New-Object SixLabors.ImageSharp.Rectangle 9,7,1,1))
})

# Blush
$img.Mutate({ param($ctx)
    $ctx.Fill($blush, (New-Object SixLabors.ImageSharp.Rectangle 5,9,1,1))
    $ctx.Fill($blush, (New-Object SixLabors.ImageSharp.Rectangle 10,9,1,1))
})

# Mouth
$img.Mutate({ param($ctx)
    $ctx.Fill($eye, (New-Object SixLabors.ImageSharp.Rectangle 7,10,2,1))
})

# Save output
$mascotDir = "mascots"
if (-not (Test-Path $mascotDir)) { New-Item -ItemType Directory $mascotDir | Out-Null }

$outFile = "$mascotDir/$Tag.png"
$img.Save($outFile)

Write-Host "Generated mascot: $outFile"
