#r "nuget: SixLabors.ImageSharp, 3.1.3"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;
using System;
using System.IO;

// ------------------------------
// FIX 1: Ensure absolute paths
// ------------------------------
var root = Directory.GetCurrentDirectory();
var outDir = Path.Combine(root, "mascots");
Directory.CreateDirectory(outDir);

var tag = Args[0];
var outFile = Path.Combine(outDir, $"{tag}.png");

// ------------------------------
// Monster generator
// ------------------------------

int size = 32;
var rand = new Random();

Color RandomColor() =>
    Color.FromRgb(
        (byte)rand.Next(80, 255),
        (byte)rand.Next(80, 255),
        (byte)rand.Next(80, 255)
    );

var body = RandomColor();
var outline = Color.FromRgb(
    (byte)Math.Max(body.R - 40, 0),
    (byte)Math.Max(body.G - 40, 0),
    (byte)Math.Max(body.B - 40, 0)
);

var img = new Image<Rgba32>(size, size);
img.Mutate(ctx => ctx.Fill(Color.Transparent));

string[] types = { "slime", "ghost", "dragon", "imp", "beast", "winged" };
string monster = types[rand.Next(types.Length)];

img.Mutate(ctx =>
{
    switch (monster)
    {
        case "slime":
            ctx.Fill(outline, new EllipsePolygon(16, 18, 12));
            ctx.Fill(body, new EllipsePolygon(16, 18, 11));
            break;

        case "ghost":
            ctx.Fill(outline, new EllipsePolygon(16, 14, 10));
            ctx.Fill(body, new EllipsePolygon(16, 14, 9));
            ctx.Fill(outline, new Polygon(new PointF[] {
                new(6,22), new(10,28), new(14,22), new(18,28), new(22,22)
            }));
            ctx.Fill(body, new Polygon(new PointF[] {
                new(7,22), new(10,27), new(14,22), new(18,27), new(21,22)
            }));
            break;

        case "dragon":
            ctx.Fill(outline, new EllipsePolygon(16, 18, 11));
            ctx.Fill(body, new EllipsePolygon(16, 18, 10));

            ctx.Fill(outline, new Polygon(new PointF[] {
                new(10,6), new(8,2), new(12,6)
            }));
            ctx.Fill(outline, new Polygon(new PointF[] {
                new(22,6), new(24,2), new(20,6)
            }));
            ctx.Fill(body, new Polygon(new PointF[] {
                new(10,6), new(9,3), new(11,6)
            }));
            ctx.Fill(body, new Polygon(new PointF[] {
                new(22,6), new(23,3), new(21,6)
            }));

            ctx.Fill(outline, new Polygon(new PointF[] {
                new(16,28), new(14,30), new(18,30)
            }));
            ctx.Fill(body, new Polygon(new PointF[] {
                new(16,28), new(15,29), new(17,29)
            }));
            break;

        case "imp":
            ctx.Fill(outline, new EllipsePolygon(16, 18, 10));
            ctx.Fill(body, new EllipsePolygon(16, 18, 9));

            ctx.Fill(outline, new EllipsePolygon(10, 6, 2));
            ctx.Fill(outline, new EllipsePolygon(22, 6, 2));
            ctx.Fill(body, new EllipsePolygon(10, 6, 1));
            ctx.Fill(body, new EllipsePolygon(22, 6, 1));
            break;

        case "beast":
            ctx.Fill(outline, new EllipsePolygon(16, 18, 12));
            ctx.Fill(body, new EllipsePolygon(16, 18, 11));

            ctx.Fill(outline, new EllipsePolygon(8, 8, 3));
            ctx.Fill(outline, new EllipsePolygon(24, 8, 3));
            ctx.Fill(body, new EllipsePolygon(8, 8, 2));
            ctx.Fill(body, new EllipsePolygon(24, 8, 2));
            break;

        case "winged":
            ctx.Fill(outline, new EllipsePolygon(16, 18, 10));
            ctx.Fill(body, new EllipsePolygon(16, 18, 9));

            ctx.Fill(outline, new EllipsePolygon(6, 18, 6, 3));
            ctx.Fill(outline, new EllipsePolygon(26, 18, 6, 3));
            ctx.Fill(body, new EllipsePolygon(6, 18, 5, 2));
            ctx.Fill(body, new EllipsePolygon(26, 18, 5, 2));
            break;
    }
});

// Belly highlight
var belly = Color.FromRgb(
    (byte)Math.Min(body.R + 40, 255),
    (byte)Math.Min(body.G + 40, 255),
    (byte)Math.Min(body.B + 40, 255)
);

img.Mutate(ctx =>
{
    ctx.Fill(belly, new EllipsePolygon(16, 20, 5));
});

// Face
var eye = Color.Black;
img.Mutate(ctx =>
{
    ctx.Fill(eye, new Rectangle(12, 16, 2, 2));
    ctx.Fill(eye, new Rectangle(18, 16, 2, 2));
    ctx.Fill(eye, new Rectangle(15, 20, 2, 1));
});

// Scale up
img.Mutate(ctx => ctx.Resize(size * 8, size * 8, KnownResamplers.NearestNeighbor));

// ------------------------------
// FIX 2: Save to absolute path
// ------------------------------
img.Save(outFile);
Console.WriteLine($"Saved mascot to: {outFile}");
