#r "nuget: SixLabors.ImageSharp, 3.1.3"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

int size = 16;
var rand = new Random();

Color Pastel() =>
    Color.FromRgb(
        (byte)rand.Next(150, 255),
        (byte)rand.Next(150, 255),
        (byte)rand.Next(150, 255)
    );

var body = Pastel();
var eye = Color.Black;
var blush = Color.FromRgb(255, 120, 120);

var img = new Image<Rgba32>(size, size);
img.Mutate(ctx => ctx.Fill(body));

// Eyes
img.Mutate(ctx =>
{
    ctx.Fill(eye, new Rectangle(6, 7, 1, 1));
    ctx.Fill(eye, new Rectangle(9, 7, 1, 1));
});

// Blush
img.Mutate(ctx =>
{
    ctx.Fill(blush, new Rectangle(5, 9, 1, 1));
    ctx.Fill(blush, new Rectangle(10, 9, 1, 1));
});

// Mouth
img.Mutate(ctx =>
{
    ctx.Fill(eye, new Rectangle(7, 10, 2, 1));
});
// Scale up 16x → 256x256 for visibility
img.Mutate(ctx => ctx.Resize(size * 16, size * 16, KnownResamplers.NearestNeighbor));

// Save
var tag = Args[0];
Directory.CreateDirectory("mascots");
img.Save($"mascots/{tag}.png");
