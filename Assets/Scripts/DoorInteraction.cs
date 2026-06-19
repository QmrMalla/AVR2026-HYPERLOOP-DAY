using UnityEngine;
using TMPro;

public class DoorInteraction : MonoBehaviour
{
    [Header("Settings")]
    public bool isPressurized = false;
    public float pressureValue = 0f;
    private bool isPressurizing = false;

    [Header("UI")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI pressureText;

    void Start()
    {
        if (warningPanel != null)
            warningPanel.SetActive(false);

        UpdatePressureDisplay();
    }

    void Update()
    {
        if (isPressurizing)
        {
            pressureValue += Time.deltaTime * 0.5f; // dauert ca. 2 Sekunden

            if (pressureValue >= 1f)
            {
                pressureValue = 1f;
                isPressurizing = false;
                isPressurized = true;
                ShowWarning("Druckausgleich abgeschlossen!\nTür kann jetzt geöffnet werden.");
            }

            UpdatePressureDisplay();
        }
    }

    // Button 1: Tür öffnen versuchen
    public void TryOpenDoor()
    {
        if (!isPressurized)
        {
            ShowWarning("Druckausgleich erforderlich!\nVakuum aktiv – Tür gesperrt.");
        }
        else
        {
            OpenDoor();
        }
    }

    // Button 2: Druckausgleich starten
    public void StartPressureEqualization()
    {
        if (!isPressurized && !isPressurizing)
        {
            isPressurizing = true;
            ShowWarning("Druckausgleich läuft...");
        }
    }

    void UpdatePressureDisplay()
    {
        if (pressureText != null)
            pressureText.text = "Druck: " + (pressureValue * 1f).ToString("F1") + " bar";
    }

    void ShowWarning(string message)
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
            if (warningText != null)
                warningText.text = message;

            CancelInvoke("HideWarning");
            Invoke("HideWarning", 3f);
        }
    }

    void HideWarning()
    {
        if (warningPanel != null)
            warningPanel.SetActive(false);
    }

    void OpenDoor()
    {
        Debug.Log("Tür öffnet sich!");
        ShowWarning("Tür öffnet sich!");
        // Hier später: Animation
    }
}