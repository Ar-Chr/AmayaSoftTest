using UnityEngine;
using System.Collections.Generic;

public abstract class LevelBuilder : MonoBehaviour
{
    public abstract void Build(List<Item> items, int columns);

    public abstract void BuildAnimated(List<Item> items, int columns, System.Action<GridCell, int> animation);
}
