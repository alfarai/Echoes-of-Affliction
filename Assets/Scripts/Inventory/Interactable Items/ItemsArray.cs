using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ItemsArray : MonoBehaviour
{
    public List<ItemData> interactables = new List<ItemData>();
    public List<GameObject> itemGameObjects = new List<GameObject>(); //must be manually inserted depending on available gameobjects for a scene
}
