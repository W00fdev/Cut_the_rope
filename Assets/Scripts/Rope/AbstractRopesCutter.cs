using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRopesCutter : MonoBehaviour
{
    public abstract event Action RopeCut;
    public abstract event Action AllRopesCut;
    
    public abstract IReadOnlyList<BakedRope> Ropes { get; }
    public abstract void Init(IInput input, Overlay overlay);
}