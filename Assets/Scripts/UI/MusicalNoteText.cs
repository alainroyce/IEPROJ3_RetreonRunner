using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class MusicalNoteText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void OnSpawnText(ENoteHitStatus status, Transform playerTransform, float lanePos)
    {
        if (status == ENoteHitStatus.Perfect)
        {
            text.text = "Perfect!";
            text.color = Color.magenta;
        }
        else if (status == ENoteHitStatus.Good)
        {
            text.text = "Good";
            text.color = Color.green;
        }
        else if (status == ENoteHitStatus.Miss)
        {
            text.text = "Miss";
            text.color = Color.red;
        }

        StartCoroutine(TextAnimationCoroutine(playerTransform, lanePos));
    }

    private IEnumerator TextAnimationCoroutine(Transform playerTransform, float lanePos)
    {
        while (transform.position.y < playerTransform.position.y + 2f)
        {
            Vector3 newPos = new Vector3(lanePos, transform.position.y + 0.01f, playerTransform.position.z);
            transform.position = newPos;
            yield return null;
        }

        DestroyImmediate(gameObject);
    }
}
