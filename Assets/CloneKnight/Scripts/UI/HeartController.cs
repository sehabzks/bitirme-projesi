using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    private GameObject[] heartContainers;
    private Image[] heartFills;
    public Transform heartsParent;
    public GameObject heartContainerPrefab;
    PlayerData playerData;

    void Start()
    {
        heartContainers = new GameObject[playerData.maxHealth];
        heartFills = new Image[playerData.maxHealth];

        playerData.onHealthChangedCallback += UpdateHeartsHUD;
        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            heartContainers[i].SetActive(i < playerData.maxHealth);
        }
    }
    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            heartFills[i].fillAmount = i < playerData.health ? 1 : 0;
        }
    }
    void InstantiateHeartContainers()
    {
        for (int i = 0; i < playerData.maxHealth; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
    void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }
}
