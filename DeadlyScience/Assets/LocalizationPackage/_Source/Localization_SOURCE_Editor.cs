#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//---Written by Matej Vanco 20.10.2018 dd/mm/yyyy
//---Language Localization - Editor

[CustomEditor(typeof(Localization_SOURCE))]
[CanEditMultipleObjects]
public class Localization_SOURCE_Editor : Editor
{

    private SerializedProperty LanguageFiles, SelectedLanguage;
    private SerializedProperty LoadLanguageOnStart;

    private SerializedProperty Categories;
    private SerializedProperty LocalizationSelector;

    private SerializedProperty AT_GameObjectChildsRoot;

    Localization_SOURCE l;
    bool addKey = false;
    bool categorySelected = false;
    int category = 0;

    private void OnEnable()
    {
        l = (Localization_SOURCE)target;

        LanguageFiles = serializedObject.FindProperty("LanguageFiles");
        SelectedLanguage = serializedObject.FindProperty("SelectedLanguage");
        LoadLanguageOnStart = serializedObject.FindProperty("LoadLanguageOnStart");
        Categories = serializedObject.FindProperty("Categories");
        LocalizationSelector = serializedObject.FindProperty("LocalizationSelector");
        AT_GameObjectChildsRoot = serializedObject.FindProperty("AT_GameObjectChildsRoot");
    }

