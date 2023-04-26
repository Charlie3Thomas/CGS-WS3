using System.Collections.Generic;

namespace CT.Lookup
{
    using Data;
    using FMOD;
    using System;

    public static class DataSheet
    {
        // Disaster cost to resources
        private static Dictionary<CTDisasters, CTCost> disaster_impact;


        #region Base Values
        public static readonly uint turn_steps = 5;
        public static readonly uint starting_year = 1900;
        public static readonly uint end_year = 2100;
        public static readonly uint turns_number = (end_year - starting_year) / turn_steps;
        public static readonly int starting_money = 15000;
        public static readonly int starting_science = 100000;
        public static readonly int starting_food = 100000;
        public static readonly float year_change_awareness_rate = 0.05f;
        public static readonly float year_override_awareness_rate = 0.15f;

        public static readonly int starting_population = 500; 
        public static readonly float starvation_rate = 0.40f;
        public static readonly float food_surplus_population_gain = 0.10f;

        public static readonly float starting_workers = 0.3f;
        public static readonly float starting_scientists = 0.2f;
        public static readonly float starting_farmers = 0.3f;
        public static readonly float starting_planners = 0.2f;

        public static readonly float policy_card_min_scale = 25.0f;
        public static readonly float policy_card_max_scale = 500.0f;
        #endregion


        // Money, Science, Food, Population costs per tern for factions
        // Positive numbers show the cost
        // Negative numbers show production
        // A negative cost is a boon
        public static readonly CTCost worker_net     = new CTCost(-3, 0, 1, 0);
        public static readonly CTCost scientist_net  = new CTCost(0, -1, 1, 0);
        public static readonly CTCost planners_net   = new CTCost(0, 5, 1, 0);
        public static readonly CTCost farmers_net    = new CTCost(0, 0, -1, 0);
        public static readonly CTCost unemployed_net = new CTCost(1, 0, 1, 0);


        public static CTCost GetTechPrice(CTTechnologies _tech, in CTYearData _year_data)
        {
            // Price scaling functionality?

            return technology_price[_tech];
            //return new CTCost(500, 500, 0, 0); // Placeholder cost
        }

        public static CTCost GetDisasterImpact(CTDisasters _disaster, in CTYearData _year_data)
        {
            //return disaster_impact[_disaster];
            //UnityEngine.Debug.Log("Disaster");

            switch (_disaster)
            {
                case (CTDisasters.Earthquake):
                    // Impacts all
                    break;

                case (CTDisasters.Volcano):
                    // Impacts 
                    break;

                case (CTDisasters.Tornado):

                    break;

                case (CTDisasters.Tsunami):

                    break;
            }


            return new CTCost(0, 0, 0, 0); // Placeholder cost 100 pop
        }

