using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    
    [SerializeField] Button quitButton;

    [SerializeField] Button creditsButton;

    [SerializeField] Button backButton;

    [SerializeField] GameObject subMenu;

    [Header("Hover Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.Level1);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        creditsButton.onClick.AddListener(() =>
        {
            subMenu.SetActive(true);
        });

        backButton.onClick.AddListener(() =>
        {
            subMenu.SetActive(false);
        });

        AddHoverEffect(playButton);
        AddHoverEffect(quitButton);
        AddHoverEffect(creditsButton);
        AddHoverEffect(backButton);
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