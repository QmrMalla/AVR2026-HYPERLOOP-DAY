using UnityEngine;
using TMPro;

/// <summary>
/// goTube-Gate: Druckausgleich (Vakuum) + Totmann-Prinzip + vertikales Schiebetor + Manometer.
/// Das Gate GLEITET nach oben (Seilzug/Winde), Sicherung mit Bolzen von unten.
/// Kern-Interaktionen des Szenarios "Oeffnen des goTube-Gates" (AVR2026-HYPERLOOP-DAY).
/// </summary>
public class DoorInteraction : MonoBehaviour
{
    [Header("Druck / Vakuum")]
    public bool isPressurized = false;
    public float pressureValue = 0f;        // 0 = Vakuum, 1 = ausgeglichen (~1 bar)
    public float pressureSpeed = 0.5f;      // bar pro Sekunde
    private bool isPressurizing = false;

    [Header("Tuer-Animation (Schiebetor - vertikal)")]
    public Transform doorLeaf;              // TubeGate1 (das blaue Gate)
    public float openDistance = 1.71f;      // wie weit es nach OBEN gleitet
    public float doorSpeed = 1.5f;          // Meter pro Sekunde
    private Vector3 closedPos;
    private bool doorIsOpen = false;

    [Header("Totmann (zwei Taster)")]
    public bool holdingButton1 = false;     // von Taster A gesetzt
    public bool holdingButton2 = false;     // von Taster B gesetzt

    [Header("UI")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI pressureText;

    [Header("Manometer")]
    public Transform needlePivot;           // NeedlePivot (Drehpunkt des Zeigers)
    public float needleAngleAt0 = 90f;      // Zeigerwinkel bei 0 bar
    public float needleAngleAt1 = -90f;     // Zeigerwinkel bei 1 bar

    [Header("Auto-Drehung zum Tor")]
public Transform playerRig;          // [BuildingBlock] Camera Rig
public float turnToAngle = 70f;      // Zielwinkel (nach rechts), am Setup anpassen
public float turnSpeed = 10f;        // Grad pro Sekunde
private bool shouldTurn = false;

[Header("Auto-Zoom zur ElectricBox")]
public float moveForwardDistance = 1.5f;   // كم تتقرّب (متر) - صغيرة
public float moveSpeed = 1f;                // سرعة التحرّك
private Vector3 targetPosition;
private bool shouldMove = false;

    void Start()
    {
        if (warningPanel != null) warningPanel.SetActive(false);
        if (doorLeaf != null) closedPos = doorLeaf.localPosition;   // Ausgangsposition merken
        UpdatePressureDisplay();
    }

    void Update()
    {
        // 1) Druckausgleich laeuft
        if (isPressurizing)
        {
            pressureValue += Time.deltaTime * pressureSpeed;
            if (pressureValue >= 1f)
            {
                pressureValue = 1f;
                isPressurizing = false;
                isPressurized = true;
                ShowWarning("Druckausgleich abgeschlossen!\nTor kann jetzt geoeffnet werden.");
                shouldTurn = true;  
                shouldMove = true;
if (playerRig != null)
    targetPosition = playerRig.localPosition + playerRig.right * moveForwardDistance;
    // playerRig.right = اتجاه اليمين (نحو الباب). لو الاتجاه غلط، نجرّب forward أو -right
            }
            UpdatePressureDisplay();
        }

        // 2) Totmann-Prinzip: Tor gleitet NUR, solange BEIDE Taster gehalten werden.
        bool deadmanActive = isPressurized && !doorIsOpen && holdingButton1 && holdingButton2;
        if (deadmanActive && doorLeaf != null)
        {
            Vector3 target = closedPos + Vector3.up * openDistance;
            doorLeaf.localPosition = Vector3.MoveTowards(doorLeaf.localPosition, target, doorSpeed * Time.deltaTime);

            if (Vector3.Distance(doorLeaf.localPosition, target) < 0.01f)
            {
                doorIsOpen = true;
                ShowWarning("Tor ist oben.\nMit Sicherungsbolzen von unten sichern.");
            }
        }

        // 3) Manometer-Zeiger immer aktualisieren (jeden Frame)
        if (needlePivot != null)
        {
            float angle = Mathf.Lerp(needleAngleAt0, needleAngleAt1, pressureValue);
            needlePivot.localEulerAngles = new Vector3(0, 0, angle);
        }
        if (shouldTurn && playerRig != null)
{
    float currentY = playerRig.localEulerAngles.y;
    float newY = Mathf.MoveTowardsAngle(currentY, turnToAngle, turnSpeed * Time.deltaTime);
    playerRig.localEulerAngles = new Vector3(0, newY, 0);
    if (Mathf.Abs(Mathf.DeltaAngle(newY, turnToAngle)) < 0.5f)
        shouldTurn = false;   // وصل، نوقف
}
 if (shouldMove && playerRig != null)
{
    playerRig.localPosition = Vector3.MoveTowards(playerRig.localPosition, targetPosition, moveSpeed * Time.deltaTime);
    if (Vector3.Distance(playerRig.localPosition, targetPosition) < 0.01f)
        shouldMove = false;
}

    }
   
    /// <summary>Vom Druckausgleich-Taster (Ventil) aufgerufen.</summary>
    public void StartPressureEqualization()
    {
        if (!isPressurized && !isPressurizing)
        {
            isPressurizing = true;
            ShowWarning("Druckausgleich laeuft...");
        }
    }

    /// <summary>Versuch, ohne Druckausgleich zu oeffnen -> zeigt die Vakuum-Warnung.</summary>
    public void TryOpenDoor()
    {
        if (!isPressurized)
            ShowWarning("Druckausgleich erforderlich!\nVakuum aktiv - Tor gesperrt.");
    }

    /// <summary>NOT-AUS: stoppt alles sofort (Sicherheit).</summary>
    public void EmergencyStop()
    {
        isPressurizing = false;
        holdingButton1 = false;
        holdingButton2 = false;
        ShowWarning("NOT-AUS aktiviert!\nVorgang gestoppt.");
    }

    // Von den zwei Totmann-Tastern (Press/Release-Events).
    public void SetButton1(bool held) { holdingButton1 = held; }
    public void SetButton2(bool held) { holdingButton2 = held; }

    void UpdatePressureDisplay()
    {
        if (pressureText != null)
            pressureText.text = "Druck: " + pressureValue.ToString("F1") + " bar";
    }

    void ShowWarning(string message)
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
            if (warningText != null) warningText.text = message;
            CancelInvoke(nameof(HideWarning));
            Invoke(nameof(HideWarning), 3f);
        }
    }

    void HideWarning()
    {
        if (warningPanel != null) warningPanel.SetActive(false);
    }
}