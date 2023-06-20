using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class Grammar : MonoBehaviour
{

    private DictationRecognizer dictationRecognizer;
    private TextController textController;
    public List<Requete> requetes;

    void Start()
    {
        textController = GetComponent<TextController>();
        textController.setArrayRequetes(requetes);

        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += onDictationResult;
        //dictationRecognizer.DictationHypothesis += onDictationHypothesis;
        dictationRecognizer.DictationError += onDictationError;

        dictationRecognizer.Start();

        requetes = new List<Requete>();
    }

    void onDictationResult(string text, ConfidenceLevel confidence)
    {
        // write your logic here
        Debug.Log("Phrase trouvée : " + text);

        Requete r = new Requete(text);
        if (r.aQuelqueChose())
        {
            requetes.Add(r);
            textController.UpdateText(requetes);
        }
    }

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






    public static Dictionary<string, string> motsARemplacer = new Dictionary<string, string>()
    {
        ["est égal à"] = "==",
        ["est égale à"] = "==",
        ["égal"] = "=",
        ["égal"] = "==",
        ["et puis"] = "&&",
        ["ou"] = "||"
    };

    private string RemplacerMots(string baseStr)
    {
        for (int i = 0; i < motsARemplacer.Count(); i++)
        {
            string key = motsARemplacer.Keys.ToArray()[i];
            baseStr = RemplacerPortionString(baseStr, key, motsARemplacer[key]);
        }

        return baseStr;
    }

    private string RemplacerPortionString(string source, string portionARemplacer, string nouvellePortion)
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