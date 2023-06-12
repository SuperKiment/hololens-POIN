using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeyRecognizer : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, Action> dico = new Dictionary<string, Action>();

    private void Start()
    {
        keywordRecognizer = new KeywordRecognizer(dico.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += PhraseReconnue;
        keywordRecognizer.Start();
    }

    private void PhraseReconnue(PhraseRecognizedEventArgs phrase)
    {

    }
}
