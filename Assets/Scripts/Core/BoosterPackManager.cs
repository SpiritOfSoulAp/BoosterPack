using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoosterPackManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CardDatabase database;
    [SerializeField] private int cardsPerPack = 5;

    private Dictionary<Rarity, List<CardData>> cardsByRarity;
    private List<(Rarity rarity, float threshold)> rarityThresholds;

    private void Awake()
    {
        GroupCardsByRarity();
        InitializeRarityThresholds();
    }

    private void GroupCardsByRarity()
    {
        cardsByRarity = new();
        foreach (Rarity rarity in System.Enum.GetValues(typeof(Rarity)))
        {
            cardsByRarity[rarity] = database.cards.Where(card => card.rarity == rarity).ToList();
        }
    }

    private void InitializeRarityThresholds()
    {
        rarityThresholds = new();
        float cumulative = 0f;
        var chances = new Dictionary<Rarity, float> {
            { Rarity.Common, 0.70f },
            { Rarity.Rare, 0.20f },
            { Rarity.Epic, 0.08f },
            { Rarity.Legendary, 0.02f }
        };
        foreach (var kvp in chances)
        {
            cumulative += kvp.Value;
            rarityThresholds.Add((kvp.Key, cumulative));
        }
    }

    public List<CardData> OpenPack(CardCollection collection)
    {
        List<CardData> result = new();

        for (int i = 0; i < cardsPerPack; i++)
        {
            Rarity selectedRarity = GetRandomRarity();
            List<CardData> pool = cardsByRarity[selectedRarity];
            if (pool.Count > 0)
            {
                CardData card = pool[Random.Range(0, pool.Count)];
                result.Add(card);
                collection?.AddCard(card);
            }
        }

        return result;
    }

    private Rarity GetRandomRarity()
    {
        float rand = Random.value;
        foreach (var threshold in rarityThresholds)
        {
            if (rand <= threshold.threshold)
            {
                return threshold.rarity;
            }
        }
        return Rarity.Common;
    }
}