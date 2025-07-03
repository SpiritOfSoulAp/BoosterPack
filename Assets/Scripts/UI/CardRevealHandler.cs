using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardRevealHandler : MonoBehaviour, IPointerClickHandler
{
    private bool isRevealed = false;
    private CardPackUIController controller;
    private CardData data;

    public void Init(CardData cardData, CardPackUIController controllerRef)
    {
        data = cardData;
        controller = controllerRef;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isRevealed)
        {
            isRevealed = true;
            controller.OnCardRevealed(data);
        }
    }
}
