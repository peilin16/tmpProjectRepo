using UnityEngine;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // we only have one spell (right now)
        spellUIs[0].SetActive(true);
        

        /*spellUIs[1].SetActive(true);
        spellUIs[2].SetActive(true);
        spellUIs[3].SetActive(true);*/
        /*for(int i = 1; i< spellUIs.Length; ++i)
        {
            spellUIs[i].SetActive(false);
        }*/


        // Optional: deactivate unused UI slots
        /*for (int i = spellList.Length; i < spellUIs.Length; i++)
        {
            spellUIs[i].SetActive(false);
        }*/

    }
    private bool initialized = false;
    
    public void Update()
    {
        if (!initialized && player.spellcaster != null && player.spellcaster.spells != null)
        {
            initialized = true;
            spellUIs[0].GetComponent<SpellUI>().SetSpell(player.spellcaster.spells[0]);
            /*
                var spellList = player.spellcaster.spells;

                for (int i = 0; i < spellUIs.Length && i < spellList.Count; i++)
                {
                    //Debug.Log(i);
                    if (spellList[i] == null)
                        break;
                    spellUIs[i].SetActive(true);
                    var spellUI = spellUIs[i].GetComponent<SpellUI>();
                    spellUI.SetSpell(spellList[i]);
                }

                for (int i = spellList.Count; i < spellUIs.Length; i++)
                {
                    if (spellList[i] == null)
                        break;
                    spellUIs[i].SetActive(false);
                }

                initialized = true; // prevent repeat execution*/
        }

    }
    public void DropSpell(int index)
    {

        // Safety check
        if (index < 0 || index >= player.spellcaster.spells.Count) return;

        int count = player.spellcaster.spells.Count;

        // Shift spells and UIs left from index
        for (int i = index; i < count - 1; i++)
        {
            player.spellcaster.spells[i] = player.spellcaster.spells[i + 1];
            // Update UI
            spellUIs[i].GetComponent<SpellUI>().SetSpell(player.spellcaster.spells[i]);
        }

        // Remove last spell
        player.spellcaster.spells.RemoveAt(count - 1);

        // Disable last UI
        spellUIs[count - 1].SetActive(false);
    }


}