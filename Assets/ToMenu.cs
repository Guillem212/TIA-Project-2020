using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMenu : MonoBehaviour
{
    // Start is called before the first frame update
private void Awake() {
    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
}
}
