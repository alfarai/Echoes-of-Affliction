using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueText : ScriptableObject
{
    public string speaker;
    [TextArea(5,10)]
    public string[] paragraphs;
}
