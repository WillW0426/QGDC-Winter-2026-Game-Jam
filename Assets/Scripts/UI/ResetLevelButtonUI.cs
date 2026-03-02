using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestLevelButtonUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button resetButton;

    [Header("Hover Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;

    public Loader.Scene resetScene;

    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>();

        resetButton.onClick.AddListener(() =>
            levelManager.Restart()
        );

        AddHoverEffect(resetButton);
    }

    private void AddHoverEffect(Button button)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

        if (buttonText == null)
        {
            Debug.LogWarning($"{button.name} has no TMP_Text child!");
            return;
        }

        // Set initial color
        buttonText.color = normalColor;

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>()
                            ?? button.gameObject.AddComponent<EventTrigger>();

        // On Hover Enter
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((_) => buttonText.color = hoverColor);
        trigger.triggers.Add(enterEntry);

        // On Hover Exit
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((_) => buttonText.color = normalColor);
        trigger.triggers.Add(exitEntry);
    }
}
