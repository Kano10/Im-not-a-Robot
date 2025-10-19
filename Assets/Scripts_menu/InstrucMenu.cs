using UnityEngine;
using UnityEngine.SceneManagement;
public class InstrucMenu : MonoBehaviour
{
    public void Back(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

}