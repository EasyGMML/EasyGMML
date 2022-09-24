using System.Text.Json;
using UndertaleModLib;
using UndertaleModLib.Models;
using EasyGMML.Types;
using Object = EasyGMML.Types.Object;

public class Program
{
    public static void Main()
    {
        List<string> objectNames = new List<string>();
        Dictionary<string, Object> objects = new Dictionary<string, Object>();

#if DEBUG
        string path = @"C:\Program Files (x86)\Steam\steamapps\common\Will You Snail\dataBackup.win";
        string CopyFrom = "A_03_hello_spikes";
        string roomName = "A_03_hello_spikes_copy";
#else
        Console.Write("Location to data.win: ");
        string? path = Console.ReadLine();
        Console.Write("\nWhat room to copy: ");
        string CopyFrom = Console.ReadLine();
        Console.Write("\nWhat to call the copied room: ");
        string roomName = Console.ReadLine();
#endif

        int size;
        UndertaleData data;

        using (FileStream fs = File.OpenRead(path))
        {
            size = (int)fs.Length;

            data = UndertaleIO.Read(fs);
        }

        int curObj = 0;
        int roomWidth = 0;
        int roomHeight = 0;
        string? creationCode = "";

        foreach (UndertaleRoom Room in data.Rooms)
        {
            if (Room.Name.Content == CopyFrom)
            {
                roomWidth = (int)Room.Width;
                roomHeight = (int)Room.Height;
                if (Room.CreationCodeId == null)
                {
                    creationCode = "";
                }
                else
                {
                    creationCode = Room.CreationCodeId.Name.Content;
                }

                foreach (UndertaleRoom.GameObject obj in Room.GameObjects)
                {
                    string layerName = "undefined";

                    foreach (UndertaleRoom.Layer layer in Room.Layers) // This is so bad but it works so fuck it
                    {
                        if (layer.InstancesData != null)
                        {
                            foreach (UndertaleRoom.GameObject gameObject in layer.InstancesData.Instances)
                            {
                                if (gameObject.InstanceID == obj.InstanceID)
                                    layerName = layer.LayerName.Content;
                            }
                        }
                    }

                    objectNames.Add(obj.ObjectDefinition.Name.Content);
                    objects.Add(curObj.ToString(), new Object { x=obj.X, y=obj.Y, vscale=obj.ScaleY, hscale=obj.ScaleX, rotation=obj.Rotation, layer=layerName });

                    curObj++;
                }
            }
        }

        String[] names = objectNames.ToArray();

        var room = new Room
        {
            name = roomName,

            width = roomWidth,

            height = roomHeight,

            creationCode = creationCode,

            objectNames = names,

            objects = objects
        };

        var options = new JsonSerializerOptions { WriteIndented = true };

        string jsonString = JsonSerializer.Serialize(room, options);

        File.WriteAllText(roomName + ".json", jsonString);

        Console.WriteLine("\nGenerated room to " + roomName + ".json");
    }
}
