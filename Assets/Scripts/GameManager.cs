using UnityEditor.VisionOS;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject weaponIcon;
    [SerializeField] Sprite[] weaponsSprites;

    [SerializeField] Image playerHpBar;
    [SerializeField] Image vehicleHpBar;

    [SerializeField] VehicleController vehicleScript;

    private float vehicleMaxHp;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        vehicleMaxHp = vehicleScript.GetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Player.OnWeaponSelect += ChangeWeaponIcon;

    }

    void OnDisable()
    {
        Player.OnWeaponSelect -= ChangeWeaponIcon;
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


    void ChangeWeaponIcon(int iconIndex)
    {
        weaponIcon.GetComponent<Image>().sprite = weaponsSprites[iconIndex];
    }
    
}
