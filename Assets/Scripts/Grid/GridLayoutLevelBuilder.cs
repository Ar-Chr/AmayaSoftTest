using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutLevelBuilder : LevelBuilder
{
    [SerializeField] private GameObject gridCell;
    [SerializeField] private GridLayoutGroup grid;

    private RectTransform gridTransform;
    private List<GridCell> cells;

    private bool isInit;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (isInit)
            return;

        gridTransform = grid.transform as RectTransform;
        cells = GetExistingCells();
        Clear();

        isInit = true;
    }

    private List<GridCell> GetExistingCells()
    {
        var result = new List<GridCell>();
        int childCount = grid.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var cell = grid.transform.GetChild(i).GetComponent<GridCell>();
            if (cell != null)
                result.Add(cell);
        }
        return result;
    }

    public override void Build(List<Item> items, int columns)
    {
        BuildAnimated(items, columns, (cell, index) => { });
    }

    public override void BuildAnimated(List<Item> items, int columns, Action<GridCell, int> animation)
    {
        if (!isInit)
            Init();

        Clear();

        float gridNewWidth =
            (grid.cellSize.x + grid.spacing.x) * columns
            + grid.padding.horizontal;

        float gridNewHeight =
            (grid.cellSize.y + grid.spacing.y) * (items.Count / columns)
            + grid.padding.vertical;

        gridTransform.sizeDelta = new Vector2(gridNewWidth, gridNewHeight);

        AddCells(items.Count - cells.Count);
        for (int i = 0; i < items.Count; i++)
        {
            cells[i].gameObject.SetActive(true);
            cells[i].Initialize(items[i]);
            animation(cells[i], i);
        }
    }

    private void AddCells(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newCell = Instantiate(gridCell, grid.transform);
            var newCellScript = newCell.GetComponent<GridCell>();
            cells.Add(newCellScript);
        }
    }

    private void Clear()
    {
        foreach (var cell in cells)
        {
            cell.gameObject.SetActive(false);
        }
    }
}
