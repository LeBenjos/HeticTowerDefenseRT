using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceTower : MonoBehaviour
{
    public GameObject towerPrefab;
    private GameObject placedTower;
    private ARRaycastManager raycastManager;
    private bool canPlace = true;
    public Button startButton;
    public EnemySpawner enemySpawner;

    static readonly List<ARRaycastHit> hits = new();

    void Start()
    {
        raycastManager = FindFirstObjectByType<ARRaycastManager>();
        startButton.interactable = false;
    }

    void Update()
    {
        if (raycastManager == null)
            return;

        if (!canPlace || Touchscreen.current == null || Touchscreen.current.primaryTouch == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        // Only on touch begin
        if (touch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = touch.position.ReadValue();
            
            if (EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue()))
                return;

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                if (placedTower == null)
                {
                    placedTower = Instantiate(towerPrefab, hitPose.position, hitPose.rotation);
                    startButton.interactable = true;
                }
                else
                {
                    placedTower.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }
        }
    }

    public void LockPlacement()
    {
        canPlace = false;
        enemySpawner.SetTarget(placedTower.transform);
    }

    public GameObject GetPlacedTower()
    {
        return placedTower;
    }
}