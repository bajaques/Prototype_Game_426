using UnityEngine;

public static class Utilities {

    public static bool isCentering;
    public static bool isZooming;
    public static bool isMoving;
    public static bool isStrafing;
    public static bool playerRotating;

    // Helper method to find accurate angle of rotation between two vectors
    public static float FindRotationAngle(Vector3 from, Vector3 to) {
        Vector3 rotationAxis = Vector3.Cross(from, to);
        return Vector3.Angle(from, to) * (rotationAxis.y >= 0 ? -1f : 1f);
    }

    // Removes y coordinate from camera's forward vector
    // This allows us to accurately move/rotate the player relative to the camera's direction
    public static Vector3 FlattenCamera(Vector3 camForward) {
        Vector3 flattenedCamera = camForward;
        flattenedCamera.y = 0.0f;
        return flattenedCamera;
    }
}
