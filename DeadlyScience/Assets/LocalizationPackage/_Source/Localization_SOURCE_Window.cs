#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

//---Written by Matej Vanco 20.10.2018 dd/mm/yyyy
//---Language Localization - Window

public class Localization_SOURCE_Window : EditorWindow {

    //---Input-Output settings
    private static string INTERNAL_SelectedPath;
    private static string INTERNAL_RegistryKey = "LOCATION_MANAGER_LocManagPath";

    private static string INTERNAL_VersionFormat = "Location_Manager_Main_System";

    //---General Variables - Language file, Category, Localization
    public static string PUBLIC_CurrentLanguage;

    private static bool INTERNAL_ManagerSelected = true;

    private static int INTERNAL_LocCategorySelected = 0;
    public static List<string> INTERNAL_LocCategories = new List<string>();
    private static string INTERNAL_LocCategory_Name;

    public class PUBLIC_LocalizationArray_
    {
        public string Key;
        public string Text;
        public int Category;
    }
    public static List<PUBLIC_LocalizationArray_> PUBLIC_LocalizationArray = new List<PUBLIC_LocalizationArray_>();

    public static bool initialized = false;
    private static bool readySteady = false;
    public static void Init()
    {
        if (initialized)
            return;

        Localization_SOURCE_Window win = new Localization_SOURCE_Window();

        initialized = true;

        PUBLIC_LocalizationArray.Clear();
        INTERNAL_FUNCT_GetManagerPath();
    }
    [MenuItem("Window/Localization Manager")]
    public static void InitWindow()
    {
        Localization_SOURCE_Window win = new Localization_SOURCE_Window();
        win.minSize = new Vector2(250, 200);
        win.maxSize = new Vector2(601, 601);
        win.name = "Localization Manager";
        win.Show();

        PUBLIC_LocalizationArray.Clear();

        INTERNAL_FUNCT_GetManagerPath();
    }

    #region SECTION_InputOutput__DataManaging

    private static void INTERNAL_FUNCT_GetManagerPath()
    {
        PUBLIC_CurrentLanguage = "";

        INTERNAL_SelectedPath = PlayerPrefs.GetString(INTERNAL_RegistryKey);
        if (string.IsNullOrEmpty(INTERNAL_SelectedPath))
        {
            readySteady = false;
            return;
        }
        readySteady = true;
        INTERNAL_ManagerSelected = true;

        FUNCT_RefreshData();
    }

    private static void INTERNAL_FUNCT_GetLanguagePath()
    {
        FUNCT_SaveData(INTERNAL_SelectedPath,false);

        string f = EditorUtility.OpenFilePanel("Select Language File Path", Application.dataPath, "xml");
        if (string.IsNullOrEmpty(f))
            return;
        INTERNAL_SelectedPath = f;
        INTERNAL_ManagerSelected = false;

        PUBLIC_CurrentLanguage = Path.GetFileNameWithoutExtension(INTERNAL_SelectedPath);

        FUNCT_ConvertLanguageData(INTERNAL_SelectedPath, true);

        FUNCT_RefreshData();
    }

    private static void INTERNAL_FUNCT_CreateLanguagePath()
    {
        FUNCT_SaveData(INTERNAL_SelectedPath, false);

        string f = EditorUtility.SaveFilePanel("Create Language File", Application.dataPath, "English" , "xml");
        if (string.IsNullOrEmpty(f))
            return;
        File.Create(f).Dispose();
        INTERNAL_SelectedPath = f;
        INTERNAL_ManagerSelected = false;

        PUBLIC_CurrentLanguage = Path.GetFileNameWithoutExtension(INTERNAL_SelectedPath);

        FUNCT_ConvertLanguageData(INTERNAL_SelectedPath, true);

        FUNCT_RefreshData();
    }

