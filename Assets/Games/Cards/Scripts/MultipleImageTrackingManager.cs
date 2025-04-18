    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;

    public class MultipleImageTrackingManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> prefabsToSpawn = new List<GameObject>();

        private ARTrackedImageManager _trackedImageManager;

        private Dictionary<string, GameObject> _arObjects;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _trackedImageManager = GetComponent<ARTrackedImageManager>();
            if (_trackedImageManager == null) return;
            _trackedImageManager.trackablesChanged.AddListener(OnImageTrackedChanged);
            _arObjects = new Dictionary<string, GameObject>();

            SetupSceneElements();
        }

        private void SetupSceneElements()
        {
            foreach (var prefab in prefabsToSpawn)
            {
                var arObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                arObject.name = prefab.name;
                arObject.gameObject.SetActive(false);
                _arObjects.Add(arObject.name, arObject);
            }
        }

        private void OnDestroy()
        {
            _trackedImageManager.trackablesChanged.RemoveListener(OnImageTrackedChanged);
        }

        private void OnImageTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
        {
            foreach (var trackedImage in eventArgs.added)
            {
                UpdateTrackedImage(trackedImage);
            }

            foreach (var trackedImage in eventArgs.updated)
            {
                UpdateTrackedImage(trackedImage);
            }

            foreach (var trackedImage in eventArgs.removed)
            {
                UpdateTrackedImage(trackedImage.Value);
            }
        }

        private void UpdateTrackedImage(ARTrackedImage trackedImage)
        {
            if (trackedImage == null || trackedImage.referenceImage == null || string.IsNullOrEmpty(trackedImage.referenceImage.name)) return;
            
            if (_arObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject arObject))
            {
                arObject.SetActive(true);
                arObject.transform.position = trackedImage.transform.position;
                arObject.transform.rotation = trackedImage.transform.rotation;
            }
            else
            {
                Debug.LogWarning($"AR object not found for tracked image: {trackedImage.referenceImage.name}");
            }
           
        }
    }

