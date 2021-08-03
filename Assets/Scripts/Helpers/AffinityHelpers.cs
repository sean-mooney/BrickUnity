using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Base types
public enum AffinityBaseTypes
{
    Rock,
    Paper,
    Scissors
}

// Full types
public enum AffinityType
{
    Rock1,
    Paper1,
    Scissors1
}


public class AffinityHelpers
{
    // String to Enum mapper
    static Dictionary<string, AffinityType> AffinityMapper = new Dictionary<string, AffinityType>{
        {"Rock", AffinityType.Rock1},
        {"Paper", AffinityType.Paper1},
        {"Scissors", AffinityType.Scissors1}
    };

    static Dictionary<string, int> AffinityMultiplierMap = new Dictionary<string, int>{
        {"Rock1:Rock1", 1},
        {"Rock1:Paper1", 0},
        {"Rock1:Scissors1", 3},
        {"Paper1:Rock1", 3},
        {"Paper1:Paper1", 1},
        {"Paper1:Scissors1", 0},
        {"Scissors1:Rock1", 0},
        {"Scissors1:Paper1", 3},
        {"Scissors1:Scissors1", 1}
    };
    static T PickRandomEnumElement<T>() where T : System.Enum => (T)System.Enum.GetValues(typeof(T)).OfType<System.Enum>().OrderBy(_ => System.Guid.NewGuid()).FirstOrDefault();
    public static AffinityType PickRandomAffinity()
    {
        return AffinityMapper[PickRandomEnumElement<AffinityBaseTypes>().ToString()];
    }

    public static int GetAffinityMultiplierValue(AffinityType type1, AffinityType type2)
    {
        string typeLookup = type1 + ":" + type2;
        return AffinityMultiplierMap[typeLookup];
    }
}
