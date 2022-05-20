using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements.Experimental;
using UnityEditor.Experimental.GraphView;


public class GraphSaveUtility
{
    private DialogueGraphView targetGraphView;

    private List<Edge> edges => targetGraphView.edges.ToList();
    private List<DialogueNode> nodes => targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();


    public static GraphSaveUtility GetInstance(DialogueGraphView targetView)
    {
        return new GraphSaveUtility
        {
            targetGraphView = targetView
        };
    }


    public void SaveGraph(string fileName)
    {
        if (edges.Any())
            return;

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedPorts = edges.Where(x => x.input.node != null).ToArray();

        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }


        foreach (var dialogueNode in nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                GUID = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                position = dialogueNode.GetPosition().position
            });
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {

    }
}
