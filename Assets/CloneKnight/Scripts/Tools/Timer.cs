using UnityEngine;

[System.Serializable]
public class Timer
{
    public float duration;
    private float time;

    public bool IsRunning => time < duration;

    public void Tick()
    {
        if (!IsRunning) return;
        time += Time.deltaTime;
    }

    public void Reset() => time = 0f;

    public bool IsFinished() => !IsRunning;
}

