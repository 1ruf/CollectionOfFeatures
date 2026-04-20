using KHG.Utilities.RandomSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeecherList", menuName = "SO/Speech/SpeecherList")]
public class SpeecherListSO : ScriptableObject
{
    public List<SpeechListSO> Speechers;
    public SpeechListSO GetRandom()
    {
        return Speechers[Random.Range(0, Speechers.Count)];
    }
}