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

                    default:
                        break;
                    }
                }
            }

            Centre = new CoreGraphics.CGPoint (centreX, centreY);
        }
    }
}

