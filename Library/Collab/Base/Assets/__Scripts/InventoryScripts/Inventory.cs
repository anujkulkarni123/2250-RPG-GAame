﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    private bool inventoryEnabled;
    

    private int allSlots;
    private int enabledSlots;
    private GameObject[] slot;

    public GameObject slotHolder;

    void Start()    {
        allSlots=4;
        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots ; i++)   {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;
            if (slot[i].GetComponent<Slot>().item == null)  {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    private void Update()   {
        if (Input.GetKeyDown(KeyCode.I))    {
            inventoryEnabled = !inventoryEnabled;
        }
            
        if (inventoryEnabled == true)   {
            inventory.gameObject.SetActive(true);
        } else {
            inventory.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Item") {
            GameObject itemPickedUp = other.gameObject;
            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(itemPickedUp, item.ID, item.type, item.description, item.icon);
        }
    }

    void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)  {
        for (int i = 0; i < allSlots; i++)  {
            if (slot[i].GetComponent<Slot>().empty) {


                // add item to slot
                itemObject.GetComponent<Item>().pickedUp = true;

                
                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().icon = itemIcon;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().description = itemDescription;

                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);


                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;
            }

            return;
        }
    }

}
