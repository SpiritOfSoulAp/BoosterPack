using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CardCollection : MonoBehaviour
{
    private Dictionary<CardData, int> collection = new();

    [SerializeField] private CardDatabase database;
    private const string SaveFile = "card_collection.json";

    public static event Action OnCollectionChanged;

    [System.Serializable]
    private class SaveEntry
    {
        public string cardName;
        public int count;
    }

    [System.Serializable]
    private class SaveData
    {
        public List<SaveEntry> entries = new();
    }

    public void AddCard(CardData card)
    {
        if (card == null) return;
        if (collection.ContainsKey(card)) collection[card]++;
        else collection[card] = 1;
        SaveToFile();
        OnCollectionChanged?.Invoke();
    }

    public IReadOnlyDictionary<CardData, int> GetAllCards() => collection;

    private void Awake()
    {
        LoadFromFile();
    }

    private void SaveToFile()
    {
        SaveData data = new();
        foreach (var pair in collection)
        {
            data.entries.Add(new SaveEntry
            {
                cardName = pair.Key.cardName,
                count = pair.Value
            });
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFile), json);
    }

    private void LoadFromFile()
    {
        collection.Clear();
        string path = Path.Combine(Application.persistentDataPath, SaveFile);
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        foreach (var entry in data.entries)
        {
            CardData card = database.cards.Find(c => c.cardName == entry.cardName);
            if (card != null)
            {
                collection[card] = entry.count;
            }
        }
    }
}
