using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }
    public int Uses { get; set; }

    public Move(MoveBase cbase)
    {
        Base = cbase;
        Uses = cbase.Uses;
    }
}
