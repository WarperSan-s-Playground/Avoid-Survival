using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public GameObject blockToPlace = null;
    public string typeOfItem = "";
    public string itemName = "";

    public void ItemSlotClicked()
    {
        if (string.IsNullOrEmpty(typeOfItem))
            return;

        if (typeOfItem.Contains("block"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<MovingCharacter>().blockToPlace = blockToPlace;
            return;
        }

        if (typeOfItem.Contains("usable_item"))
        {
            // Find the player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            MovingCharacter movingCharacterScript = player.GetComponent<MovingCharacter>();

            string itemName = typeOfItem.Replace("usable_item:", "");

            if (itemName == "shield")
            {
                // Activate the shield and reset it's hp
                movingCharacterScript.shield.SetActive(true);
                movingCharacterScript.shield.GetComponent<ShieldScript>().health = DifficultyManagerScript.defaultShieldHealth;
                movingCharacterScript.shield.GetComponent<ShieldScript>().shieldUpdate();

                movingCharacterScript.UpdateSlot(movingCharacterScript.inventory.IndexOf("Shield"), -1, "Shield");
                return;
            }

            // Weapon system
            switch (itemName)
            {
                // Summoning Staff Tier 0
                case "SST0":
                    itemName = "Summoning Staff T0";
                    GameObject summoningStaff = movingCharacterScript.summoningStaff;

                    DisableOtherWeapons(movingCharacterScript, summoningStaff);

                    SummoningStaff(0, summoningStaff, itemName);
                    return;

                // Summoning Staff Tier 1
                case "SST1":
                    itemName = "Summoning Staff T1";
                    summoningStaff = movingCharacterScript.summoningStaff;

                    DisableOtherWeapons(movingCharacterScript, summoningStaff);

                    SummoningStaff(1, summoningStaff, itemName);
                    return;

                case "club":

                    if (movingCharacterScript.clubWeapon.activeInHierarchy)
                    {
                        movingCharacterScript.clubWeapon.SetActive(false);
                        movingCharacterScript.Damage = 2;
                        movingCharacterScript.speedModifier = 1;
                        return;
                    }

                    DisableOtherWeapons(movingCharacterScript, movingCharacterScript.clubWeapon);

                    movingCharacterScript.clubWeapon.SetActive(true);
                    movingCharacterScript.Damage = movingCharacterScript.clubWeapon.GetComponent<WeaponClass>().damage;
                    movingCharacterScript.speedModifier = movingCharacterScript.clubWeapon.GetComponent<WeaponClass>().speedModifier;
                    break;

                case "dagger":

                    if (movingCharacterScript.daggerWeapon.activeInHierarchy)
                    {
                        movingCharacterScript.daggerWeapon.SetActive(false);
                        movingCharacterScript.Damage = 2;
                        movingCharacterScript.speedModifier = 1;
                        return;
                    }

                    DisableOtherWeapons(movingCharacterScript, movingCharacterScript.daggerWeapon);

                    movingCharacterScript.daggerWeapon.SetActive(true);
                    movingCharacterScript.Damage = movingCharacterScript.daggerWeapon.GetComponent<WeaponClass>().damage;
                    movingCharacterScript.speedModifier = movingCharacterScript.daggerWeapon.GetComponent<WeaponClass>().speedModifier;
                    break;

                case "bow":

                    if (movingCharacterScript.bowWeapon.activeInHierarchy)
                    {
                        movingCharacterScript.bowWeapon.SetActive(false);
                        movingCharacterScript.speedModifier = 1;
                        return;
                    }

                    DisableOtherWeapons(movingCharacterScript, movingCharacterScript.bowWeapon);

                    movingCharacterScript.bowWeapon.SetActive(true);
                    movingCharacterScript.speedModifier = movingCharacterScript.bowWeapon.GetComponent<WeaponClass>().speedModifier;
                    break;

                case "gun":

                    if (movingCharacterScript.gunWeapon.activeInHierarchy)
                    {
                        movingCharacterScript.gunWeapon.SetActive(false);
                        movingCharacterScript.speedModifier = 1;
                        return;
                    }

                    DisableOtherWeapons(movingCharacterScript, movingCharacterScript.gunWeapon);

                    movingCharacterScript.gunWeapon.SetActive(true);
                    movingCharacterScript.speedModifier = movingCharacterScript.gunWeapon.GetComponent<WeaponClass>().speedModifier;
                    break;

                default:
                    Debug.Log("Item used");
                    break;
            }
            return;
        }
    }

    private void SummoningStaff(int tier, GameObject staff, string itemName)
    {
        // If the player requip the same staff
        if (staff.GetComponent<summoningStaffScript>().tier > tier)
            return;

        staff.SetActive(true);
        staff.GetComponent<summoningStaffScript>().tier = tier;
        staff.GetComponent<summoningStaffScript>().itemName = itemName;
        staff.GetComponent<summoningStaffScript>().ResetStaff();
    }

    private void DisableOtherWeapons(MovingCharacter movingCharacter, GameObject objectActive)
    {
        for (int i = 0; i < movingCharacter.weaponList.Length; i++)
        {
            if(movingCharacter.weaponList[i]!=objectActive)
            {
                movingCharacter.weaponList[i].SetActive(false);
            }
        }
    }
}
