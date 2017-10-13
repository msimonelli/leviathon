//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
//  REMAINS UNCHANGED.
//
//  Email:  gustavo_franco@hotmail.com
//
//  Copyright (C) 2006 Franco, Gustavo 
//
//#define DEBUGON

using System;
using System.Collections.Generic;


#region Structs
public struct PathFinderNode
{
    #region Variables Declaration
    public int     F;
    public int     Cost;
    public int     H;  // f = gone + heuristic
    public int     X;
    public int     Y;
    public int     ParentX; // Parent
    public int     ParentY;
    #endregion
}
#endregion

#region Enum
public enum PathFinderNodeType
{
    Start   = 1,
    End     = 2,
    Open    = 4,
    Close   = 8,
    Current = 16,
    Path    = 32
}

public enum HeuristicFormula
{
    Manhattan           = 1,
    MaxDXDY             = 2,
    DiagonalShortCut    = 3,
    Euclidean           = 4,
    EuclideanNoSQR      = 5,
    Custom1             = 6
}
#endregion

#region Delegates
public delegate void PathFinderDebugHandler(int fromX, int fromY, int x, int y, PathFinderNodeType type, int totalCost, int cost);
#endregion

public class PathFinder : IPathFinder
{
    #region Events
    public event PathFinderDebugHandler PathFinderDebug;
    #endregion

    #region Variables Declaration
    private byte[,]                         mGrid                   = null;
    private PriorityQueueB<PathFinderNode>  mOpen                   = new PriorityQueueB<PathFinderNode>(new ComparePFNode());
    private List<PathFinderNode>            mClose                  = new List<PathFinderNode>();
    private bool                            mStop                   = false;
    private bool                            mStopped                = true;
    private int                             mHoriz                  = 0;
    private HeuristicFormula                mFormula                = HeuristicFormula.DiagonalShortCut;
    private bool                            mDiagonals              = true;
    private int                             mHEstimate              = 1;
    private bool                            mPunishChangeDirection  = false;
    private bool                            mTieBreaker             = false;
    private bool                            mHeavyDiagonals         = true;
    private int                             mSearchLimit            = 100;
    private bool                            mDebugProgress          = false;
    private bool                            mDebugFoundPath         = false;
    #endregion

    #region Constructors
    public PathFinder(byte[,] grid)
    {
        if (grid == null)
            throw new Exception("Grid cannot be null");

        mGrid = grid;
    }
    #endregion

    #region Properties
    public bool Stopped
    {
        get { return mStopped; }
    }

    public HeuristicFormula Formula
    {
        get { return mFormula; }
        set { mFormula = value; }
    }

    public bool Diagonals
    {
        get { return mDiagonals; }
        set { mDiagonals = value; }
    }

    public bool HeavyDiagonals
    {
        get { return mHeavyDiagonals; }
        set { mHeavyDiagonals = value; }
    }

    public int HeuristicEstimate
    {
        get { return mHEstimate; }
        set { mHEstimate = value; }
    }

    public bool PunishChangeDirection
    {
        get { return mPunishChangeDirection; }
        set { mPunishChangeDirection = value; }
    }

    public bool TieBreaker
    {
        get { return mTieBreaker; }
        set { mTieBreaker = value; }
    }

    public int SearchLimit
    {
        get { return mSearchLimit; }
        set { mSearchLimit = value; }
    }

    public bool DebugProgress
    {
        get { return mDebugProgress; }
        set { mDebugProgress = value; }
    }

    public bool DebugFoundPath
    {
        get { return mDebugFoundPath; }
        set { mDebugFoundPath = value; }
    }
    #endregion

    #region Methods
    public void FindPathStop()
    {
        mStop = true;
    }

