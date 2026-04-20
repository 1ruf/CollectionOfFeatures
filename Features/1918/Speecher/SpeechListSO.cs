#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeechListSO", menuName = "SO/SpeechListSO")]
public class SpeechListSO : ScriptableObject
{
    public string Speecher;
    public Color SpeecherColor;
    [TextArea] public List<string> Speech;

    [Header("AutoNameBuildSetting")]
    public bool autoNameBuild = false;

    [SerializeField] private FirstNameListSO firstNameList;
    [SerializeField] private LastNameListSO lastNameList;

    [ContextMenu("Generate Random Name")]
    private void GenerateRandomName()
    {
#if UNITY_EDITOR
        if (autoNameBuild == false) return;

        if (firstNameList == null || lastNameList == null ||
            firstNameList.FirstNames == null || lastNameList.LastNames == null ||
            firstNameList.FirstNames.Count == 0 || lastNameList.LastNames.Count == 0)
        {
            Debug.LogWarning("[SpeechListSO] 이름 리스트가 비어있습니다.");
            return;
        }

        string firstName = firstNameList.FirstNames[Random.Range(0, firstNameList.FirstNames.Count)];
        string lastName = string.Empty;

        for (int i = 0; i < lastNameList.LastNames.Count; ++i)
        {
            string candidate = lastNameList.LastNames[Random.Range(0, lastNameList.LastNames.Count)];

            if (candidate[0] != firstName[0])
            {
                lastName = candidate;
                break;
            }
        }

        if (string.IsNullOrEmpty(lastName)) return;

        Speecher = $"{firstName} {lastName}";

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

        Debug.Log($"[SpeechListSO] 새로운 이름 생성: {Speecher}");
#endif
    }
}