using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour
{
    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;

    [Space]
    [Header("main button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;

    Button mainButton;
    SettingsMenuItem[] menuItems;
    bool isExpanded = false;

    Vector2 mainButtonPosition;
    int itemsCount;

    void Start()
    {
        //get the number of menu items (without main button)
        itemsCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];

        //get references to each menu item
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        //get ref to the main button
        mainButton = transform.GetChild(0).GetComponent<Button>();
        //atach ToggleMenu to the main button OnCklick 
        mainButton.onClick.AddListener(ToggleMenu);
        //move the main button to the front
        mainButton.transform.SetAsLastSibling();

        //store position of main button
        mainButtonPosition = mainButton.transform.position;

        //reset all menu items position to mainButtonPosition
        ResetPositions();
    }

    void ResetPositions()
    {
        //set the posion of each menu item 
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].trans.position = mainButtonPosition;
        }
    }

    void ToggleMenu()
    {
        //toggle
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            //menu opened
            for (int i = 0; i < itemsCount; i++)
            {
                // move
                menuItems[i].trans.DOMove(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                //fade in each menu item
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
            }
        }
        else
        {
            //menu closed
            for (int i = 0; i < itemsCount; i++)
            {
                //move
                menuItems[i].trans.DOMove(mainButtonPosition, collapseDuration).SetEase(collapseEase);
                //fade in ech menu item
                menuItems[i].img.DOFade(0f, collapseDuration);
            }
        }

        //rotate main button
        mainButton.transform
            .DORotate(Vector3.forward * 180f, rotationDuration)
            .From(Vector3.zero)
            .SetEase(rotationEase);
    }

    private void OnDestroy()
    {
        //remove 
        mainButton.onClick.RemoveListener(ToggleMenu);
    }


}