        // Technology cost lookup table
        public static readonly Dictionary<CTTechnologies, CTCost> technology_price = new Dictionary<CTTechnologies, CTCost>()
        {
            //                                           Cost : Money | Science | Food | Population
            [CTTechnologies.Banking]                = new CTCost( 5000,   500,      0,      0 ),
            [CTTechnologies.Laboratory]             = new CTCost( 5000,  1000,      0,      0 ),
            [CTTechnologies.TownPlanning]           = new CTCost(10000,  1000,      0,      0 ),
            [CTTechnologies.Granary]                = new CTCost( 1000,   500,      0,      0 ),
            [CTTechnologies.Lockers]                = new CTCost( 1000,   200,      0,      0 ),
            [CTTechnologies.DamageMitigation1]      = new CTCost( 1000,   250,      0,      0 ),
            [CTTechnologies.DamageMitigation2]      = new CTCost( 1000,   250,      0,      0 ),
            [CTTechnologies.Stockpiling]            = new CTCost( 2000,   500,      0,      0 ),
            [CTTechnologies.Revenue]                = new CTCost(    0,   500,      0,      0 ),
            [CTTechnologies.Innovation]             = new CTCost( 1000,   250,      0,      0 ),
            [CTTechnologies.Construction1]          = new CTCost( 1500,   500,      0,      0 ),
            [CTTechnologies.Construction2]          = new CTCost( 2000,   750,      0,      0 ),
            [CTTechnologies.SuperRevenue]           = new CTCost(    0,  1000,      0,      0 ),
            [CTTechnologies.SuperLockers]           = new CTCost( 2000,   400,      0,      0 ),
            [CTTechnologies.Inspiration]            = new CTCost( 2500,  1000,      0,      0 ),
            [CTTechnologies.Zoning]                 = new CTCost( 2500,  1000,      0,      0 ),
            [CTTechnologies.SuperStockpiling]       = new CTCost( 1500,   500,      0,      0 ),
            [CTTechnologies.FinanceManagement]      = new CTCost( 500,    750,      0,      0 ),
            [CTTechnologies.AlarmSystem]            = new CTCost( 3000,  1000,      0,      0 ),
            [CTTechnologies.ResearchCentre]         = new CTCost( 5000,   100,      0,      0 ),
            [CTTechnologies.StoreHouse]             = new CTCost( 1750,   750,      0,      0 ),
            [CTTechnologies.SafeHouse]              = new CTCost( 3500,  1500,      0,      0 ),
            [CTTechnologies.RiskAssessment]         = new CTCost( 5000,  1000,      0,      0 ),
            [CTTechnologies.PopulationAssessment]   = new CTCost( 5000,  1000,      0,      0 ),
            [CTTechnologies.Digitization]           = new CTCost( 1000,  1250,      0,      0 ),
            [CTTechnologies.CropRotation]           = new CTCost( 3000,   750,      0,      0 ),
            [CTTechnologies.Fertilization]          = new CTCost( 2500,   500,      0,      0 ),
            [CTTechnologies.Alchemy]                = new CTCost( 7500,     0,      0,      0 ),
            [CTTechnologies.Seismometer]            = new CTCost( 3000,  1000,      0,      0 ),
            [CTTechnologies.Automation]             = new CTCost( 3000,  1000,      0,      0 ),
            [CTTechnologies.FoodTechnology]         = new CTCost( 3000,  1000,      0,      0 ),
            [CTTechnologies.InterestBoost]          = new CTCost( 1000,  2000,      0,      0 ),
            [CTTechnologies.DigitalSeismometer]     = new CTCost( 3500,  1250,      0,      0 ),
            [CTTechnologies.Seismography]           = new CTCost( 3500,  1250,      0,      0 ),
            [CTTechnologies.DigitalCurrency]        = new CTCost( 3000,  1000,      0,      0 ),
            [CTTechnologies.DigitalPayment]         = new CTCost(    0,  1000,      0,      0 ),
            [CTTechnologies.Blueprints]             = new CTCost( 4000,  1000,      0,      0 ),
            [CTTechnologies.Greenprints]            = new CTCost( 4000,  1000,      0,      0 ),
            [CTTechnologies.NetBanking]             = new CTCost( 5000,  2000,      0,      0 ),
            [CTTechnologies.ResearchDome]           = new CTCost( 5000,     0,      0,      0 ),
            [CTTechnologies.RapidRelief]            = new CTCost( 5000,  1500,      0,      0 ),
            [CTTechnologies.SeismicInvisibility]    = new CTCost( 5000,  1500,      0,      0 ),
            [CTTechnologies.FloodManagement]        = new CTCost( 5000,  1500,      0,      0 ),
            [CTTechnologies.Robotics]               = new CTCost( 5000,  3000,      0,      0 ),
            [CTTechnologies.Bluffing]               = new CTCost( 5000,  3000,      0,      0 ),
            [CTTechnologies.Marketplace1]           = new CTCost( 7500,  3500,      0,      0 ),
            [CTTechnologies.Marketplace2]           = new CTCost( 7500,  3500,      0,      0 ),
            [CTTechnologies.SafetyRating]           = new CTCost( 7500,  3500,      0,      0 ),
            [CTTechnologies.Warehousing]            = new CTCost( 5000,  2500,      0,      0 ),
            [CTTechnologies.TechnologyCentre]       = new CTCost( 5000,  2500,      0,      0 ),
            [CTTechnologies.LifelineSupport]        = new CTCost( 5000,  2500,      0,      0 ),
            [CTTechnologies.SupremeFinancing]       = new CTCost(    0,  2000,      0,      0 ),
            [CTTechnologies.SupremeChemistry]       = new CTCost( 5000,     0,      0,      0 ),
            [CTTechnologies.SupremeStealth]         = new CTCost(10000,  5000,      0,      0 ),
            [CTTechnologies.SuperCivilization]      = new CTCost(25000, 10000,      0,      0 ),
            [CTTechnologies.ArtificialIntelligence] = new CTCost(25000, 10000,      0,      0 ),
            [CTTechnologies.InhumanForesight]       = new CTCost(25000, 10000,      0,      0 ),
            [CTTechnologies.RobotWorkers]           = new CTCost(25000, 10000,      0,      0 ),
            [CTTechnologies.AlienMastery]           = new CTCost(25000, 10000,      0,      0 ),
            [CTTechnologies.AlienSuperiority]       = new CTCost(27500, 11000,      0,      0 ),
            [CTTechnologies.AlienDomination]        = new CTCost(30000, 12000,      0,      0 ),
            [CTTechnologies.MemoryFlash]            = new CTCost(50000, 15000,      0,      0 )
        };

