using UnityEngine;
using TMPro;

/// <summary>
/// goTube-Gate: Druckausgleich (Vakuum) + Totmann-Prinzip + vertikales Schiebetor.
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
    public float openDistance = 2f;         // wie weit es nach OBEN gleitet (am Asset pruefen)
    public float doorSpeed = 1.5f;          // Meter pro Sekunde
    private Vector3 closedPos;
    private bool doorIsOpen = false;
    // Gleitet entlang der lokalen Y-Achse nach oben. Bei geneigter Fuehrung:
    // doorLeaf unter ein passend gedrehtes Empty haengen und dieses als Referenz nutzen.

    [Header("Totmann (zwei Taster)")]
    public bool holdingButton1 = false;     // von Taster A gesetzt (Press = true / Release = false)
    public bool holdingButton2 = false;     // von Taster B gesetzt

    [Header("UI")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI pressureText;

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
            }
            UpdatePressureDisplay();
        }

        // 2) Totmann-Prinzip: Tor gleitet NUR, solange BEIDE Taster gehalten werden.
        //    Wird ein Taster losgelassen, stoppt die Bewegung (Haende ausserhalb des Bewegungsbereichs).
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

    // Von den zwei Totmann-Tastern (z. B. ueber Poke-Interaction, Press/Release-Events).
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