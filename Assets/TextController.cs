using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    private TextMeshPro textMesh;
    public List<Requete> requetes;

    private void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        Debug.Log(textMesh.text);
        // Accédez au texte et modifiez-le
        textMesh.text = "Dis un truc";
    }

    public void UpdateText(List<Requete> req)
    {
        requetes = req;
        if (requetes != null)
        {
            //Debug.Log("update de texte");

            textMesh.text = "";
            for (int i = 0; i < requetes.Count; i++)
            {
                textMesh.text += requetes[i].codeSortant + "\n";
            }
        }
    }

    public void setArrayRequetes(List<Requete> req)
    {
        requetes = req;
    }

    /*
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
        if (text != null && textMesh.text != null)
        {
            textMesh.text += "\n" + text;
        }
    }
    */
}

