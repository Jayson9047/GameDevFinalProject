using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }
}
