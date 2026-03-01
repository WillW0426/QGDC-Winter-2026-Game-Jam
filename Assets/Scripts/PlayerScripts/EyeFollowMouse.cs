using UnityEngine;
using UnityEngine.InputSystem;

public class EyeFollowMouse : MonoBehaviour
{
    [Header("References")]
    public Transform anchor;

    [Header("Movement Limits")]
    public float maxXDistance = 0.1f;
    public float maxYDistance = 0.05f;

    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = _cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 0f));
        mouseWorld.z = anchor.position.z;

        Vector3 direction = mouseWorld - anchor.position;

        // Normalize direction within ellipse bounds
        float ex = direction.x / maxXDistance;
        float ey = direction.y / maxYDistance;

        // If outside the ellipse, project back onto its edge
        float ellipseDist = Mathf.Sqrt(ex * ex + ey * ey);
        if (ellipseDist > 1f)
        {
            ex /= ellipseDist;
            ey /= ellipseDist;
        }

        float offsetX = ex * maxXDistance;
        float offsetY = ey * maxYDistance;

        transform.position = new Vector3(
            anchor.position.x + offsetX,
            anchor.position.y + offsetY,
            anchor.position.z
        );
    }
}