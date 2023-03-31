using CT.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Technologies
{
    public static class DataSheet
    {
        public static uint turns_number;

        public static int starting_money = 10000;
        public static int starting_science = 10000;
        public static int starting_food = 10000;
        public static int starting_population = 500;

        // Technology cost to resources
        private static Dictionary<CTTechnologies, CTCost> technology_price;

        // Disaster cost to resources
        private static Dictionary<CTDisasters, CTCost> disaster_impact;

        // Money, Science, Food, Population costs per tern for factions
        // Positive numbers show the cost
        // Negative numbers show production
        // A negative cost is a boon
        public static CTCost worker_net     = new CTCost(-3, 0, 1, 0);
        public static CTCost scientist_net  = new CTCost(0, -1, 1, 0);
        public static CTCost planners_net   = new CTCost(0, 5, 1, 0);
        public static CTCost farmers_net    = new CTCost(0, 0, -1, 0);


        public static CTCost GetTechPrice(CTTechnologies _tech, in CTYearData _year_data)
        {
            // Price scaling functionality?

            //return technology_price[_tech];
            return new CTCost(500, 500, 0, 0); // Placeholder cost
        }

        public static CTCost GetDisasterImpact(CTDisasters _disaster, in CTYearData _year_data)
        {
            //return disaster_impact[_disaster];
            return new CTCost(0, 0, 0, 100); // Placeholder cost 100 pop
        }
    }

    public enum CTTechnologies
    {
        // Placeholder
        Spoons,
        Forks,
        Knives,
        Bowls,
        Plates,
        Cups,
        Mugs,
        Tables,
        Chairs
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
}