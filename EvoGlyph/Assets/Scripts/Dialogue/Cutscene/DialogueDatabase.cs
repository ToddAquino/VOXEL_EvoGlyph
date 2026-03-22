using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Database")]
public class DialogueDatabase : ScriptableObject
{
    public List<Conversation> conversations;

    public Conversation Get(string id)
    {
        return conversations.Find(c => c.id == id);
    }
}

[System.Serializable]
public class Conversation
{
    public string id;
    public List<Dialogueline> lines;
}

[System.Serializable]
public class Dialogueline
{
    [TextArea(3, 5)]
    public string text;
}
