using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace lsy
{
    public class LoadingSceneController : MonoBehaviour
    {
        [SerializeField]
        private Image progressBar;


        private void Start()
        {
            StartCoroutine(LoadScene());
        }


        private IEnumerator LoadScene()
        {
            yield return null;

            AsyncOperation ao = SceneManager.LoadSceneAsync(2);
            ao.allowSceneActivation = false;

            float elapsedTime = 0f;
            progressBar.fillAmount = 0f;

            while (!ao.isDone)
            {
                elapsedTime += Time.deltaTime;

                if (ao.progress < 0.9f)
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, ao.progress, elapsedTime);
                }
                else
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, elapsedTime);

                    if (progressBar.fillAmount >= 1f)
                    {
                        ao.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }
    }
}
