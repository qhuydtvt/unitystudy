using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Facebook.Unity;


public class QuizScript : MonoBehaviour
{
    public Text textLoading;
    public Text question;
    public Button[] ChoiceButtons;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTest());
        for(int i = 0; i < ChoiceButtons.Length; i++)
        {
            Button choiceButton = ChoiceButtons[i];
            choiceButton.onClick.AddListener(() => {
                print(choiceButton.name);
                int choiceNo = int.Parse(choiceButton.name.Replace("BtnChoice", ""));
                SubmitAnswer(choiceNo);
            });
        }
    }

    void SubmitAnswer(int choiceNo)
    {
        string URL = "https://reqres.in/api/users";
        StartCoroutine(PostRequest(URL, new User()));
    }

    IEnumerator PostRequest(string url, object data)
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

    //Wait for the www Request
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    public void Share()
    {
        FB.ShareLink(
            new System.Uri("https://techkids.vn"),
            "Check it out", "This one is good",
            new System.Uri("https://via.placeholder.com/300x300")
        );
    }

    IEnumerator GetTest()
    {
        this.textLoading.enabled = true;
        UnityWebRequest www = UnityWebRequest.Get(URL.API + "/quiz");
        yield return www.SendWebRequest();
        this.textLoading.enabled = false;
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Quiz[] qList = JsonConvert.DeserializeObject<Quiz[]>(www.downloadHandler.text);
            if (qList.Length > 0)
            {
                askQuestion(qList[0]);
            }
        }
    }

    private void askQuestion(Quiz q)
    {
        this.question.text = q.question;
        int length = Mathf.Min(ChoiceButtons.Length, q.choices.Length);
        for(int i = 0; i < length; i++) {
            this.ChoiceButtons[i].GetComponentInChildren<Text>().text = q.choices[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
