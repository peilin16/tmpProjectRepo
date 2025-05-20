using UnityEngine;
using System.Collections.Generic;



public class CharacterSelector : MonoBehaviour
{
    public SpriteView spriteView1;
    public SpriteView spriteView2;
    public SpriteView spriteView3;
    public GameObject DifficultSelector;
    public PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        spriteView1.Apply("mage", GameManager.Instance.playerSpriteManager.Get(0));
        spriteView2.Apply("warlock", GameManager.Instance.playerSpriteManager.Get(1));
        spriteView3.Apply("battlemage", GameManager.Instance.playerSpriteManager.Get(2));
    }


    public void Chosen(int index)
    {
        
        GameManager.Instance.playerSpriteManager.currentIconIndex = index;
        playerController.loadCharacter(index);
        DifficultSelector.SetActive(true);
        gameObject.SetActive(false);
    }




}
