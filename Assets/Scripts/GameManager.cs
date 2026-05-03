using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject weaponIcon;
    [SerializeField] Sprite[] weaponsSprites;

    [SerializeField] Image PlayerHpBar;

    private void Awake()
    {
        instance = this;
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
        PlayerHpBar.fillAmount = hp / 100;
    }

    void ChangeWeaponIcon(int iconIndex)
    {
        weaponIcon.GetComponent<Image>().sprite = weaponsSprites[iconIndex];
    }
    
}
