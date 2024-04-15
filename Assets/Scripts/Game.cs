using UnityEngine;

[CreateAssetMenu(fileName = "Game")]
public class Game : ScriptableObject
{
    public string heroStatus { get; set; }
    public int eggCount { get; private set; } = 0;
    public float eggCooldown = 0.0f;
    public bool hideMode { get; set; } = false;
    public bool pauseMode { get; set; } = false;
    public bool sequentialMode { get; set; } = false;

    public void IncrementEggCount()
    {
        ++eggCount;
    }
    public void DecrementEggCount()
    {
        --eggCount;
    }
    public int enemyCount { get; private set; } = 0;
    public void IncrementEnemyCount()
    {
        ++enemyCount;
    }
    public void DecrementEnemyCount()
    {
        --enemyCount;
    }
    public int enemyTouched { get; private set; } = 0;
    public void IncrementEnemyTouched()
    {
        ++enemyTouched;
    }
    public int enemyDestroyed { get; private set; } = 0;
    public void IncrementEnemyDestroyed()
    {
        ++enemyDestroyed;
    }

    private void OnEnable()
    {
        heroStatus = "";
        eggCount = 0;
        enemyCount = 0;
        enemyTouched = 0;
        enemyDestroyed = 0;
        hideMode = false;
        pauseMode = false;
        sequentialMode = false;
    }
}
