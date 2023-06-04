using UnityEngine;
using UnityEngine.InputSystem;

public static class Utils
{
    public static Color GetUnityColorOfRGBA(Color color)
    {
        var r = color.r == 0 ? 0 : color.r / 255;
        var g = color.g == 0 ? 0 : color.g / 255;
        var b = color.b == 0 ? 0 : color.b / 255;
        var a = color.a == 0 ? 0 : color.a / 255;
        return new Color(r, g, b, a);
    }
    
    public static Vector3 GetMousePositionIn3D()
    {
        // Debug.DrawRay(cam.transform.position, cam.transform.forward * distance, Color.magenta, 3f);
        Vector3 mousePositionIn3D;
        Camera cam = Camera.main;
        int layerMask = 2;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue()); // Creates a Ray from the mouse position
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, ~layerMask, QueryTriggerInteraction.Collide))
        {
            mousePositionIn3D = hitInfo.point;
        }
        else
        {
            mousePositionIn3D = Vector3.zero;
        }
        
        return mousePositionIn3D;
    }
    
    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 0)
    {
        if (color == null) 
            color = Color.white;
        
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
        
    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.eulerAngles = 90f * Vector3.right;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}