    public static void FUNCT_RefreshData()
    {
        INTERNAL_LocCategories.Clear();
        INTERNAL_LocCategories.Add("Default");
        PUBLIC_LocalizationArray.Clear();

        #region Checking file
        if (!File.Exists(INTERNAL_SelectedPath))
        {
            FUNCT_MESS_Error("The file path " + INTERNAL_SelectedPath + " doesn't exist!");
            PlayerPrefs.DeleteKey(INTERNAL_RegistryKey);
            readySteady = false;
            return;
        }
        else
        {
            if (File.ReadAllLines(INTERNAL_SelectedPath).Length > 0 && File.ReadAllLines(INTERNAL_SelectedPath)[0] == INTERNAL_VersionFormat)
            { }
            else
            {
                FUNCT_MESS_Error("The file path " + INTERNAL_SelectedPath + " is not in correct format or is empty!");
                PlayerPrefs.DeleteKey(INTERNAL_RegistryKey);
                readySteady = false;
                return;
            }
        }
        #endregion

        string[] AllLines = File.ReadAllLines(INTERNAL_SelectedPath);
        if (AllLines.Length <= 1)
            return;

        //----------------------GETTING CATEGORIES
        for (int i = 1; i < AllLines.Length; i++)
        {
            string currentLine = AllLines[i];
            if (currentLine.StartsWith(Localization_SOURCE.INTERNAL_DivisionCharacter))
                INTERNAL_LocCategories.Add(currentLine.Replace("=", ""));
        }

        //----------------------GETTING LOCALIZATION
        int CurrentCategory = 0;
        for (int i = 1; i < AllLines.Length; i++)
        {
            string currentLine = AllLines[i];
            if (currentLine.StartsWith(Localization_SOURCE.INTERNAL_DivisionCharacter))
            {
                CurrentCategory++;
                continue;
            }

            PUBLIC_LocalizationArray_ locA = new PUBLIC_LocalizationArray_();

            if (currentLine.Length <= 1)
                continue;
            if (currentLine.IndexOf("=") <= 1)
                continue;
            string _Key = currentLine.Substring(0, currentLine.IndexOf("="));
            locA.Key = _Key;
            string _Text = currentLine.Substring(_Key.Length + 1, currentLine.Length - _Key.Length - 1);
            locA.Text = _Text;
            locA.Category = CurrentCategory;

            PUBLIC_LocalizationArray.Add(locA);
        }
    }

    public static void FUNCT_SaveData(string ToPath, bool RefreshData = true)
    {
        if (!File.Exists(ToPath))
        {
            FUNCT_MESS_Error("The file path " + ToPath + " doesn't exist!");
            return;
        }

        File.WriteAllText(ToPath, INTERNAL_VersionFormat);

        FileStream fstream = new FileStream(ToPath, FileMode.Append);
        StreamWriter fwriter = new StreamWriter(fstream);

        fwriter.WriteLine("");

        foreach (string category in INTERNAL_LocCategories)
        {
            if(category != "Default")
                fwriter.WriteLine(Localization_SOURCE.INTERNAL_DivisionCharacter + category);

            foreach (PUBLIC_LocalizationArray_ locArray in PUBLIC_LocalizationArray)
            {
                if (category == "Default" && locArray.Category == 0)
                { }
                else if (category == INTERNAL_LocCategories[locArray.Category])
                { }
                else
                    continue;

                if (string.IsNullOrEmpty(locArray.Key))
                    continue;
                locArray.Key = locArray.Key.Replace(Localization_SOURCE.INTERNAL_DivisionCharacter, "");
                fwriter.WriteLine(locArray.Key + Localization_SOURCE.INTERNAL_DivisionCharacter + locArray.Text);
            }
        }

        fwriter.Dispose();
        fstream.Close();

        UnityEditor.AssetDatabase.Refresh();

        if(RefreshData)
            FUNCT_RefreshData();
    }

    public static void FUNCT_ConvertLanguageData(string ToPath, bool RefreshData = true)
    {
        if (!File.Exists(ToPath))
        {
            FUNCT_MESS_Error("The file path " + ToPath + " doesn't exist!");
            return;
        }

        if (File.ReadAllLines(ToPath).Length > 1)
        {
            List<string> storedFilelines = new List<string>();
            for (int i = 1; i < File.ReadAllLines(ToPath).Length; i++)
                storedFilelines.Add(File.ReadAllLines(ToPath)[i]);

            foreach (string categories in INTERNAL_LocCategories)
            {
                foreach(PUBLIC_LocalizationArray_ locArray in PUBLIC_LocalizationArray)
                {
                    if(INTERNAL_GetLocalizationCategory(categories) == locArray.Category)
                    {
                        foreach(string s in storedFilelines)
                        {
                            if (string.IsNullOrEmpty(s))
                                continue;
                            if (s.StartsWith(Localization_SOURCE.INTERNAL_DivisionCharacter))
                                continue;
                            string Key = s.Substring(0, s.IndexOf(Localization_SOURCE.INTERNAL_DivisionCharacter));
                            if (string.IsNullOrEmpty(Key))
                                continue;
                            if (Key == locArray.Key)
                            {
                                if (s.Length < Key.Length + 1)
                                    continue;
                                locArray.Text = s.Substring(Key.Length + 1, s.Length - Key.Length - 1);
                                break;
                            }
                        }
                    }
                }
            }
        }

        File.WriteAllText(ToPath, INTERNAL_VersionFormat);

        FileStream fstream = new FileStream(ToPath, FileMode.Append);
        StreamWriter fwriter = new StreamWriter(fstream);

        fwriter.WriteLine("");

        foreach (string category in INTERNAL_LocCategories)
        {
            if (category != "Default")
                fwriter.WriteLine(Localization_SOURCE.INTERNAL_DivisionCharacter + category);

            foreach (PUBLIC_LocalizationArray_ locArray in PUBLIC_LocalizationArray)
            {
                if (category == "Default" && locArray.Category == 0)
                { }
                else if (category == INTERNAL_LocCategories[locArray.Category])
                { }
                else
                    continue;

                if (string.IsNullOrEmpty(locArray.Key))
                    continue;
                locArray.Key = locArray.Key.Replace(Localization_SOURCE.INTERNAL_DivisionCharacter, "");
                fwriter.WriteLine(locArray.Key + Localization_SOURCE.INTERNAL_DivisionCharacter + locArray.Text);
            }
        }

        fwriter.Dispose();
        fstream.Close();

        if (RefreshData)
            FUNCT_RefreshData();
    }

