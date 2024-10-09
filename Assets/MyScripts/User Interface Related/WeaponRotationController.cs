using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponRotationController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Transform weaponTransform;  // The 3D weapon you want to rotate
    [SerializeField] private float rotationSpeed = 5f;    // Speed of rotation based on swipe
    [SerializeField] private float inertiaDamping = 0.95f; // Damping factor for the inertia after swipe
    [SerializeField] private float returnSpeed = 2f;      // Speed of returning to default rotation
    [SerializeField] private float defaultRotationSpeed = 20f; // Constant rotation speed around the Y-axis when not dragging

    private Vector2 lastDragPosition;                     // Store the last drag position for calculating delta
    private Vector2 currentInertia;                       // Stores the current inertia/momentum after drag ends
    private bool isDragging = false;                      // Track whether the player is currently dragging

    private Quaternion defaultRotation;                   // Default rotation of the weapon

    private void Start()
    {
        // Store the default rotation of the weapon (the "horizontal" position you want it to return to)
        defaultRotation = weaponTransform.rotation;
    }

    // Called when the drag starts
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        lastDragPosition = eventData.position;            // Record initial drag position
        currentInertia = Vector2.zero;                    // Reset inertia when new drag starts
    }

    // Called while dragging (finger or mouse is being held down and moved)
    public void OnDrag(PointerEventData eventData)
    {
        // Calculate the difference in position (delta) between the current and last drag position
        Vector2 currentDragPosition = eventData.position;
        Vector2 delta = currentDragPosition - lastDragPosition;

        // Rotate the weapon based on both horizontal and vertical swipe direction
        RotateWeapon(delta);

        // Update the last drag position to the current one
        lastDragPosition = currentDragPosition;
    }

    // Called when the drag ends
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // When dragging ends, store the last swipe delta as inertia (momentum)
        currentInertia = eventData.delta;
    }

    private void Update()
    {
        // Apply inertia if not dragging (slow down over time using damping)
        if (!isDragging && currentInertia != Vector2.zero)
        {
            RotateWeapon(currentInertia);

            // Dampen inertia over time (gradually reduce its magnitude)
            currentInertia *= inertiaDamping;

            // If inertia becomes very small, stop applying it
            if (currentInertia.magnitude < 0.1f)
            {
                currentInertia = Vector2.zero;
            }
        }

        // If no more dragging or inertia, slowly rotate weapon back to default rotation (only X and Z axes)
        if (!isDragging && currentInertia == Vector2.zero)
        {
            RotateBackToDefault();
        }

        // Constantly apply a slow rotation around the Y-axis (for default spinning)
        weaponTransform.Rotate(Vector3.up, defaultRotationSpeed * Time.deltaTime);
    }

    // Method to rotate the weapon based on swipe delta (for both axes)
    private void RotateWeapon(Vector2 swipeDelta)
    {
        // Calculate rotation amounts based on the swipe delta and rotation speed
        float rotationX = swipeDelta.y * rotationSpeed * Time.deltaTime; // Vertical swipe affects X-axis rotation
        float rotationY = -swipeDelta.x * rotationSpeed * Time.deltaTime; // Horizontal swipe affects Y-axis rotation

        // Apply the rotation to the weapon, taking into account the current forward direction
        // This ensures that vertical swipe is more like a pitch rotation based on Y axis
        Vector3 rightAxis = weaponTransform.right; // Local right axis of the weapon for vertical rotation
        weaponTransform.Rotate(rightAxis, -rotationX, Space.World);  // Rotate around the weapon's local right axis

        // Rotate around the Y-axis for horizontal movement
        weaponTransform.Rotate(Vector3.up, rotationY, Space.World);
    }

    // Method to gradually return weapon to its default horizontal rotation (without affecting Y-axis rotation)
    private void RotateBackToDefault()
    {
        // Get the current rotation around the Y-axis
        float currentYRotation = weaponTransform.eulerAngles.y;

        // Slerp towards the default rotation, but only adjust the X and Z axes
        Quaternion targetRotation = Quaternion.Euler(defaultRotation.eulerAngles.x, currentYRotation, defaultRotation.eulerAngles.z);
        weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, targetRotation, returnSpeed * Time.deltaTime);
    }
}
