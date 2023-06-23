using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class Grammar : MonoBehaviour
{
    //Le reconnaisseur de mots
    private DictationRecognizer dictationRecognizer;

    //Les displays de texte
    private TextController textePrincipal, textErreurs;

    //Liste de toutes les requetes
    public List<Requete> requetes;

    void Start()
    {
        //On trouve les deux displays de texte
        textePrincipal = GameObject.Find("Texte").GetComponent<TextController>();
        textePrincipal.setArrayRequetes(requetes);

        textErreurs = GameObject.Find("Erreurs").GetComponent<TextController>();
        textErreurs.setArrayRequetes(requetes);

        dictationRecognizer = new DictationRecognizer();
        //Ajout de la fonction onDictationResult à l'event
        dictationRecognizer.DictationResult += onDictationResult;
        //dictationRecognizer.DictationHypothesis += onDictationHypothesis;
        dictationRecognizer.DictationError += onDictationError;

        dictationRecognizer.Start();

        requetes = new List<Requete>();

        
        Requete r = new Requete("Si la variable test est égale à la variable test2");
        requetes.Add(r);

        Requete r2 = new Requete("fermer bloc");
        requetes.Add(r2);

        textePrincipal.UpdateText(requetes);
        textErreurs.UpdateText(requetes);
        
    }

    void onDictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("Phrase trouvée : " + text);

        Requete r = new Requete(text);
        //Si la requete n'est pas vide
        if (r.aQuelqueChose())
        {
            requetes.Add(r);
            textePrincipal.UpdateText(requetes);
            textErreurs.UpdateText(requetes);
        }
    }

    //Utilisé que en débug
    void onDictationHypothesis(string text)
    {
        // write your logic here
        Debug.LogFormat("Dictation hypothesis: {0}", text);
    }
    void onDictationComplete(DictationCompletionCause cause)
    {
        // write your logic here
        if (cause != DictationCompletionCause.Complete)
            Debug.LogErrorFormat("Bug quelque part : {0}.", cause);
    }
    void onDictationError(string error, int hresult)
    {
        // write your logic here
        Debug.LogErrorFormat("Euuuh oep : {0}; HResult = {1}.", error, hresult);
    }

}