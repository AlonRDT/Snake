using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteLibrary
{
    public static Sprite BackgroundTile { get => Resources.Load<Sprite>("Images/im_Background"); }
    public static Sprite CandyTile { get => Resources.Load<Sprite>("Images/im_Candy"); }
    public static Sprite EndLevelTile { get => Resources.Load<Sprite>("Images/im_EndLevel"); }
    public static Sprite SnakeBodyTile { get => Resources.Load<Sprite>("Images/im_SnakeBody"); }
    public static Sprite SnakeBodyTurnTile { get => Resources.Load<Sprite>("Images/im_SnakeBodyTurn"); }
    public static Sprite SnakeHeadLeftTile { get => Resources.Load<Sprite>("Images/im_SnakeFaceLeft"); }
    public static Sprite SnakeHeadRightTile { get => Resources.Load<Sprite>("Images/im_SnakeFaceRight"); }
    public static Sprite SnakeHeadUpTile { get => Resources.Load<Sprite>("Images/im_SnakeFaceUp"); }
    public static Sprite SnakeHeadDownTile { get => Resources.Load<Sprite>("Images/im_SnakeFaceDown"); }
}
