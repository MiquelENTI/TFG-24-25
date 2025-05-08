using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{
    private FMOD.Studio.VCA vca;
    public string VcaName;

    private Slider slider;
    private string prefsKey;

    void Start()
    {
        vca = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
        slider = GetComponent<Slider>();
        prefsKey = "Volume_" + VcaName; // Clave única para cada VCA

        if (slider != null)
        {
            slider.onValueChanged.AddListener(SetVolume);

            // Cargar el volumen guardado, o si no existe, usar volumen actual del VCA
            float savedVolume = PlayerPrefs.GetFloat(prefsKey, -1f);
            if (savedVolume >= 0f)
            {
                slider.value = savedVolume;
                vca.setVolume(savedVolume);
            }
            else
            {
                float currentVolume;
                vca.getVolume(out currentVolume);
                slider.value = currentVolume;
            }
        }
    }

    public void SetVolume(float volume)
    {
        vca.setVolume(volume);
        PlayerPrefs.SetFloat(prefsKey, volume); // Guardar el nuevo volumen
        PlayerPrefs.Save(); // Forzar guardado inmediato
    }
}
