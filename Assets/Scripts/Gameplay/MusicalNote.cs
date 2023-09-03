using UnityEngine;

public class MusicalNote : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider = null;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject musicalNoteTextPrefab = null;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        transform.Rotate(0, 200f * Time.deltaTime, 0);

        if (playerTransform != null && transform.position.z - playerTransform.position.z < -3.5f)
        {
            ScoreManager.Instance.OnMiss();

            GameObject go = GameObject.Instantiate(musicalNoteTextPrefab, playerTransform.position, Quaternion.identity);
            MusicalNoteText script = go.GetComponent<MusicalNoteText>();
            script.OnSpawnText(ENoteHitStatus.Miss, playerTransform, transform.position.x);

            DestroyImmediate(gameObject);
        }
    }

    public void OnMusicalNoteHit()
    {
        float distance = Mathf.Abs(transform.position.z - playerTransform.position.z);
        
        if (distance / sphereCollider.radius < 0.4f)
        {
            ScoreManager.Instance.OnMusicalNoteHit(ENoteHitStatus.Perfect);

            GameObject go = GameObject.Instantiate(musicalNoteTextPrefab, playerTransform.position, Quaternion.identity);
            MusicalNoteText script = go.GetComponent<MusicalNoteText>();
            script.OnSpawnText(ENoteHitStatus.Perfect, playerTransform, transform.position.x);

            DestroyImmediate(gameObject);
        }
        else
        {
            ScoreManager.Instance.OnMusicalNoteHit(ENoteHitStatus.Good);

            GameObject go = GameObject.Instantiate(musicalNoteTextPrefab, playerTransform.position, Quaternion.identity); 
            MusicalNoteText script = go.GetComponent<MusicalNoteText>();
            script.OnSpawnText(ENoteHitStatus.Good, playerTransform, transform.position.x);

            DestroyImmediate(gameObject);
        }
    }
}
