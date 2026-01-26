using UnityEngine;
//public enum GlyphType
//{
//    Attack,
//    Buff
//}
[System.Serializable]
public class PatternPossibilities
{
    public int[] GlyphPattern;
}
[CreateAssetMenu(fileName = "GlyphData", menuName = "ScriptableObjects/GlyphData")]
public class GlyphData : ScriptableObject
{
    public PatternPossibilities[] PatternPossibilities;
    public string Name;
    //public GlyphType Type;
}


