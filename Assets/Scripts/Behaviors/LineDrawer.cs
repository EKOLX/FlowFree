using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private GameObject toucherObject = default;
    [SerializeField] private GameObject uiCanvas = default;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private RectTransform toucherRectTransform = default;
    private CanvasGroup toucherCanvasGroup = default;
    private LineRenderer lineRenderer = default;
    private Vector2 mousePosition = default;
    private Vector2 mousePositionStart = default;
    private List<RaycastResult> raycastResult = new List<RaycastResult>();

    private void Awake()
    {
        toucherRectTransform = toucherObject.GetComponent<RectTransform>();
        toucherCanvasGroup = toucherObject.GetComponent<CanvasGroup>();
        raycaster = uiCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = uiCanvas.GetComponent<EventSystem>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.positionCount = 0;
        pointerEventData = new PointerEventData(eventSystem);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            toucherRectTransform.position = mousePositionStart;
            toucherCanvasGroup.alpha = 0.6f;

            CastRays();

            print($"RaycastResultCount: {raycastResult.Count}");
            foreach (RaycastResult result in raycastResult)
            {
                if (result.gameObject.name.Contains(K.slot))
                {
                    Slot slot = result.gameObject.GetComponent<Slot>();
                    if (slot.isDrawn)
                    {
                        lineRenderer.SetPosition(lineRenderer.positionCount++, new Vector3(mousePositionStart.x, mousePositionStart.y, 0));
                        lineRenderer.SetColors(slot.color, slot.color);
                    }
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            toucherRectTransform.position = mousePosition;

            CastRays();

            foreach (RaycastResult result in raycastResult)
            {
                if (result.gameObject.name.Contains(K.slot))
                {
                    Slot slot = result.gameObject.GetComponent<Slot>();
                    if (!slot.isDrawn)
                    {
                        lineRenderer.SetPosition(lineRenderer.positionCount++, new Vector3(mousePosition.x, mousePosition.y, 0));
                        slot.isDrawn = true;
                        slot.color = lineRenderer.endColor;
                        print(slot.index);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            toucherCanvasGroup.alpha = 0.0f;

        }
    }

    private void CastRays()
    {
        pointerEventData.position = Input.mousePosition;
        raycaster.Raycast(pointerEventData, raycastResult);
    }

}