        // Tech buffs lookup table
        public static readonly Dictionary<CTTechnologies, BuffsNerfs> technology_buffs = new Dictionary<CTTechnologies, BuffsNerfs>()
        {
            [CTTechnologies.Banking]                = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.Laboratory]             = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.TownPlanning]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.Granary]                = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.Lockers]                = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_CAPACITY },                                  new List<float>() { 500.0f }),
            [CTTechnologies.DamageMitigation1]      = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 1.0f }),
            [CTTechnologies.DamageMitigation2]      = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 1.0f }),
            [CTTechnologies.Stockpiling]            = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.FOOD_RESERVES },                                   new List<float>() { 100.0f }),
            [CTTechnologies.Revenue]                = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_BOOST },                                     new List<float>() { 2500.0f}),
            [CTTechnologies.Innovation]             = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_CAPACITY },                                new List<float>() { 250.0f }),
            [CTTechnologies.Construction1]          = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 1.0f }),
            [CTTechnologies.Construction2]          = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 2.0f }),
            [CTTechnologies.SuperRevenue]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_BOOST },                                     new List<float>() { 2000.0f }),
            [CTTechnologies.SuperLockers]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_CAPACITY },                                  new List<float>() { 1000.0f }),
            [CTTechnologies.Inspiration]            = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_CAPACITY },                                new List<float>() { 500.0f }),
            [CTTechnologies.Zoning]                 = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 2.0f }),
            [CTTechnologies.SuperStockpiling]       = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.FOOD_RESERVES },                                   new List<float>() { 250.0f }),
            [CTTechnologies.FinanceManagement]      = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_BOOST, BuffsNerfsType.MONEY_CAPACITY },      new List<float>() { 2500.0f, 2500.0f }),
            [CTTechnologies.AlarmSystem]            = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 3.0f }),
            [CTTechnologies.ResearchCentre]         = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_CAPACITY, BuffsNerfsType.SCIENCE_BOOST },  new List<float>() { 500.0f, 500.0f }),
            [CTTechnologies.StoreHouse]             = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.FOOD_RESERVES },                                   new List<float>() { 300.0f }),
            [CTTechnologies.SafeHouse]              = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 4.0f }),
            [CTTechnologies.RiskAssessment]         = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.PopulationAssessment]   = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.Digitization]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_BOOST, BuffsNerfsType.MONEY_CAPACITY },      new List<float>() { 3500.0f, 3500.0f }),
            [CTTechnologies.CropRotation]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.01f }),
            [CTTechnologies.Fertilization]          = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.FOOD_RESERVES },                                   new List<float>() { 500.0f }),
            [CTTechnologies.Alchemy]                = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_CAPACITY, BuffsNerfsType.SCIENCE_BOOST },  new List<float>() { 750.0f, 750.0f }),
            [CTTechnologies.Seismometer]            = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.01f }),
            [CTTechnologies.Automation]             = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.02f }),
            [CTTechnologies.FoodTechnology]         = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.FOOD_RESERVES },                                   new List<float>() { 1000.0f }),
            [CTTechnologies.InterestBoost]          = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_BOOST },                                     new List<float>() { 5000.0f }),
            [CTTechnologies.DigitalSeismometer]     = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.025f }),
            [CTTechnologies.Seismography]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.025f }),
            [CTTechnologies.DigitalCurrency]        = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_CAPACITY },                                  new List<float>() { 5000.0f }),
            [CTTechnologies.DigitalPayment]         = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_BOOST },                                     new List<float>() { 5000.0f }),
            [CTTechnologies.Blueprints]             = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 5.0f }),
            [CTTechnologies.Greenprints]            = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.FOOD_RESERVES },                                   new List<float>() { 1250.0f }),
            [CTTechnologies.NetBanking]             = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_CAPACITY },                                  new List<float>() { 7500.0f }),
            [CTTechnologies.ResearchDome]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_BOOST, BuffsNerfsType.SCIENCE_CAPACITY },  new List<float>() { 2000.0f, 1000.0f }),
            [CTTechnologies.RapidRelief]            = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.03f }),
            [CTTechnologies.SeismicInvisibility]    = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.03f }),
            [CTTechnologies.FloodManagement]        = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.03f }),
            [CTTechnologies.Robotics]               = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_UPKEEP },                                  new List<float>() { -1.0f }),
            [CTTechnologies.Bluffing]               = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.AWARENESS_FACTOR },                                new List<float>() { 0.05f }),
            [CTTechnologies.Marketplace1]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.Marketplace2]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.SafetyRating]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
            [CTTechnologies.Warehousing]            = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_CAPACITY },                                  new List<float>() { 10000.0f }),
            [CTTechnologies.TechnologyCentre]       = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_CAPACITY },                                new List<float>() { 5000.0f }),
            [CTTechnologies.LifelineSupport]        = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SAFETY_FACTOR },                                   new List<float>() { 8.0f }),
            [CTTechnologies.SupremeFinancing]       = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_BOOST },                                     new List<float>() { 5000.0f }),
            [CTTechnologies.SupremeChemistry]       = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_BOOST },                                   new List<float>() { 3000.0f }),
            [CTTechnologies.SupremeStealth]         = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.AWARENESS_FACTOR },                                new List<float>() {0.075f }),
            [CTTechnologies.SuperCivilization]      = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_GAIN },                                    new List<float>() { 1.0f }),
            [CTTechnologies.ArtificialIntelligence] = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_UPKEEP },                                  new List<float>() { -2.0f }),
            [CTTechnologies.InhumanForesight]       = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.RESOURCE_FACTOR },                                 new List<float>() { 0.07f }),
            [CTTechnologies.RobotWorkers]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_GAIN },                                      new List<float>() {1.0f }),
            [CTTechnologies.AlienMastery]           = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.MONEY_CAPACITY },                                  new List<float>() { float.MaxValue }),
            [CTTechnologies.AlienSuperiority]       = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.SCIENCE_CAPACITY },                                new List<float>() { float.MaxValue }),
            [CTTechnologies.AlienDomination]        = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.FOOD_RESERVES },                                   new List<float>() { 10000.0f }),
            [CTTechnologies.MemoryFlash]            = new BuffsNerfs(new List<BuffsNerfsType>() { BuffsNerfsType.CUSTOM },                                          new List<float>() { 0.0f }),
        };

        public static readonly string[] policy_buff_suffixes = new string[]
        {
            "Bonus",
            "Reward", 
            "Gain",
            "Boost",
            "Increase",
            "Enhancement",
            "Raise",
            "Windfall",
            "Surge"
        };

        public static readonly string[] policy_nerf_suffixes = new string[]
        {
            "Loss",
            "Penalty",
            "Misfortune",
            "Decrease",
            "Deprivation",
            "Waste",
            "Diminishment"
        };

        public static readonly string[] policy_degree_prefixes = new string[]
        {
            "Minor",
            "Moderate",
            "Major",
            "Extreme"
        };

        public static readonly Dictionary<BuffsNerfsType, string> policy_type = new Dictionary<BuffsNerfsType, string>()
        {
            [BuffsNerfsType.MONEY_GAIN]         = "Wealth",
            [BuffsNerfsType.FOOD_GAIN]          = "Agriculture",
            [BuffsNerfsType.SCIENCE_GAIN]       = "Research",
            [BuffsNerfsType.MONEY_UPKEEP]       = "Wealth",
            [BuffsNerfsType.FOOD_UPKEEP]        = "Agriculture",
            [BuffsNerfsType.SCIENCE_UPKEEP]     = "Research",
            [BuffsNerfsType.SAFETY_FACTOR]      = "Security",
            [BuffsNerfsType.MONEY_BOOST]        = "Wealth",
            [BuffsNerfsType.SCIENCE_BOOST]      = "Research",
            [BuffsNerfsType.AWARENESS_FACTOR]   = "Stealth",
            [BuffsNerfsType.RESOURCE_FACTOR]    = "INVALID BUFF/NERF TYPE",
            [BuffsNerfsType.FOOD_RESERVES]      = "INVALID BUFF/NERF TYPE",
            [BuffsNerfsType.MONEY_CAPACITY]     = "INVALID BUFF/NERF TYPE",
            [BuffsNerfsType.SCIENCE_CAPACITY]   = "INVALID BUFF/NERF TYPE",
            [BuffsNerfsType.CUSTOM]             = "INVALID BUFF/NERF TYPE"
        };
    }

    public enum CTTechnologies
    {
        Banking,
        Laboratory,
        TownPlanning,
        Granary,
        Lockers,
        DamageMitigation1,
        DamageMitigation2,
        Stockpiling,
        Revenue,
        Innovation,
        Construction1,
        Construction2,
        SuperRevenue,
        SuperLockers,
        Inspiration,
        Zoning,
        SuperStockpiling,
        FinanceManagement,
        AlarmSystem,
        ResearchCentre,
        StoreHouse,
        SafeHouse,
        RiskAssessment,
        PopulationAssessment,
        Digitization,
        CropRotation,
        Fertilization,
        Alchemy,
        Seismometer,
        Automation,
        FoodTechnology,
        InterestBoost,
        DigitalSeismometer,
        Seismography,
        DigitalCurrency,
        DigitalPayment,
        Blueprints,
        Greenprints,
        NetBanking,
        ResearchDome,
        RapidRelief,
        SeismicInvisibility,
        FloodManagement,
        Robotics,
        Bluffing,
        Marketplace1,
        Marketplace2,
        SafetyRating,
        Warehousing,
        TechnologyCentre,
        LifelineSupport,
        SupremeFinancing,
        SupremeChemistry,
        SupremeStealth,
        SuperCivilization,
        ArtificialIntelligence,
        InhumanForesight,
        RobotWorkers,
        AlienMastery,
        AlienSuperiority,
        AlienDomination,
        MemoryFlash
    };

    public enum CTDisasters
    {
        Earthquake,
        Tsunami,
        Volcano,
        Tornado
    };

    public enum CTPolicies
    {
        DrinkMoreWater,
        DrinkLessCoffee,
        GoToBedOnTime,
        GoToGYMMore
    };

    public enum BuffsNerfsType
    {
        MONEY_GAIN,
        FOOD_GAIN,
        SCIENCE_GAIN,
        MONEY_UPKEEP,
        FOOD_UPKEEP,
        SCIENCE_UPKEEP,
        SAFETY_FACTOR,
        MONEY_BOOST,
        SCIENCE_BOOST,
        AWARENESS_FACTOR,
        RESOURCE_FACTOR,
        FOOD_RESERVES,
        MONEY_CAPACITY,
        SCIENCE_CAPACITY,
        CUSTOM
    }
}