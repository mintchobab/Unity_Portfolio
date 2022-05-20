using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView graphView;
    private string fileName = "New Narrative";


    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        DialogueGraph window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }


    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
    }


    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView
        {
            name = "Dialogue Graph"
        };

        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolBar()
    {
        Toolbar toolbar = new Toolbar();

        TextField fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(e => fileName = e.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        var nodeCreateButton = new Button(() => { graphView.CreateNode("Dialogue Node"); });

        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }


    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("파일명 오류", "올바른 파일명을 입력해주세요.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(graphView);

        if (save)
        {
            saveUtility.SaveGraph(fileName);
        }
        else
        {
            saveUtility.LoadGraph(fileName);
        }
    }


    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

}