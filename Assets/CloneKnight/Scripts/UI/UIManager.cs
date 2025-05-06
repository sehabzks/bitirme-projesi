public class UIManager : PersistentSingleton<UIManager>
{
    public SceneFader sceneFader;

    protected override void Awake()
    {
        base.Awake();
        sceneFader = GetComponentInChildren<SceneFader>();
    }
}
