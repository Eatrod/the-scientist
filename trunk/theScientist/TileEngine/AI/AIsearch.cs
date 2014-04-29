using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Tiles;
using TileEngine.Sprite;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.AI
{
    public class AIsearch
    {
        private List<TileNode> tileNodes;
        private int[,] Grid;
        private Dictionary<TileNode, TileNode> finalPath;
        public Dictionary<TileNode,TileNode> FinalPath
        {
            get { return finalPath; }
            set { finalPath = value; }
        }
        public AIsearch(int [,] Map)
        {
            this.finalPath = new Dictionary<TileNode, TileNode>();
            this.tileNodes = new List<TileNode>();
            this.Grid = Map;
            //GenerateNeighboursForTileNodes();
            
        
        }
        public void GenerateNeighboursForTileNodes()
        {
            foreach(TileNode tn in tileNodes)
            {
                tn.AddNeighbours(tileNodes);
            }
        }
        public void GenerateTileNodes(Vector2 Position)
        {
            int topY = (int)Position.Y + 30;
            int bottomY = (int)Position.Y - 30;
            int topX = (int)Position.X + 30;
            int bottomX = (int)Position.X - 30;
            if (bottomX < 0)
                bottomX = 0;
            if (bottomY < 0)
                bottomY = 0;

            for(int y = bottomY; y < topY; y++)
            {
                for (int x = bottomX; x < topX; x++ )
                {
                    if(Grid[y,x] == -1)
                    {
                        TileNode tn = new TileNode(new Vector2(x, y), -1);
                        tileNodes.Add(tn);
                    }
                }
            }
        }
        public TileNode GetNodeFromNodes(Vector2 Position)
        {
            foreach(TileNode tn in tileNodes)
            {
                if(tn.PositionInGrid == Position)
                {
                    return tn;
                }
            }
            return null;
        }
        public void ResetNodes(Vector2 GridPositionGoal)
        {
            foreach (TileNode tn1 in tileNodes)
            {
                tn1.Cost = 0;
                //tn1.ManhattanDistance = Vector2.Distance(tn1.PositionInGrid, GridPositionGoal); 
                tn1.ManhattanDistance = Math.Abs(tn1.PositionInGrid.X - GridPositionGoal.X) +
                    Math.Abs(tn1.PositionInGrid.Y - GridPositionGoal.Y);
            }

        }
        public void SearchForShortestPath(TileNode TileNodeStart, TileNode TileNodeGoal)
        {
            FinalPath.Clear();
            if (TileNodeGoal == null || TileNodeStart == null)
                return;
            ResetNodes(TileNodeGoal.PositionInGrid);
            Queue<TileNode> Frontier = new Queue<TileNode>();
            Dictionary<TileNode, TileNode> FirstPath = new Dictionary<TileNode, TileNode>();
            List<TileNode> visitedNodes = new List<TileNode>();
            TileNode startPoint = TileNodeStart;
            TileNode goalPoint = TileNodeGoal;
            int Count = 0;
            startPoint.Cost = Count;
            Frontier.Enqueue(startPoint);
            while (Frontier.Count > 0)
            {
                SortQueue(Frontier);
                TileNode node = Frontier.Dequeue();
                Count++;
                if (node == goalPoint)
                {
                    TileNode e = node;
                    while (e != startPoint)
                    {
                        FinalPath.Add(e, FirstPath[e]);
                        e = FinalPath[e];
                    }
                    break;
                }
                visitedNodes.Add(node);
                foreach (TileNode neighbour in node.Neighbours)
                {
                    if (!visitedNodes.Exists(Node => Node == neighbour))
                    {
                        //if(!Frontier.Contains(neighbour))
                        Frontier.Enqueue(neighbour);
                        if (FirstPath.ContainsKey(neighbour))
                            FirstPath.Remove(neighbour);
                        FirstPath.Add(neighbour, node);
                        neighbour.Cost = node.Cost + 1;
                    }
                    else if (neighbour.Cost < node.Cost + 1)
                    {
                        
                        if (FirstPath.ContainsKey(neighbour))
                            FirstPath.Remove(neighbour);
                        FirstPath.Add(neighbour, node);
                        //if (!Frontier.Contains(neighbour))
                        Frontier.Enqueue(neighbour);
                        visitedNodes.Remove(neighbour);
                        neighbour.Cost = node.Cost + 1;
                    }
                }
            }
        }
        public void SortQueue(Queue<TileNode> Frontier)
        {
            Frontier.OrderBy(x => x.Cost).ThenBy(x => x.ManhattanDistance);
        }
    }
}
