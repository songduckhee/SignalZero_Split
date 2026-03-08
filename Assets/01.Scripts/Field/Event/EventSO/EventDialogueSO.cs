using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class EventDialogueSO : ScriptableObject
{
    public EventType EventType;
    public List<string> Dialogue;
    public int ButtonCount = 2;
    public GameObject? spawnStructure;
}