    public List<PathFinderNode> FindPath(Position start, Position end)
    {
        PathFinderNode parentNode;
        bool found  = false;
        int  gridX  = mGrid.GetUpperBound(0);
        int  gridY  = mGrid.GetUpperBound(1);

        mStop       = false;
        mStopped    = false;
        mOpen.Clear();
        mClose.Clear();

        #if DEBUGON
        if (mDebugProgress && PathFinderDebug != null)
            PathFinderDebug(0, 0, start.X, start.Y, PathFinderNodeType.Start, -1, -1);
        if (mDebugProgress && PathFinderDebug != null)
            PathFinderDebug(0, 0, end.X, end.Y, PathFinderNodeType.End, -1, -1);
        #endif

        sbyte[,] direction;
        if (mDiagonals)
            direction = new sbyte[8,2]{ {0,-1} , {1,0}, {0,1}, {-1,0}, {1,-1}, {1,1}, {-1,1}, {-1,-1}};
        else
            direction = new sbyte[4,2]{ {0,-1} , {1,0}, {0,1}, {-1,0}};

        parentNode.Cost      = 0;
        parentNode.H         = mHEstimate;
        parentNode.F         = parentNode.Cost + parentNode.H;
        parentNode.X         = start.X;
        parentNode.Y         = start.Y;
        parentNode.ParentX   = parentNode.X;
        parentNode.ParentY   = parentNode.Y;
        mOpen.Push(parentNode);
        while(mOpen.Count > 0 && !mStop)
        {
            parentNode = mOpen.Pop();

            #if DEBUGON
            if (mDebugProgress && PathFinderDebug != null)
                PathFinderDebug(0, 0, parentNode.X, parentNode.Y, PathFinderNodeType.Current, -1, -1);
            #endif

            if (parentNode.X == end.X && parentNode.Y == end.Y)
            {
                mClose.Insert(0, parentNode);
                found = true;
                break;
            }

            if (mClose.Count > mSearchLimit)
            {
                mStopped = true;
                return null;
            }

            if (mPunishChangeDirection)
               mHoriz = (parentNode.X - parentNode.ParentX); 

            //Lets calculate each successors
            for (int i=0; i<(mDiagonals ? 8 : 4); i++)
            {
                PathFinderNode newNode;
                newNode.X = parentNode.X + direction[i,0];
                newNode.Y = parentNode.Y + direction[i,1];

                if (newNode.X < 0 || newNode.Y < 0 || newNode.X >= gridX || newNode.Y >= gridY)
                    continue;

                int newG;
                if (mHeavyDiagonals && i>3)
                    newG = parentNode.Cost + (int)(mGrid[newNode.X, newNode.Y] * 2.41);
                else
                    newG = parentNode.Cost + mGrid[newNode.X, newNode.Y];


                if (newG == parentNode.Cost)
                {
                    //Unbrekeable
                    continue;
                }

                if (mPunishChangeDirection)
                {
                    if ((newNode.X - parentNode.X) != 0)
                    {
                        if (mHoriz == 0)
                            newG += 20;
                    }
                    if ((newNode.Y - parentNode.Y) != 0)
                    {
                        if (mHoriz != 0)
                            newG += 20;

                    }
                }

                int     foundInOpenIndex = -1;
                for(int j=0; j<mOpen.Count; j++)
                {
                    if (mOpen[j].X == newNode.X && mOpen[j].Y == newNode.Y)
                    {
                        foundInOpenIndex = j;
                        break;
                    }
                }
                if (foundInOpenIndex != -1 && mOpen[foundInOpenIndex].Cost <= newG)
                    continue;

                int     foundInCloseIndex = -1;
                for(int j=0; j<mClose.Count; j++)
                {
                    if (mClose[j].X == newNode.X && mClose[j].Y == newNode.Y)
                    {
                        foundInCloseIndex = j;
                        break;
                    }
                }
                if (foundInCloseIndex != -1 && mClose[foundInCloseIndex].Cost <= newG)
                    continue;

                newNode.ParentX = parentNode.X;
                newNode.ParentY = parentNode.Y;
                newNode.Cost    = newG;

                switch(mFormula)
                {
                    default:
                    case HeuristicFormula.Manhattan:
                        newNode.H       = mHEstimate * (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));
                        break;
                    case HeuristicFormula.MaxDXDY:
                        newNode.H       = mHEstimate * (Math.Max(Math.Abs(newNode.X - end.X), Math.Abs(newNode.Y - end.Y)));
                        break;
                    case HeuristicFormula.DiagonalShortCut:
                        int h_diagonal  = Math.Min(Math.Abs(newNode.X - end.X), Math.Abs(newNode.Y - end.Y));
                        int h_straight  = (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));
                        newNode.H       = (mHEstimate * 2) * h_diagonal + mHEstimate * (h_straight - 2 * h_diagonal);
                        break;
                    case HeuristicFormula.Euclidean:
                        newNode.H       = (int) (mHEstimate * Math.Sqrt(Math.Pow((newNode.X - end.X) , 2) + Math.Pow((newNode.Y - end.Y), 2)));
                        break;
                    case HeuristicFormula.EuclideanNoSQR:
                        newNode.H       = (int) (mHEstimate * (Math.Pow((newNode.X - end.X) , 2) + Math.Pow((newNode.Y - end.Y), 2)));
                        break;
                    case HeuristicFormula.Custom1:
					    int dx         = Math.Abs(end.X - newNode.X);
					    int dy          = Math.Abs(end.Y - newNode.Y);
                        int Orthogonal  = Math.Abs(dx - dy);
                        int Diagonal    = Math.Abs(((dx + dy) - Orthogonal) / 2);
                        newNode.H       = mHEstimate * (Diagonal + Orthogonal + dx + dy);
                        break;
                }
                if (mTieBreaker)
                {
                    int dx1 = parentNode.X - end.X;
                    int dy1 = parentNode.Y - end.Y;
                    int dx2 = start.X - end.X;
                    int dy2 = start.Y - end.Y;
                    int cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                    newNode.H = (int) (newNode.H + cross * 0.001);
                }
                newNode.F       = newNode.Cost + newNode.H;

                #if DEBUGON
                if (mDebugProgress && PathFinderDebug != null)
                    PathFinderDebug(parentNode.X, parentNode.Y, newNode.X, newNode.Y, PathFinderNodeType.Open, newNode.F, newNode.G);
                #endif
                

                //It is faster if we leave the open node in the priority queue
                //When it is removed, all nodes around will be closed, it will be ignored automatically
                //if (foundInOpenIndex != -1)
                //    mOpen.RemoveAt(foundInOpenIndex);

                //if (foundInOpenIndex == -1)
                    mOpen.Push(newNode);
            }

