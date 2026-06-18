using UnityEngine;
using TMPro;

public class DoorInteraction : MonoBehaviour
{
    [Header("Settings")]
    public bool isPressurized = false;
    
    [Header("UI")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;

    private bool playerNearDoor = false;

    void Start()
    {
        // اخفي لوحة التحذير في البداية
        if (warningPanel != null)
            warningPanel.SetActive(false);
    }

    // يُستدعى عند محاولة فتح الباب
    public void TryOpenDoor()
    {
        if (!isPressurized)
        {
            // الباب لا يفتح — يظهر تحذير
            ShowWarning("⚠️ Druckausgleich erforderlich!\nVakuum aktiv – Tür gesperrt.");
        }
        else
        {
            // الباب يفتح
            OpenDoor();
        }
    }

    void ShowWarning(string message)
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
            if (warningText != null)
                warningText.text = message;
            
            // اخفي التحذير بعد 3 ثواني
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
        // هنا نضيف Animation لاحقاً
    }
}