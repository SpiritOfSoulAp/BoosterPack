using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "BoosterPack/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> cards;
}