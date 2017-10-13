using System.Collections.Generic;


interface IPathFinder
{
    #region Events
    event PathFinderDebugHandler PathFinderDebug;
    #endregion

    #region Properties
    bool Stopped
    {
        get;
    }

    HeuristicFormula Formula
    {
        get;
        set;
    }

    bool Diagonals
    {
        get;
        set;
    }

    bool HeavyDiagonals
    {
        get;
        set;
    }

    int HeuristicEstimate
    {
        get;
        set;
    }

    bool PunishChangeDirection
    {
        get;
        set;
    }

    bool TieBreaker
    {
        get;
        set;
    }

    int SearchLimit
    {
        get;
        set;
    }

    bool DebugProgress
    {
        get;
        set;
    }

    bool DebugFoundPath
    {
        get;
        set;
    }
    #endregion

    #region Methods
    void FindPathStop();
    List<PathFinderNode> FindPath(Position start, Position end);
    List<PathFinderNode> FindLegalMoves(Position start, int max_moves);
    #endregion

}
