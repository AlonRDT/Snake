using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic : MonoBehaviour
{
    private ETileStatus m_Status;
    private int m_CurrentColorIndex;
    private int m_TicksSinceStart;
    private int m_TicksToChangeColor;
    private int m_BackgroundType;
    public bool m_IsDebug;
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }

    public void Initiaize(int rowIndex, int columnIndex)
    {
        ColumnIndex = columnIndex;
        RowIndex = rowIndex;
        m_Status = ETileStatus.Void;
        m_TicksSinceStart = 0;
        m_CurrentColorIndex = 0;
        m_BackgroundType = 0;
        m_TicksToChangeColor = Settings.SameColorTilesInLineAmount;
        m_IsDebug = false;

        float newLocationXScreenFraction = ((float)rowIndex / Settings.RowAmount + 1f / (Settings.RowAmount * 2));
        float newLocationYScreenFraction = ((float)columnIndex / Settings.ColumnAmount + 1f / (Settings.ColumnAmount * 2));
        Vector3 newTilePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * newLocationXScreenFraction, Screen.height * newLocationYScreenFraction, 10));
        transform.position = newTilePosition;
        transform.localScale = new Vector2(Settings.ScreenSize.x * 1.4f / Settings.RowAmount, Settings.ScreenSize.y * 1.4f / Settings.ColumnAmount);
    }

    public void ActivateDebug()
    {
        m_IsDebug = true;
    }

    public void ChangeStatus(ETileStatus newStatus)
    {
        switch (newStatus)
        {
            case ETileStatus.Void:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.BackgroundTile;
                break;
            case ETileStatus.Background:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.BackgroundTile;
                gameObject.GetComponent<SpriteRenderer>().color = Settings.getColorByIndex(m_CurrentColorIndex, m_BackgroundType, m_IsDebug);
                break;
            case ETileStatus.Candy:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.CandyTile;
                break;
            case ETileStatus.EndLevel:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.EndLevelTile;
                break;
            case ETileStatus.SnakeHeadLeft:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.SnakeHeadLeftTile;
                break;
            case ETileStatus.SnakeHeadRight:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.SnakeHeadRightTile;
                break;
            case ETileStatus.SnakeHeadUp:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.SnakeHeadUpTile;
                break;
            case ETileStatus.SnakeHeadDown:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.SnakeHeadDownTile;
                break;
            case ETileStatus.SnakeBody:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.SnakeBodyTile;
                break;
            case ETileStatus.SnakeBodyTurn:
                gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.SnakeBodyTurnTile;
                break;
            default:
                break;
        }
        m_Status = newStatus;
    }

    public bool CanSnakeStepOnTile()
    {
        bool output = true;
        if (m_Status == ETileStatus.Background || m_Status == ETileStatus.SnakeBody || m_Status == ETileStatus.SnakeBodyTurn)
        {
            output = false;
        }

        return output;
    }

    public Vector2 getTileCoordinate()
    {
        return new Vector2(RowIndex, ColumnIndex);
    }

    public bool IsGameOverOnMoveOnto()
    {
        return m_Status == ETileStatus.EndLevel;
    }

    public bool IsCandy()
    {
        return m_Status == ETileStatus.Candy;
    }

    public bool IsCanBeCandy()
    {
        return m_Status == ETileStatus.Void;
    }

    public void BackgroundTick()
    {
        bool changeColor = false;
        m_TicksSinceStart++;
        if (m_TicksSinceStart % m_TicksToChangeColor == 0)
        {
            m_CurrentColorIndex++;
            changeColor = true;
        }
        if (m_Status == ETileStatus.Background && changeColor)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Settings.getColorByIndex(m_CurrentColorIndex, m_BackgroundType, m_IsDebug);
            changeColor = false;
        }

    }

    public void UpdateBackgroundInfo(int startTick, int startColor, int type)
    {
        m_TicksSinceStart = startTick;
        m_CurrentColorIndex = startColor;
        m_BackgroundType = type;
    }

    private void printDebug(string content)
    {
        if (m_IsDebug)
        {
            Debug.Log(content);
        }
    }
}
