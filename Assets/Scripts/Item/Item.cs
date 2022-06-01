using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")] // 아이템의 경로
public class Item : ScriptableObject // 게임 오브젝트에 붙일 필요 없는 오브젝트를 만들기 위해 필요하다.
{

    public string itemName; // 아이템의 이름
    public ItemType itemType; // 아이템의 유형
    public Sprite itemImage; // 아이템의 이미지
    public GameObject itemPrefab; // 아이템의 프리팹

    public string weaponType; // 무기 유형
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
    
  
}
