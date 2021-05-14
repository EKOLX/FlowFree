using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector2 mousePosition;
    private Vector2 mousePositionStart;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0, new Vector3(mousePositionStart.x, mousePositionStart.y, 0));
            lineRenderer.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0));

        }
    }

}
