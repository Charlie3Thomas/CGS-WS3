using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CTModifiers
{
    public CTModifiers( float _total_money_modifier,
                        float _total_science_modifer,
                        float _total_food_modifier,
                        float _total_safety_modifier,
                        float _total_flat_money,
                        float _total_flat_science,
                        float _total_awareness_modifier ) 
    { 
        money_modifier = _total_money_modifier;
        science_modifier = _total_science_modifer;
        food_modifier = _total_food_modifier;
        safety_modifier = _total_safety_modifier;
        flat_money = _total_flat_money;
        flat_science = _total_flat_science;
        awareness_modifier = _total_awareness_modifier;
    }

    public readonly float money_modifier { get; }
    public readonly float science_modifier { get; }
    public readonly float food_modifier { get; }
    public readonly float safety_modifier { get; }
    public readonly float flat_money { get; }
    public readonly float flat_science { get; }
    public readonly float awareness_modifier { get; }
}
