using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tileUnknown;
    public Tile tileEmpty;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile tileFlag;
    public Tile tileNumber1;
    public Tile tileNumber2;
    public Tile tileNumber3;
    public Tile tileNumber4;
    public Tile tileNumber5;
    public Tile tileNumber6;
    public Tile tileNumber7;
    public Tile tileNumber8;





    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int x = 0; x< width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell_data = state[x, y];
               tilemap.SetTile(cell_data.position, GetTile(cell_data));


            }
        }

    }
    private Tile GetTile(Cell cell)
    {
        if (cell.revealed)
        {
            return reveal_tile_data(cell);
        }
        else if (cell.flagged)
        {
            return tileFlag;    
        }
        else
        {
            return tileUnknown;
        }
    }
    private Tile reveal_tile_data(Cell cell)
    {
        switch (cell.typ)
        {
            case Cell.Type.Empty:
                    return tileEmpty;
            case Cell.Type.Mine:
                return   tileExploded;
            case Cell.Type.Number:
                return Get_tile_num(cell);
            default: return null;

        }
    }
    private Tile Get_tile_num(Cell cell)
    {
        switch (cell.number)
        {
            case 1:
                return tileNumber1;
            case 2:
                return tileNumber2;
            case 3:
                return tileNumber3;
            case 4:
                return tileNumber4;
            case 5:
                return tileNumber5;
            case 6:
                return tileNumber6;
            case 7:
                return tileNumber7;
            case 8:
                return tileNumber8;
            default: return null;
        }

    }



}
