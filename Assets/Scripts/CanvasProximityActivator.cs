using UnityEngine;
using StarterAssets;

public class CanvasProximityActivator : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas; // Reference to the Canvas to display
    [SerializeField] private float interactionDistance = 2f; // Distance at which canvas becomes visible
    [SerializeField] private Transform activationPoint; // Object position to measure distance from (e.g., door)
    
    private StarterAssetsInputs playerInputs;
    private bool isCanvasActive = false;

    void Start()
    {
        // Deactivate canvas at start
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(false);
        }

        // Find player inputs if not already set
        if (playerInputs == null)
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerInputs = playerObject.GetComponent<StarterAssetsInputs>();
            }
        }

        // If no activation point specified, use this object's transform
        if (activationPoint == null)
        {
            activationPoint = transform;
        }
    }

    void Update()
    {
        if (activationPoint == null || targetCanvas == null)
            return;

        // Find player
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        // Calculate distance from player to activation point
        float distance = Vector3.Distance(player.transform.position, activationPoint.position);
        bool shouldBeActive = distance < interactionDistance;

        // Only update if state changed
        if (shouldBeActive != isCanvasActive)
        {
            isCanvasActive = shouldBeActive;
            targetCanvas.gameObject.SetActive(shouldBeActive);

            // Control cursor and camera input based on canvas state
            if (playerInputs != null)
            {
                if (shouldBeActive)
                {
                    // Canvas is active - unlock cursor and disable camera input
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    playerInputs.cursorInputForLook = false;
                }
                else
                {
                    // Canvas is inactive - lock cursor and enable camera input
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    playerInputs.cursorInputForLook = true;
                }
            }
        }
    }
}
