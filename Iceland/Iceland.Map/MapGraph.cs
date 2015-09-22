using System;
using System.Collections.Generic;

using GameplayKit;
namespace Iceland.Map
{
    public class MapGraph : GKGraph
    {
        GKGraphNode[] gridNodes;
        Map parentMap;

        public MapGraph (Map map, GKGraphNode[] nodes) : base (nodes)
        {
            parentMap = map;
        }

        GKGraphNode NodeInDirection (Map.Position position, Direction direction)
        {
            Map.Position newPosition = position.PositionInDirection (direction);
            if (!parentMap.PositionIsValid (newPosition)) {
                return null;
            }

            int index = parentMap.PositionToIndex (newPosition);
            return gridNodes [index];
        }

        GKGraphNode[] ConnectedNodes (Map.Position position, Tile t)
        {
            List<GKGraphNode> nodes = new List<GKGraphNode> ();
            GKGraphNode node;

            Console.WriteLine ("Node {0} - {1}, {2}", position, t.ImageName, t.ValidExits);
            if ((t.ValidExits & Tile.Exits.North) == Tile.Exits.North) {
                Console.WriteLine ("   North");
                node = NodeInDirection (position, Direction.North);
                if (node != null) {
                    nodes.Add (node);
                }
            }

            if ((t.ValidExits & Tile.Exits.South) == Tile.Exits.South) {
                Console.WriteLine ("   South");
                node = NodeInDirection (position, Direction.South);
                if (node != null) {
                    nodes.Add (node);
                }
            }

            if ((t.ValidExits & Tile.Exits.East) == Tile.Exits.East) {
                Console.WriteLine ("   East");
                node = NodeInDirection (position, Direction.East);
                if (node != null) {
                    nodes.Add (node);
                }
            }

            if ((t.ValidExits & Tile.Exits.West) == Tile.Exits.West) {
                Console.WriteLine ("   West");
                node = NodeInDirection (position, Direction.West);
                if (node != null) {
                    nodes.Add (node);
                }
            }

            return nodes.ToArray ();
        }

        public MapGraph (Map map)
        {
            parentMap = map;

            int index = 0;
            gridNodes = new MapGraphNode[map.Width * map.Height];

            // Start with a map of unconnected nodes
            for (int r = 0; r < map.Height; r++) {
                for (int c = 0; c < map.Width; c++) {
                    Map.Position position = new Map.Position { Row = r, Column = c };
                    gridNodes [(r * map.Width) + c] = new MapGraphNode { NodePosition = position };
                }
            }

            // Go through each tile in the map connecting it up
            foreach (var tid in map.TIDs) {
                Tile t = map.TIDToTile [tid];
                if (t.ValidExits == Tile.Exits.None) {
                    index++;
                    continue;
                }

                Map.Position p = map.IndexToPosition (index);

                GKGraphNode thisNode = gridNodes [index];
                GKGraphNode[] connectedNodes = ConnectedNodes (p, t);

                thisNode.AddConnections (connectedNodes, false);
                index++;
            } 
        }

        public GKGraphNode[] FindPath (Map.Position from, Map.Position to)
        {
            GKGraphNode startNode, endNode;

            startNode = gridNodes [parentMap.PositionToIndex (from)];
            endNode = gridNodes [parentMap.PositionToIndex (to)];
            return base.FindPath (startNode, endNode);
        }
    }
}