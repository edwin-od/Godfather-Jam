using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour {

    [Header("VS Animation:")]
    [SerializeField, Range(0.1f, 10.0f)]
    private float animationTime = 0.5f;   // for each in and out anim
    [SerializeField]
    private GameObject eclaireBas;
    [SerializeField]
    private Vector3 eclaireBasIn = new Vector3(-1.01f, -2.19f, 0f);
    [SerializeField]
    private Vector3 eclaireBasOut = new Vector3(-2.7f, -7.89f, 0f);
    [SerializeField]

    private GameObject eclaireHaut;
    [SerializeField]
    private Vector3 eclaireHautIn = new Vector3(0.54f, 2.23f, 0f);
    [SerializeField]
    private Vector3 eclaireHautOut = new Vector3(3.13f, 8.07f, 0f);
    [SerializeField]
    private GameObject vs;

    [Header("Incorrect Input Feedback:")]

    [SerializeField, Range(1, 5)]
    private int shakeAmount = 1;

    [SerializeField, Range(0.1f, 1f)]
    private float shakeAmplitude = 0.1f;

    [SerializeField, Range(0.1f, 10.0f)]
    private float shakeTime = 1f;

    [Header("Correct Input Feedback:")]

    [SerializeField, Range(0.1f, 10.0f)]
    private float fadeTime = 1f;

    [SerializeField, Range(0.01f, 0.5f)]
    private float expandAmount = 0.1f;

    public static FeedbackManager Get;
    private void Awake()
    {
        if (Get == null)
        {
            Get = this;
        }
        else Destroy(this.gameObject);
    }

    private void Start() {
        eclaireBas.transform.position = eclaireBasOut;
        eclaireHaut.transform.position = eclaireHautOut;
        vs.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
    }

    public void vsAnimationIn() {
        eclaireBas.transform.position = eclaireBasOut;
        eclaireHaut.transform.position = eclaireHautOut;
        vs.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);

        StartCoroutine(vsAnimationInStart());
    }

    public void vsAnimationOut() {
        eclaireBas.transform.position = eclaireBasIn;
        eclaireHaut.transform.position = eclaireHautIn;
        vs.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 1);

        StartCoroutine(vsAnimationOutStart());
    }

    public void correctInputFeedback(InputObject inputObj) {
        StartCoroutine(fadeAndExpandSpriteRenderer(inputObj));
    }

    public void IncorrectInputFeedback(InputObject inputObj) {
        StartCoroutine(shakeObj(inputObj.gameObject));
    }

    private IEnumerator shakeObj(GameObject obj) {

        if(obj == null) 
            yield break;

        float posDepart = obj.transform.position.x;
        float posIntermediaire = 0;

        int realAmount = shakeAmount * 2;
        float time = shakeTime / (2*realAmount);    // l'allee et le retour de chaque 'half rev'
        
        for(int i=0; i < realAmount; i++) {
            float posFinal = (i%2 == 0) ? (posDepart - shakeAmplitude) : (posDepart + shakeAmplitude);

            float timer = 0f;
            while (timer <= time) {
                timer += Time.deltaTime;
                posIntermediaire = Mathf.Lerp(posDepart, posFinal, timer / time);
                if(obj)
                    obj.transform.position = new Vector3(posIntermediaire, obj.transform.position.y, obj.transform.position.z);

                yield return null;
            }
            timer = 0f;
            while (timer <= time) {
                timer += Time.deltaTime;
                posIntermediaire = Mathf.Lerp(posFinal, posDepart, timer / time);
                if(obj)
                    obj.transform.position = new Vector3(posIntermediaire, obj.transform.position.y, obj.transform.position.z);

                yield return null;
            }
        }
        if(obj)
            obj.transform.position = new Vector3(posDepart, obj.transform.position.y, obj.transform.position.z);
    }

    private IEnumerator fadeAndExpandSpriteRenderer(InputObject inputObj) {
        if(inputObj == null) 
            yield break;

        GameObject obj = Instantiate(inputObj.gameObject);
        obj.transform.position = new Vector3(inputObj.gameObject.transform.position.x, inputObj.gameObject.transform.position.y, inputObj.gameObject.transform.position.z);
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        float alpha = spriteRenderer.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime) {
            float scaleDepart = obj.gameObject.transform.localScale.x;
            float intermAlpha = Mathf.Lerp(alpha, 0f, t), intermScale = Mathf.Lerp(scaleDepart, scaleDepart + expandAmount, t);

            Color newColor = new Color(1, 1, 1, intermAlpha);

            if(obj) {
                spriteRenderer.material.color = newColor;
                obj.gameObject.transform.localScale = new Vector3(intermScale, intermScale, intermScale);
            }
            yield return null;
        }
        Destroy(obj);
     }

     private IEnumerator vsAnimationInStart() {
        float timer = 0f;
        while (timer <= animationTime) {
            timer += Time.deltaTime;

            eclaireBas.transform.position = new Vector3(Mathf.Lerp(eclaireBasOut.x, eclaireBasIn.x, timer / animationTime),
                                                        Mathf.Lerp(eclaireBasOut.y, eclaireBasIn.y, timer / animationTime), transform.position.z);
            eclaireHaut.transform.position = new Vector3(Mathf.Lerp(eclaireHautOut.x, eclaireHautIn.x, timer / animationTime),
                                                        Mathf.Lerp(eclaireHautOut.y, eclaireHautIn.y, timer / animationTime), transform.position.z);

            vs.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, Mathf.Lerp(0f, 1f, timer/animationTime));

            yield return null;
        }
        eclaireBas.transform.position = eclaireBasIn;
        eclaireHaut.transform.position = eclaireHautIn;
        vs.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 1);
     }

     private IEnumerator vsAnimationOutStart() {
        float timer = 0f;
        while (timer <= animationTime) {
            timer += Time.deltaTime;

            eclaireBas.transform.position = new Vector3(Mathf.Lerp(eclaireBasIn.x, eclaireBasOut.x, timer / animationTime),
                                                        Mathf.Lerp(eclaireBasIn.y, eclaireBasOut.y, timer / animationTime), transform.position.z);
            eclaireHaut.transform.position = new Vector3(Mathf.Lerp(eclaireHautIn.x, eclaireHautOut.x, timer / animationTime),
                                                        Mathf.Lerp(eclaireHautIn.y, eclaireHautOut.y, timer / animationTime), transform.position.z);

            vs.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, timer/animationTime));

            yield return null;
        }
        eclaireBas.transform.position = eclaireBasOut;
        eclaireHaut.transform.position = eclaireHautOut;
        vs.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
     }
}
