using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Requete
{
    RequeteType type;
    public string phraseDeBase;
    public string[] phraseDecoupee;
    public string codeSortant;

    public static string[] motsASupprimer = {"euh", "la", "le"};

    public Requete(string p)
    {
        phraseDeBase = p.ToLower();
        if (phraseDeBase != null)
        {
            Process();
        }
    }

    public void Process()
    {
        Decoupe();

        for (int i = 0;i<phraseDecoupee.Length;i++)
        {
            codeSortant += (i > 0 ? " "+phraseDecoupee[i] : phraseDecoupee[i]);
        }
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

    public bool aQuelqueChose() => (codeSortant != null && codeSortant.Length > 0 ? true : false);
}
