using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

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

    public static Dictionary<string, RequeteType> phraseToRequeteDico = new Dictionary<string, RequeteType>()
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

        for (int i = 0; i < phraseDecoupee.Length; i++)
        {
            codeSortant += (i > 0 ? " " + phraseDecoupee[i] : phraseDecoupee[i]);
        }

        type = DefinirRequete();

        if (type == RequeteType.NULL)
        {
            UnityEngine.Debug.Log("Requete pas reconnue");
        }
        else
        {
            codeSortant = TransformerEnCode(phraseDecoupee);
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

    private RequeteType DefinirRequete()
    {
        for (int i = 0; i < phraseToRequeteDico.Count(); i++)
        {
            //Tableau de plusieurs mots par exemple : Key : "tant que" -> ["tant", "que"] voilà voilà
            string[] motsDeRequete = phraseToRequeteDico.Keys.ToArray()[i].Split(" ");
            int compt = 0;
            for (int j = 0; j < motsDeRequete.Count(); j++)
            {
                if (motsDeRequete[j].Equals(phraseDecoupee[j])) compt++;
            }

            if (compt == motsDeRequete.Count())
            {
                return phraseToRequeteDico[phraseToRequeteDico.Keys.ToArray()[i]];
            }
        }

        return RequeteType.NULL;

        /*
        if (motPourRequeteType.ContainsKey(phraseDecoupee[0]))
        {
            type = motPourRequeteType[phraseDecoupee[0]];
        }
        */
    }

    public bool aQuelqueChose() => (codeSortant != null && codeSortant.Length > 0 ? true : false);

    private string TransformerEnCode(string[] phraseD)
    {
        string res = "";

        switch (type)
        {
            case RequeteType.NULL:
                return "";
            case RequeteType.IF:
                string restePhrase = "";
                for (int i = 0; i<phraseD.Count(); i++) restePhrase += " "+phraseD[i];
                return "if ("+restePhrase;
            case RequeteType.DECLARATION_VARIABLE:
                return phraseD[2] +" "+ phraseD[3] + ";";
            case RequeteType.DECLARATION_FONCTION:
                break;
            case RequeteType.FONCTION:
                break;
            case RequeteType.VARIABLE:
                break;
            case RequeteType.FOR:
                break;
            case RequeteType.WHILE:
                break;

        }

        return res;
    }
}
