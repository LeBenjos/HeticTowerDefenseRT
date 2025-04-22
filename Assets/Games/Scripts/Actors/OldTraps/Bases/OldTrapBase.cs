using UnityEngine;

public class OldTrapBase : MonoBehaviour
{
    private PlaceTower placeTower;
    private Transform towerTransform;

    void Start()
    {
        placeTower = FindFirstObjectByType<PlaceTower>();
        TryGetTowerTransform();
    }

    void Update()
    {
        if (towerTransform == null)
        {
            TryGetTowerTransform();
            if (towerTransform == null)
                return;
        }

        // Caler la position Y sur la tour
        Vector3 position = transform.position;
        position.y = towerTransform.position.y;
        transform.position = position;

        // Bloquer la rotation X et Z pour rester droit
        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0f;
        rotation.z = 0f;
        transform.eulerAngles = rotation;
    }

    void TryGetTowerTransform()
    {
        if (placeTower != null && placeTower.GetPlacedTower() != null)
        {
            towerTransform = placeTower.GetPlacedTower().transform;
        }
    }
}
