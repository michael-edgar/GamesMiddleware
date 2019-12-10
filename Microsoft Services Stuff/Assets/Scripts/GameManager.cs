using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxAsteroids = 10;
    [SerializeField] private GameObject asteroidPrefab;
    private KeywordRecognizer _keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private readonly List<GameObject> _asteroids = new List<GameObject>();

    private void Start()
    {
        keywords.Add( "Fire", () => CheckIfAnyAsteroidsshouldBeDestroyed(true));
        _keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();
    }

    private void Fire()
    {
        
        print("You said FIRE!!!");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            // Code from: https://answers.unity.com/questions/899037/applicationquit-not-working-1.html
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        if (_asteroids.Count >= 1)
        {
            CheckIfAnyAsteroidsshouldBeDestroyed(false);
        }
        
        if (_asteroids.Count < maxAsteroids)
        {
            GenerateAsteroid();
        }
    }

    private void CheckIfAnyAsteroidsshouldBeDestroyed(bool shouldBeDestroying)
    {
        if (shouldBeDestroying)
        {
            print("You said FIRE!!!");
        }
        // Code from: https://forum.unity.com/threads/invalidoperationexception-collection-was-modified-enumeration-operation-may-not-execute.252443/
        List<int> indexesToRemove = new List<int>();
        int asteroidCount = _asteroids.Count;
        for (int i = 0; i < asteroidCount; i++)
        {
            GameObject asteroid = _asteroids[i];
            if (asteroid.GetComponent<AsteroidController>().ShouldBeDestroyed(shouldBeDestroying))
            {
                indexesToRemove.Add(i);
            }
        }

        if (indexesToRemove.Count > 0)
        {
            foreach (int index in indexesToRemove)
            {
                DestroyAsteroid(_asteroids[index]);
            }
        }
    }
    
    // Note there is approx 2 second delay between saying fire and it recognizing the input, it does work though
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    private void GenerateAsteroid()
    {
        if (asteroidPrefab == null)
        {
            Debug.LogError("No prefab set for asteroid");
        }

        GameObject newAsteroid = Instantiate(asteroidPrefab);
        newAsteroid.name = "Asteroid " +(_asteroids.Count + 1)+"";
        newAsteroid.GetComponent<AsteroidController>().SetAsteroidPosition(transform.position.z + 25);
        _asteroids.Add(newAsteroid);
    }

    private void DestroyAsteroid(GameObject asteroid)
    {
        _asteroids.Remove(asteroid);
        Destroy(asteroid);
    }
}
