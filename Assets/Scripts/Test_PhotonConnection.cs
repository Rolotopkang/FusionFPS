using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Test_PhotonConnection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "Alpha";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