    public override void OnInspectorGUI()
    {
        if (target == null)
            return;
        serializedObject.Update();

        pDrawSpace();

        GUILayout.BeginVertical("Box");
        pDrawProperty(LanguageFiles, "Language Files", "", true);
        pDrawProperty(SelectedLanguage, "Selected Language", "Currently selected language for the localization");
        pDrawSpace(5);
        pDrawProperty(LoadLanguageOnStart, "Load Language On Start", "Load localization after the game startup");
        GUILayout.EndVertical();

        pDrawSpace();

        GUILayout.BeginVertical("Box");
        pDrawLabel("Localization Settings");
        if (GUILayout.Button("Add Key"))
        {
            Localization_SOURCE_Window.Init();
            addKey = true;
            l.Internal_RefreshInternalLocalization();
        }

        if(addKey)
        {
            GUILayout.BeginVertical("Box");
            if(GUILayout.Button("X",GUILayout.Width(40)))
            {
                addKey = false;
                categorySelected = false;
                category = 0;
                return;
            }
            pDrawLabel("From Category:");
            GUILayout.BeginHorizontal("Box");
            for(int i = 0; i<l.Categories.Count;i++)
            {
                if(GUILayout.Button(l.Categories[i]))
                {
                    categorySelected = true;
                    category = i;
                    return;
                }
            }
            GUILayout.EndHorizontal();
            if(categorySelected)
            {
                pDrawLabel("Key:");
                if (GUILayout.Button("Add All",GUILayout.Width(120)))
                {
                    for (int i = 0; i < Localization_SOURCE_Window.PUBLIC_LocalizationArray.Count; i++)
                    {
                        if (Localization_SOURCE_Window.PUBLIC_LocalizationArray[i].Category != category)
                            continue;
                        l.Internal_AddKey(Localization_SOURCE_Window.PUBLIC_LocalizationArray[i].Key);
                    }
                    addKey = false;
                    categorySelected = false;
                    category = 0;
                    return;
                }
                EditorGUILayout.BeginVertical("Box");
                for (int i = 0; i < Localization_SOURCE_Window.PUBLIC_LocalizationArray.Count; i++)
                {
                    if (Localization_SOURCE_Window.PUBLIC_LocalizationArray[i].Category != category)
                        continue;
                    bool passed = true;
                    foreach(Localization_SOURCE._LocalizationSelector sel in l.LocalizationSelector)
                    {
                        if(sel.Key == Localization_SOURCE_Window.PUBLIC_LocalizationArray[i].Key)
                        {
                            passed = false;
                            break;
                        } 
                    }
                    if (!passed)
                        continue;
                    if (GUILayout.Button(Localization_SOURCE_Window.PUBLIC_LocalizationArray[i].Key))
                    {
                        l.Internal_AddKey(Localization_SOURCE_Window.PUBLIC_LocalizationArray[i].Key);
                        addKey = false;
                        categorySelected = false;
                        category = 0;
                        return;
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();

        pDrawSpace();

        GUILayout.BeginVertical("Box");
        pDrawLabel("Selected Keys Settings");
        pDrawProperty(AT_GameObjectChildsRoot, "GameObject Childs Root","Starting root for keys containing 'GameObjectChild' assignation type");
        if(l.LocalizationSelector.Count>0)
            pDrawList(LocalizationSelector);
        GUILayout.EndVertical();
    }

    private void pDrawList(SerializedProperty p)
    {
        for (int i = 0; i < l.LocalizationSelector.Count; i++)
        {
            GUILayout.BeginVertical("Box");
            SerializedProperty item = LocalizationSelector.GetArrayElementAtIndex(i);
            GUILayout.BeginHorizontal();
            pDrawProperty(item, l.LocalizationSelector[i].Key);
            if (GUILayout.Button("X", GUILayout.Width(40)))
            {
                l.LocalizationSelector.RemoveAt(i);
                return;
            }
            GUILayout.EndHorizontal();
            if (!item.isExpanded)
            {
                GUILayout.EndVertical();
                continue;
            }

            Localization_SOURCE._LocalizationSelector sec = l.LocalizationSelector[i];

            pDrawSpace(5);

            EditorGUI.indentLevel += 1;
            GUILayout.BeginHorizontal("Box");
            pDrawDefaultLabel("Key: "+ sec.Key);
            pDrawDefaultLabel("Category: " + l.Categories[sec.Category]);
            GUILayout.EndHorizontal();

            pDrawSpace();

            pDrawProperty(item.FindPropertyRelative("AssignationType"), "Assignation Type");

            switch(sec.AssignationType)
            {
                case Localization_SOURCE._LocalizationSelector._AssignationType.GameObjectChild:
                    pDrawProperty(item.FindPropertyRelative("AT_FindChildByKeyName"), "Find Child By Key Name", "If enabled, the system will find the child of the selected component type [below] by the key name");
                    if (!sec.AT_FindChildByKeyName)
                        pDrawProperty(item.FindPropertyRelative("AT_ChildName"), "Child Name");

                    pDrawSpace(3);

                    pDrawProperty(item.FindPropertyRelative("AT_UseGeneralChildsRootObject"), "Use General Childs Root Object");
                    if(!sec.AT_UseGeneralChildsRootObject)
                        pDrawProperty(item.FindPropertyRelative("AT_CustomChildsRootObject"), "Custom Childs Root Object");

                    pDrawSpace(3);

                    pDrawProperty(item.FindPropertyRelative("AT_UITextComponentAllowed"), "UIText Component Allowed");
                    pDrawProperty(item.FindPropertyRelative("AT_TextMeshComponentAllowed"), "TextMesh Component Allowed");
                    break;

                case Localization_SOURCE._LocalizationSelector._AssignationType.SpecificUIText:
                    pDrawProperty(item.FindPropertyRelative("AT_UITextObject"), "Specific UI Text", "Assign specific UI Text object");
                    break;

                case Localization_SOURCE._LocalizationSelector._AssignationType.SpecificTextMesh:
                    pDrawProperty(item.FindPropertyRelative("AT_TextMeshbject"), "Specific Text Mesh", "Assign specific Text Mesh object");
                    break;
            }
           

            EditorGUI.indentLevel -= 1;
            GUILayout.EndVertical();
        }
    }

    #region SECTION_GUILayout
    private void pDrawDefaultLabel(string text)
    {
        GUILayout.Label(text);
    }
    private void pDrawLabel(string text, bool bold = false)
    {
        if (bold)
        {
            string add = "<b>";
            add += text + "</b>";
            text = add;
        }
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.normal.textColor = Color.white;
        EditorGUILayout.LabelField(text, style);
    }
    private void pDrawSpace(float space = 10)
    {
        GUILayout.Space(space);
    }
    private void pDrawProperty(SerializedProperty p, string Text, string ToolTip = "", bool includeChilds = false)
    {
        EditorGUILayout.PropertyField(p, new GUIContent(Text, ToolTip), includeChilds, null);
        serializedObject.ApplyModifiedProperties();
    }
    #endregion
}
#endif