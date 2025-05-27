using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public enum MATERIALS { YESO, LADRILLO, PARQUET, GRES, MARMOL, VIDRIO, MOQUETA };

public struct MaterialAbsorption
{
    public string name;
    public Dictionary<int, float> absorptionCoefficients;

    public MaterialAbsorption(string name, Dictionary<int, float> absorption)
    {
        this.name = name;
        this.absorptionCoefficients = absorption;
    }

    public float GetAverageAbsorption()
    {
        float sum = 0;
        foreach (var value in absorptionCoefficients.Values)
        {
            sum += value;
        }
        return sum / absorptionCoefficients.Count;
    }
}

public class CustomizedRoom : MonoBehaviour
{
    [SerializeField] TMP_InputField heightInput;
    [SerializeField] TMP_InputField largeInput;
    [SerializeField] TMP_InputField wideInput;
    [SerializeField] TMP_Dropdown materialsDropdownHeight;
    [SerializeField] TMP_Dropdown materialsDropdownLarge;
    [SerializeField] TMP_Dropdown materialsDropdownWide;

    MATERIALS materialHeight;
    MATERIALS materialLarge;
    MATERIALS materialWide;

    Dictionary<MATERIALS, MaterialAbsorption> materialsData = new Dictionary<MATERIALS, MaterialAbsorption>()
    {
        { MATERIALS.YESO, new MaterialAbsorption("Yeso", new Dictionary<int, float> { {125, 0.02f}, {250, 0.03f}, {500, 0.04f}, {1000, 0.05f}, {2000, 0.06f} }) },
        { MATERIALS.LADRILLO, new MaterialAbsorption("Ladrillo", new Dictionary<int, float> { {125, 0.03f}, {250, 0.03f}, {500, 0.04f}, {1000, 0.05f}, {2000, 0.07f} }) },
        { MATERIALS.PARQUET, new MaterialAbsorption("Parquet", new Dictionary<int, float> { {125, 0.10f}, {250, 0.15f}, {500, 0.11f}, {1000, 0.10f}, {2000, 0.08f} }) },
        { MATERIALS.GRES, new MaterialAbsorption("Gres", new Dictionary<int, float> { {125, 0.01f}, {250, 0.01f}, {500, 0.02f}, {1000, 0.02f}, {2000, 0.03f} }) },
        { MATERIALS.MARMOL, new MaterialAbsorption("Mármol", new Dictionary<int, float> { {125, 0.01f}, {250, 0.01f}, {500, 0.02f}, {1000, 0.02f}, {2000, 0.03f} }) },
        { MATERIALS.VIDRIO, new MaterialAbsorption("Vidrio", new Dictionary<int, float> { {125, 0.02f}, {250, 0.03f}, {500, 0.05f}, {1000, 0.06f}, {2000, 0.07f} }) },
        { MATERIALS.MOQUETA, new MaterialAbsorption("Moqueta", new Dictionary<int, float> { {125, 0.35f}, {250, 0.45f}, {500, 0.55f}, {1000, 0.60f}, {2000, 0.65f} }) },
    };

    void Start()
    {
        materialHeight = MATERIALS.YESO;
        materialLarge = MATERIALS.PARQUET;
        materialWide = MATERIALS.VIDRIO;
    }

    public void Done()
    {
        materialHeight = (MATERIALS)materialsDropdownHeight.value;
        materialLarge = (MATERIALS)materialsDropdownLarge.value;
        materialWide = (MATERIALS)materialsDropdownWide.value;

        if (int.TryParse(heightInput.text, out int height) &&
            int.TryParse(wideInput.text, out int wide) &&
            int.TryParse(largeInput.text, out int large))
        {
            int volume = CalculateVolume(height, wide, large);
            float absorptionArea = CalculateAbsorptionArea(height, wide, large, materialHeight, materialLarge, materialWide);
            float t60 = CalculateSabine(volume, absorptionArea); // Recibe el RT60 calculado

            SendRT60ToFMOD(t60); // Envía a FMOD
        }
        else
        {
            Debug.Log("Por favor, introduce valores numéricos válidos para las medidas.");
        }
    }

    public int CalculateVolume(int height, int wide, int large)
    {
        int volume = height * wide * large;
        Debug.Log("El volumen de la habitación es: " + volume + " metros cúbicos.");
        return volume;
    }

    public float CalculateAbsorptionArea(int height, int wide, int large, MATERIALS matHeight, MATERIALS matWide, MATERIALS matLarge)
    {
        float areaAltura = 2 * height * wide;
        float areaLargo = 2 * height * large;
        float areaSueloTecho = wide * large * 2;

        float coeffAltura = materialsData[matHeight].GetAverageAbsorption();
        float coeffLargo = materialsData[matLarge].GetAverageAbsorption();
        float coeffAncho = materialsData[matWide].GetAverageAbsorption();

        float absorptionArea =
            (areaAltura * coeffAltura) +
            (areaLargo * coeffLargo) +
            (areaSueloTecho * coeffAncho);

        Debug.Log("Área total de absorción: " + absorptionArea.ToString("F2") + " m²");
        return absorptionArea;
    }

    public float CalculateSabine(int volume, float absorptionArea)
    {
        if (absorptionArea > 0)
        {
            float reverbTime = (0.161f * volume) / absorptionArea;
            Debug.Log("Tiempo de reverberación (Sabine): " + reverbTime.ToString("F2") + " segundos.");
            return reverbTime;
        }
        else
        {
            Debug.Log("El área de absorción es 0, no se puede calcular el tiempo de reverberación.");
            return 0f;
        }
    }

    public void SendRT60ToFMOD(float t60)
    {
        if (t60 > 0)
        {
            RuntimeManager.StudioSystem.setParameterByName("RT60", t60);
            Debug.Log("RT60 enviado a FMOD: " + t60.ToString("F2") + " segundos.");
        }
        else
        {
            Debug.LogWarning("RT60 no válido para enviar a FMOD.");
        }
    }
}
