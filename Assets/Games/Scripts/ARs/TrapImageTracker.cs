using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrapImageTracker : MonoBehaviour
{
    [System.Serializable]
    public struct TrapImageEntry
    {
        public string imageName;
        public GameObject trapPrefab;
    }

    [SerializeField] private List<TrapImageEntry> trapPrefabs;

    private ARTrackedImageManager imageManager;
    private Dictionary<string, GameObject> spawnedTraps = new();

    void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
    }

    void Update()
    {
        foreach (var trackedImage in imageManager.trackables)
        {
            if (trackedImage.trackingState != TrackingState.Tracking)
                continue;

            string imageName = trackedImage.referenceImage.name;

            if (!spawnedTraps.ContainsKey(imageName))
            {
                GameObject prefab = trapPrefabs.Find(p => p.imageName == imageName).trapPrefab;

                if (prefab != null)
                {
                    GameObject trap = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    trap.transform.SetParent(trackedImage.transform);
                    spawnedTraps[imageName] = trap;
                }
            }
            else
            {
                // Update position/rotation if needed
                GameObject trap = spawnedTraps[imageName];
                trap.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
                trap.SetActive(true);
            }
        }
    }
}
