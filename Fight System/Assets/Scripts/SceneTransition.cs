using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Text loadingPercentage;
    public Image loadingProgressBar;

    private static SceneTransition instance;
    public static bool shouldPlayAnimation = false;

    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    private void Start()
    {
        instance = this;

        componentAnimator = GetComponent<Animator>();

        if (shouldPlayAnimation) componentAnimator.SetTrigger("sceneEnd");
    }

    public static void SwitchToScene(string sceneName)
    {
        instance.componentAnimator.SetTrigger("sceneStart");

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        instance.loadingSceneOperation.allowSceneActivation = false;
    }

    public void OnAnimationOver()
    {
        shouldPlayAnimation = true;
        loadingSceneOperation.allowSceneActivation = true;
    }

    private void Update()
    {
        if(loadingSceneOperation != null)
        {
            loadingPercentage.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";
            loadingProgressBar.fillAmount = Mathf.Lerp(loadingProgressBar.fillAmount, loadingSceneOperation.progress,
                Time.deltaTime * 5);
        }
    }
}
