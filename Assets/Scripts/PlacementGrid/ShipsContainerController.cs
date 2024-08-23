using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipsContainerController : MonoBehaviour
{
    private static bool _hasTransferred;
    private static GameObject _container;
    [SerializeField] private GameObject container;

    private void Awake()
    {
        _container = container;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        _hasTransferred = false;
        DontDestroyOnLoad(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_hasTransferred)
        {
            _hasTransferred = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
        else
        {
            _hasTransferred = true;
        }
    }

    public static void ChangePosition(float x, float y)
    {
        _container.transform.position = new Vector3(x, 0, y);
    }
}