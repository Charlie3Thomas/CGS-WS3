using System.Collections.Generic;

namespace CT.Lookup
{
    using Data;
    public static class DataSheet
    {
        // Disaster cost to resources
        private static Dictionary<CTDisasters, CTCost> disaster_impact;


        #region Base Values
        public static uint turns_number = 40;
        public static uint starting_year = 1900;
        public static uint end_year = 2100;
        public static int starting_money = 1000;
        public static int starting_science = 1000;
        public static int starting_food = 1000;
        public static int starting_population = 500;
        public static float starvation_death_rate = 0.90f;
        #endregion


        // Money, Science, Food, Population costs per tern for factions
        // Positive numbers show the cost
        // Negative numbers show production
        // A negative cost is a boon
        public static CTCost worker_net     = new CTCost(-3, 0, 1, 0);
        public static CTCost scientist_net  = new CTCost(0, -1, 1, 0);
        public static CTCost planners_net   = new CTCost(0, 5, 1, 0);
        public static CTCost farmers_net    = new CTCost(0, 0, -1, 0);
        public static CTCost unemployed_net = new CTCost(1, 0, 1, 0);


        public static CTCost GetTechPrice(CTTechnologies _tech, in CTYearData _year_data)
        {
            // Price scaling functionality?

            return technology_price[_tech];
            //return new CTCost(500, 500, 0, 0); // Placeholder cost
        }

        public static CTCost GetDisasterImpact(CTDisasters _disaster, in CTYearData _year_data)
        {
            //return disaster_impact[_disaster];
            return new CTCost(0, 0, 0, 100); // Placeholder cost 100 pop
        }

