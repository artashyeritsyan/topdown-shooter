using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject overlayPanel;
    [SerializeField] GameObject gameWinPanel;

    [SerializeField] GameObject weaponIcon;
    [SerializeField] Sprite[] weaponsSprites;

    [SerializeField] Image playerShieldIcon;
    [SerializeField] Image playerHpBar;
    [SerializeField] Image vehicleHpBar;

    [SerializeField] VehicleController vehicleScript;

    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 finishPosition;

    [SerializeField] Image progressBar;

    [SerializeField] Image btrIcon;
    [SerializeField] Transform playerTransform;
    [SerializeField] float edgePadding = 50f;
    RectTransform overlayRect;


    private float vehicleMaxHp;

    private void Awake()
    {
        overlayRect = overlayPanel.GetComponent<RectTransform>();
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
        UpdateBTRIcon();
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
        //Debug.Log("Progess = " + progress);
        progressBar.fillAmount = progress;
    }

    public void SetPlayerHpBar(float hp)
    {
        // Currently max hp is 100, but it needs to be refactored to get maxHp from Player script
        playerHpBar.fillAmount = hp / 100;
    }

    public void SetShieldActive(bool isActive)
    {
        playerShieldIcon.gameObject.SetActive(isActive);
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

    void UpdateBTRIcon()
    {
        if (vehicleScript == null) return;

        Vector3 enemyPos = vehicleScript.transform.position;
        Vector3 playerPos = playerTransform.position;

        Vector3 dir = enemyPos - playerPos;
        dir.y = 0;

        // Direction relative to player
        Vector3 localDir = playerTransform.InverseTransformDirection(dir.normalized);

        // Get screen bounds (adaptive!)
        float halfWidth = overlayRect.rect.width * 0.5f - edgePadding;
        float halfHeight = overlayRect.rect.height * 0.5f - edgePadding;

        // Map direction to screen space
        Vector2 uiPos = new Vector2(localDir.x * halfWidth, localDir.z * halfHeight);

        // Clamp to stay inside screen
        uiPos.x = Mathf.Clamp(uiPos.x, -halfWidth, halfWidth);
        uiPos.y = Mathf.Clamp(uiPos.y, -halfHeight, halfHeight);

        RectTransform iconRect = btrIcon.GetComponent<RectTransform>();
        iconRect.anchoredPosition = uiPos;

        // No rotation
        //iconRect.rotation = Quaternion.identity;
    }

}
