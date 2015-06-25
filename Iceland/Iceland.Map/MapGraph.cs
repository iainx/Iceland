using System;
using System.Collections.Generic;

//using GameplayKit;
namespace Iceland.Map
{
    public class MapGraph //: GKGraph
    {
        #if GAMEPLAYKIT
        GKGraphNode[] gridNodes;
        Map parentMap;

        public MapGraph (Map map, GKGraphNode[] nodes) : base (nodes)
        {
            parentMap = map;
        }

        GKGraphNode NodeInDirection (Position position, Direction direction)
        {
            Position newPosition = position.PositionInDirection (Direction.North);
            if (!Map.PositionIsValid (newPosition)) {
                return null;
            }

            int index = Map.PositionToIndex (newPosition);
            return gridNodes [index];
        }

        GKGraphNode[] ConnectedNodes (Position position, Tile t)
        {
            List<GKGraphNode> nodes = new List<GKGraphNode> ();
            GKGraphNode node;
            Position newPosition;

            if (t.ValidExits & Tile.Exits.North) {
                node = NodeInDirection (position, Direction.North);
                if (node != null) {
                    nodes.Add (node);
                }
            }

            if (t.ValidExits & Tile.Exits.South) {
                node = NodeInDirection (position, Direction.South);
                if (node != null) {
                    nodes.Add (node);
                }
            }

            if (t.ValidExits & Tile.Exits.East) {
                node = NodeInDirection (position, Direction.East);
                if (node != null) {
                    nodes.Add (node);
                }
            }

            if (t.ValidExits & Tile.Exits.West) {
                node = NodeInDirection (position, Direction.West);
                if (node != null) {
                    nodes.Add (node);
                }
            }

            return nodes.ToArray ();
        }

        public static MapGraph GraphFromMap (Map map)
        {
            int index = 0;
            GKGraphNode nodes = new GKGraphNode[map.Width * map.Height];
            // Start with a map of unconnected nodes
            for (int r = 0; r < map.Height; r++) {
                for (int c = 0; c < map.Width; c++) {
                    nodes [r] [c] = new GkGraphNode ();
                }
            }

            // Go through each tile in the map connecting it up
            foreach (var tid in map.TIDs) {
                Tile t = map.TIDToTile [tid];
                if (t.ValidExits == Tile.Exits.None) {
                    continue;
                }

                Position p = map.IndexToPosition (index);

                GKGraphNode thisNode = nodes [index];
                GkGraphNode[] connectedNodes = ConnectedNodes (p, t);

                thisNode.addConnectionsToNodes (connectedNodes, false);

                index++;
            } 
        }
        #endif
    }
}