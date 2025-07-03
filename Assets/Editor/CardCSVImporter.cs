using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CardCSVImporter
{
    [MenuItem("Tools/Import Cards from CSV")]
    public static void ImportCards()
    {
        string path = EditorUtility.OpenFilePanel("Select Card CSV", "", "csv");
        if (string.IsNullOrEmpty(path)) return;

        string[] lines = File.ReadAllLines(path);
        List<CardData> cardList = new();

        for (int i = 1; i < lines.Length; i++)
        { // skip header
            string[] fields = lines[i].Split(',');

            if (fields.Length < 4) continue;

            CardData card = new()
            {
                cardName = fields[1].Trim(),
                rarity = System.Enum.TryParse(fields[2].Trim(), out Rarity r) ? r : Rarity.Common,
                cardImage = AssetDatabase.LoadAssetAtPath<Sprite>(fields[3].Trim()),
                description = fields[4].Trim()
            };

            cardList.Add(card);
        }

        CardDatabase db = ScriptableObject.CreateInstance<CardDatabase>();
        db.cards = cardList;

        string assetPath = "Assets/Data/CardDatabase.asset";
        Directory.CreateDirectory("Assets/Data");
        AssetDatabase.CreateAsset(db, assetPath);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Import Complete", $"Imported {cardList.Count} cards.", "OK");
    }
}
