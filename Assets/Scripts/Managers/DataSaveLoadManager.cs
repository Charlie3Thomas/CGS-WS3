using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.CloudSave;

[Serializable]
public class ResourceData
{
    public Turn turn;
    public int total_population;
    public int currency;
    public int researchPoints;
    public int food;
    public int safety;
    public List<Turn> turnList;
}
[Serializable]
public class DisasterData
{
    public int numOfDisasters;
    public List<Disaster> disasterList;
}
[Serializable]
public class PolicyData
{
    public List<Policy> currentPolicies;
    public Policy currentSelectedPolicy;
    public List<Policy> policyList;
    public HashSet<string> finalChoices;
}
[Serializable]
public class PlayerData
{
    public ResourceData resources;
    public DisasterData disasters;
    public PolicyData policies;
}

public class DataSaveLoadManager : MonoBehaviour
{
    private ResourceManager resourceManager;
    private DisasterManager disasterManager;
    private PolicyManager policyManager;

    private const string resourceKey = "RESOURCE";
    private const string disasterKey = "DISASTER";
    private const string policyKey = "POLICY";
    private const string playerDataKey = "PLAYER_DATA";

    private ResourceData resourceData;
    private DisasterData disasterData;
    private PolicyData policyData;
    private PlayerData playerData;

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
        resourceManager = ResourceManager.instance;
        disasterManager = DisasterManager.instance;
        policyManager = PolicyManager.instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Save();
        else if (Input.GetKeyDown(KeyCode.RightControl))
            Load();
    }

    public void Save()
    {
        saveResource();
        saveDisaster();
        savePolicy();
        savePlayerData();
    }

    public void Load()
    {
        loadPlayerData();
        loadResources();
        loadDisaster();
        loadPolicy();
    }

    private void saveResource()
    {
        resourceData = new ResourceData();
        resourceData.turn = resourceManager.current_turn;
        resourceData.total_population = resourceManager.current_total_population;
        resourceData.researchPoints = resourceManager.current_researchPoints;
        resourceData.food = resourceManager.current_food;
        resourceData.safety = resourceManager.current_safety;
        resourceData.currency = resourceManager.current_currency;
        resourceData.turnList = resourceManager.turnList;

        string s = JsonUtility.ToJson(resourceData);
        PlayerPrefs.SetString(resourceKey, s);

        Debug.Log("Resources Saved");
    }

    private void loadResources()
    {
        resourceData = playerData.resources;
        resourceManager.current_turn = resourceData.turn;
        resourceManager.current_total_population = resourceData.total_population;
        resourceManager.current_researchPoints = resourceData.researchPoints;
        resourceManager.current_food = resourceData.food;
        resourceManager.current_safety = resourceData.safety;
        resourceManager.current_currency = resourceData.currency;
        resourceManager.turnList = resourceData.turnList;
        Debug.Log("Resouce loaded");
    }

    private void saveDisaster()
    {
        disasterData = new DisasterData();
        disasterData.numOfDisasters = disasterManager.numOfDisasters;
        disasterData.disasterList = disasterManager.disasterList;
        //string s = JsonUtility.ToJson(disasterData);
        //PlayerPrefs.SetString(disasterKey, s);
    }
    private void loadDisaster()
    {
        disasterData = playerData.disasters;
        disasterManager.numOfDisasters = disasterData.numOfDisasters;
        disasterManager.disasterList = disasterData.disasterList;
        Debug.Log("Disaster loaded");
    }
    private void savePolicy()
    {
        policyData = new PolicyData();
        policyData.currentPolicies = policyManager.currentPolicies;
        policyData.currentSelectedPolicy = policyManager.currentSelectedPolicy;
        policyData.policyList = policyManager.policyList;
        policyData.finalChoices = policyManager.finalChoices;
    }

    private void loadPolicy()
    {
        policyData = playerData.policies;
        policyManager.currentPolicies = policyData.currentPolicies;
        policyManager.currentSelectedPolicy = policyData.currentSelectedPolicy;
        policyManager.policyList = policyData.policyList;
        policyManager.finalChoices = policyData.finalChoices;
        Debug.Log("Policy loaded");
    }

    private async void savePlayerData()
    {
        playerData = new PlayerData();
        playerData.resources = resourceData;
        playerData.disasters = disasterData;
        playerData.policies = policyData;
        string s = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(playerDataKey, s);
        await ForceSaveSingleData(playerDataKey, s);
    }

    private async void loadPlayerData()
    {
        playerData = await RetrieveSpecificData<PlayerData>(playerDataKey);
        resourceData = playerData.resources;
        disasterData = playerData.disasters;
        policyData = playerData.policies;
        Debug.Log("DATA RECEIVED :: " + resourceData.total_population);
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
                return JsonUtility.FromJson<T>(value);
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
}
