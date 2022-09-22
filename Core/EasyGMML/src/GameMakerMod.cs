using GmmlPatcher;
using System.Text.Json;
using UndertaleModLib;
using EasyGMML.Types;
using Object = EasyGMML.Types.Object;
using UndertaleModLib.Models;
using GmmlHooker;

namespace EasyGMML;

public class GameMakerMod : IGameMakerMod {
    public void Load(int audioGroup, UndertaleData data, ModData currentMod) {
        try
        {
            // idk a better way to load both
            foreach (string sound in Directory.GetFiles(Path.Combine(currentMod.path, "Sounds"), "*.wav"))
            {
                data.AddSound(audioGroup, 2, sound);
            }
            foreach (string sound in Directory.GetFiles(Path.Combine(currentMod.path, "Sounds"), "*.ogg"))
            {
                data.AddSound(audioGroup, 2, sound);
            }
        }
        catch
        {
            Console.WriteLine("If you see this please tell name on discord about it (Sound loading error)");
        }

        if (audioGroup != 0) return;

        UndertaleSprite sprite = new UndertaleSprite();

        sprite.

        data.Sprites.Add(sprite);

        try
        {
            string[] infos = Directory.GetFiles(Path.Combine(currentMod.path, "GlobalScripts"));

            for (int i = 0; i < infos.Length; i++)
            {
                FileInfo fo = new FileInfo(infos[i]);
                if (fo.Extension == ".json")
                {
                    GlobalScript? gs = new GlobalScript();

                    try
                    {
                        gs = JsonSerializer.Deserialize<GlobalScript>(File.ReadAllText(infos[i]));
                    }
                    catch
                    {
                        Console.WriteLine("Failed loading " + infos[i] + " skipping it");
                        continue;
                    }

                    if (!gs.scriptName.Contains(".gml"))
                    {
                        gs.scriptName = gs.scriptName + ".gml";
                    }

                    data.CreateLegacyScript(gs.name, File.ReadAllText(Path.Combine(currentMod.path, "Code\\" + gs.scriptName)), (ushort)gs.argumentCount);
                }
            }
        }
        catch
        {
            Console.WriteLine("If you see this please tell name on discord about it (GlobalScript loading error)");
        }

        try
        {
            string[] infos = Directory.GetFiles(Path.Combine(currentMod.path, "Objects"));

            for (int i = 0; i < infos.Length; i++)
            {
                FileInfo fo = new FileInfo(infos[i]);
                if (fo.Extension == ".json")
                {
                    GameObject? obj = new GameObject();

                    try
                    {
                        obj = JsonSerializer.Deserialize<GameObject>(File.ReadAllText(infos[i]));
                    }
                    catch
                    {
                        Console.WriteLine("Failed loading " + infos[i] + " skipping it");
                        continue;
                    }

                    UndertaleGameObject gameObject = new UndertaleGameObject();

                    gameObject.Name = data.Strings.MakeString(obj.name);

                    foreach (EventType type in Enum.GetValues(typeof(EventType)))
                    {
                        foreach (KeyValuePair<string, CodeEntry> entry in obj.code)
                        {
                            if (type == (EventType)Enum.Parse(typeof(EventType), entry.Value.type))
                            {
                                EventType etype = (EventType)Enum.Parse(typeof(EventType), entry.Value.type);

                                uint stype = (uint)Helpers.GetValueOf("UndertaleModLib.Models.EventSubtype" + entry.Value.type, entry.Value.subtype);

                                if (!entry.Value.name.Contains(".gml"))
                                {
                                    entry.Value.name = entry.Value.name + ".gml";
                                }

                                gameObject.EventHandlerFor(etype, stype , data.Strings, data.Code, data.CodeLocals).AppendGmlSafe(File.ReadAllText(Path.Combine(currentMod.path, "Code\\" + entry.Value.name)), data);
                            }
                        }
                    }

                    data.GameObjects.Add(gameObject);
                }
            }
        }
        catch
        {
            Console.WriteLine("If you see this please tell name on discord about it (Object loading error)");
        }
        
        try
        {
            string[] infos = Directory.GetFiles(Path.Combine(currentMod.path, "Rooms"));

            for (int i = 0; i < infos.Length; i++)
            {
                FileInfo fo = new FileInfo(infos[i]);
                if (fo.Extension == ".json")
                {
                    Room? room = new Room();

                    try
                    {
                        room = JsonSerializer.Deserialize<Room>(File.ReadAllText(infos[i]));
                    }
                    catch
                    {
                        Console.WriteLine("Failed loading " + infos[i] + " skipping it");
                        continue;
                    }

                    UndertaleRoom newroom = Helpers.CreateBlankLevelRoom(room.name, data);

                    newroom.Width = (uint)room.width;
                    newroom.Height = (uint)room.height;
                    newroom.CreationCodeId = data.Code.ByName(room.creationCode);

                    foreach (KeyValuePair<string, Object> entry in room.objects)
                    {
                        int index = Int32.Parse(entry.Key);

                        string objName = room.objectNames.GetValue(index).ToString();

                        UndertaleRoom.GameObject obj = Helpers.AddObjectToLayer(newroom, data, objName, entry.Value.layer);

                        obj.ScaleX = entry.Value.hscale;
                        obj.ScaleX = entry.Value.vscale;
                        obj.X = entry.Value.x;
                        obj.Y = entry.Value.y;
                        obj.Rotation = entry.Value.rotation;
                    }

                    data.Rooms.Add(newroom);
                }
            }
        }
        catch
        {
            Console.WriteLine("If you see this please tell name on discord about it (Room loading error)");
        }

        // read file into replace i think
        // data.Code.ByName("gml_Object_obj_player_Create_0").ReplaceGmlSafe("gml_Object_obj_player_Create_0", data); // not using cache because its weird soemtimes
        // mp_player_obj.EventHandlerFor(EventType.Other, EventSubtypeOther.User0, data.Strings, data.Code, data.CodeLocals)
                // .AppendGmlSafe(GMLkvp["gml_Object_obj_mp_player_Other_10"], data);
    }
}