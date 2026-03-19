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
// Absolute output path
// ------------------------------
var root = Directory.GetCurrentDirectory();
var outDir = System.IO.Path.Combine(root, "/mascots");
Directory.CreateDirectory(outDir);

var tag = Args[0];
var outFile = System.IO.Path.Combine(outDir, $"{tag}.png");

// ------------------------------
// Helpers
// ------------------------------
Rgba32 RandomColor()
{
    return new Rgba32(
        (byte)Random.Shared.Next(80, 255),
        (byte)Random.Shared.Next(80, 255),
        (byte)Random.Shared.Next(80, 255)
    );
}

Rgba32 Darken(Rgba32 c, int amount)
{
    return new Rgba32(
        (byte)Math.Max(c.R - amount, 0),
        (byte)Math.Max(c.G - amount, 0),
        (byte)Math.Max(c.B - amount, 0)
    );
}

Rgba32 Lighten(Rgba32 c, int amount)
{
    return new Rgba32(
        (byte)Math.Min(c.R + amount, 255),
        (byte)Math.Min(c.G + amount, 255),
        (byte)Math.Min(c.B + amount, 255)
    );
}

// ------------------------------
// Monster setup
// ------------------------------
int size = 32;
var body = RandomColor();
var outline = Darken(body, 40);
var belly = Lighten(body, 40);

var img = new Image<Rgba32>(size, size);
img.Mutate(ctx => ctx.Fill(Color.Transparent));

string[] types = { "slime", "ghost", "dragon", "imp", "beast", "winged" };
string monster = types[Random.Shared.Next(types.Length)];

// ------------------------------
// Silhouette drawing helpers
// ------------------------------
void FillEllipse(IImageProcessingContext ctx, Rgba32 color, float cx, float cy, float rx, float ry)
{
    var shape = new EllipsePolygon(cx, cy, rx, ry);
    ctx.Fill(color, shape);
}

void FillPoly(IImageProcessingContext ctx, Rgba32 color, params (float x, float y)[] pts)
{
    var points = new PointF[pts.Length];
    for (int i = 0; i < pts.Length; i++)
        points[i] = new PointF(pts[i].x, pts[i].y);

    var poly = new Polygon(new LinearLineSegment(points));
    ctx.Fill(color, poly);
}

// ------------------------------
// Draw monster silhouette
// ------------------------------
img.Mutate(ctx =>
{
    switch (monster)
    {
        case "slime":
            FillEllipse(ctx, outline, 16, 18, 12, 12);
            FillEllipse(ctx, body,    16, 18, 11, 11);
            break;

        case "ghost":
            FillEllipse(ctx, outline, 16, 14, 10, 10);
            FillEllipse(ctx, body,    16, 14,  9,  9);

            FillPoly(ctx, outline, (6,22), (10,28), (14,22), (18,28), (22,22));
            FillPoly(ctx, body,    (7,22), (10,27), (14,22), (18,27), (21,22));
            break;

        case "dragon":
            FillEllipse(ctx, outline, 16, 18, 11, 11);
            FillEllipse(ctx, body,    16, 18, 10, 10);

            // horns
            FillPoly(ctx, outline, (10,6), (8,2), (12,6));
            FillPoly(ctx, outline, (22,6), (24,2), (20,6));
            FillPoly(ctx, body,    (10,6), (9,3), (11,6));
            FillPoly(ctx, body,    (22,6), (23,3), (21,6));

            // tail spike
            FillPoly(ctx, outline, (16,28), (14,30), (18,30));
            FillPoly(ctx, body,    (16,28), (15,29), (17,29));
            break;

        case "imp":
            FillEllipse(ctx, outline, 16, 18, 10, 10);
            FillEllipse(ctx, body,    16, 18,  9,  9);

            FillEllipse(ctx, outline, 10, 6, 2, 2);
            FillEllipse(ctx, outline, 22, 6, 2, 2);
            FillEllipse(ctx, body,    10, 6, 1, 1);
            FillEllipse(ctx, body,    22, 6, 1, 1);
            break;

        case "beast":
            FillEllipse(ctx, outline, 16, 18, 12, 12);
            FillEllipse(ctx, body,    16, 18, 11, 11);

            FillEllipse(ctx, outline, 8, 8, 3, 3);
            FillEllipse(ctx, outline, 24, 8, 3, 3);
            FillEllipse(ctx, body,    8, 8, 2, 2);
            FillEllipse(ctx, body,    24, 8, 2, 2);
            break;

        case "winged":
            FillEllipse(ctx, outline, 16, 18, 10, 10);
            FillEllipse(ctx, body,    16, 18,  9,  9);

            FillEllipse(ctx, outline, 6, 18, 6, 3);
            FillEllipse(ctx, outline, 26,18, 6, 3);
            FillEllipse(ctx, body,    6, 18, 5, 2);
            FillEllipse(ctx, body,    26,18, 5, 2);
            break;
    }
});

// Belly highlight
img.Mutate(ctx =>
{
    FillEllipse(ctx, belly, 16, 20, 5, 5);
});

// Face
img.Mutate(ctx =>
{
    ctx.Fill(Color.Black, new Rectangle(12, 16, 2, 2));
    ctx.Fill(Color.Black, new Rectangle(18, 16, 2, 2));
    ctx.Fill(Color.Black, new Rectangle(15, 20, 2, 1));
});

// Scale up
img.Mutate(ctx => ctx.Resize(size * 8, size * 8, KnownResamplers.NearestNeighbor));

// Save
img.Save(outFile);
Console.WriteLine($"Saved mascot to: {outFile}");
