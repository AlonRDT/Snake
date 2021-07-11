using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public List<Vector2> SnakeCoords { get; set; }
    private EDirection m_CurrentDirection;
    public bool IsSnakeVertical { get; set; }
    public EDirection NextDirection { get; private set; }

    private bool m_CanChangeDirection;

    public void Awake()
    {
        m_CurrentDirection = EDirection.Left;
        NextDirection = EDirection.Left;
        m_CanChangeDirection = true;
        IsSnakeVertical = false;

        int startX, stayY, length;
        SnakeCoords = new List<Vector2>();
        //Debug.Log("Snake Awake");
        stayY = Settings.ColumnAmount / 2;
        length = Settings.RowAmount % 2 == 1 ? 3 : 4;
        startX = Settings.RowAmount % 2 == 1 ? Settings.RowAmount / 2 - 1 : Settings.RowAmount / 2 - 2;
        for (int i = 0; i < length; i++)
        {
            SnakeCoords.Add(new Vector2(startX + i, stayY));
        }
        //Debug.Log(SnakeCoords[0]);
    }

    public void ApplyDirection()
    {
        m_CurrentDirection = NextDirection;
    }

    public bool IsTurn()
    {
        bool output = m_CurrentDirection != NextDirection;

        if(output)
        {
            IsSnakeVertical = !IsSnakeVertical;
        }

        return output;
    }

    private void Update()
    {
        if (m_CanChangeDirection)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && m_CurrentDirection != EDirection.Down)
            {
                NextDirection = EDirection.Up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && m_CurrentDirection != EDirection.Up)
            {
                NextDirection = EDirection.Down;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && m_CurrentDirection != EDirection.Right)
            {
                NextDirection = EDirection.Left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && m_CurrentDirection != EDirection.Left)
            {
                NextDirection = EDirection.Right;
            }
        }
    }
}
