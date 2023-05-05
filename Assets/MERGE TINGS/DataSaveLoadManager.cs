//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using Unity.Services.Authentication;
//using System.Threading.Tasks;
//using Unity.Services.Core;
//using Unity.Services.CloudSave;

//[Serializable]
//public class ResourceData
//{
//    public Turn turn;
//    public int total_population;
//    public int currency;
//    public int researchPoints;
//    public int food;
//    public int safety;
//    public List<Turn> turnList;
//}
//[Serializable]
//public class DisasterData
//{
//    public int numOfDisasters;
//    public List<Disaster> disasterList;
//}
//[Serializable]
//public class PolicyData
//{
//    public List<CTPolicyCard> currentPolicies;
//    public CTPolicyCard currentSelectedPolicy;
//    public List<CTPolicyCard> policyList;
//    public HashSet<string> finalChoices;
//}
//[Serializable]
//public class PlayerData
//{
//    public ResourceData resources;
//    public DisasterData disasters;
//    public PolicyData policies;
//}

//public class DataSaveLoadManager : MonoBehaviour
//{
//    private ResourceManager resourceManager;
//    private DisasterManager disasterManager;
//    private PolicyManager policyManager;

//    private const string resourceKey = "RESOURCE";
//    private const string disasterKey = "DISASTER";
//    private const string policyKey = "POLICY";
//    private const string playerDataKey = "PLAYER_DATA";

//    private ResourceData resourceData;
//    private DisasterData disasterData;
//    private PolicyData policyData;

    
//    void Start()
//    {
//        resourceManager = ResourceManager.instance;
//        disasterManager = DisasterManager.instance;
//        policyManager = PolicyManager.instance;
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//            Save();
//        else if (Input.GetKeyDown(KeyCode.RightControl))
//            Load();
//    }

//    public void Save()
//    {
//        saveResource();
//        saveDisaster();
//        savePolicy();
//        savePlayerData();
//    }

//    public void Load()
//    {
//        loadPlayerData();
//    }

//    private void saveResource()
//    {
//        resourceData = new ResourceData();
//        resourceData.turn = resourceManager.current_turn;
//        resourceData.total_population = resourceManager.current_total_population;
//        resourceData.researchPoints = resourceManager.current_researchPoints;
//        resourceData.food = resourceManager.current_food;
//        resourceData.safety = resourceManager.current_safety;
//        resourceData.currency = resourceManager.current_currency;
//        resourceData.turnList = resourceManager.turnList;

//        string s = JsonUtility.ToJson(resourceData);
//        PlayerPrefs.SetString(resourceKey, s);

//        Debug.Log("Resources Saved");
//    }

//    private void loadResources()
//    {
//        string s = PlayerPrefs.GetString(resourceKey);
//        resourceData = JsonUtility.FromJson<ResourceData>(s);
//        resourceManager.current_turn = resourceData.turn;
//        resourceManager.current_total_population = resourceData.total_population;
//        resourceManager.current_researchPoints = resourceData.researchPoints;
//        resourceManager.current_food = resourceData.food;
//        resourceManager.current_safety = resourceData.safety;
//        resourceManager.current_currency = resourceData.currency;
//        resourceManager.turnList = resourceData.turnList;
//        Debug.Log("Resource Loaded :: " + s);
//    }

