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
    private Color32 currentColor = default;
    //private LineRenderer lineRenderer = default;
    private Vector2 mousePosition = default;
    private Vector2 mousePositionStart = default;
    private List<RaycastResult> raycastResult = new List<RaycastResult>();

    private void Awake()
    {
        toucherRectTransform = toucherObject.GetComponent<RectTransform>();
        toucherCanvasGroup = toucherObject.GetComponent<CanvasGroup>();
        raycaster = uiCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = uiCanvas.GetComponent<EventSystem>();
        //lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        //lineRenderer.positionCount = 0;
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

            foreach (RaycastResult result in raycastResult)
            {
                if (result.gameObject.name.Contains(K.slot))
                {
                    Slot slot = result.gameObject.GetComponent<Slot>();
                    if (slot.isStarter)
                    {
                        currentColor = slot.color;
                        //lineRenderer.SetColors(slot.color, slot.color);
                        DrawLine(result.gameObject);
                        //slot.isStarter = false;
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
                        print($"slot.index: {slot.index}");
                        if (mousePosition.y < mousePositionStart.y) // Down
                        {
                            if (slot.groupNumber == 0)
                            {
                                GameObject upObject = result.gameObject.transform.GetChild(2).gameObject;
                                upObject.SetActive(true);
                                upObject.GetComponent<Image>().color = currentColor;
                            }

                            GameObject downObject = result.gameObject.transform.GetChild(3).gameObject;
                            downObject.SetActive(true);
                            downObject.GetComponent<Image>().color = currentColor;
                        }


                        //DrawLine(result.gameObject);
                        slot.isDrawn = true;
                        //slot.color = lineRenderer.endColor;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            toucherCanvasGroup.alpha = 0.0f;

            CastRays();

            foreach (RaycastResult result in raycastResult)
            {
                if (result.gameObject.name.Contains(K.slot))
                {
                    Slot slot = result.gameObject.GetComponent<Slot>();
                    slot.isStarter = true;
                }
            }
        }
    }

    private void CastRays()
    {
        pointerEventData.position = Input.mousePosition;
        raycaster.Raycast(pointerEventData, raycastResult);
    }

    private void DrawLine(GameObject basedOnObject)
    {
        RectTransform slotTransform = basedOnObject.GetComponent<RectTransform>();
        //lineRenderer.SetPosition(lineRenderer.positionCount++,
        //    new Vector3(slotTransform.position.x, slotTransform.position.y, 0));
    }

}
