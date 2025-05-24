using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RewardScreen : MonoBehaviour
{
    public PlayerController playerController;
    public SpellUI spellui1;
    public SpellUI spellui2;
    public SpellUI spellui3;

    public TextMeshProUGUI SpellImfomation1;
    public TextMeshProUGUI SpellImfomation2;
    public TextMeshProUGUI SpellImfomation3;
    private List<Spell> generatedSpells = new List<Spell>();
    private Spell generatedSpell;
    public SpellUIContainer playerSpellUIs;



    void Update()
    {

    }

    void OnEnable()
    {
        ShowSpellReward();
    }
    public void ShowSpellReward()
    {
        generatedSpells.Clear();

        Spell[] uiSpells = new Spell[3];
        TextMeshProUGUI[] infoFields = { SpellImfomation1, SpellImfomation2, SpellImfomation3 };
        SpellUI[] spellUIs = { spellui1, spellui2, spellui3 };

        for (int i = 0; i < 3; i++)
        {
            Spell spell = playerController.player.spellcaster.builder.MakeRandomSpell(playerController.player.spellcaster);
            generatedSpells.Add(spell);
            spellUIs[i].GetComponent<SpellUI>().SetSpell(spell);

            string modText = "";
            int modCount = spell.modifierSpells.Count;

            if (modCount > 0)
            {
                modText += $"\n<b>Modifiers:</b> {modCount}\n";
                foreach (var mod in spell.modifierSpells)
                {
                    modText += $"- {mod.name}\n";
                }
            }
            else
            {
                modText += "\n<b>Modifiers:</b> None";
            }

            infoFields[i].text =
                $"<b>{spell.GetName()}</b>\n" +
                $"{spell.GetDescription()}\n\n" +
                $"<b>Damage:</b> {spell.GetDamage()}\n" +
                $"<b>Mana Cost:</b> {spell.GetManaCost()}\n" +
                $"<b>Cooldown:</b> {spell.GetCooldown():0.00}s" +
                modText;
        }
    }

    public void acceptSpell(int index)
    {
        if (index < 0 || index >= generatedSpells.Count)
        {
            Debug.LogWarning("Invalid spell index selected.");
            return;
        }

        //Spell selectedSpell = generatedSpells[index];
        playerController.player.spellcaster.spells.Add(generatedSpells[index]);

        playerSpellUIs.spellUIs[playerController.spellNum].SetActive(true);
        playerSpellUIs.spellUIs[playerController.spellNum].GetComponent<SpellUI>().SetSpell(generatedSpells[index]);

        playerController.spellNum += 1;

        spellui1.SetSpell(null);
        spellui2.SetSpell(null);
        spellui3.SetSpell(null);
        generatedSpells.Clear();
        this.gameObject.SetActive(false);
        // GameManager.Instance.NextWave(); // Uncomment if wave advancing is handled here
    }




    private bool IsModifierSpell(Spell spell)
    {
        // Replace this with a better check if needed
        return spell.GetName().ToLower().Contains("chaotic") || spell.GetName().ToLower().Contains("homing");
    }
}