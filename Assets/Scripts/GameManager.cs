using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject weaponIcon;
    [SerializeField] Sprite[] weaponsSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeWeaponIcon(int iconIndex)
    {
        weaponIcon.GetComponent<Image>().sprite = weaponsSprites[iconIndex];
    }

    void OnEnable()
    {
        Player.OnWeaponSelect += ChangeWeaponIcon;

    }

    void OnDisable()
    {
        Player.OnWeaponSelect -= ChangeWeaponIcon;
    }
}
