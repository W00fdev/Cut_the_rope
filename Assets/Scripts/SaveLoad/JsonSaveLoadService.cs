using UnityEngine;

public class JsonSaveLoadService : ISaveLoadService
{
    public bool HasChanges(ISavable objectToCheck)
    {
        string json = JsonUtility.ToJson(objectToCheck);
        return PlayerPrefs.GetString(objectToCheck.SaveKey) != json;
    }

    public void Save(ISavable objectToSave)
    {
        string json = JsonUtility.ToJson(objectToSave);
        PlayerPrefs.SetString(objectToSave.SaveKey, json);
    }

    public void Load(ISavable objectToOverwrite)
    {
        string json = PlayerPrefs.GetString(objectToOverwrite.SaveKey);
        if (!string.IsNullOrWhiteSpace(json))
            JsonUtility.FromJsonOverwrite(json, objectToOverwrite);
    }
}