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
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lineRenderer.SetPosition(0, new Vector3(mousePositionStart.x, mousePositionStart.y, 0));

            toucherRectTransform.position = mousePositionStart;
            toucherCanvasGroup.alpha = 0.6f;
        }

        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lineRenderer.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0));

            toucherRectTransform.position = mousePosition;

            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name.Contains(K.slot))
                {
                    Slot slot = result.gameObject.GetComponent<Slot>();
                    if (slot.isEmpty)
                    {
                        print("Drawing...");
                        slot.isEmpty = false;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            toucherCanvasGroup.alpha = 0.0f;
            lineRenderer.SetPosition(1, new Vector3(mousePositionStart.x, mousePositionStart.y, 0));

        }
    }

}
