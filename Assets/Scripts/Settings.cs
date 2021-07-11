using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Settings
{
    public static Vector2 ScreenSize { get; set; }
    public static int ColumnAmount { get; set; }
    public static int RowAmount { get; set; }
    public static EMap m_CurrentMap { private get; set; }
    public static bool IsSpawnSnake
    {
        get
        {
            bool output = true;

            if (m_CurrentMap == EMap.Entry)
            {
                output = false;
            }

            return output;
        }

    }

    public static bool IsSpawnCandyOnPickup
    {
        get
        {
            bool output = false;

            if (m_CurrentMap == EMap.GameInfinite)
            {
                output = true;
            }

            return output;
        }

    }

    public static int RowAmountToTrimAsBackground
    {
        get
        {
            int output = 0;

            switch (m_CurrentMap)
            {
                case EMap.Entry:
                    output = RowAmount;
                    break;
                case EMap.GameLevel1:
                    output = 6;
                    break;
                case EMap.GameLevel2:
                    output = 3;
                    break;
                case EMap.GameInfinite:
                    output = 3;
                    break;
                default:
                    break;
            }

            return output;
        }

    }

    public static int ColumnAmountToTrimAsBackground
    {
        get
        {
            int output = 0;

            switch (m_CurrentMap)
            {
                case EMap.Entry:
                    output = ColumnAmount;
                    break;
                case EMap.GameLevel1:
                    output = 6;
                    break;
                case EMap.GameLevel2:
                    output = 3;
                    break;
                case EMap.GameInfinite:
                    output = 3;
                    break;
                default:
                    break;
            }

            return output;
        }

    }

    public static int SameColorTilesInLineAmount
    {
        get
        {
            int output = 1;

            if (m_CurrentMap == EMap.Entry)
            {
                output = 3;
            }

            return output;
        }

    }

    public static float MoveDelayInitial
    {
        get
        {
            float output = 0;

            switch (m_CurrentMap)
            {
                case EMap.Entry:
                    output = 0.3f;
                    break;
                case EMap.GameLevel1:
                    output = 0.7f;
                    break;
                case EMap.GameLevel2:
                    output = 1;
                    break;
                case EMap.GameInfinite:
                    output = 0.8f;
                    break;
                default:
                    break;
            }

            return output;
        }

    }

    public static float MoveDelay { get; set; }

    public static List<Vector2> CandyCoordinates
    {
        get
        {
            List<Vector2> output = new List<Vector2>();

            switch (m_CurrentMap)
            {
                case EMap.GameLevel1:
                    addCoordinateOnFieldToList(output, 0, 2);
                    addCoordinateOnFieldToList(output, 4, 3);
                    addCoordinateOnFieldToList(output, 2, 5);
                    addCoordinateOnFieldToList(output, 5, 7);
                    addCoordinateOnFieldToList(output, 1, 6);
                    break;
                case EMap.GameLevel2:
                    addCoordinateOnFieldToList(output, 0, 0);
                    addCoordinateOnFieldToList(output, 13, 13);
                    addCoordinateOnFieldToList(output, 4, 10);
                    addCoordinateOnFieldToList(output, 2, 9);
                    addCoordinateOnFieldToList(output, 11, 6);
                    addCoordinateOnFieldToList(output, 9, 1);
                    addCoordinateOnFieldToList(output, 8, 13);
                    addCoordinateOnFieldToList(output, 12, 5);
                    addCoordinateOnFieldToList(output, 5, 5);
                    addCoordinateOnFieldToList(output, 1, 6);
                    break;
                case EMap.GameInfinite:
                    output.Add(GetRandomCoordinateInGameField());
                    break;
                default:
                    break;
            }

            return output;
        }
    }

    private static void addCoordinateOnFieldToList(List<Vector2> targetList, int row, int column)
    {
        targetList.Add(new Vector2(row + RowAmountToTrimAsBackground, column + ColumnAmountToTrimAsBackground));
    }

    public static Vector2 GetRandomCoordinateInGameField()
    {
        return new Vector2(UnityEngine.Random.Range(RowAmountToTrimAsBackground, RowAmount - RowAmountToTrimAsBackground), UnityEngine.Random.Range(ColumnAmountToTrimAsBackground, ColumnAmount - ColumnAmountToTrimAsBackground));
    }

    static Settings()
    {
        float ScreenSizeX, ScreenSizeY;
        ScreenSizeX = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        ScreenSizeY = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
        ScreenSize = new Vector2(ScreenSizeX, ScreenSizeY);

        ColumnAmount = 20;
        RowAmount = 20;
    }

    public static void ChangeLevel(EMap mapToLoad)
    {
        m_CurrentMap = mapToLoad;
        SceneManager.LoadScene("Game");
    }

    public static void Victory()
    {
        if (m_CurrentMap == EMap.GameLevel1)
        {
            PlayerPrefs.SetInt("CanPlayLevel2", 1);
            ChangeLevel(EMap.GameLevel2);
        }
        else
        {
            ChangeLevel(EMap.Entry);
        }
    }

    public static void ChangeSnakeSpeedOnPickup()
    {
        switch (m_CurrentMap)
        {
            case EMap.GameLevel1:
                MoveDelay -= 0.1f;
                break;
            case EMap.GameLevel2:
                MoveDelay *= 0.7f;
                break;
            case EMap.GameInfinite:
                MoveDelay *= 0.8f;
                break;
            default:
                break;
        }
    }

    public static void InitializeDelay()
    {
        MoveDelay = MoveDelayInitial;
    }

    public static Color getColorByIndex(int accumulativeIndex, int type, bool debug)
    {
        Color output = Color.black;
        switch (m_CurrentMap)
        {
            case EMap.Entry:
                output = getColorEntryScheme(accumulativeIndex);
                break;
            default:
                output = getColorDefaultScheme(type, accumulativeIndex, debug);
                break;
        }

        return output;
    }

    private static Color getColorEntryScheme(int accumulativeIndex)
    {
        Color output = Color.black;
        switch (accumulativeIndex % 8)
        {
            case 0:
                output = Color.green;
                break;
            case 1:
                output = Color.yellow;
                break;
            case 2:
                output = Color.cyan;
                break;
            case 3:
                output = Color.red;
                break;
            case 4:
                output = Color.blue;
                break;
            case 5:
                output = Color.magenta;
                break;
            default:
                break;
        }

        return output;
    }

    private static Color getColorDefaultScheme(int type, int accumulativeIndex, bool debug)
    {
        //if(type)
        Color output = Color.black, color1 = Color.blue, color2 = Color.green, color3 = Color.red, color4 = Color.cyan;
        if (debug)
        {
            Debug.Log(accumulativeIndex % 4);
        }
        switch (type % 4)
        {
            case 0:
                if (accumulativeIndex % 4 == 1)
                {
                    output = color2;
                    //Debug.Log("1");
                }
                else if (accumulativeIndex % 4 == 3)
                {
                    output = color3;
                    //Debug.Log("3");
                }
                else
                {
                    //Debug.Log("0 2");
                    output = color1;
                }
                break;
            case 1:
                if (accumulativeIndex % 4 == 1)
                {
                    output = color1;
                }
                else if (accumulativeIndex % 4 == 3)
                {
                    output = color4;
                }
                else
                {
                    output = color2;
                }
                break;
            case 2:
                if (accumulativeIndex % 4 == 1)
                {
                    output = color4;
                }
                else if (accumulativeIndex % 4 == 3)
                {
                    output = color1;
                }
                else
                {
                    output = color3;
                }
                break;
            case 3:
                if (accumulativeIndex % 4 == 1)
                {
                    output = color3;
                }
                else if (accumulativeIndex % 4 == 3)
                {
                    output = color2;
                }
                else
                {
                    output = color4;
                }
                break;
            default:
                break;
        }

        return output;
    }

    public static void GetBackgroundInfoByGridLocation(out int startTick, out int startColor, out int type, int col, int row)
    {
        if (m_CurrentMap == EMap.Entry)
        {
            startTick = (col + row) % SameColorTilesInLineAmount;
            startColor = (col + row) / SameColorTilesInLineAmount;
            type = 0;
        }
        else
        {
            startTick = 0;
            startColor = 0;
            type = (col + row * 2) / SameColorTilesInLineAmount;

        }
    }
}

