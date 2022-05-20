using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


public class DialogueNode : Node
{
    public string GUID;

    public string DialogueText;

    public bool EntryPoint = false;
}
