using UnityEngine;

public static class CardCollectionSaveManager
{
    private const string SaveKey = "CardCollectionSave";

    public static void Save(CardCollection collection)
    {
        string json = JsonUtility.ToJson(collection);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save(); // จำเป็นใน WebGL
        Debug.Log("Card collection saved.");
    }

    public static CardCollection Load()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            Debug.Log("No saved card collection found.");
            return new CardCollection(); // หรือ new object พร้อมค่า default
        }

        string json = PlayerPrefs.GetString(SaveKey);
        Debug.Log("Card collection loaded.");
        return JsonUtility.FromJson<CardCollection>(json);
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        Debug.Log("Card collection cleared.");
    }
}
