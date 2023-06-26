using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

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
        ["fermer bloc"] = RequeteType.FIN,
        ["fermer blocs"] = RequeteType.FIN,
        ["fermer bloque"] = RequeteType.FIN,
    };

    //Premier tri des phrases traduisibles
    public static Dictionary<string, string> motsARemplacer = new Dictionary<string, string>()
    {
        //Plus long
        ["décrémente"] = "--",
        ["incrémente"] = "++",
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

        ["un"] = "1",

        ["entier"] = "int",
        ["chaîne de caractères"] = "string",
        ["chaîne de caractère"] = "string",
        ["chaînes de caractères"] = "string",
        ["chaînes de caractère"] = "string",

        ["égal"] = "=",
        ["égale"] = "=",

        ["et puis"] = "&&",
        //Plus court
        //Pr éviter genre ça : if (variable 1 est supérieur ou = à variable... bruh
    };

    //J'ai passé 10min à initialiser ce truc chuis un ptn de génie
    public static Dictionary<RequeteType, Func<string[], string>> requeteTypeVersAction = new Dictionary<RequeteType, Func<string[], string>>()
    {
        [RequeteType.IF] = TraitementIF,
        [RequeteType.WHILE] = TraitementWHILE,
        [RequeteType.FOR] = TraitementFOR,
        [RequeteType.FONCTION] = TraitementFONCTION,
        [RequeteType.FIN] = TraitementFIN,
        [RequeteType.DECLARATION_FONCTION] = TraitementDECLARATIONFONCTION,
        [RequeteType.DECLARATION_VARIABLE] = TraitementDECLARATIONVARIABLE,
        [RequeteType.VARIABLE] = TraitementVARIABLE,
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
        phraseDecoupee = Decoupe(phraseDeBase);

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




    private string[] Decoupe(string phrase)
    {
        string[] phraseD = phraseDeBase.Split(' ');
        List<string> mots = new List<string>();

        //On enlève tous les mots à supprimer
        for (int i = 0; i < phraseD.Length; i++)
        {
            if (!motsASupprimer.Contains(phraseD[i]))
            {
                mots.Add(phraseD[i]);
            }
        }

        phraseD = new string[mots.Count];

        for (int i = 0; i < phraseD.Length; i++)
        {
            phraseD[i] = mots[i];
        }

        if (phraseD[0].Count() < 2) phraseD = GetTail(phraseD, 1);

        return phraseD;
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

    //=========================TRAITEMENT REQUETE VERS CODE

    private static string TraitementIF(string[] phraseD)
    {
        string code = "";
        int indexAlors = phraseD.Count();
        if (phraseD.Contains("alors"))
        {
            indexAlors = Array.IndexOf(phraseD, "alors");
        }
        for (int i = 1; i <= indexAlors - 1; i++)
        {
            if (!phraseD[i].Equals("variable"))
                code += (i == 1 ? "" : " ") + phraseD[i];
        }

        string[] actionCode = GetTail(phraseD, indexAlors);

        return "if (" + code + ") { " + ActionCode(actionCode);
    }

    private static string TraitementWHILE(string[] phraseD)
    {
        string code = "";

        for (int i = 1; i <= phraseD.Count() - 1; i++)
        {
            if (!phraseD[i].Equals("variable"))
                code += (i == 1 ? "" : " ") + phraseD[i];
        }

        return "while (" + code + ") { ";
    }

    private static string TraitementFOR(string[] phraseD)
    {
        string code = "";
        string variable = phraseD[1];
        code += variable + " = ";

        int indexAllant = Array.IndexOf(phraseD, "allant");
        int indexJusquA = Array.IndexOf(phraseD, "à");

        code += phraseD[indexAllant + 2] + "; " + variable + "<" + phraseD[indexJusquA + 1];

        return "for (int " + code + "; " + variable + "++) {";
    }

    private static string TraitementFONCTION(string[] phraseD)
    {
        return "Faire fonction";
    }

    private static string TraitementDECLARATIONFONCTION(string[] phraseD)
    {
        if (phraseD.Count() >= 4)
        {
            string[] camel = new string[phraseD.Count() - 3];

            for (int i = 3; i < phraseD.Count(); i++) camel[i - 3] = phraseD[i];

            int indexPrenant = 0;
            string[] suite = new string[phraseD.Count() - indexPrenant];
            if (phraseD.Contains("prenant"))
            {
                indexPrenant = Array.IndexOf(phraseD, "prenant");

                suite = new string[phraseD.Count() - indexPrenant];
                for (int i = indexPrenant + 1; i < phraseD.Count(); i++)
                {
                    suite[i - indexPrenant] = phraseD[i];
                }
            }

            return phraseD[2] + " " + ToCamelCase(camel) + "("+(suite.Count() > 3 ? suite[1] +" "+ suite[2] : "") +") {";
        }
        return "";
    }

    private static string TraitementDECLARATIONVARIABLE(string[] phraseD)
    {
        if (phraseD.Count() >= 4)
        {
            return phraseD[2] + " " + ToCamelCase(GetTail(phraseD, 3))+";";
        }
        return "";
    }

    private static string TraitementVARIABLE(string[] phraseD)
    {
        string resString = "";
        
        for (int i=1; i< phraseD.Count(); i++)
        {
            resString += phraseD[i];
        }

        return resString+";";
    }

    private static string TraitementFIN(string[] phraseD)
    {
        return "}";
    }

    private static string ActionCode(string[] phraseD)
    {
        if (phraseD.Count() > 0)
        {
            string code = "";

            for (int i = 0; i < phraseD.Count(); i++)
            {
                code += (i == 0 ? "" : " ") + phraseD[i];
            }

            return "\n" + code + ";";
        }
        return "";
    }

    //============================= FONCTIONS UTILES

    private static string ToCamelCase(string[] inputList)
    {
        if (inputList.Count() > 0)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < inputList.Count(); i++)
            {
                string currentWord = inputList[i];
                if (currentWord != null)
                {
                    if (currentWord.Count() > 0)
                    {
                        if (i == 0)
                        {
                            sb.Append(char.ToLower(currentWord[0]));
                        }
                        else
                        {
                            sb.Append(char.ToUpper(currentWord[0]));
                        }

                        sb.Append(currentWord.Substring(1));
                    }
                }
            }

            return sb.ToString();
        }
        return "";
    }
    public static string[] GetTail(string[] arr, int skipCount)
    {
        if (skipCount >= arr.Length)
        {
            return new string[0];
        }

        int tailLength = arr.Length - skipCount;
        string[] tail = new string[tailLength];
        Array.Copy(arr, skipCount, tail, 0, tailLength);

        return tail;
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
}