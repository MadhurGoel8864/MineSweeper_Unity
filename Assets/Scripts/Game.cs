using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height= 16;
    private bool gameover;
    public Camera cam;
    private Board board;
    public Cell[,] state;
    public int mine_count = 16;


    private void OnValidate()
    {
        mine_count = Mathf.Clamp(mine_count, 0, width*height);
    }

    private void Awake()
    {
        board = GetComponentInChildren<Board>();  
        gameover = false;
    }


    private void Start()
    {
        cam.transform.position  = new Vector3 (width/2, height/2, -10);

        New_Game();    
    }

    private void New_Game()
    {
        state = new Cell[width, height];
        GenerateCells();
        Generate_Mines();
        Generate_Numbers();
        board.Draw(state);
    }
    private void GenerateCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.typ= Cell.Type.Empty;
                state[x, y] = cell;

            }
        }
    }

    private void Generate_Mines()
    { 
        for (int i = 0; i < mine_count; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while (state[x, y].typ == Cell.Type.Mine)
            {
                x++;
                x = x%width;
            }
        state[x, y].typ = Cell.Type.Mine;
        //state[x, y].revealed = true;
        }
    }

    private void Generate_Numbers()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = state[i, j];
                if(cell.typ == Cell.Type.Mine)
                {
                    continue;
                }
                cell.number = CountMinesAround(i, j);
                if(cell.number>0)
                {
                    cell.typ = Cell.Type.Number;
                }
                //cell.revealed = true;
                state[i, j] = cell;
            }
        }
    }

    private int CountMinesAround(int x, int y)
    {
        int ans = 0;
        for(int i = x-1; i <= x+1; i++)
        {
            for(int j = y-1; j <= y+1; j++)
            {
                if(i<0 || j<0 || i>=width || j >= height)
                {
                    continue;
                }
                else if(i==x && y == j)
                { 
                    continue;
                }
                else if (state[i,j].typ== Cell.Type.Mine)
                {
                    ans++;
                }
            }
        }
        return ans;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            New_Game();
        }
        if (!gameover)
        {
            if (Input.GetMouseButtonDown(1))
            {
                FlagIt();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                revealit();
            }
        }
    }

    private void revealit() {


        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int CellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(CellPosition.x, CellPosition.y);
        int a = CellPosition.x;
        int b = CellPosition.y;
        if (cell.typ == Cell.Type.Invalid || cell.revealed || cell.flagged)
        {
            return;
        }
        else if (cell.typ == Cell.Type.Mine)
        {
            Explode(cell);
        }
        else if (cell.typ == Cell.Type.Empty)
        {
            
            Flood(cell);
            CheckWin();
        }
        else
        {
            CheckWin();
        }
        cell.revealed = true;
        state[a, b] = cell;
        board.Draw(state);
    }

    private void Explode(Cell cell)
    {
        gameover = true;

        cell.revealed = true;
        cell.exploded = true;
        state[cell.position.x, cell.position.y] = cell;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell= state[x, y];
                if(cell.typ == Cell.Type.Mine)
                {
                    cell. revealed = true;
                    state[x, y] = cell;
                }
            }
        }

    }


    private void Flood(Cell cell)
    {
        if (cell.revealed)
            return;
        if(cell.typ== Cell.Type.Mine || cell.typ == Cell.Type.Invalid)
        {
            return;
        }
        cell.revealed = true;
        state[cell.position.x,cell.position.y]  = cell;
        int a = cell.position.x;
        int b = cell.position.y;
        if (cell.typ == Cell.Type.Empty)
        {
            Flood(GetCell(a-1,b));
            Flood(GetCell(a+1,b));
            Flood(GetCell(a,b-1));
            Flood(GetCell(a,b+1));
        }

    }

    private void FlagIt()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int CellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(CellPosition.x, CellPosition.y);
        int a = CellPosition.x;
        int b = CellPosition.y;
        if(cell.typ== Cell.Type.Invalid || cell.revealed)
        {
            return;
        }
        
        cell.flagged = !cell.flagged;
        state[a, b] = cell;
        board.Draw(state);


    }

    private Cell GetCell(int i, int j)
    {
        if (i < 0 || j < 0 || i >= width || j >= height)
        {
            return new Cell();
        }
        else
        {
            return state[i,j];

        }


    }


    private void CheckWin()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.typ!= Cell.Type.Mine && !cell.revealed)
                {
                    return;
                }
            }
        }
        print("Won!!");
        gameover = true;


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                if (cell.typ == Cell.Type.Mine)
                {
                    cell.flagged = true;
                    state[x, y] = cell;
                }
            }
        }



    }

}
