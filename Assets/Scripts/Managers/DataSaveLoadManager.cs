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
    public Dictionary<int, DisasterEnum.type> disaster_timeline;
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
public class YearSaveData
{
    public Dictionary<int, bool> changed_years;
    public int earliest_year;
    public int latest_year;
    public int current_year;
}
[Serializable]
public class FactionData
{
    public Dictionary<int, Dictionary<FactionEnum.type, int>> number_data;
    public Dictionary<int, Dictionary<FactionEnum.type, int>> happiness_data;
    public Dictionary<int, Dictionary<FactionEnum.type, int>> budget_data;
}
[Serializable]
public class PlayerData
{
    public ResourceData resources;
    public DisasterData disasters;
    public PolicyData policies;
    public YearSaveData yearData;
    public FactionData factionData;
}

public class DataSaveLoadManager : MonoBehaviour
{
    private ResourceManager resourceManager;
    private DisasterManager disasterManager;
    private PolicyManager policyManager;
    private YearData yearData;
    private DisasterTimelineData disasterTimelineData;
    private FactionNumberData factionNumberData;
    private FactionHappinessData factionHappinessData;
    private FactionBudgetData factionBudgetData;

    private const string playerDataKey = "PLAYER_DATA";

    private ResourceData resourceData;
    private DisasterData disasterData;
    private PolicyData policyData;
    private PlayerData playerData;
    private YearSaveData yearSaveData;
    private FactionData factionData;

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
        yearData = YearData._INSTANCE;
        disasterTimelineData = DisasterTimelineData._INSTANCE;
        factionNumberData = FactionNumberData._INSTANCE;
        factionHappinessData = FactionHappinessData._INSTANCE;
        factionBudgetData = FactionBudgetData._INSTANCE;
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
        saveYearData();
        saveFactionData();
        savePlayerData();
    }

    public void Load()
    {
        loadPlayerData();
        
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

        //string s = JsonConvert.SerializeObject(resourceData);
        //PlayerPrefs.SetString(resourceKey, s);

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
        disasterData.disaster_timeline = disasterTimelineData.disaster_timeline;
        //string s = JsonUtility.ToJson(disasterData);
        //PlayerPrefs.SetString(disasterKey, s);
    }
    private void loadDisaster()
    {
        disasterData = playerData.disasters;
        disasterManager.numOfDisasters = disasterData.numOfDisasters;
        disasterManager.disasterList = disasterData.disasterList;
        disasterTimelineData.disaster_timeline = disasterData.disaster_timeline;
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

    private void saveYearData()
    {
        yearSaveData = new YearSaveData();
        yearSaveData.changed_years = yearData.changed_years;
        yearSaveData.current_year = yearData.current_year;
        yearSaveData.earliest_year = yearData.earliest_year;
        yearSaveData.latest_year = yearData.latest_year;
    }

    private void loadYearData()
    {
        yearSaveData = playerData.yearData;
        yearData.current_year = yearSaveData.current_year;
        yearData.latest_year = yearSaveData.latest_year;
        yearData.earliest_year = yearSaveData.earliest_year;
        yearData.changed_years = yearSaveData.changed_years;
    }

    private void saveFactionData()
    {
        factionData = new FactionData();
        factionData.number_data = factionNumberData.number_data;
        factionData.happiness_data = factionHappinessData.happiness_data;
        factionData.budget_data = factionBudgetData.budget_data;
    }

    private void loadFactionData()
    {
        factionData = playerData.factionData;
        factionNumberData.number_data = factionData.number_data;
        factionHappinessData.happiness_data = factionData.happiness_data;
        factionBudgetData.budget_data = factionData.budget_data;
    }

    private async void savePlayerData()
    {
        playerData = new PlayerData();
        playerData.resources = resourceData;
        playerData.disasters = disasterData;
        playerData.policies = policyData;
        playerData.yearData = yearSaveData;
        playerData.factionData = factionData;
        string s = JsonConvert.SerializeObject(playerData);
        PlayerPrefs.SetString(playerDataKey, s);
        Debug.Log($"SAVING DATA : {s}");
        writeToFile(s);
        await ForceSaveSingleData(playerDataKey, s);
    }

    private async void loadPlayerData()
    {
        playerData = await RetrieveSpecificData<PlayerData>(playerDataKey);
        //Debug.Log("DATA RECEIVED :: " + playerData.resources.total_population);
        
        resourceData = playerData.resources;
        disasterData = playerData.disasters;
        policyData = playerData.policies;

        loadResources();
        loadDisaster();
        loadPolicy();
        loadYearData();
        loadFactionData();
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
                return JsonConvert.DeserializeObject<T>(value);
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
        string path = Application.dataPath + "/StreamingAssets/";
        StreamWriter sw = new StreamWriter(path + "saveData.txt");
        try
        {
            sw.Write(str);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("Error opening or writing file saveData.txt" + e.Message);
        }
    }
}
