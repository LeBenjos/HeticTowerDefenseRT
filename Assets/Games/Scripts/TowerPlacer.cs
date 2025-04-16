using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TowerPlacer : MonoBehaviour
{
    public GameObject towerPrefab; // Assign in Inspector
    private GameObject spawnedTower;

    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public bool placementLocked = false;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (placementLocked) return;
        
        if (Input.touchCount > 0)
        {
            Debug.Log("Touched");
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    if (spawnedTower == null)
                    {
                        spawnedTower = Instantiate(towerPrefab, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        spawnedTower.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                    }
                }
            }
        }
    }
    
    public bool IsTowerPlaced()
    {
        return spawnedTower != null;
    }

    public void LockPlacement()
    {
        placementLocked = true;
    }
}
