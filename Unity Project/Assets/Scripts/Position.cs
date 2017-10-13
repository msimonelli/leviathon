using System;


/// <summary>
/// This represents a position and direction on a map.
///
/// This class is primarily used for positioning the party in the game map,
/// and characters/monsters in a battle.
/// </summary>
[Serializable]
public class Position
{
  /// <summary>
  /// The X axis position
  /// </summary>
  public int X { get; set; }

  /// <summary>
  /// The X axis position
  /// </summary>
  public int Y { get; set; }

  /// <summary>
  /// The facing direction.
  /// </summary>
  public Direction Direction { get; set; }

  /// <summary>
  /// Constructs an empty position object.
  /// </summary>
  public Position()
  {
     X = 0;
     Y = 0;
     Direction = Direction.North;
  }


	/// <summary>
	/// Constructs an new position object.
	/// </summary>
	/// <param name="x">The horizontal position.</param>
	/// <param name="y">The vertical position.</param>
	/// <param name="dir">The direction to face.</param>
	public Position(int x, int y)
	{
		X = x;
		Y = y;
		Direction = Direction.None;
	}

  /// <summary>
  /// Constructs an new position object.
  /// </summary>
  /// <param name="x">The horizontal position.</param>
  /// <param name="y">The vertical position.</param>
  /// <param name="dir">The direction to face.</param>
  public Position(int x, int y, Direction dir)
  {
     X = x;
     Y = y;
     Direction = dir;
  }


  /// <summary>
  /// Constructs a copy of a position object.
  /// </summary>
  /// <param name="other">The position to copy from.</param>
  public Position(Position other)
  {
     X = other.X;
     Y = other.Y;
     Direction = other.Direction;
  }


  /// <summary>
  /// Offsets the position by the specified amounts.
  /// </summary>
  /// <param name="x_delta">The x delta</param>
  /// <param name="y_delta">The y delta</param>
  public void Offset(int x_delta, int y_delta)
  {
     X += x_delta;
     Y += y_delta;
  }


  /// <summary>
  /// Offsets the X position by the specified amounts.
  /// </summary>
  /// <param name="x_delta">The x delta</param>
  public void OffsetX(int x_delta)
  {
     X += x_delta;
  }


  /// <summary>
  /// Offsets the Y position by the specified amounts.
  /// </summary>
  /// <param name="x_delta">The y delta</param>
  public void OffsetY(int y_delta)
  {
     Y += y_delta;
  }


  /// <summary>
  /// Offsets the position in the specified direction(s). You can include multiple
  /// directions to offset more than one way.
  /// </summary>
  /// <param name="offset_dir">The direction to offset.</param>
  /// <returns>The position object</returns>
  public Position OffsetDirection(Move offset_dir)
  {
     switch (Direction)
     {
        case Direction.North:
           if ((offset_dir & Move.Forward) > 0)
              OffsetY(-1);
           if ((offset_dir & Move.Back) > 0)
              OffsetY(1);
           if ((offset_dir & Move.Left) > 0)
              OffsetX(-1);
           if ((offset_dir & Move.Right) > 0)
              OffsetX(1);
           break;

        case Direction.NorthEast:
           if ((offset_dir & Move.Forward) > 0)
              Offset(1, -1);
           if ((offset_dir & Move.Back) > 0)
              Offset(-1, 1);
           if ((offset_dir & Move.Left) > 0)
              Offset(-1, -1);
           if ((offset_dir & Move.Right) > 0)
              Offset(1, 1);
           break;

        case Direction.East:
           if ((offset_dir & Move.Forward) > 0)
              OffsetX(1);
           if ((offset_dir & Move.Back) > 0)
              OffsetX(-1);
           if ((offset_dir & Move.Left) > 0)
              OffsetY(-1);
           if ((offset_dir & Move.Right) > 0)
              OffsetY(1);
           break;

        case Direction.SouthEast:
           if ((offset_dir & Move.Forward) > 0)
              Offset(1, 1);
           if ((offset_dir & Move.Back) > 0)
              Offset(-1, -1);
           if ((offset_dir & Move.Left) > 0)
              Offset(1, -1);
           if ((offset_dir & Move.Right) > 0)
              Offset(1, -1);
           break;
        case Direction.South:
           if ((offset_dir & Move.Forward) > 0)
              OffsetY(1);
           if ((offset_dir & Move.Back) > 0)
              OffsetY(-1);
           if ((offset_dir & Move.Left) > 0)
              OffsetX(1);
           if ((offset_dir & Move.Right) > 0)
              OffsetX(-1);
           break;

        case Direction.SouthWest:
           if ((offset_dir & Move.Forward) > 0)
              Offset(-1, 1);
           if ((offset_dir & Move.Back) > 0)
              Offset(1, -1);
           if ((offset_dir & Move.Left) > 0)
              Offset(1, 1);
           if ((offset_dir & Move.Right) > 0)
              Offset(-1, -1);
           break;

        case Direction.West:
           if ((offset_dir & Move.Forward) > 0)
              OffsetX(-1);
           if ((offset_dir & Move.Back) > 0)
              OffsetX(1);
           if ((offset_dir & Move.Left) > 0)
              OffsetY(1);
           if ((offset_dir & Move.Right) > 0)
              OffsetY(-1);
           break;

        case Direction.NorthWest:
           if ((offset_dir & Move.Forward) > 0)
              Offset(-1, -1);
           if ((offset_dir & Move.Back) > 0)
              Offset(1, 1);
           if ((offset_dir & Move.Left) > 0)
              Offset(-1, 1);
           if ((offset_dir & Move.Right) > 0)
              Offset(1, -1);
           break;
     };

     return this;
  }


