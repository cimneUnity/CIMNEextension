using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainScript : MonoBehaviour
{
    private float time = 1f;
    private bool accident;
    private int it = 12;
    private byte alfa = 60;
    private GameObject imagePain, camera;
    private void Start()
    {
        if (GameObject.Find("FirstPersonPlayer").GetComponent<PlayerManager>().walkSpeed > 0.5f) GameObject.Find("FirstPersonPlayer").GetComponent<PlayerManager>().walkSpeed -= 0.5f;
        if (GameObject.Find("FirstPersonPlayer").GetComponent<PlayerManager>().runSpeed > 1f) GameObject.Find("FirstPersonPlayer").GetComponent<PlayerManager>().runSpeed -= 1f;

        imagePain = GameObject.Find("PainImage");
        camera = GameObject.Find("PlayerCamera");
        Vector3 vec = camera.transform.position;
        vec.y = vec.y - 0.6f;
        camera.transform.position = vec;
        imagePain.GetComponent<UnityEngine.UI.Image>().color = new Color32(254, 0, 0, alfa);
    }

    void Update()
    {
        if (it == 0)
        { 
            Destroy(gameObject);
            Destroy(this);
        } else
        {
            --it;
            alfa -= 5;
            imagePain.GetComponent<UnityEngine.UI.Image>().color = new Color32(254, 0, 0, alfa);
            Vector3 vec = camera.transform.position;
            vec.y = vec.y + 0.05f;
            camera.transform.position = vec;
        }
    }
}
