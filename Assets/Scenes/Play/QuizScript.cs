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
                StartCoroutine(SubmitAnswer(choiceNo));
            });
        }
    }

    IEnumerator SubmitAnswer(int choiceNo)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("question", "We are young, right?"));
        UnityWebRequest www = UnityWebRequest.Post(URL.API + "/quiz", formData);
        textLoading.enabled = true;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            textLoading.enabled = false;
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
