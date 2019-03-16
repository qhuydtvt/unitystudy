using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Facebook.Unity;

public class LoginScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() => {
                if (FB.IsInitialized)
                {
                    print("Initialized");
                    FB.ActivateApp();
                    print("Activated");
                }
                else
                {
                    print("Init failed");
                }
            });
        }
        else
        {
            print("Initialized");
        }
    }

    public void Login()
    {
        var permissions = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
