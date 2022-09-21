using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndertaleModLib.Models;

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
    // GameObjects have more params so they extend a normal object
    public class GameObject : Object
    {
        public string? name { get; set; } // probally will be removed ask name will move to Object sometime soon
        public Dictionary<string, dynamic>? objects { get; set; }
    }
}
