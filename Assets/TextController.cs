using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    private TextMeshPro textMesh;
    public List<Requete> requetes;
    public bool isErreurs;

    RequeteType[] indenteurs =
    {
        RequeteType.IF,
        RequeteType.FOR,
        RequeteType.WHILE,
        RequeteType.DECLARATION_FONCTION,

    };

    private void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        Debug.Log(textMesh.text);
        // Accédez au texte et modifiez-le
        textMesh.text = (isErreurs ? "Ici s'affichent les erreurs" : "Dis un truc");
    }

    public void UpdateText(List<Requete> req)
    {
        if (!isErreurs)
        {
            requetes = req;
            if (requetes != null)
            {
                //Debug.Log("update de texte");
                int comptIndent = 0;

                textMesh.text = "";
                for (int i = 0; i < requetes.Count; i++)
                {
                    if (requetes[i].type != RequeteType.NULL)
                    {
                        //Debug.Log(comptIndent);
                        string indent = "";

                        if (requetes[i].type == RequeteType.FIN) comptIndent--;

                        for (int j = 0; j < comptIndent; j++) indent += "   ";

                        textMesh.text += indent+requetes[i].codeSortant + "\n";

                        if (indenteurs.Contains(requetes[i].type)) comptIndent++;
                    }

                }
            }
        }
        else
        {
            if (req[req.Count - 1].type == RequeteType.NULL)
            {
                ChangeText("Phrase non reconnue : " + req[req.Count - 1].phraseDeBase);
            }
        }
    }

    public void setArrayRequetes(List<Requete> req)
    {
        requetes = req;
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
        if (text != null && textMesh.text != null)
        {
            textMesh.text += "\n" + text;
        }
    }

}

