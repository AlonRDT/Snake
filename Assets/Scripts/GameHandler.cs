using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_TilePrefab;
    [SerializeField] private GameObject m_SnakePrefab;
    [SerializeField] private GameObject m_MenuPrefab;
    [SerializeField] private GameObject m_Background;
    private int m_RemainingCandy;
    private float m_AcummulatedMoveTime;
    private float m_AcummulatedBackgroundTickTime;
    private Snake m_Snake;
    private TileLogic[,] m_Tiles;
    private List<Vector2> m_Candy;

    // The script is set to load first in project settings since it changes the settings file in a signifcant way
    void Start()
    {
        Settings.InitializeDelay();
        m_Tiles = new TileLogic[Settings.RowAmount, Settings.ColumnAmount];
        m_Candy = new List<Vector2>();
        m_AcummulatedMoveTime = 0;
        m_AcummulatedBackgroundTickTime = 0;

        spawnTiles();
        if (Settings.IsSpawnSnake)
        {
            m_Snake = spawnSnake();
        }
        else
        {
            spawnMenu();
        }
        spawnCandy(Settings.CandyCoordinates);
        initializeBackgroundColor();
        m_Background.gameObject.transform.transform.localScale = new Vector2(Settings.ScreenSize.x * 2, Settings.ScreenSize.y * 2);
    }

    private void spawnMenu()
    {
        Instantiate(m_MenuPrefab, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        m_AcummulatedMoveTime += Time.deltaTime;
        m_AcummulatedBackgroundTickTime += Time.deltaTime;

        float tickDelay = (float)Settings.MoveDelay / Settings.SameColorTilesInLineAmount;
        if (m_AcummulatedBackgroundTickTime > tickDelay)
        {
            m_AcummulatedBackgroundTickTime -= tickDelay;
            BackgroundTick();
        }

        if (m_AcummulatedMoveTime >= Settings.MoveDelay && Settings.IsSpawnSnake)
        {
            m_AcummulatedMoveTime -= Settings.MoveDelay;
            TileLogic headTile = getTileByCoordinate(m_Snake.SnakeCoords[0]), tailTile = getTileByCoordinate(m_Snake.SnakeCoords[m_Snake.SnakeCoords.Count - 1]), nextTile = getNextTile();
            if (nextTile.CanSnakeStepOnTile())
            {
                moveSnake(headTile, tailTile, nextTile);
                m_Snake.ApplyDirection();
            }
            else
            {
                gameOver(false);
            }
        }
    }

    private void BackgroundTick()
    {
        foreach (TileLogic tile in m_Tiles)
        {
            tile.BackgroundTick();
        }
    }

    private TileLogic getNextTile()
    {
        int headRow = (int)m_Snake.SnakeCoords[0].x, columnRow = (int)m_Snake.SnakeCoords[0].y;
        TileLogic output;
        switch (m_Snake.NextDirection)
        {
            case EDirection.Up:
                output = getTileByCoordinate(new Vector2(headRow, columnRow + 1));
                break;
            case EDirection.Down:
                output = getTileByCoordinate(new Vector2(headRow, columnRow - 1));
                break;
            case EDirection.Left:
                output = getTileByCoordinate(new Vector2(headRow - 1, columnRow));
                break;
            case EDirection.Right:
                output = getTileByCoordinate(new Vector2(headRow + 1, columnRow));
                break;
            default:
                output = null;
                break;
        }

        return output;
    }

    private void gameOver(bool isVictory)
    {
        if (isVictory)
        {
            Settings.Victory();
        }
        else
        {
            Settings.ChangeLevel(EMap.Entry);
        }
    }

    private void moveSnake(TileLogic headTile, TileLogic tailTile, TileLogic nextTile)
    {
        if (nextTile.IsGameOverOnMoveOnto())
        {
            gameOver(true);
        }
        else if (nextTile.IsCandy())
        {
            handleHeadTileChange(headTile);
            handleNextTileChange(nextTile);
            if (Settings.IsSpawnCandyOnPickup)
            {
                List<Vector2> newCandy = new List<Vector2>();
                newCandy.Add(Settings.GetRandomCoordinateInGameField());
                spawnCandy(newCandy);
            }
            else
            {
                m_RemainingCandy--;
                if (m_RemainingCandy == 1)
                {
                    updateEndLevelTile();
                }
            }

            Settings.ChangeSnakeSpeedOnPickup();
        }
        else
        {
            handleHeadTileChange(headTile);
            handleNextTileChange(nextTile);
            handleTailTileChange(tailTile);
        }
    }

    private void updateEndLevelTile()
    {
        foreach (Vector2 candyCoord in Settings.CandyCoordinates)
        {
            TileLogic tileInQuestion = getTileByCoordinate(candyCoord);
            if (tileInQuestion.IsCandy())
            {
                tileInQuestion.ChangeStatus(ETileStatus.EndLevel);
            }
        }
    }

    private void handleHeadTileChange(TileLogic headTile)
    {
        if (m_Snake.IsTurn())
        {
            headTile.ChangeStatus(ETileStatus.SnakeBodyTurn);
        }
        else
        {
            headTile.ChangeStatus(ETileStatus.SnakeBody);
        }
    }

    private void handleTailTileChange(TileLogic tailTile)
    {
        m_Snake.SnakeCoords.Remove(tailTile.getTileCoordinate());
        tailTile.ChangeStatus(ETileStatus.Void);
    }

    private void handleNextTileChange(TileLogic nextTile)
    {
        m_Snake.SnakeCoords.Insert(0, nextTile.getTileCoordinate());
        switch (m_Snake.NextDirection)
        {
            case EDirection.Up:
                nextTile.ChangeStatus(ETileStatus.SnakeHeadUp);
                break;
            case EDirection.Down:
                nextTile.ChangeStatus(ETileStatus.SnakeHeadDown);
                break;
            case EDirection.Left:
                nextTile.ChangeStatus(ETileStatus.SnakeHeadLeft);
                break;
            case EDirection.Right:
                nextTile.ChangeStatus(ETileStatus.SnakeHeadRight);
                break;
            default:
                break;
        }
    }

    private void initializeBackgroundColor()
    {
        TileLogic tile;
        int startTick, startColor, type;
        for (int col = 0; col < Settings.ColumnAmount; col++)
        {
            for (int row = 0; row < Settings.RowAmount; row++)
            {
                bool isInRowRange = row < Settings.RowAmountToTrimAsBackground || row > Settings.RowAmount - Settings.RowAmountToTrimAsBackground - 1;
                bool isInColumnRange = col < Settings.ColumnAmountToTrimAsBackground || col > Settings.ColumnAmount - Settings.ColumnAmountToTrimAsBackground - 1;
                if (isInRowRange || isInColumnRange)
                {
                    tile = getTileByCoordinate(new Vector2(row, col));
                    Settings.GetBackgroundInfoByGridLocation(out startTick, out startColor, out type, col, row);
                    tile.UpdateBackgroundInfo(startTick, startColor, type);
                    tile.ChangeStatus(ETileStatus.Background);
                }
            }
        }
    }

    private void spawnCandy(List<Vector2> candyCoords)
    {
        m_RemainingCandy = candyCoords.Count;
        TileLogic tileInQuestion;
        foreach (Vector2 candyCoord in candyCoords)
        {
            tileInQuestion = getTileByCoordinate(candyCoord);
            while (!tileInQuestion.IsCanBeCandy())
            {
                tileInQuestion = getTileByCoordinate(Settings.GetRandomCoordinateInGameField());
            }
            tileInQuestion.ChangeStatus(ETileStatus.Candy);
        }
    }

    private Snake spawnSnake()
    {
        bool isFirst = true;
        Snake player = Instantiate(m_SnakePrefab, Vector3.zero, Quaternion.identity).GetComponent<Snake>();
        foreach (Vector2 coord in player.SnakeCoords)
        {
            if (isFirst)
            {
                getTileByCoordinate(coord).ChangeStatus(ETileStatus.SnakeHeadLeft);
                isFirst = !isFirst;
            }
            else
            {
                getTileByCoordinate(coord).ChangeStatus(ETileStatus.SnakeBody);
            }
        }

        return player;
    }

    private TileLogic getTileByCoordinate(Vector2 coord)
    {
        return m_Tiles[(int)coord.x, (int)coord.y];
    }

    private void spawnTiles()
    {
        TileLogic newTile;

        for (int columnIndex = 0; columnIndex < Settings.ColumnAmount; columnIndex++)
        {
            for (int rowIndex = 0; rowIndex < Settings.RowAmount; rowIndex++)
            {
                newTile = Instantiate(m_TilePrefab, Vector3.zero, Quaternion.identity).GetComponent<TileLogic>();
                newTile.Initiaize(rowIndex, columnIndex);
                newTile.transform.parent = transform;
                addTileToArray(newTile);
            }
        }

        //m_Tiles[0, 0].ActivateDebug();
    }

    private void addTileToArray(TileLogic newTile)
    {
        m_Tiles[newTile.RowIndex, newTile.ColumnIndex] = newTile;
    }
}
