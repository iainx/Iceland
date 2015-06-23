using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using CoreGraphics;

namespace Iceland.Map
{
    public class Tile
    {
        public string ImageName { get; private set; }
        public float BaseOffset { get; private set; }

        public CoreGraphics.CGSize Size { get; set; }
        public CoreGraphics.CGPoint Centre { get; set; }

        [Flags]
        public enum Exits {
            None = 0,
            North = 1,
            South = 2,
            East = 4,
            West = 8
        };

        public Exits ValidExits { get; set; }

        Exits ParseExits (string exitString)
        {
            Exits exits = Exits.None;

            foreach (var exit in exitString.Split (',')) {
                switch (exit) {
                case "north":
                    exits |= Exits.North;
                    break;

                case "south":
                    exits |= Exits.South;
                    break;

                case "east":
                    exits |= Exits.East;
                    break;

                case "west":
                    exits |= Exits.West;
                    break;

                default:
                    break;
                }
            }

            return exits;
        }

        public Tile (string filename, XElement properties)
        {
            ImageName = Path.GetFileNameWithoutExtension (filename);
            BaseOffset = 15.0f;

            int centreX = 50, centreY = 30;
            if (properties != null) {
                foreach (var propElement in properties.Elements ()) {
                    switch (propElement.Attribute ("name").Value) {
                    case "centre-x":
                        centreX = Convert.ToInt32 (propElement.Attribute ("value").Value);
                        break;

                    case "centre-y":
                        centreY = Convert.ToInt32 (propElement.Attribute ("value").Value);
                        break;

                    case "exits":
                        ValidExits = ParseExits (propElement.Attribute ("value").Value);
                        Console.WriteLine ("Exits: {0}", ValidExits);
                        break;

                    default:
                        break;
                    }
                }
            }

            Centre = new CoreGraphics.CGPoint (centreX, centreY);
        }
    }
}

