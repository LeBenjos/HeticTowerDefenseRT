using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("AR")]
    public ARRaycastManager raycastManager;

    [Header("Tour de défense")]
    public GameObject towerPrefab;
    private GameObject spawnedTower;
    private bool placementLocked = false;

    [Header("UI")]
    public Button startButton;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        startButton.gameObject.SetActive(false); // Caché au départ
        startButton.onClick.AddListener(OnStartGame);
    }

    void Update()
    {
        if (!placementLocked)
        {
            HandlePlacement();
        }

        // Si la tour est placée mais pas encore verrouillée → afficher le bouton
        if (!placementLocked && spawnedTower != null)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    void HandlePlacement()
    {
        if (Input.touchCount == 0) return;

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

    void OnStartGame()
    {
		Debug.Log("button clicked");
        if (spawnedTower != null)
        {
            placementLocked = true;
            startButton.gameObject.SetActive(false);
            Debug.Log("Début du jeu ! Tour verrouillée.");
            // 👉 Tu peux lancer tes vagues ici
        }
    }
}