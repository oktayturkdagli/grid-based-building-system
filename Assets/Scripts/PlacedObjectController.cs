using UnityEngine;

public class PlacedObjectController : MonoBehaviour
{
    private Transform currentPrefab;
    private PlacedObjectTypeSO placedObjectTypeSO;
    
    private void Start()
    {
        RefreshModel();
        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }
    
    private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    {
        RefreshModel();
    }
    
    private void LateUpdate()
    {
        Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
        targetPosition.y = 0f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }
    
    private void RefreshModel()
    {
        if (currentPrefab)
        {
            Destroy(currentPrefab.gameObject);
            currentPrefab = null;
        }
        
        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSo();
        
        if (placedObjectTypeSO)
        {
            currentPrefab = Instantiate(placedObjectTypeSO.prefab, Vector3.zero, Quaternion.identity);
            currentPrefab.parent = transform;
            currentPrefab.localPosition = Vector3.zero;
            currentPrefab.localEulerAngles = Vector3.zero;

            var renderer = currentPrefab.GetChild(0).gameObject.GetComponent<Renderer>();
            var materials = renderer.materials;
            foreach (var material in materials)
            {
                // change rendering mode to transparent
                material.SetFloat("_Mode", 3);
                material.color = Utils.GetUnityColorOfRGBA(new Color(204, 204, 204, 40));
            }
        }
    }
}