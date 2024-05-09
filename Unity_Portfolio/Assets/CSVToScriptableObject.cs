using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace lsy
{
    public class CSVToScriptableObject : MonoBehaviour
    {
        public static string CSVFolderPath = "Assets/08.Tables/CSV";
        public static string ScriptableFolderPath = "Assets/Resources/TableScriptableObject";

        public const string ScriptFolderPath = "Assets/02.Scripts/Table";


        public static void MakeTableScript()
        {
            try
            {
                List<string> tableNames = new List<string>();

                string[] guids = AssetDatabase.FindAssets("", new string[] { CSVFolderPath });

                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                    if (asset == null || !path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        continue;

                    string name = path.Substring(path.LastIndexOf('/') + 1);
                    name = name.Substring(0, name.IndexOf('.'));

                    tableNames.Add(name);

                    List<Dictionary<string, object>> tableDataList = TableCSVReader.Read(asset, out string[] header, out string[] types);

                    if (header.Length == 0 || types.Length == 0)
                        throw new Exception($"{nameof(CSVToScriptableObject)} : Table Header or Type Error");

                    WriteCode(asset.name, header, types);
                }

                WriteTables(tableNames);

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(CSVToScriptableObject)} : {e.Message}");
            }
        }        


        private static void WriteCode(string tableName, string[] header, string[] types)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using lsy;");
            sb.AppendLine();

            sb.AppendLine($"public class {tableName} : ScriptableObject");
            sb.AppendLine("{");

            sb.AppendLine("\t[SerializeField]");
            sb.AppendLine("\tpublic List<TableData> datas = new List<TableData>();");
            sb.AppendLine();

            sb.AppendLine($"\tpublic TableData this[{types[0]} index]");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tget");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn datas.Find(x => x.ID == index);");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");

            sb.AppendLine();

            sb.AppendLine("\t[Serializable]");
            sb.AppendLine("\tpublic class TableData");
            sb.AppendLine("\t{");

            for (int i = 0; i < header.Length; i++)
            {
                sb.AppendLine($"\t\tpublic {types[i]} {header[i]};");
            }

            sb.AppendLine("\t}");
            sb.AppendLine();

            sb.AppendLine("\tpublic void AddData(TableData data)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tdatas.Add(data);");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine();


            string textsaver = $"{ScriptFolderPath}/{tableName}.cs";

            if (File.Exists(textsaver))
            {
                File.Delete(textsaver);
            }

            File.AppendAllText(textsaver, sb.ToString());
        }


        public static void MakeScriptableObject()
        {
            try
            {
                string[] guids = AssetDatabase.FindAssets("", new string[] { CSVFolderPath });

                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);

                    TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                    if (asset == null || !path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        continue;

                    string name = path.Substring(path.LastIndexOf('/') + 1);
                    name = name.Substring(0, name.IndexOf('.'));

                    if (AssetDatabase.LoadAssetAtPath<ScriptableObject>($"{ScriptableFolderPath}/{name}.asset") != null)
                        AssetDatabase.DeleteAsset($"{ScriptableFolderPath}/{name}.asset");

                    Type tableType = Type.GetType(name);

                    ScriptableObject scriptableObj = ScriptableObject.CreateInstance(tableType);
                    AssetDatabase.CreateAsset(scriptableObj, $"{ScriptableFolderPath}/{name}.asset");

                    List<Dictionary<string, object>> tableDataList = TableCSVReader.Read(asset);

                    Type innerTableData = tableType.GetNestedType("TableData");

                    for (int i = 0; i < tableDataList.Count; i++)
                    {
                        object tableDataInstance = Activator.CreateInstance(innerTableData);

                        foreach (string key in tableDataList[i].Keys)
                        {
                            FieldInfo fieldInfo = innerTableData.GetField(key);
                            fieldInfo.SetValue(tableDataInstance, tableDataList[i][key]);
                        }

                        MethodInfo methodInfo = tableType.GetMethod("AddData");
                        methodInfo.Invoke(scriptableObj, new object[] { tableDataInstance });
                    }

                    EditorUtility.SetDirty(scriptableObj);
                }

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();                
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(CSVToScriptableObject)} : {e.Message}");
            }
        }


        private static void WriteTables(List<string> tableNames)
        {
            if (tableNames.Count == 0)
            {
                Debug.LogError($"{nameof(CSVToScriptableObject)} : Table Name Error");
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using UnityEngine;");
            sb.AppendLine();

            sb.AppendLine("public static class Tables");
            sb.AppendLine("{");

            foreach (string tableName in tableNames)
            {
                sb.AppendLine($"\tpublic static {tableName} {tableName};");
            }

            sb.AppendLine();

            sb.AppendLine("\tstatic Tables()");
            sb.AppendLine("\t{");

            foreach (string tableName in tableNames)
            {
                sb.AppendLine($"\t\tif ({tableName} == null)");
                sb.AppendLine($"\t\t\t{tableName} = Load<{tableName}>();");
                sb.AppendLine();
            }

            sb.AppendLine("\t}");
            sb.AppendLine();

            sb.AppendLine("\tpublic static T Load<T>() where T : ScriptableObject");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tT[] asset = Resources.LoadAll<T>(\"\");");
            sb.AppendLine();
            sb.AppendLine("\t\tif (asset == null || asset.Length != 1)");
            sb.AppendLine("\t\t\tthrow new System.Exception($\"{nameof(Tables)} : Tables Load Error\");");
            sb.AppendLine();

            sb.AppendLine("\t\treturn asset[0];");
            sb.AppendLine("\t}");

            sb.AppendLine("}");

            string textsaver = $"{ScriptFolderPath}/Tables.cs";

            if (File.Exists(textsaver))
            {
                File.Delete(textsaver);
            }

            File.AppendAllText(textsaver, sb.ToString());
        }
    }


    #if UNITY_EDITOR

    public class CSVToScriptableWindow : EditorWindow
    {
        [MenuItem("Custom/TableMakerWindow")]
        public static void Init()
        {
            CSVToScriptableWindow window = (CSVToScriptableWindow)EditorWindow.GetWindow(typeof(CSVToScriptableWindow));
            window.minSize = new Vector2(500, 300);
            window.Show();
        }


        public void OnGUI()
        {
            GUILayout.Label("Path Settings", EditorStyles.boldLabel);

            CSVToScriptableObject.CSVFolderPath = EditorGUILayout.TextField("CSV Folder Path", CSVToScriptableObject.CSVFolderPath);
            CSVToScriptableObject.ScriptableFolderPath = EditorGUILayout.TextField("Scriptable Folder Path", CSVToScriptableObject.ScriptableFolderPath);

            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Make Table Script"))
            {
                CSVToScriptableObject.MakeTableScript();
            }
            else if (GUILayout.Button("Make Scriptable Object"))
            {
                CSVToScriptableObject.MakeScriptableObject();
            }

            GUILayout.EndHorizontal();
        }
    }

    #endif
}
