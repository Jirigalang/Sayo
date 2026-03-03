using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Sayo.Core;


[JsonSourceGenerationOptions(IncludeFields = true)]
[JsonSerializable(typeof(Dictionary<string, Rectangle>))]
public partial class JsonContext : JsonSerializerContext { }