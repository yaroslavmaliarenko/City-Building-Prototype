using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {

    [SerializeField] Image loadCircle;
    [SerializeField] Text percentText;


    // Use this for initialization
    void Start () {

        StartCoroutine(LoadNewScene());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator LoadNewScene()
    {
       AsyncOperation operation  = SceneManager.LoadSceneAsync("MainScene");

        while (!operation.isDone)
        {
            yield return null;
            loadCircle.fillAmount = operation.progress / 0.9f;
            percentText.text = ((int)(operation.progress/0.9f * 100)) + "%";
            
        }

    }
}
