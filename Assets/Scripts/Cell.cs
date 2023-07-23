using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public enum Type
    {
        Invalid,
        Empty,
        Mine,
        Number,
    }

    public Type typ;
    public Vector3Int position;
    public bool revealed;
    public bool exploded;
    public int number;

    public bool flagged;




}
