using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    private TextMeshPro textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        // Accédez au texte et modifiez-le
        textMesh.text = "Dis un truc";
    }

    public void ChangeText(string text)
    {
        textMesh.text = text;
    }

    public void AddText(string text)
    {
        textMesh.text += text;
    }

    public void AddTextBeginning(string text)
    {
        textMesh.text = text + textMesh.text;
    }
    public void AddTextAndLine(string text)
    {
        textMesh.text += "\n"+text;
    }
}

