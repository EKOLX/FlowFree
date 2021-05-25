using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab = default;
    [SerializeField] private GameObject toucherObject = default;
    [SerializeField] private GameObject uiCanvas = default;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private List<GameObject> lines = new List<GameObject>();
    private RectTransform toucherRectTransform = default;
    private CanvasGroup toucherCanvasGroup = default;
    private Color32 currentColor = default;
    private Vector2 mousePosition = default;
    private Vector2 mousePositionStart = default;
    private bool drawingHorizontally;

    private void Awake()
    {
        toucherRectTransform = toucherObject.GetComponent<RectTransform>();
        toucherCanvasGroup = toucherObject.GetComponent<CanvasGroup>();
        raycaster = uiCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = uiCanvas.GetComponent<EventSystem>();
    }

    private void Start()
    {
        pointerEventData = new PointerEventData(eventSystem);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            toucherRectTransform.position = mousePositionStart;
            toucherCanvasGroup.alpha = 0.6f;

            List<RaycastResult> raycastResult = CastRays();

            foreach (RaycastResult result in raycastResult)
            {
                if (result.gameObject.name.Contains(K.slot))
                {
                    Slot slot = result.gameObject.GetComponent<Slot>();
                    if (slot.isDrawn)
                    {
                        GameObject lineObject = Instantiate(linePrefab, transform);
                        lines.Add(lineObject);
                        currentColor = slot.color;

                        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
                        lineRenderer.positionCount = 0;
                        lineRenderer.startColor = slot.color;
                        lineRenderer.endColor = slot.color;

                        RectTransform slotTransform = result.gameObject.GetComponent<RectTransform>();
                        lineObject.GetComponent<Line>().rectTransform = slotTransform;

                        lineRenderer.SetPosition(lineRenderer.positionCount++, new Vector3(slotTransform.position.x, slotTransform.position.y, 0));
                    }
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            toucherRectTransform.position = mousePosition;

            List<RaycastResult> raycastResult = CastRays();

            foreach (RaycastResult result in raycastResult)
            {
                if (result.gameObject.name.Contains(K.slot))
                {
                    Slot slot = result.gameObject.GetComponent<Slot>();
                    if (!slot.isDrawn)
                    {
                        print($"Slot index: {slot.index}");
                        RectTransform slotTransform = result.gameObject.GetComponent<RectTransform>();
                        LineRenderer lineRenderer = lines[lines.Count - 1].GetComponent<LineRenderer>();
                        RectTransform initTransform = lines[lines.Count - 1].GetComponent<Line>().rectTransform;

                        print($"mousePosition: {mousePosition.x}, {mousePosition.y}.\n mousePositionStart: {mousePositionStart.x}, {mousePositionStart.y}");
                        if (mousePosition.y < mousePositionStart.y) // Down
                        {
                            lineRenderer.SetPosition(lineRenderer.positionCount++,
                                new Vector3(initTransform.position.x, slotTransform.position.y, 0));

                            slot.isDrawn = true;
                            slot.color = lineRenderer.endColor;
                            drawingHorizontally = false;
                        }
                        else if (mousePosition.x < mousePositionStart.x) // Left
                        {
                            if (!drawingHorizontally)
                            {

                            }
                            lineRenderer.SetPosition(lineRenderer.positionCount++,
                                new Vector3(slotTransform.position.x, initTransform.position.y, 0));

                            slot.isDrawn = true;
                            slot.color = lineRenderer.endColor;
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            toucherCanvasGroup.alpha = 0.0f;

            List<RaycastResult> raycastResult = CastRays();

            foreach (RaycastResult result in raycastResult)
            {
                if (result.gameObject.name.Contains(K.slot))
                {

                }
            }
        }
    }

    private List<RaycastResult> CastRays()
    {
        List<RaycastResult> result = new List<RaycastResult>();
        pointerEventData.position = Input.mousePosition;
        raycaster.Raycast(pointerEventData, result);
        return result;
    }

}