  /// <summary>
  /// Offsets the position forward the specified number of times.
  /// </summary>
  /// <param name="num">The number of times to offset.</param>
  /// <returns>The position object.</returns>
  public Position OffsetForward(int num = 1)
  {
     if (num >= 0)
        for (int x = 0; x < num; x++)
           OffsetDirection(Move.Forward);
     else
        for (int x = num; x < 0; x++)
           OffsetDirection(Move.Back);

     return this;
  }


  /// <summary>
  /// Offsets the position back the specified number of times.
  /// </summary>
  /// <param name="num">The number of times to offset.</param>
  /// <returns>The position object.</returns>
  public Position OffsetBack(int num = 1)
  {
     if (num >= 0)
        for (int x = 0; x < num; x++)
           OffsetDirection(Move.Back);
     else
        for (int x = num; x < 0; x++)
           OffsetDirection(Move.Forward);

     return this;
  }


  /// <summary>
  /// Offsets the position left the specified number of times.
  /// </summary>
  /// <param name="num">The number of times to offset.</param>
  /// <returns>The position object.</returns>
  public Position OffsetLeft(int num = 1)
  {
     if (num >= 0)
        for (int x = 0; x < num; x++)
           OffsetDirection(Move.Left);
     else
        for (int x = num; x < 0; x++)
           OffsetDirection(Move.Right);

     return this;
  }


  /// <summary>
  /// Offsets the position right the specified number of times.
  /// </summary>
  /// <param name="num">The number of times to offset.</param>
  /// <returns>The position object.</returns>
  public Position OffsetRight(int num = 1)
  {
     if (num >= 0)
        for (int x = 0; x < num; x++)
           OffsetDirection(Move.Right);
     else
        for (int x = num; x < 0; x++)
           OffsetDirection(Move.Left);

     return this;
  }


  /// <summary>
  /// Turns the facing direction 180 degrees.
  /// </summary>
  public void FlipDirection()
  {
     if (Direction == Direction.North)
        Direction = Direction.South;
     else if (Direction == Direction.NorthEast)
        Direction = Direction.SouthWest;
     else if (Direction == Direction.East)
        Direction = Direction.West;
     else if (Direction == Direction.SouthEast)
        Direction = Direction.NorthWest;
     else if (Direction == Direction.South)
        Direction = Direction.North;
     else if (Direction == Direction.SouthWest)
        Direction = Direction.NorthEast;
     else if (Direction == Direction.West)
        Direction = Direction.East;
     else if (Direction == Direction.NorthWest)
        Direction = Direction.SouthEast;
  }


  /// <summary>
  /// Changes the direction of this position to face the specified
  /// direction.
  /// </summary>
  /// <param name="look_at_pos">The position to face</param>
  public void LookAtPosition(Position look_at_pos)
  {
     int x_diff = X - look_at_pos.X;
     int y_diff = Y - look_at_pos.Y;

     if (x_diff == 0 && y_diff == 0)
        return;

     if (Math.Abs(x_diff) == Math.Abs(y_diff))
     {
        if (x_diff < 0 && y_diff < 0)
           Direction = Direction.SouthEast;
        else if (x_diff > 0 && y_diff < 0)
           Direction = Direction.SouthWest;
        else if (x_diff < 0 && y_diff > 0)
           Direction = Direction.NorthEast;
        else if (x_diff > 0 && y_diff > 0)
           Direction = Direction.NorthWest;
     }
     else if (Math.Abs(x_diff) > Math.Abs(y_diff))
     {
        if (x_diff < 0)
           Direction = Direction.East;
        else
           Direction = Direction.West;
     }
     else if (Math.Abs(x_diff) < Math.Abs(y_diff))
     {
        if (y_diff < 0)
           Direction = Direction.South;
        else
           Direction = Direction.North;
     }
  }


  /// <summary>
  /// Adds the two positions.
  /// </summary>
  /// <param name="pos1">The first position</param>
  /// <param name="pos2">The second position</param>
  /// <returns>The new position</returns>
  public static Position operator+ (Position pos1, Position pos2)
  {
     Position pos = new Position(pos1.X + pos2.X, pos1.Y + pos2.Y, pos1.Direction);
     return pos;
  }


  /// <summary>
  /// Subtracts the two positions.
  /// </summary>
  /// <param name="pos1">The first position</param>
  /// <param name="pos2">The second position</param>
  /// <returns>The new position</returns>
  public static Position operator- (Position pos1, Position pos2)
  {
     Position pos = new Position(pos1.X - pos2.X, pos1.Y - pos2.Y, pos1.Direction);
     return pos;
  }


  /// <summary>
  /// Returns true if the two positions are equal.
  /// </summary>
  /// <param name="pos1">The first position</param>
  /// <param name="pos2">The second position</param>
  /// <returns>True if the positions are equal.</returns>
  public static bool operator==(Position pos1, Position pos2) 
  {
     return (pos1.X == pos2.X && pos1.Y == pos2.Y && pos1.Direction == pos2.Direction);
  }


  /// <summary>
  /// Returns true if the two positions are not equal.
  /// </summary>
  /// <param name="pos1">The first position</param>
  /// <param name="pos2">The second position</param>
  /// <returns>True if the positions are not equal.</returns>
  public static bool operator!=(Position pos1, Position pos2) 
  {
     return (pos1.X != pos2.X || pos1.Y != pos2.Y || pos1.Direction != pos2.Direction);
  }


  /// <summary>
  /// Returns true if the two positions are equal.
  /// </summary>
  /// <param name="obj">The position to compare to.</param>
  public override bool Equals(object obj)
  {
     if (!(obj is Position))
        return false;

     return (this == (Position)obj);
  }


  public override int GetHashCode()
  {
     return X ^ Y * (int)Direction;
  }
}
