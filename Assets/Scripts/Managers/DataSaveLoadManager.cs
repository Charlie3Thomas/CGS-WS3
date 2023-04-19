using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using Newtonsoft.Json;
using System.IO;
using CT.Data.Changes;
using CT;

[Serializable]
public class PlayerData
{
    public List<CTChange>[] user_changes;
    public List<CTChange>[] game_changes;
}

public class DataSaveLoadManager : MonoBehaviour
{
    private GameManager gameManager;
    private const string playerDataKey = "PLAYER_DATA";
    private PlayerData playerData;
    private JsonSerializerSettings settings;

    private static DataSaveLoadManager _instance;
    public static DataSaveLoadManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null || _instance != this)
            _instance = this;
    }

    void Start()
    {
        gameManager = GameManager._INSTANCE;
        settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Save();
        //else if (Input.GetKeyDown(KeyCode.RightControl))
        //    Load();
    }

    public void Save()
    {
        savePlayerData();
    }

    public void Load()
    {
        loadPlayerData();
    }

    private async void savePlayerData()
    {
        playerData = new PlayerData();
        playerData.user_changes = gameManager.UserChanges;
        playerData.game_changes = gameManager.GameChanges;
        string s = JsonConvert.SerializeObject(playerData, settings);
        PlayerPrefs.SetString(playerDataKey, s);
        Debug.Log($"SAVING DATA : {s}");
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            writeToFile(s);
        await ForceSaveSingleData(playerDataKey, s);
    }

    private async void loadPlayerData()
    {
        playerData = await RetrieveSpecificData<PlayerData>(playerDataKey);
        Debug.Log("DATA LOADED FROM CLOUD");
        gameManager.LoadData(playerData.user_changes, playerData.game_changes);
    }

    private async Task ForceSaveSingleData(string key, string value)
    {
        try
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add(key, value);
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log($"Successfully saved {key}:{value}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    private async Task<T> RetrieveSpecificData<T>(string key)
    {
        try
        {
            var results = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });

            if (results.TryGetValue(key, out string value))
            {
                return JsonConvert.DeserializeObject<T>(value,settings);
            }
            else
            {
                Debug.Log($"Key not found : {key}!");
            }
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return default;
    }
    private void writeToFile(string str)
    {
        string path = Application.dataPath + "/StreamingAssets/saveData.txt";
        try
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(str);
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("Error opening or writing file saveData.txt" + e.Message);
        }
    }
}
