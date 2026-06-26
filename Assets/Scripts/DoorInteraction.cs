using UnityEngine;
using TMPro;

/// <summary>
/// goTube-Gate: Druckausgleich + Totmann + Schiebetor + Manometer + NOT-AUS
/// + Audio-Anweisungen (Input gesperrt waehrend Audio) + Auto-Drehung/Zoom zum Tor
/// + textuelle Sicherheitshinweise (Warnungen aus der 360-Tour).
/// AVR2026-HYPERLOOP-DAY.
/// </summary>
public class DoorInteraction : MonoBehaviour
{
    [Header("Druck / Vakuum")]
    public bool isPressurized = false;
    public float pressureValue = 0f;        // 0 = Vakuum, 1 = ausgeglichen (~1 bar)
    public float pressureSpeed = 0.5f;
    private bool isPressurizing = false;

    [Header("Tuer-Animation (Schiebetor - vertikal)")]
    public Transform doorLeaf;              // TubeGate1
    public float openDistance = 1.71f;
    public float doorSpeed = 1.5f;
    private Vector3 closedPos;
    private bool doorIsOpen = false;

    [Header("Totmann (zwei Taster)")]
    public bool holdingButton1 = false;
    public bool holdingButton2 = false;

    [Header("UI")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI pressureText;

    [Header("Manometer")]
    public Transform needlePivot;
    public float needleAngleAt0 = 90f;
    public float needleAngleAt1 = -90f;

    [Header("Auto-Drehung / Zoom zum Tor")]
    public Transform playerRig;             // [BuildingBlock] Camera Rig
    public float turnToAngle = 90f;
    public float turnSpeed = 60f;
    public float moveForwardDistance = 0.8f;
    public float moveSpeed = 1f;
    private bool shouldTurn = false;
    private bool shouldMove = false;
    private Vector3 targetPosition;

    [Header("Audio - Anweisungen")]
    public AudioSource audioSource;
    public AudioClip clipWillkommen;        // Begruessung am Start
    public AudioClip clipDruckLaeuft;       // Druckausgleich laeuft
    public AudioClip clipDruckFertig;       // abgeschlossen, bitte zum Tor drehen
    public AudioClip clipTasterDruecken;    // beide Taster druecken
    public AudioClip clipTorOffen;          // Tor offen
    public AudioClip clipNotAus;            // NOT-AUS

    // Input-Sperre waehrend eine Audio-Anweisung laeuft
    private bool inputLocked = false;

    void Start()
    {
        if (warningPanel != null) warningPanel.SetActive(false);
        if (doorLeaf != null) closedPos = doorLeaf.localPosition;
        UpdatePressureDisplay();

        // Begruessung + Sicherheitshinweis zu Beginn
        ShowWarning("Willkommen bei goTube.\nPruefen Sie zuerst Kabel, Schalter, Sensoren\nund das Seilzugsystem.");
        PlayInstruction(clipWillkommen);
    }

    void Update()
    {
        // 1) Druckausgleich
        if (isPressurizing)
        {
            pressureValue += Time.deltaTime * pressureSpeed;
            if (pressureValue >= 1f)
            {
                pressureValue = 1f;
                isPressurizing = false;
                isPressurized = true;
                ShowWarning("Druckausgleich abgeschlossen!\nBitte zum Tor drehen.");

                PlayInstruction(clipDruckFertig);
                float wait = (clipDruckFertig != null) ? clipDruckFertig.length : 1.5f;
                Invoke(nameof(StartTurnAndMove), wait);
            }
            UpdatePressureDisplay();
        }

        // 2) Totmann: Tor gleitet NUR, solange beide Taster gehalten werden
        bool deadmanActive = !inputLocked && isPressurized && !doorIsOpen && holdingButton1 && holdingButton2;
        if (deadmanActive && doorLeaf != null)
        {
            Vector3 target = closedPos + Vector3.up * openDistance;
            doorLeaf.localPosition = Vector3.MoveTowards(doorLeaf.localPosition, target, doorSpeed * Time.deltaTime);
            if (Vector3.Distance(doorLeaf.localPosition, target) < 0.01f)
            {
                doorIsOpen = true;
                ShowWarning("Tor geoeffnet.\nMit Sicherungsbolzen von unten sichern.");
                PlayInstruction(clipTorOffen);
            }
        }

        // 3) Manometer-Zeiger immer aktualisieren
        if (needlePivot != null)
        {
            float angle = Mathf.Lerp(needleAngleAt0, needleAngleAt1, pressureValue);
            needlePivot.localEulerAngles = new Vector3(0, 0, angle);
        }

        // 4) Auto-Drehung zum Tor
        if (shouldTurn && playerRig != null)
        {
            float currentY = playerRig.localEulerAngles.y;
            float newY = Mathf.MoveTowardsAngle(currentY, turnToAngle, turnSpeed * Time.deltaTime);
            playerRig.localEulerAngles = new Vector3(0, newY, 0);
            if (Mathf.Abs(Mathf.DeltaAngle(newY, turnToAngle)) < 0.5f)
            {
                shouldTurn = false;
                ShowWarning("Stellen Sie sicher, dass der Bewegungsbereich frei ist.\nKeine Personen im Gefahrenbereich!\nDann beide Taster gleichzeitig druecken.");
                PlayInstruction(clipTasterDruecken);
            }
        }

        // 5) Auto-Zoom (leichtes Vorwaerts-Bewegen)
        if (shouldMove && playerRig != null)
        {
            playerRig.localPosition = Vector3.MoveTowards(playerRig.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(playerRig.localPosition, targetPosition) < 0.01f)
                shouldMove = false;
        }
    }

    /// <summary>Spielt eine Audio-Anweisung ab und sperrt solange alle Eingaben.</summary>
    void PlayInstruction(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        inputLocked = true;
        audioSource.Stop();
        audioSource.PlayOneShot(clip);
        CancelInvoke(nameof(UnlockInput));
        Invoke(nameof(UnlockInput), clip.length);
    }

    void UnlockInput() { inputLocked = false; }

    void StartTurnAndMove()
    {
        shouldTurn = true;
        shouldMove = true;
        if (playerRig != null)
            targetPosition = playerRig.localPosition + playerRig.right * moveForwardDistance;
    }

    /// <summary>Vom Druckausgleich-Taster (Ventil) aufgerufen.</summary>
    public void StartPressureEqualization()
    {
        if (inputLocked) return;
        if (!isPressurized && !isPressurizing)
        {
            isPressurizing = true;
            ShowWarning("Druckausgleich laeuft...\nDie Roehre wird ueber spezielle Ventile belueftet.");
            PlayInstruction(clipDruckLaeuft);
        }
    }

    /// <summary>Versuch, ohne Druckausgleich zu oeffnen.</summary>
    public void TryOpenDoor()
    {
        if (inputLocked) return;
        if (!isPressurized)
            ShowWarning("Druckausgleich erforderlich!\nVakuum aktiv - der Atmosphaerendruck wirkt\nmit ca. 200 kN auf das Tor. Tor gesperrt.");
    }

    /// <summary>NOT-AUS: stoppt alles sofort.</summary>
    public void EmergencyStop()
    {
        if (inputLocked) return;
        isPressurizing = false;
        holdingButton1 = false;
        holdingButton2 = false;
        ShowWarning("NOT-AUS aktiviert!\nVorgang gestoppt. Bleiben Sie ausserhalb\ndes Gefahrenbereichs.");
        PlayInstruction(clipNotAus);
    }

    // Von den zwei Totmann-Tastern (Press/Release). Waehrend Audio gesperrt.
    public void SetButton1(bool held) { if (!inputLocked) holdingButton1 = held; }
    public void SetButton2(bool held) { if (!inputLocked) holdingButton2 = held; }

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
            Invoke(nameof(HideWarning), 5f);   // 5 Sekunden anzeigen (mehr Lesezeit)
        }
    }

    void HideWarning()
    {
        if (warningPanel != null) warningPanel.SetActive(false);
    }
}