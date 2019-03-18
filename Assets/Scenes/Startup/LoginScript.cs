using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using Newtonsoft.Json;

public class LoginBody
{
    string accessToken;
    public LoginBody(string at)
    {
        this.accessToken = at;
    }
}

public class LoginScript : MonoBehaviour
{
    public Button PlayButton;
    // Start is called before the first frame update
    void Awake()
    {
        PlayButton.gameObject.SetActive(false);
        PlayButton.onClick.AddListener(() => SceneManager.LoadScene("PlayScene", LoadSceneMode.Single));
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            print("Init OK");
            // Signal an app activation App Event
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    void Start()
    {
    }

    public void Login()
    {
        var permissions = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID

            Debug.Log("Logged in OK");

            Debug.Log(aToken);
            Debug.Log(aToken.TokenString);

            StartCoroutine(Login("https://game-serivce.herokuapp.com/api/v1/auth", new LoginBody(aToken.TokenString)));

            //PlayButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Login(string url, object data)
    {
        var bodyJsonString = JsonConvert.SerializeObject(data);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        Debug.Log("Response: " + request.downloadHandler.text);
    }
}
