using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

[Serializable]
public class ConfrontData
{
    public bool win = false;

    public bool wasMiss = false;

    public bool wasCritic = false;

    public int damage = 0;
}
