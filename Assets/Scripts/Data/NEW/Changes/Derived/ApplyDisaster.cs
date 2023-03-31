using CT.Data;
using CT.Data.Changes;
using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a template disaster class for testing, which can be made for each disaster type in the game.
/// Each disaster type will inherit from CTChange.
/// This allows for custom ApplyChange methods for each disaster.
/// Each disaster will be mitigated by different technologies and require a different ApplyChange method.
/// </summary>
public class ApplyDisaster : CTChange
{
    public ApplyDisaster() { }

    public ApplyDisaster(CTDisasters _disaster)
    {
        this.disaster = _disaster;
    }

    public CTDisasters disaster;

    public override void ApplyChange(ref CTYearData _year)
    {
        // Look at all modifiers to disaster impact and apply them to the base disaster impact value
        // Apply final modified disaster impact
        _year.ApplyCosts(DataSheet.GetDisasterImpact(disaster, in _year));
    }
}
