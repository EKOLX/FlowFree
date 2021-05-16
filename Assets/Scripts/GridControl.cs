using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridControl : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab = default;

    private GridLayoutGroup gridLayout = default;
    private float width;

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();

        width = Screen.width;

        PopulateGrid(4);
    }

    private void PopulateGrid(int size)
    {
        int cellCount = (int)Mathf.Pow(size, 2);
        float cellSize = (width / size) - (size * 10);

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = size;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        for (int i = 0; i < cellCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);
            slot.name = $"{K.slot}{i}";
        }
    }
}
