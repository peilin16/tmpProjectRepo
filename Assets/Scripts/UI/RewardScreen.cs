using UnityEngine;
using TMPro;
public class RewardScreen : MonoBehaviour
{
    public PlayerController player;
    public SpellUI spellui;
    
    public TextMeshProUGUI SpellImfomation;
    private Spell generatedSpell;
    public SpellUIContainer playerSpellUIs;

    void Start()
    {
        ShowSpellReward();
        //GameManager.Instance.rewardScreen = this;
    }

    void Update()
    {

    }

    public void ShowSpellReward()
    {
        generatedSpell = player.spellcaster.builder.MakeRandomSpell(player.spellcaster);
        spellui.SetSpell(generatedSpell);

        if (generatedSpell != null)
        {
            string modText = "";
            int modCount = generatedSpell.modifierSpells.Count;

            if (modCount > 0)
            {
                modText += $"\n<b>Modifiers:</b> {modCount}\n";
                foreach (var mod in generatedSpell.modifierSpells)
                {
                    modText += $"- {mod.name}\n";
                }
            }
            else
            {
                modText += "\n<b>Modifiers:</b> None";
            }

            SpellImfomation.text =
                $"<b>{generatedSpell.GetName()}</b>\n" +
                $"<i>{generatedSpell.GetDescription()}</i>\n\n" +
                $"<b>Damage:</b> {generatedSpell.GetDamage()}\n" +
                $"<b>Mana Cost:</b> {generatedSpell.GetManaCost()}\n" +
                $"<b>Cooldown:</b> {generatedSpell.GetCooldown():0.00}s" +
                modText;
        }
    }

    public void acceptSpell()
    {
        if (generatedSpell == null)
        {
            Debug.LogWarning("No generated spell to accept!");
            return;
        }

        player.spellcaster.spells.Add(generatedSpell);

        player.spellNum += 1;
        playerSpellUIs.spellUIs[player.spellNum - 1].SetActive(true);
        playerSpellUIs.spellUIs[player.spellNum - 1].GetComponent<SpellUI>().SetSpell(this.generatedSpell);

        spellui.SetSpell(null); // clear UI
        generatedSpell = null;

        // Hide reward screen and move to next wave
       // GameManager.Instance.NextWave();
    }



    private bool IsModifierSpell(Spell spell)
    {
        // Replace this with a better check if needed
        return spell.GetName().ToLower().Contains("chaotic") || spell.GetName().ToLower().Contains("homing");
    }
}