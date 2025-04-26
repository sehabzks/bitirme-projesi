using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string transitionTo;

    [SerializeField] private Transform startPoint;

    [SerializeField] private Vector2 exitDirection;

    [SerializeField] private float exitTime;

    PlayerMovement playerMovement;

    private void Start()
    {
        if(GameManager.Instance.transitionedFromScene == transitionTo)
        {
            PlayerController.Instance.transform.position = startPoint.position;

            StartCoroutine(playerMovement.WalkIntoNewScene(exitDirection, exitTime));
        }

        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            GameManager.Instance.transitionedFromScene = SceneManager.GetActiveScene().name;

            PlayerController.Instance.pState.cutscene = true;
            PlayerController.Instance.pState.invincible = true;

            StartCoroutine(UIManager.Instance.sceneFader.FadeAndLoadScene(SceneFader.FadeDirection.In, transitionTo));
        }
    }
}
