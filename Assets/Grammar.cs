using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;

public class Grammar : MonoBehaviour
{

    private DictationRecognizer dictationRecognizer;
    private TextController textController;
    public ArrayList requetes;

    void Start()
    {
        textController = GetComponent<TextController>();

        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += onDictationResult;
        //dictationRecognizer.DictationHypothesis += onDictationHypothesis;
        dictationRecognizer.DictationError += onDictationError;

        dictationRecognizer.Start();

        requetes = new ArrayList();
    }

    void onDictationResult(string text, ConfidenceLevel confidence)
    {
        // write your logic here
        Debug.Log("Phrase trouvée : "+text);

        Requete r = new Requete(text);
        if (r.aQuelqueChose())
        {
            requetes.Add(r);
            textController.AddTextAndLine(r.codeSortant);
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
}