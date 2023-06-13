using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Requete
{
    RequeteType type;
    public string phraseDeBase;
    public string[] phraseDecoupee;
    public string codeSortant;

    public static string[] motsASupprimer = {
        "euh",
        "la",
        "le"
    };

    public static Dictionary<string, RequeteType> motPourRequeteType = new Dictionary<string, RequeteType>()
    {
        ["si"] = RequeteType.IF,
        ["pour"] = RequeteType.FOR,
        ["tant que"] = RequeteType.WHILE,
        ["nouvelle fonction"] = RequeteType.DECLARATION_FONCTION,
        ["nouvelle variable"] = RequeteType.DECLARATION_VARIABLE,
        ["variable"] = RequeteType.VARIABLE,
        ["fonction"] = RequeteType.FONCTION,
    };

    public Requete(string p)
    {
        phraseDeBase = p.ToLower();
        if (phraseDeBase != null)
        {
            Process();
        }
        UnityEngine.Debug.Log(type);
    }

    public void Process()
    {
        Decoupe();

        for (int i = 0;i<phraseDecoupee.Length;i++)
        {
            codeSortant += (i > 0 ? " "+phraseDecoupee[i] : phraseDecoupee[i]);
        }

        DefinirRequete();
    }

    private void Decoupe()
    {
        phraseDecoupee = phraseDeBase.Split(' ');
        ArrayList mots = new ArrayList();
        for (int i = 0; i < phraseDecoupee.Length; i++)
        {
            if (!motsASupprimer.Contains(phraseDecoupee[i]))
            {
                mots.Add(phraseDecoupee[i]);
            }
        }

        phraseDecoupee = new string[mots.Count];

        for (int i = 0; i < phraseDecoupee.Length; i++)
        {
            phraseDecoupee[i] = (string)mots[i];
        }
    }

    private void DefinirRequete()
    {
        if (motPourRequeteType.ContainsKey(phraseDecoupee[0]))
        {
            type = motPourRequeteType[phraseDecoupee[0]];
        }
    }

    public bool aQuelqueChose() => (codeSortant != null && codeSortant.Length > 0 ? true : false);
}