        // Technology cost lookup table
        private static Dictionary<CTTechnologies, CTCost> technology_price = new Dictionary<CTTechnologies, CTCost>()
        {
            //                                  Cost : Money | Science | Food | Population
            [CTTechnologies.Banking]                = new CTCost( 5000,   500, 0, 0 ),
            [CTTechnologies.Laboratory]             = new CTCost( 5000,  1000, 0, 0 ),
            [CTTechnologies.TownPlanning]           = new CTCost(10000,  1000, 0, 0 ),
            [CTTechnologies.Granary]                = new CTCost( 1000,   500, 0, 0 ),
            [CTTechnologies.Lockers]                = new CTCost( 1000,   200, 0, 0 ),
            [CTTechnologies.DamageMitigation1]      = new CTCost( 1000,   250, 0, 0 ),
            [CTTechnologies.DamageMitigation2]      = new CTCost( 1000,   250, 0, 0 ),
            [CTTechnologies.Stockpiling]            = new CTCost( 2000,   500, 0, 0 ),
            [CTTechnologies.Revenue]                = new CTCost(    0,   500, 0, 0 ),
            [CTTechnologies.Innovation]             = new CTCost( 1000,   250, 0, 0 ),
            [CTTechnologies.Construction1]          = new CTCost( 1500,   500, 0, 0 ),
            [CTTechnologies.Construction2]          = new CTCost( 2000,   750, 0, 0 ),
            [CTTechnologies.SuperRevenue]           = new CTCost(    0,  1000, 0, 0 ),
            [CTTechnologies.SuperLockers]           = new CTCost( 2000,   400, 0, 0 ),
            [CTTechnologies.Inspiration]            = new CTCost( 2500,  1000, 0, 0 ),
            [CTTechnologies.Zoning]                 = new CTCost( 2500,  1000, 0, 0 ),
            [CTTechnologies.SuperStockpiling]       = new CTCost( 1500,   500, 0, 0 ),
            [CTTechnologies.FinanceManagement]      = new CTCost( 500,    750, 0, 0 ),
            [CTTechnologies.AlarmSystem]            = new CTCost( 3000,  1000, 0, 0 ),
            [CTTechnologies.ResearchCentre]         = new CTCost( 5000,   100, 0, 0 ),
            [CTTechnologies.StoreHouse]             = new CTCost( 1750,   750, 0, 0 ),
            [CTTechnologies.SafeHouse]              = new CTCost( 3500,  1500, 0, 0 ),
            [CTTechnologies.RiskAssessment]         = new CTCost( 5000,  1000, 0, 0 ),
            [CTTechnologies.PopulationAssessment]   = new CTCost( 5000,  1000, 0, 0 ),
            [CTTechnologies.Digitization]           = new CTCost( 1000,  1250, 0, 0 ),
            [CTTechnologies.CropRotation]           = new CTCost( 3000,   750, 0, 0 ),
            [CTTechnologies.Fertilization]          = new CTCost( 2500,   500, 0, 0 ),
            [CTTechnologies.Alchemy]                = new CTCost( 7500,     0, 0, 0 ),
            [CTTechnologies.Seismometer]            = new CTCost( 3000,  1000, 0, 0 ),
            [CTTechnologies.Automation]             = new CTCost( 3000,  1000, 0, 0 ),
            [CTTechnologies.FoodTechnology]         = new CTCost( 3000,  1000, 0, 0 ),
            [CTTechnologies.InterestBoost]          = new CTCost( 1000,  2000, 0, 0 ),
            [CTTechnologies.DigitalSeismometer]     = new CTCost( 3500,  1250, 0, 0 ),
            [CTTechnologies.Seismography]           = new CTCost( 3500,  1250, 0, 0 ),
            [CTTechnologies.DigitalCurrency]        = new CTCost( 3000,  1000, 0, 0 ),
            [CTTechnologies.DigitalPayment]         = new CTCost(    0,  1000, 0, 0 ),
            [CTTechnologies.Blueprints]             = new CTCost( 4000,  1000, 0, 0 ),
            [CTTechnologies.Greenprints]            = new CTCost( 4000,  1000, 0, 0 ),
            [CTTechnologies.NetBanking]             = new CTCost( 5000,  2000, 0, 0 ),
            [CTTechnologies.ResearchDome]           = new CTCost( 5000,     0, 0, 0 ),
            [CTTechnologies.RapidRelief]            = new CTCost( 5000,  1500, 0, 0 ),
            [CTTechnologies.SeismicInvisibility]    = new CTCost( 5000,  1500, 0, 0 ),
            [CTTechnologies.FloodManagement]        = new CTCost( 5000,  1500, 0, 0 ),
            [CTTechnologies.Robotics]               = new CTCost( 5000,  3000, 0, 0 ),
            [CTTechnologies.Bluffing]               = new CTCost( 5000,  3000, 0, 0 ),
            [CTTechnologies.Marketplace1]           = new CTCost( 7500,  3500, 0, 0 ),
            [CTTechnologies.Marketplace2]           = new CTCost( 7500,  3500, 0, 0 ),
            [CTTechnologies.SafetyRating]           = new CTCost( 7500,  3500, 0, 0 ),
            [CTTechnologies.Warehousing]            = new CTCost( 5000,  2500, 0, 0 ),
            [CTTechnologies.TechnologyCentre]       = new CTCost( 5000,  2500, 0, 0 ),
            [CTTechnologies.LifelineSupport]        = new CTCost( 5000,  2500, 0, 0 ),
            [CTTechnologies.SupremeFinancing]       = new CTCost(    0,  2000, 0, 0 ),
            [CTTechnologies.SupremeChemistry]       = new CTCost( 5000,     0, 0, 0 ),
            [CTTechnologies.SupremeStealth]         = new CTCost(10000,  5000, 0, 0 ),
            [CTTechnologies.SuperCivilization]      = new CTCost(25000, 10000, 0, 0 ),
            [CTTechnologies.ArtificialIntelligence] = new CTCost(25000, 10000, 0, 0 ),
            [CTTechnologies.InhumanForesight]       = new CTCost(25000, 10000, 0, 0 ),
            [CTTechnologies.RobotWorkers]           = new CTCost(25000, 10000, 0, 0 ),
            [CTTechnologies.AlienMastery]           = new CTCost(25000, 10000, 0, 0 ),
            [CTTechnologies.AlienSuperiority]       = new CTCost(27500, 11000, 0, 0 ),
            [CTTechnologies.AlienDomination]        = new CTCost(30000, 12000, 0, 0 ),
            [CTTechnologies.MemoryFlash]            = new CTCost(50000, 15000, 0, 0 )
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
        // Placeholder
        NoCoffee,
        NoMilk,
        TrainsGone,
        Overslept,
        RanOutOfMoney,
        ForgotToEat
    };

    public enum CTPolicies
    {
        DrinkMoreWater,
        DrinkLessCoffee,
        GoToBedOnTime,
        GoToGYMMore
    };
}