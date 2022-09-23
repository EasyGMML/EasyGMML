using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndertaleModLib.Models;
using static UndertaleModLib.Models.UndertaleSprite;

namespace EasyGMML.Types
{
    public class Room
    {
        public string? name { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string? creationCode { get; set; }
        public string[]? objectNames { get; set; }
        public Dictionary<string, Object>? objects { get; set; }
    }

    public class Object
    {
        public int x { get; set; }
        public int y { get; set; }
        public float hscale { get; set; }
        public float vscale { get; set; }
        public float rotation { get; set; }
        public string? layer { get; set; }
    }

    public class GameObject
    {
        public string? name { get; set; }
        public string? sprite { get; set; }
        public bool persistent { get; set; }
        public Dictionary<string, CodeEntry>? code { get; set; }
    }

    public class CodeEntry
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public string? subtype { get; set; }
    }

    public class GlobalScript
    {
        public string? name { get; set; }
        public string? scriptName { get; set; }
        public int argumentCount { get; set; }
    }
}
