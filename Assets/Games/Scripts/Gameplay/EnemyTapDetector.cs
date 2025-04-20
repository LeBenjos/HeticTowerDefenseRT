using UnityEngine;
using UnityEngine.InputSystem;

public class ZombieTapDetector : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Touchscreen.current == null || Touchscreen.current.primaryTouch == null)
            return;

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = mainCamera.ScreenPointToRay(touchPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<EnemyBase>(out var enemy))
                {
                    enemy.TakeDamage(50);
                }
            }
        }
    }
}
