using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChanger : MonoBehaviour
{
    public int sceneIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    public void SceneChanger()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
