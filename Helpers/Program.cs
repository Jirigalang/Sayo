using Microsoft.Xna.Framework;
using SkiaSharp;
using System.Text.Json;



Environment.CurrentDirectory = Environment.GetEnvironmentVariable("OneDrive")! + @"\repos\Sayo\SayoKNIContent";
var fileNames = Directory.GetFiles(Environment.CurrentDirectory, "*.png");
Dictionary<string, SKBitmap> bitmaps = [];
Dictionary<string, Rectangle> rects = [];
foreach (var file in fileNames)
{
    Console.WriteLine(Path.GetFileName(file));
    var bitmap = SKBitmap.Decode(file);
    bitmaps.Add(Path.GetFileName(file), bitmap);
}
var width = 370;
var height = 3 * 64;
var result = new SKBitmap(width, height);
var canvas = new SKCanvas(result);
int i = 0;
foreach (var body in bitmaps.Where(x => x.Key.StartsWith(@"SayoHead")))
{
    canvas.DrawBitmap(body.Value, i, 0);
    rects.Add(Path.GetFileNameWithoutExtension(body.Key), new Rectangle(i, 0, body.Value.Width, body.Value.Height));
    i += 64;
}
i = 0;
foreach (var body in bitmaps.Where(x => x.Key.StartsWith(@"SayoBody")))
{
    canvas.DrawBitmap(body.Value, i, 64);
    rects.Add(Path.GetFileNameWithoutExtension(body.Key), new Rectangle(i, 64, body.Value.Width, body.Value.Height));
    i += 64;
}

var food = bitmaps["Food.png"];
canvas.DrawBitmap(food, 0, 64 * 2);
rects.Add("Food", new Rectangle(0, 64 * 2, food.Width, food.Height));

using SKImage image = SKImage.FromBitmap(result);
using SKData data = image.Encode(SKEncodedImageFormat.Png, 90);
var options = new JsonSerializerOptions
{
    IncludeFields = true
};
string json = JsonSerializer.Serialize(rects, options);
File.WriteAllText(@"Rectangle.json", json);
File.WriteAllBytes(@"Sprite.png", data.ToArray());