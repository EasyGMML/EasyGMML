using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UndertaleModLib;
using UndertaleModLib.Models;

namespace EasyGMML
{
    public static class Helpers
    {
        // taken from mtms api
        public static UndertaleRoom.GameObject AddObjectToLayer(this UndertaleRoom room, UndertaleData data, string objectname, string layername)
        {
            data.GeneralInfo.LastObj++;
            UndertaleRoom.GameObject obj = new UndertaleRoom.GameObject()
            {
                InstanceID = data.GeneralInfo.LastObj,
                ObjectDefinition = data.GameObjects.ByName(objectname),
                X = 0,
                Y = 0
            };

            room.Layers.First(layer => layer.LayerName.Content == layername).InstancesData.Instances.Add(obj);

            room.GameObjects.Add(obj);

            return obj;
        }

        // again taken from mtms api
        public static UndertaleRoom CreateBlankLevelRoom(string roomname, UndertaleData data)
        {
            UndertaleRoom copyme_room = data.Rooms.First(room => room.Name.Content == "level_basic_copy_me");

            if (copyme_room == null)
            {
                throw new NullReferenceException("Unable to find: level_basic_copy_me!");
            }
            else
            {
                UndertaleRoom newroom = new UndertaleRoom();
                newroom.Name = data.Strings.MakeString(roomname);
                newroom.Width = copyme_room.Width;
                newroom.Height = copyme_room.Height;
                newroom.BackgroundColor = copyme_room.BackgroundColor;
                newroom.Flags = copyme_room.Flags;

                for (int i = 0; i < copyme_room.Views.Count; i++)
                {
                    UndertaleRoom.View copyview = copyme_room.Views[i];
                    newroom.Views[i] = new UndertaleRoom.View()
                    {
                        Enabled = copyview.Enabled,
                        BorderX = copyview.BorderX,
                        BorderY = copyview.BorderY,
                        ObjectId = copyview.ObjectId,
                        PortHeight = copyview.PortHeight,
                        PortWidth = copyview.PortWidth,
                        PortX = copyview.PortX,
                        PortY = copyview.PortY,
                        ViewHeight = copyview.ViewHeight,
                        ViewWidth = copyview.ViewWidth,
                        ViewX = copyview.ViewX,
                        ViewY = copyview.ViewY,
                        SpeedX = copyview.SpeedX,
                        SpeedY = copyview.SpeedY

                    };
                }

                uint largest_layerid = 0;

                // Find the largest layer id
                // Shamelessly stolen from UMT source
                foreach (UndertaleRoom Room in data.Rooms)
                {
                    foreach (UndertaleRoom.Layer Layer in Room.Layers)
                    {
                        if (Layer.LayerId > largest_layerid)
                            largest_layerid = Layer.LayerId;
                    }
                }

                foreach (UndertaleRoom.Layer copylayer in copyme_room.Layers)
                {
                    UndertaleRoom.Layer layer = new UndertaleRoom.Layer() //thanks to config for making my code actually good :P
                    {
                        LayerId = largest_layerid++, //maybe??
                        LayerName = copylayer.LayerName,
                        LayerType = copylayer.LayerType,
                        IsVisible = copylayer.IsVisible,
                        LayerDepth = copylayer.LayerDepth,
                    };
                    layer.Data = (UndertaleRoom.Layer.LayerData)Activator.CreateInstance(copylayer.Data.GetType()); //again thanks to config!!!
                    layer.EffectProperties = copylayer.EffectProperties;

                    layer.EffectProperties = new UndertaleSimpleList<UndertaleRoom.EffectProperty>();

                    if (layer.LayerType == UndertaleRoom.LayerType.Background)
                    {
                        layer.BackgroundData.AnimationSpeed = copylayer.BackgroundData.AnimationSpeed;
                        layer.BackgroundData.AnimationSpeedType = copylayer.BackgroundData.AnimationSpeedType;
                        layer.BackgroundData.CalcScaleX = copylayer.BackgroundData.CalcScaleX;
                        layer.BackgroundData.CalcScaleY = copylayer.BackgroundData.CalcScaleY;
                        layer.BackgroundData.Color = copylayer.BackgroundData.Color;
                        layer.BackgroundData.FirstFrame = copylayer.BackgroundData.FirstFrame;
                        layer.BackgroundData.Foreground = copylayer.BackgroundData.Foreground;
                        layer.BackgroundData.Sprite = copylayer.BackgroundData.Sprite;
                        layer.BackgroundData.Stretch = copylayer.BackgroundData.Stretch;
                        layer.BackgroundData.TiledHorizontally = copylayer.BackgroundData.TiledHorizontally;
                        layer.BackgroundData.TiledVertically = copylayer.BackgroundData.TiledVertically;
                        layer.BackgroundData.Visible = copylayer.BackgroundData.Visible;
                    }

                    // Somewhat shamefully stolen from UMT source
                    if (layer.LayerType == UndertaleRoom.LayerType.Assets)
                    {
                        // create a new pointer list (if null)
                        layer.AssetsData.LegacyTiles ??= new UndertalePointerList<UndertaleRoom.Tile>();
                        // create new sprite pointer list (if null)
                        layer.AssetsData.Sprites ??= new UndertalePointerList<UndertaleRoom.SpriteInstance>();
                        // create new sequence pointer list (if null)
                        layer.AssetsData.Sequences ??= new UndertalePointerList<UndertaleRoom.SequenceInstance>();
                    }
                    else if (layer.LayerType == UndertaleRoom.LayerType.Tiles)
                    {
                        // create new tile data (if null)
                        layer.TilesData.TileData ??= Array.Empty<uint[]>();
                    }

                    newroom.Layers.Add(layer);

                    newroom.UpdateBGColorLayer();

                    newroom.SetupRoom(false);
                }

                newroom.GridHeight = 60;

                newroom.GridWidth = 60;


                newroom.SetupRoom(false);

                return newroom;
            }
        }

        public static Type FindType(string qualifiedTypeName)
        {
            Type t = Type.GetType(qualifiedTypeName);

            if (t != null)
            {
                return t;
            }
            else
            {
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    t = asm.GetType(qualifiedTypeName);
                    if (t != null)
                        return t;
                }
                return null;
            }
        }

        public static int GetValueOf(string enumName, string enumConst)
        {
            Type enumType = FindType(enumName);
            if (enumType == null)
            {
                throw new ArgumentException("Specified enum type could not be found", "enumName");
            }

            object value = Enum.Parse(enumType, enumConst);
            return Convert.ToInt32(value);
        }
    }
}