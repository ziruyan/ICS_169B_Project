using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_MultiScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		int i1 = MultiSceneManager.get_Win();
        Debug.Log(i1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
