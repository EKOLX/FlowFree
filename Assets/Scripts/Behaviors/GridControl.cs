using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridControl : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab = default;
    [SerializeField] private GameObject connectorPrefab = default;
    [SerializeField] private GameObject[] slotObjects;
    [Header("Editor")]
    [Tooltip("Used within Editor only")]
    [SerializeField] private bool isPortrait;

    private GridLayoutGroup gridLayout = default;
    private FileManager fileManager;
    private int[,] level;
    private Dictionary<int, Color32> colorGroup = new Dictionary<int, Color32>();

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        fileManager = new FileManager();

        CreateGrid();
    }

    private void CreateGrid()
    {
        int size = 4; // TODO: Determine randomly or get from a user
        level = fileManager.ReadLevel(size);

        int cellsCount = (int)Mathf.Pow(size, 2);
        slotObjects = new GameObject[cellsCount];

        // TODO: Refactoring into chunks

        float cellSize;
#if UNITY_EDITOR
        cellSize = ((isPortrait ? Screen.width : Screen.height) / size) - (size * 10);
#else
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            cellSize = (Screen.width / size) - (size * 10);
        }
        else
        {
            cellSize = (Screen.height / size) - (size * 10);
        }
#endif

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = size;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        List<Color32> colors = GenerateColors();

        int counter = 0;
        for (int x = 0; x < level.GetLength(0); x++)
        {
            for (int y = 0; y < level.GetLength(1); y++)
            {
                GameObject slotObject = Instantiate(slotPrefab, transform);
                slotObjects[counter++] = slotObject;
                slotObject.name = $"{K.slot}{counter}";

                int groupNumber = level[x, y];

                Slot slot = slotObject.GetComponent<Slot>();
                slot.index = counter;
                slot.groupNumber = groupNumber;

                if (groupNumber > 0)
                {
                    slot.isDrawn = true;
                    GameObject connectorObject = Instantiate(connectorPrefab, slotObject.transform);
                    connectorObject.name = $"Connector{groupNumber}";

                    if (!colorGroup.ContainsKey(groupNumber))
                    {
                        int randomIndex = Random.Range(0, colors.Count);
                        Color32 color = colors[randomIndex];
                        colorGroup.Add(groupNumber, color);
                        slot.color = color;
                        colors.RemoveAt(randomIndex);
                    }

                    connectorObject.GetComponent<Image>().color = colorGroup[groupNumber];
                }
            }
        }
    }

    private List<Color32> GenerateColors()
    {
        List<Color32> colors = new List<Color32>();
        colors.Add(K.ColorKey.blue);
        colors.Add(K.ColorKey.red);
        colors.Add(K.ColorKey.green);
        colors.Add(K.ColorKey.orange);

        return colors;
    }

}
