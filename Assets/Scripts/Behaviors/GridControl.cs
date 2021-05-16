using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridControl : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab = default;
    [SerializeField] private GameObject connectorPrefab = default;
    [Header("Editor")]
    [SerializeField] private bool isPortrait; // TODO: Temp for editor

    private GridLayoutGroup gridLayout = default;
    private FileManager fileManager;
    private float width, height;
    private int[,] level;
    private Dictionary<int, Color32> colorGroup = new Dictionary<int, Color32>();

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        fileManager = new FileManager();

        width = Screen.width;
        height = Screen.height;

        CreateGrid(4);
    }

    private void CreateGrid(int size)
    {
        level = fileManager.ReadLevel(size);

        // TODO: Refactoring into chunks

        float cellSize;
#if UNITY_EDITOR
        cellSize = ((isPortrait ? width : height) / size) - (size * 10);
#else
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            cellSize = (width / size) - (size * 10);
        }
        else
        {
            cellSize = (height / size) - (size * 10);
        }
#endif

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = size;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        List<Color32> colors = GenerateColors();

        for (int x = 0; x < level.GetLength(0); x++)
        {
            for (int y = 0; y < level.GetLength(1); y++)
            {
                GameObject slotObject = Instantiate(slotPrefab, transform);
                int groupNumber = level[x, y];
                slotObject.name = $"{K.slot}{groupNumber}";
                Slot slot = slotObject.GetComponent<Slot>();
                slot.groupNumber = groupNumber;
                if (groupNumber > 0)
                {
                    slot.isEmpty = false;
                    GameObject connector = Instantiate(connectorPrefab, slotObject.transform);
                    connector.name = $"Connector{groupNumber}";

                    if (!colorGroup.ContainsKey(groupNumber))
                    {
                        int randomIndex = Random.Range(0, colors.Count);
                        colorGroup.Add(groupNumber, colors[randomIndex]);
                        colors.RemoveAt(randomIndex);
                    }

                    connector.GetComponent<Image>().color = colorGroup[groupNumber];
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
