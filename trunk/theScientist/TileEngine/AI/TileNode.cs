using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.AI
{
    public class TileNode
    {
        private Vector2 positionInGrid;
        private List<TileNode> neighbours;
        private int cost;
        private float manhattanDistance;
        private int valueOfGrid;
        public Vector2 PositionInGrid
        {
            get { return positionInGrid; }
            set { positionInGrid = value; }
        }
        public List<TileNode> Neighbours
        {
            get { return neighbours; }
        }
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        public float ManhattanDistance
        {
            get { return manhattanDistance; }
            set { manhattanDistance = value; }
        }
        public int ValueOfGrid
        {
            get { return valueOfGrid; }
            set { valueOfGrid = value; }
        }
        public TileNode(Vector2 positionInGrid, int valueOfGrid)
        {
            this.neighbours = new List<TileNode>();
            this.positionInGrid = positionInGrid;
            this.cost = 0;
            this.valueOfGrid = valueOfGrid;
            this.manhattanDistance = 0.0f;
        }
        public void AddNeighbours(List<TileNode> TileNodes)
        {
            //if (Map[(int)positionInGrid.X, (int)positionInGrid.Y + 1] == -1)
            //    neighbours.Add(MappositionInGrid.X, positionInGrid.Y + 1]);
            //GenerateManhattanDistance(TileNodes);
            foreach (TileNode tn in TileNodes)
            {
                if (tn.positionInGrid != positionInGrid)
                {
                    //till vänster eller höger
                    if ((tn.positionInGrid.X == positionInGrid.X - 1 ||
                        tn.positionInGrid.X == positionInGrid.X + 1) &&
                        tn.positionInGrid.Y == positionInGrid.Y)
                        neighbours.Add(tn);
                    //ovanför eller under
                    else if ((tn.positionInGrid.Y == positionInGrid.Y - 1 ||
                        tn.positionInGrid.Y == positionInGrid.Y + 1) &&
                        tn.positionInGrid.X == positionInGrid.X)
                        neighbours.Add(tn);
                        //snett vänster eller snett höger under
                    //else if
                    //    ((tn.PositionInGrid.Y == positionInGrid.Y - 1 &&
                    //    tn.positionInGrid.X == positionInGrid.X - 1 ||
                    //    tn.positionInGrid.X == positionInGrid.X + 1))
                    //    neighbours.Add(tn);
                    //// snett höger eller vänster ovanför
                    //else if
                    //    ((tn.PositionInGrid.Y == positionInGrid.Y + 1 &&
                    //    tn.positionInGrid.X == positionInGrid.X - 1 ||
                    //    tn.positionInGrid.X == positionInGrid.X + 1))
                    //    neighbours.Add(tn);
                }
                if (neighbours.Count >= 8)
                    break;
            }
        }
        public void GenerateManhattanDistance(List<TileNode> TileNodes)
        {
            foreach (TileNode tn1 in TileNodes)
            {
                foreach (TileNode tn2 in TileNodes)
                {
                    if (tn2.valueOfGrid == 2)
                    {
                        this.manhattanDistance = Vector2.Distance(tn1.positionInGrid, tn2.positionInGrid);
                    }
                }
            }
        }
    }
}
