using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class Requete
{
    public RequeteType type;
    public string phraseDeBase;
    public string[] phraseDecoupee;
    public string codeSortant;

    //Pour enlever les parasites
    public static string[] motsASupprimer = {
        "euh",
        "la",
        "le"
    };

    //Si on commence la phrase par un de ces strings, on définit la requête
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

    //Premier tri des phrases traduisibles
    public static Dictionary<string, string> motsARemplacer = new Dictionary<string, string>()
    {
        //Plus long
        ["est inférieur ou égal à"] = "<=",
        ["est inférieure ou égale à"] = "<=",
        ["est supérieur ou égal à"] = ">=",
        ["est supérieure ou égale à"] = ">=",
        ["est supérieur à"] = ">",
        ["est supérieure à"] = ">",
        ["est inférieur à"] = "<",
        ["est inférieure à"] = "<",
        ["est égal à"] = "==",
        ["est égale à"] = "==",
        ["ou bien"] = "||",

        ["égal"] = "=",
        ["égale"] = "=",

        ["et puis"] = "&&",
        //Plus court
        //Pr éviter genre ça : if (variable 1 est supérieur ou = à variable... bruh
    };

    //J'ai passé 10min à initialiser ce truc chuis un ptn de génie
    public static Dictionary<RequeteType, Func<string[], string>> requeteTypeVersAction = new Dictionary<RequeteType, Func<string[], string>>()
    {
        [RequeteType.IF] = TraitementIF
    };

    public Requete(string p)
    {
        phraseDeBase = p.ToLower();
        if (phraseDeBase != null)
        {
            Process();
        }
        UnityEngine.Debug.Log("Type de requete : " + type);
    }

    public void Process()
    {
        //Premier tri
        phraseDeBase = RemplacerMots(phraseDeBase);

        //On enleve les parasites
        Decoupe();

        //Recréation de la phrase dans le code sortant
        for (int i = 0; i < phraseDecoupee.Length; i++)
        {
            codeSortant += (i > 0 ? " " + phraseDecoupee[i] : phraseDecoupee[i]);
        }

        type = DefinirRequete();

        if (type == RequeteType.NULL) UnityEngine.Debug.Log("Requete pas reconnue");
        else
            //Si la phrase a un type de requête on transforme en code ! 
            codeSortant = TransformerEnCode(phraseDecoupee);

    }

    private void Decoupe()
    {
        phraseDecoupee = phraseDeBase.Split(' ');
        List<string> mots = new List<string>();

        //On enlève tous les mots à supprimer
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
            phraseDecoupee[i] = mots[i];
        }
    }
    private RequeteType DefinirRequete()
    {
        //Si on a plus qu'un mot
        if (phraseDecoupee.Count() > 1)
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
        //Eh wola ce truc m'a pété le crane mais qu'est-ce que c'est pratique
        return requeteTypeVersAction[type](phraseD);


        /*                  Switch case de avant que ça soit un dico
        switch (type)
        {
            case RequeteType.NULL:
                return "";
            case RequeteType.IF:
                string restePhrase = "";
                for (int i = 0; i < phraseD.Count(); i++) restePhrase += " " + (phraseD[i].Equals("si") ? "" : phraseD[i]);
                return "if (" + restePhrase+") {";
            case RequeteType.DECLARATION_VARIABLE:
                return phraseD[2] + " " + phraseD[3] + ";";
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
        */


        //return res;
    }

    private static string RemplacerMots(string baseStr)
    {
        for (int i = 0; i < motsARemplacer.Count(); i++)
        {
            string key = motsARemplacer.Keys.ToArray()[i];
            baseStr = RemplacerPortionString(baseStr, key, motsARemplacer[key]);
        }

        return baseStr;
    }
    private static string RemplacerPortionString(string source, string portionARemplacer, string nouvellePortion)
    {
        int index = source.IndexOf(portionARemplacer);

        if (index != -1)
        {
            source = source.Remove(index, portionARemplacer.Length);
            source = source.Insert(index, nouvellePortion);
        }

        return source;
    }

    private static string TraitementIF(string[] phraseD)
    {
        string restePhrase = "";
        for (int i = 0; i < phraseD.Count(); i++) restePhrase += " " + (phraseD[i].Equals("si") ? "" : phraseD[i]);
        return "if (" + restePhrase + ") {";
    }
}
