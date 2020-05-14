using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

//---Written by Matej Vanco 20.10.2018 dd/mm/yyyy
//---Language Localization - Source

[AddComponentMenu("Matej Vanco/Language Localization")]
public class Localization_SOURCE : MonoBehaviour {

    public static string INTERNAL_DivisionCharacter = "=";

    public TextAsset[] LanguageFiles;
    public int SelectedLanguage;

    public bool LoadLanguageOnStart = true;

    public List<string> Categories = new List<string>();

	[System.Serializable]
    public class _LocalizationSelector
    {
        public enum _AssignationType {GameObjectChild, SpecificUIText, SpecificTextMesh};
        public _AssignationType AssignationType;

        public string Key;
        public string Text;
        public int Category;

        //AT - GameObjectChild
        public bool AT_FindChildByKeyName = true;
        public string AT_ChildName;
        public bool AT_UseGeneralChildsRootObject = true;
        public Transform AT_CustomChildsRootObject;

        public bool AT_UITextComponentAllowed = true;
        public bool AT_TextMeshComponentAllowed = true;

        public GameObject AT_FoundObject;

        //AT - Specific_UIText
        public Text AT_UITextObject;
        //AT - Specific_TextMesh
        public TMP_Text AT_TextMeshbject;
    }
    public List<_LocalizationSelector> LocalizationSelector = new List<_LocalizationSelector>();
    public Transform AT_GameObjectChildsRoot;

    private void Awake()
    {

        SelectedLanguage = PlayerPrefs.HasKey("language") ? PlayerPrefs.GetInt("language") : 0;
        PUBLIC_LoadAssignationTypes();
        if (LoadLanguageOnStart)
            PUBLIC_LoadLanguage(SelectedLanguage);
    }

    #region INTERNAL

#if UNITY_EDITOR
    public void Internal_RefreshInternalLocalization()
    {
        Categories.Clear();
        Categories.AddRange(Localization_SOURCE_Window.INTERNAL_LocCategories);
    }
    public void Internal_AddKey(string KeyName)
    {
        foreach(Localization_SOURCE_Window.PUBLIC_LocalizationArray_ a in Localization_SOURCE_Window.PUBLIC_LocalizationArray)
        {
            if(a.Key == KeyName)
            {
                LocalizationSelector.Add(new _LocalizationSelector() { Key = a.Key, Text = a.Text, Category = a.Category });
                return;
            }
        }
    }
#endif

    public string Internal_ConvertAndReturnText(_LocalizationSelector lSelector, string[] lines)
    {
        if (lines.Length > 1)
        {
            List<string> storedFilelines = new List<string>();
            for (int i = 1; i < lines.Length; i++)
                storedFilelines.Add(lines[i]);

            foreach (string categories in Categories)
            {
                if (Internal_GetLocalizationCategory(categories) == lSelector.Category)
                {
                    foreach (string s in storedFilelines)
                    {
                        if (string.IsNullOrEmpty(s))
                            continue;
                        if (s.StartsWith(INTERNAL_DivisionCharacter))
                            continue;
                        string Key = s.Substring(0, s.IndexOf(INTERNAL_DivisionCharacter));
                        if (string.IsNullOrEmpty(Key))
                            continue;
                        if (Key == lSelector.Key)
                        {
                            if (s.Length < Key.Length + 1)
                                continue;
                            lSelector.Text = s.Substring(Key.Length + 1, s.Length - Key.Length - 1);
                            return lSelector.Text;
                        }
                    }
                }
            }
        }
        return "";
    }

    private int Internal_GetLocalizationCategory(string entry)
    {
        int c = 0;
        foreach (string categ in Categories)
        {
            if (categ == entry)
                return c;
            c++;
        }
        return 0;
    }

    #endregion

    /// <summary>
    /// Load and Refresh all text objects by the selected options
    /// </summary>
    public void PUBLIC_LoadAssignationTypes()
    {
        foreach (_LocalizationSelector sel in LocalizationSelector)
        {
            switch (sel.AssignationType)
            {
                case _LocalizationSelector._AssignationType.GameObjectChild:
                    string childName = sel.AT_ChildName;
                    if (sel.AT_FindChildByKeyName)
                        childName = sel.Key;
                    GameObject FoundChild = null;
                    if(sel.AT_UseGeneralChildsRootObject)
                        foreach (Transform t in AT_GameObjectChildsRoot.GetComponentsInChildren<Transform>())
                        {
                            if (t.name == childName)
                            {
                                if (sel.AT_UITextComponentAllowed && t.GetComponent<Text>())
                                {
                                    FoundChild = t.gameObject;
                                    break;
                                }
                                else if (t.GetComponent<TMP_Text>())
                                {
                                    FoundChild = t.gameObject;
                                    break;
                                }
                            }
                        }
                    else if(sel.AT_CustomChildsRootObject)
                        foreach (Transform t in sel.AT_CustomChildsRootObject.GetComponentsInChildren<Transform>())
                        {
                            if (t.name == childName)
                            {
                                if (sel.AT_UITextComponentAllowed && t.GetComponent<Text>())
                                {
                                    FoundChild = t.gameObject;
                                    break;
                                }
                                else if (t.GetComponent<TMP_Text>())
                                {
                                    FoundChild = t.gameObject;
                                    break;
                                }
                            }
                        }
                    else
                        Debug.Log("Localization: The key '" + sel.Key + "' has empty variable [AT_CustomChildsRootObject].");
                    if (FoundChild)
                        sel.AT_FoundObject = FoundChild;
                    else
                        Debug.Log("Localization: The key '"+sel.Key+"' couldn't find its object in the root object.");
                    break;
            }
        }
    }

    /// <summary>
    ///  Load whole language database by the selected language index
    /// </summary>
    public void PUBLIC_LoadLanguage(int languageIndex)
    {
        if (LanguageFiles.Length <= languageIndex)
        {
            Debug.LogError("Localization: The index for language selection is incorrect! Languages count: "+ LanguageFiles.Length+", Your index: "+languageIndex);
            return;
        }
        else if (LanguageFiles[languageIndex] == null)
        {
            Debug.LogError("Localization: The language that you've selected with your index is empty!");
            return;
        }

        foreach (_LocalizationSelector sel in LocalizationSelector)
        {
            sel.Text = Internal_ConvertAndReturnText(sel, LanguageFiles[languageIndex].text.Split('\n')).Replace(@"\n", System.Environment.NewLine);

            if (sel.AssignationType == _LocalizationSelector._AssignationType.GameObjectChild && sel.AT_FoundObject)
            {
                if (sel.AT_FoundObject.GetComponent<Text>() && sel.AT_UITextComponentAllowed)
                    sel.AT_FoundObject.GetComponent<Text>().text = sel.Text;
                else if (sel.AT_FoundObject.GetComponent<TextMesh>() && sel.AT_TextMeshComponentAllowed)
                    sel.AT_FoundObject.GetComponent<TextMesh>().text = sel.Text;
            }
            else if (sel.AssignationType == _LocalizationSelector._AssignationType.SpecificTextMesh && sel.AT_TextMeshbject)
                sel.AT_TextMeshbject.text = sel.Text;
            else if (sel.AssignationType == _LocalizationSelector._AssignationType.SpecificUIText && sel.AT_UITextObject)
                sel.AT_UITextObject.text = sel.Text;
        }
     }
}