//    private void saveDisaster()
//    {
//        disasterData = new DisasterData();
//        disasterData.numOfDisasters = disasterManager.numOfDisasters;
//        disasterData.disasterList = disasterManager.disasterList;
//        string s = JsonUtility.ToJson(disasterData);
//        PlayerPrefs.SetString(disasterKey, s);
//    }
//    private void loadDisaster()
//    {
//        string s = PlayerPrefs.GetString(disasterKey);
//        disasterData = JsonUtility.FromJson<DisasterData>(s);
//        disasterManager.numOfDisasters = disasterData.numOfDisasters;
//        disasterManager.disasterList = disasterData.disasterList;
//        Debug.Log("Disaster Loaded :: " + s);
//    }
//    private void savePolicy()
//    {
//        Debug.LogError("DataSaveLoadManager.savePolicy(): Container types have changed since last merge, this will require a fix!");
//        PolicyData policy = new PolicyData();
//        //policy.currentPolicies = policyManager.currentPolicies;
//        //policy.currentSelectedPolicy = policyManager.currentSelectedPolicy;
//        //policy.policyList = policyManager.policyList;
//        //policy.finalChoices = policyManager.finalChoices;
//        string s = JsonUtility.ToJson(policy);
//        PlayerPrefs.SetString(policyKey, s);
//    }

//    private void loadPolicy()
//    {
//        Debug.LogError("DataSaveLoadManager.loadPolicy(): Container types have changed since last merge, this will require a fix!");
//        string s = PlayerPrefs.GetString(policyKey);
//        PolicyData policy = JsonUtility.FromJson<PolicyData>(s);
//        //policyManager.currentPolicies = policy.currentPolicies;
//        //policyManager.currentSelectedPolicy = policy.currentSelectedPolicy;
//        //policyManager.policyList = policy.policyList;
//        //policyManager.finalChoices = policy.finalChoices;
//        Debug.Log("Policy Loaded :: " + s);
//    }

//    private async void savePlayerData()
//    {
//        PlayerData data = new PlayerData();
//        data.resources = resourceData;
//        data.disasters = disasterData;
//        data.policies = policyData;
//        string s = JsonUtility.ToJson(data);
//        PlayerPrefs.SetString(playerDataKey, s);
//        Debug.Log("Player Data :: " + s);

//        await ForceSaveSingleData(playerDataKey, s);
//    }

//    private async void loadPlayerData()
//    {
//        //string s = PlayerPrefs.GetString(playerDataKey);
//        PlayerData data = await RetrieveSpecificData<PlayerData>(playerDataKey);
//        resourceData = data.resources;
//        disasterData = data.disasters;
//        policyData = data.policies;
//        Debug.Log("DATA RECEIVED :: " + resourceData.total_population);
//    }

//    private async Task ForceSaveSingleData(string key, string value)
//    {
//        try
//        {
//            Dictionary<string, object> oneElement = new Dictionary<string, object>();

//            // It's a text input field, but let's see if you actually entered a number.
//            if (Int32.TryParse(value, out int wholeNumber))
//            {
//                oneElement.Add(key, wholeNumber);
//            }
//            else if (Single.TryParse(value, out float fractionalNumber))
//            {
//                oneElement.Add(key, fractionalNumber);
//            }
//            else
//            {
//                oneElement.Add(key, value);
//            }

//            await CloudSaveService.Instance.Data.ForceSaveAsync(oneElement);

//            Debug.Log($"Successfully saved {key}:{value}");
//        }
//        catch (CloudSaveValidationException e)
//        {
//            Debug.LogError(e);
//        }
//        catch (CloudSaveRateLimitedException e)
//        {
//            Debug.LogError(e);
//        }
//        catch (CloudSaveException e)
//        {
//            Debug.LogError(e);
//        }
//    }

//    private async Task<T> RetrieveSpecificData<T>(string key)
//    {
//        try
//        {
//            var results = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });

//            if (results.TryGetValue(key, out string value))
//            {
//                return JsonUtility.FromJson<T>(value);
//            }
//            else
//            {
//                Debug.Log($"There is no such key as {key}!");
//            }
//        }
//        catch (CloudSaveValidationException e)
//        {
//            Debug.LogError(e);
//        }
//        catch (CloudSaveRateLimitedException e)
//        {
//            Debug.LogError(e);
//        }
//        catch (CloudSaveException e)
//        {
//            Debug.LogError(e);
//        }

//        return default;
//    }
//}
