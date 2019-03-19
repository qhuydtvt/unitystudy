using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject reward;
    RectTransform rewardTransform;

    private void Awake()
    {
        reward = GameObject.Find("Reward");
        rewardTransform = reward.GetComponent<RectTransform>();
    }

    void Start()
    {
        reward.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Animate()
    {
        var animator = this.GetComponentInChildren<Animator>();
        animator.Play("Animate");
        StartCoroutine(showReward());
    }

    public void ShowReward()
    {

    }

    IEnumerator showReward()
    {
        yield return new WaitForSeconds(1.15f);
        reward.SetActive(true);
        float scaleX = 0.1f;
        float scaleY = 0.1f;
        float time = 0;
        while (time <= 0.3f)
        {
            print(time);
            rewardTransform.localScale = new Vector3(scaleX, scaleY, 1f);
            scaleX = Mathf.Lerp(scaleX, 1, time / 0.3f);
            scaleY = Mathf.Lerp(scaleY, 1, time / 0.3f);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return null;
    }
}