    #endregion

    private static int INTERNAL_GetLocalizationCategory(string entry)
    {
        int c = 0;
        foreach(string categ in INTERNAL_LocCategories)
        {
            if (categ == entry)
                return c;
            c++;
        }
        return 0;
    }

    private static Vector2 guiINTERNAL_ScrollHelper;
    private void OnGUI()
    {
        EditorGUI.indentLevel++;
        pDrawSpace();

        pDrawLabel("Localization Manager by Matej Vanco", true);

        pDrawSpace();

        //---Not ready:
        if(!readySteady)
        {
            GUILayout.BeginVertical("Box");

            EditorGUILayout.HelpBox("There is no Localization Manager file. To set up keys structure and language system, select or create a Localization Manager file.",MessageType.Info);
            GUILayout.BeginHorizontal("Box");
            if(GUILayout.Button("Select Localization Manager file",GUILayout.Width(250)))
            {
                string f = EditorUtility.OpenFilePanel("Select Localization Manager file", Application.dataPath, "txt");
                if (string.IsNullOrEmpty(f))
                    return;
                INTERNAL_SelectedPath = f;
                PlayerPrefs.SetString(INTERNAL_RegistryKey, INTERNAL_SelectedPath);
                FUNCT_MESS_Info("Great! The Localization Manager is now ready.");
                INTERNAL_FUNCT_GetManagerPath();
                return;
            }
            if (GUILayout.Button("Create Localization Manager file"))
            {
                string f = EditorUtility.SaveFilePanel("Create Localization Manager file", Application.dataPath, "LocalizationManager", "txt");
                if (string.IsNullOrEmpty(f))
                    return;
                File.Create(f).Dispose();
                INTERNAL_SelectedPath = f;
                PlayerPrefs.SetString(INTERNAL_RegistryKey, INTERNAL_SelectedPath);
                FUNCT_MESS_Info("Great! The Localization Manager is now ready.");
                FUNCT_SaveData(INTERNAL_SelectedPath, false);
                INTERNAL_FUNCT_GetManagerPath();
                return;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            return;
        }
        //---Else...:

        #region SECTION__UPPER

        GUILayout.BeginHorizontal("Box");
        if (GUILayout.Button("Save System"))
            FUNCT_SaveData(INTERNAL_SelectedPath);
        pDrawSpace();
        if (INTERNAL_ManagerSelected)
            if (GUILayout.Button("Reset Manager Path"))
            {
                if (EditorUtility.DisplayDialog("Question", "You are about to reset the Localization Manager path... Are you sure?", "Yes", "No"))
                {
                    PlayerPrefs.DeleteKey(INTERNAL_RegistryKey);
                    this.Close();
                }
            }
        GUILayout.EndHorizontal();

        pDrawSpace(5);

        string lang = "";
        if (!string.IsNullOrEmpty(PUBLIC_CurrentLanguage))
        {
            INTERNAL_ManagerSelected = false;
            lang = PUBLIC_CurrentLanguage;
        }
        else
        {
            INTERNAL_ManagerSelected = true;
            lang = "Language Manager";
        }

        GUILayout.BeginHorizontal("Box");
        pDrawLabel("Selected: "+lang);
        if (GUILayout.Button("Deselect Language"))
        {
            FUNCT_SaveData(INTERNAL_SelectedPath,false);
            INTERNAL_FUNCT_GetManagerPath();
        }
        if (GUILayout.Button("Select Language"))
            INTERNAL_FUNCT_GetLanguagePath();
        pDrawSpace();
        if (GUILayout.Button("Create Language"))
            INTERNAL_FUNCT_CreateLanguagePath();
        GUILayout.EndHorizontal();

        #endregion

        pDrawSpace(5);

        GUILayout.BeginVertical("Box");

        #region SECTION__CATEGORIES

        GUILayout.BeginHorizontal("Box");
        EditorGUIUtility.labelWidth -= 70;
        INTERNAL_LocCategorySelected = EditorGUILayout.Popup("Category:", INTERNAL_LocCategorySelected, INTERNAL_LocCategories.ToArray(), GUILayout.MaxWidth(300), GUILayout.MinWidth(50));
        EditorGUIUtility.labelWidth += 70;
        if (INTERNAL_ManagerSelected)
        {
            pDrawSpace();
            INTERNAL_LocCategory_Name = EditorGUILayout.TextField(INTERNAL_LocCategory_Name);
            if (GUILayout.Button("+ Category"))
            {
                if (string.IsNullOrEmpty(INTERNAL_LocCategory_Name))
                {
                    FUNCT_MESS_Error("Please fill the required field! [Category Name]");
                    return;
                }
                INTERNAL_LocCategories.Add(INTERNAL_LocCategory_Name);
                INTERNAL_LocCategory_Name = "";
                GUI.FocusControl("Set");
                return;
            }
            if (GUILayout.Button("- Category") && INTERNAL_LocCategories.Count > 1)
            {
                if (EditorUtility.DisplayDialog("Question", "You are going to remove category... Are you sure?", "Yes", "No"))
                {
                    if (string.IsNullOrEmpty(INTERNAL_LocCategory_Name))
                    {
                        INTERNAL_LocCategories.RemoveAt(INTERNAL_LocCategories.Count - 1);
                        INTERNAL_LocCategorySelected = 0;
                    }
                    else
                    {
                        int cc = 0;
                        bool notfound = true;
                        foreach (string cat in INTERNAL_LocCategories)
                        {
                            if (INTERNAL_LocCategory_Name == cat)
                            {
                                INTERNAL_LocCategories.RemoveAt(cc);
                                INTERNAL_LocCategorySelected = 0;
                                notfound = false;
                                break;
                            }
                            cc++;
                        }
                        if (notfound)
                            FUNCT_MESS_Error("The category couldn't be found.");
                        INTERNAL_LocCategory_Name = "";
                    }
                    return;
                }
            }
        }
        GUILayout.EndHorizontal();

        #endregion

        pDrawSpace();

        #region SECTION__LOCALIZATION_ARRAY

        GUILayout.BeginHorizontal();
        pDrawLabel("Localization Keys & Translations");
        if (INTERNAL_ManagerSelected && GUILayout.Button("+"))
            PUBLIC_LocalizationArray.Add(new PUBLIC_LocalizationArray_() {Category = INTERNAL_LocCategorySelected });
        GUILayout.EndHorizontal();

        if (PUBLIC_LocalizationArray.Count == 0)
            pDrawLabel(" - - Empty! - -");
        else
        {
            guiINTERNAL_ScrollHelper = EditorGUILayout.BeginScrollView(guiINTERNAL_ScrollHelper);

            int c = 0;
            foreach (PUBLIC_LocalizationArray_ locA in PUBLIC_LocalizationArray)
            {
                if (locA.Category >= INTERNAL_LocCategories.Count)
                {
                    locA.Category = 0;
                    break;
                }
                if (INTERNAL_LocCategories[locA.Category] != INTERNAL_LocCategories[INTERNAL_LocCategorySelected])
                    continue;

                EditorGUIUtility.labelWidth -= 100;
                EditorGUILayout.BeginHorizontal("Box");
                if (!INTERNAL_ManagerSelected)
                {
                    EditorGUILayout.LabelField(locA.Key,GUILayout.Width(100));

                    EditorGUILayout.LabelField("Text:", GUILayout.Width(100));
                    locA.Text = EditorGUILayout.TextField(locA.Text, GUILayout.MaxWidth(300), GUILayout.MinWidth(100));
                }
                else
                {
                    EditorGUILayout.LabelField("Key:", GUILayout.Width(45));

                    locA.Key = EditorGUILayout.TextField(locA.Key, GUILayout.MaxWidth(100), GUILayout.MinWidth(30));
                    EditorGUILayout.LabelField("Category:", GUILayout.Width(75));
                    locA.Category = EditorGUILayout.Popup(locA.Category, INTERNAL_LocCategories.ToArray());
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        PUBLIC_LocalizationArray.Remove(locA);
                        return;
                    }
                }
                
                EditorGUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth += 100;
                c++;
            }
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndVertical();

        #endregion

        EditorGUI.indentLevel--;
    }

    #region SECTION_GUILayout
    private void pDrawDefaultLabel(string text)
    {
        GUILayout.Label(text);
    }
    private void pDrawLabel(string text,bool bold = false)
    {
        if(bold)
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
    #endregion

    private static void FUNCT_MESS_Error(string msg)
    {
        EditorUtility.DisplayDialog("Error", msg, "OK");
        return;
    }
    private static void FUNCT_MESS_Info(string msg)
    {
        EditorUtility.DisplayDialog("Info", msg, "OK");
        return;
    }
}
#endif