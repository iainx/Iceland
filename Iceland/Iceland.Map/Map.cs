using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Iceland.Map
{
    public class Map
    {
        public struct Position {
            public int Row;
            public int Column;

            public override string ToString ()
            {
                return string.Format ("{0},{1}", Row, Column);
            }
        };

        enum MapOrientation {
            Orthogonal,
            Isometric,
            Staggered,
            Hexagonal,
            Unsupported
        }

        enum MapStaggerIndex {
            Odd,
            Even,
            Unsupported
        }

        enum MapRenderOrder {
            RightDown,
            RightUp,
            LeftDown,
            LeftUp,
            Unsupported
        }

        enum MapStaggerAxis {
            X,
            Y,
            Unsupported
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Dictionary<UInt32, Tile> TIDToTile { get; private set; }
        public UInt32[] TIDs { get; private set; }

        public Map ()
        {
        }

        public static Map LoadFromFile (string filename)
        {
            XDocument input = XDocument.Load(filename);

            MapOrientation mapOrientation = MapOrientation.Unsupported;
            switch (input.Document.Root.Attribute("orientation").Value) {
            case "orthogonal":
                mapOrientation = MapOrientation.Orthogonal;
                break;

            case "isometric":
                mapOrientation = MapOrientation.Isometric;
                break;

            case "staggered":
                mapOrientation = MapOrientation.Staggered;
                break;

            case "hexagonal":
                mapOrientation = MapOrientation.Hexagonal;
                break;

            default:
                mapOrientation = MapOrientation.Unsupported;
                break;
            }

            if (mapOrientation == MapOrientation.Unsupported)
                throw new NotSupportedException("Map - Unsupported map type: " + input.Document.Root.Attribute("orientation").Value);

            String mapName = Path.GetFileNameWithoutExtension(filename);
            float mapWidth = Convert.ToSingle(input.Document.Root.Attribute("width").Value);
            float mapHeight = Convert.ToSingle(input.Document.Root.Attribute("height").Value);
            float mapTileWidth = Convert.ToSingle(input.Document.Root.Attribute("tilewidth").Value);
            float mapTileHeight = Convert.ToSingle(input.Document.Root.Attribute("tileheight").Value);

            MapRenderOrder mapRenderOrder = MapRenderOrder.LeftDown;
            if (input.Document.Root.Attribute("renderorder") != null) {
                switch (input.Document.Root.Attribute("renderorder").Value) {
                case "right-down":
                    mapRenderOrder = MapRenderOrder.RightDown;
                    break;

                case "right-up":
                    mapRenderOrder = MapRenderOrder.RightUp;
                    break;

                case "left-down":
                    mapRenderOrder = MapRenderOrder.LeftDown;
                    break;

                case "left-up":
                    mapRenderOrder = MapRenderOrder.LeftUp;
                    break;

                default:
                    mapRenderOrder = MapRenderOrder.Unsupported;
                    break;
                }

                if (mapRenderOrder == MapRenderOrder.Unsupported)
                    throw new NotSupportedException("Unsupported map render order: " + input.Document.Root.Attribute("renderorder").Value);
            }

            Console.WriteLine ("Map Name: " + mapName);
            Console.WriteLine ("   " + mapWidth + "x" + mapHeight + " (" + mapTileWidth + "x" + mapTileHeight + ")");
            Console.WriteLine ("   RenderOrder: " + mapRenderOrder.ToString());

            Dictionary<UInt32, Tile> gidToTile = new Dictionary<UInt32, Tile>();

            foreach (var elem in input.Document.Root.Elements("tileset")) {
                UInt32 FirstGID = Convert.ToUInt32(elem.Attribute("firstgid").Value);
                XElement tsElem = elem;

                String tsImageBaseDir = Path.GetDirectoryName(filename);
                if (elem.Attribute("source") != null) {
                    XDocument tsx = XDocument.Load(Path.Combine(tsImageBaseDir, elem.Attribute("source").Value));
                    tsElem = tsx.Root;
                    tsImageBaseDir = Path.GetDirectoryName(Path.Combine(tsImageBaseDir, elem.Attribute("source").Value));
                }
             
                Int32 tsTileWidth = tsElem.Attribute("tilewidth") == null ? 0 : Convert.ToInt32(tsElem.Attribute("tilewidth").Value);
                Int32 tsTileHeight = tsElem.Attribute("tileheight") == null ? 0 : Convert.ToInt32(tsElem.Attribute("tileheight").Value);
//                Int32 tsSpacing = tsElem.Attribute("spacing") == null ? 0 : Convert.ToInt32(tsElem.Attribute("spacing").Value);
//                Int32 tsMargin = tsElem.Attribute("margin") == null ? 0 : Convert.ToInt32(tsElem.Attribute("margin").Value);

                /*
                CoreGraphics.CGPoint tileCentre = new CoreGraphics.CGPoint (0f, 0f);
                if (tsElem.Element("tileoffset") != null) {
                    Int32 tsTileOffsetX = Convert.ToInt32(tsElem.Element("tileoffset").Attribute("x").Value);
                    Int32 tsTileOffsetY = Convert.ToInt32(tsElem.Element("tileoffset").Attribute("y").Value);

                    tileCentre.X = tsTileOffsetX;
                    tileCentre.Y = tsTileOffsetY;
                }
*/
                foreach (var tile in elem.Elements("tile")) {
                    UInt32 gid = Convert.ToUInt32 (tile.Attribute("id").Value);
                    var image = tile.Element ("image");
                    if (image == null) {
                        continue;
                    }

                    string exits = null;
                    XElement properties = tile.Element ("properties");

                    float imageWidth = Convert.ToSingle (image.Attribute("width").Value);
                    float imageHeight = Convert.ToSingle (image.Attribute("height").Value);
                    string sourceFile = image.Attribute("source").Value;
                    Tile tileObject = new Tile(sourceFile, properties) {
                        Size = new CoreGraphics.CGSize (tsTileWidth, tsTileHeight)
                    };

                    gidToTile[gid + FirstGID] = tileObject;
                }
            }

            List<UInt32> gids = null;

            foreach (var lElem in input.Document.Root.Elements()) {
                if (lElem.Name.LocalName.Equals("layer")) {
                    gids = parseLayer(lElem);
                }
            }

            if (gids == null) {
                return null;
            }

            return new Map {
                Width = (int)mapWidth,
                Height = (int)mapHeight,
                TIDToTile = gidToTile,
                TIDs = gids.ToArray ()
            };
        }

        static List<UInt32> parseLayer(XElement layerElement) {
            List<UInt32> gids = new List<UInt32>();

            if (layerElement.Element("data") != null) {
                if (layerElement.Element("data").Attribute("encoding") != null || layerElement.Element("data").Attribute("compression") != null) {

                    // parse csv formatted data
                    if (layerElement.Element("data").Attribute("encoding") != null && layerElement.Element("data").Attribute("encoding").Value.Equals("csv")) {
                        foreach (var gid in layerElement.Element("data").Value.Split(",\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)) {
                            gids.Add(Convert.ToUInt32(gid));
                        }
                    }
                }
            }

            return gids;
        }

        public Position IndexToPosition (int index)
        {
            return new Position {
                Row = index / Width,
                Column = index % Width
            };
        }

        public int PositionToIndex (Position position)
        {
            return (position.Row * Width) + position.Column;
        }

        public CoreGraphics.CGPoint PositionToPoint (Position position, bool topOfTile = false)
        {
            float row, column, x, y;

            row = (float)position.Row;
            column = (float)position.Column;

            y = (column + row) * 25.0f;
            x = (column - row) * 50.0f;

            // The offset depending on whether we're standing on the tile or placing the tile
            if (topOfTile) {
                Tile tile = TileAtPosition (position);
                y -= (float)tile.Centre.Y;
            }
            return new CoreGraphics.CGPoint (x, -y);
        }

        public Position PointToPosition (CoreGraphics.CGPoint point)
        {
            float row, column;
            float y = (float)Math.Abs (point.Y - 65);

            row = ((y / 25f) - ((float)point.X / 50f)) / 2f;
            column = (((float)point.X / 50f) + (y / 25f)) / 2f;

            return new Position { Row = (int)row, Column = (int)column };
        }

        public Tile TileAtPosition (Position position)
        {
            int idx = PositionToIndex (position);
            UInt32 tid = TIDs [idx];
            return TIDToTile [tid];
        }

        public int ZLevelForPosition (Position position)
        {
            return ((position.Row * 11) * Width + (position.Column * 11)) + 11;
        }
    }
}

