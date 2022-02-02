using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;
        }
        instance = this;
    }
    #endregion

     public List<Item> items = new List<Item>();
    [SerializeField] int space;
    [SerializeField] GameEvent onItemChanged;

    Transform player;

	private void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough space, in inventory");
            return false;
        }
        items.Add(item);
        onItemChanged.Raise();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        onItemChanged.Raise();
    }

    public void RemoveAndSpawn(Item item)
	{
        Remove(item);
        SpawnManager.istance.SpawnObject(item, false, 1, player.transform.position);
	}
}
