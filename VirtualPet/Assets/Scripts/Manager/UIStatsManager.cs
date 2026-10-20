using UnityEngine;
using UnityEngine.UI;

public class UIStatsManager : MonoBehaviour
{
    [Header("References")]
    public PetState petState;

    [Header("UI Fill Bars")]
    public Image happyFill;
    public Image energyFill;
    public Image cleanFill;
    public Image hungerFill;

    private void Update()
    {
        if (petState != null)
        {
            if (happyFill != null) happyFill.fillAmount = petState.happy / 100f;
            if (energyFill != null) energyFill.fillAmount = petState.energy / 100f;
            if (cleanFill != null) cleanFill.fillAmount = petState.clean / 100f;

            if (hungerFill != null)
            {
                float invertedHunger = 100f - petState.hunger;
                hungerFill.fillAmount = invertedHunger / 100f;
            }

        }
    }
}
