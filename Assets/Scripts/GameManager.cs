using UnityEditor.VisionOS;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject overlayPanel;
    [SerializeField] GameObject gameWinPanel;

    [SerializeField] GameObject weaponIcon;
    [SerializeField] Sprite[] weaponsSprites;

    [SerializeField] Image playerHpBar;
    [SerializeField] Image vehicleHpBar;

    [SerializeField] VehicleController vehicleScript;

    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 finishPosition;

    [SerializeField] Image progressBar;


    private float vehicleMaxHp;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        overlayPanel.gameObject.SetActive(true);
        gameWinPanel.gameObject.SetActive(false);
        vehicleMaxHp = vehicleScript.GetMaxHealth();
        progressBar.fillAmount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgressBar();
    }

    void OnEnable()
    {
        Player.OnWeaponSelect += ChangeWeaponIcon;

    }

    void OnDisable()
    {
        Player.OnWeaponSelect -= ChangeWeaponIcon;
    }

    void UpdateProgressBar()
    {
        float fullDistance = Mathf.Abs(finishPosition.x - startPosition.x);
        float progress = Mathf.Abs(fullDistance - vehicleScript.GetDistanceToFinish()) / fullDistance;
        Debug.Log("Progess = " + progress);
        progressBar.fillAmount = progress;
    }

    public void SetPlayerHpBar(float hp)
    {
        // Currently max hp is 100, but it needs to be refactored to get maxHp from Player script
        playerHpBar.fillAmount = hp / 100;
    }

    public void SetVehicleHpBar(float hp)
    {
        vehicleHpBar.fillAmount = hp / vehicleMaxHp;
    }

    public Vector3 GetFinishPosition()
    {
        return finishPosition;
    }

    public void GameWin()
    {
        overlayPanel.gameObject.SetActive(false);
        gameWinPanel.gameObject.SetActive(true);
    }


    void ChangeWeaponIcon(int iconIndex)
    {
        weaponIcon.GetComponent<Image>().sprite = weaponsSprites[iconIndex];
    }
    
}
