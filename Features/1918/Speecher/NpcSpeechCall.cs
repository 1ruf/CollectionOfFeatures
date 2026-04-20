using _01_Work.KJY.Code.Events;
using _01_Work.LCM._01.Scripts.Core;
using UnityEngine;

public class NpcSpeechCall : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO subtitleChannel;
    [SerializeField] private SpeecherListSO speechersList;

    [SerializeField] private SpeechListSO speecherData;

    private int _currentIndex;
    private int _anchorIndex;

    private void Start()
    {
        InitializeSpeecher();
        InitializeAnchorIndex();
    }

    public void TalkRandom()
    {
        if (!HasSpeechData()) return;

        int index = Random.Range(0, speecherData.Speech.Count);
        RaiseSubtitle(speecherData.Speech[index]);
    }

    public void TalkSimple()
    {
        if (!HasSpeechData()) return;

        if (Random.Range(0, 2) == 0)
        {
            RaiseSubtitleByIndex(_anchorIndex);
        }
        else
        {
            RaiseSubtitle("....");
        }
    }

    public void Talk()
    {
        if (!HasSpeechData()) return;

        if (_currentIndex >= speecherData.Speech.Count)
        {
            _currentIndex = 0;
        }

        RaiseSubtitleByIndex(_currentIndex++);
    }

    private void InitializeSpeecher()
    {
        if (speechersList != null)
        {
            speecherData = speechersList.GetRandom();
        }
    }

    private void InitializeAnchorIndex()
    {
        if (!HasSpeechData()) return;

        _anchorIndex = Random.Range(0, speecherData.Speech.Count);
    }

    private bool HasSpeechData()
    {
        return speecherData != null && speecherData.Speech != null && speecherData.Speech.Count > 0;
    }

    private void RaiseSubtitleByIndex(int index)
    {
        if (index < 0 || index >= speecherData.Speech.Count) return;

        RaiseSubtitle(speecherData.Speech[index]);
    }

    private void RaiseSubtitle(string message)
    {
        subtitleChannel.RaiseEvent(
            SubtitleChannel.AddSubtitleEvent.Initialize(
                speecherData.Speecher,
                message,
                speecherData.SpeecherColor
            )
        );
    }
}