using UnityEngine;

/// <summary>
/// Laesst ein UI-Canvas dem Blick des Spielers folgen (World-Space-Canvas in VR).
/// Das Panel bleibt immer angenehm vor dem Nutzer, ohne ruckartiges Mitdrehen.
/// Auf das Canvas (oder ein Eltern-Objekt des WarningPanels) legen.
/// </summary>
public class FollowCamera : MonoBehaviour
{
    [Header("Kamera")]
    public Transform cameraTransform;    // CenterEyeAnchor / Main Camera

    [Header("Position")]
    public float distance = 1f;          // Abstand vor dem Gesicht (Meter)
    public float heightOffset = -0.2f;   // etwas unter Augenhoehe (angenehmer)

    [Header("Sanftheit")]
    public float followSpeed = 4f;       // wie schnell das Panel nachzieht
    public float rotateSpeed = 4f;       // wie schnell es sich zum Blick ausrichtet

    void Start()
    {
        // Falls keine Kamera gesetzt: automatisch die Main Camera nehmen
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Zielposition: vor der Kamera, nur horizontale Blickrichtung nutzen
        Vector3 fwd = cameraTransform.forward;
        fwd.y = 0f;
        if (fwd.sqrMagnitude < 0.001f) fwd = Vector3.forward;
        fwd.Normalize();

        Vector3 targetPos = cameraTransform.position + fwd * distance;
        targetPos.y = cameraTransform.position.y + heightOffset;

        // sanft zur Zielposition bewegen
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        // sanft zum Spieler ausrichten (Panel schaut den Nutzer an)
        Vector3 lookDir = transform.position - cameraTransform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
    }
}