            mClose.Insert(0, parentNode);

            #if DEBUGON
            if (mDebugProgress && PathFinderDebug != null)
                PathFinderDebug(0, 0, parentNode.X, parentNode.Y, PathFinderNodeType.Close, parentNode.F, parentNode.G);
            #endif
        }

        if (found)
        {
            PathFinderNode fNode = mClose[mClose.Count - 1];
            for(int i=mClose.Count - 1; i>=0; i--)
            {
                if (fNode.ParentX == mClose[i].X && fNode.ParentY == mClose[i].Y || i == mClose.Count - 1)
                {
                    #if DEBUGON
                    if (mDebugFoundPath && PathFinderDebug != null)
                        PathFinderDebug(fNode.X, fNode.Y, mClose[i].X, mClose[i].Y, PathFinderNodeType.Path, mClose[i].F, mClose[i].G);
                    #endif
                    fNode = mClose[i];
                }
                else
                    mClose.RemoveAt(i);
            }
            mStopped = true;
            return mClose;
        }
        mStopped = true;
        return null;
    }

    public List<PathFinderNode> FindLegalMoves(Position start, int max_moves)
    {
       lock (this)
       {
          List<PathFinderNode> all_legal_moves = new List<PathFinderNode>();
          List<PathFinderNode> last_legal_moves = new List<PathFinderNode>();
          PathFinderNode node;
          ushort mGridX = (ushort)(mGrid.GetUpperBound(0) + 1);
          ushort mGridY = (ushort)(mGrid.GetUpperBound(1) + 1);

          node.F = 0;
          node.H = 0;
          node.ParentX = 0;
          node.ParentY = 0;
          node.X = start.X;
          node.Y = start.Y;
          node.Cost = 0;

          all_legal_moves.Add(node);
          last_legal_moves.Add(node);

          for (int x = 1; x <= max_moves; x++)
          {
             List<PathFinderNode> cur_legal_moves = new List<PathFinderNode>();

             for (int y = 0; y < last_legal_moves.Count; y++)
             {
                PathFinderNode pos = last_legal_moves[y];
                node.ParentX = pos.X;
                node.ParentY = pos.Y;

                // Check down
                node.X = pos.X;
                node.Y = pos.Y - 1;
                if (node.Y >= 0 && node.Y < mGridY && pos.Cost + mGrid[node.X, node.Y] <= max_moves && mGrid[node.X, node.Y] > 0)
                {
                   node.Cost = pos.Cost + mGrid[node.X, node.Y];
                   cur_legal_moves.Add(node);
                }

                // Check up
                node.X = pos.X;
                node.Y = pos.Y + 1;
                if (node.Y >= 0 && node.Y < mGridY && pos.Cost + mGrid[node.X, node.Y] <= max_moves && mGrid[node.X, node.Y] > 0)
                {
                   node.Cost = pos.Cost + mGrid[node.X, node.Y];
                   cur_legal_moves.Add(node);
                }

                // Check left
                node.X = pos.X - 1;
                node.Y = pos.Y;
                if (node.X >= 0 && node.X < mGridX && pos.Cost + mGrid[node.X, node.Y] <= max_moves && mGrid[node.X, node.Y] > 0)
                {
                   node.Cost = pos.Cost + mGrid[node.X, node.Y];
                   cur_legal_moves.Add(node);
                }

                // Check right
                node.X = pos.X + 1;
                node.Y = pos.Y;
                if (node.X >= 0 && node.X < mGridX && pos.Cost + mGrid[node.X, node.Y] <= max_moves && mGrid[node.X, node.Y] > 0)
                {
                   node.Cost = pos.Cost + mGrid[node.X, node.Y];
                   cur_legal_moves.Add(node);
                }
             }

             // Add to our list of last legal moves;
             for (int z = cur_legal_moves.Count - 1; z >= 0; z--)
             {
                int existing_index = -1;
                for (int i = 0; i < all_legal_moves.Count; i++)
                {
                   if (all_legal_moves[i].X == cur_legal_moves[z].X && all_legal_moves[i].Y == cur_legal_moves[z].Y)
                   {
                      existing_index = i;
                      break;
                   }
                }

                // If this tile has already been added, see if we have
                // a lower cost
                if (existing_index >= 0)
                {
                   if (all_legal_moves[existing_index].Cost > cur_legal_moves[z].Cost)
                      all_legal_moves[existing_index] = cur_legal_moves[z];
                   else
                      cur_legal_moves.RemoveAt(z);
                }
                else
                   all_legal_moves.Add(cur_legal_moves[z]);
             }

             last_legal_moves = cur_legal_moves;
          }

          return all_legal_moves;
       }
    }
    #endregion

    #region Inner Classes
    internal class ComparePFNode : IComparer<PathFinderNode>
    {
        #region IComparer Members
        public int Compare(PathFinderNode x, PathFinderNode y)
        {
            if (x.F > y.F)
                return 1;
            else if (x.F < y.F)
                return -1;
            return 0;
        }
        #endregion
    }
    #endregion
}
