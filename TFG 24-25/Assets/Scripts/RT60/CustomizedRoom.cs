using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MATERIALS {HORMIGON, PLADUR, MADERA, MOQUETA, PARQUE, VIDRIO};

// Estructura de datos para almacenar coeficientes de absorción en distintas frecuencias
public struct MaterialAbsorption
{
    public string name;
    public Dictionary<int, float> absorptionCoefficients; // Clave: frecuencia, Valor: coef. absorción

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
    [SerializeField] TMP_Dropdown materialsDropdown;
    MATERIALS material;

    // Diccionario de materiales con coeficientes de absorción en varias frecuencias
    Dictionary<MATERIALS, MaterialAbsorption> materialsData = new Dictionary<MATERIALS, MaterialAbsorption>()
    {
        { MATERIALS.HORMIGON, new MaterialAbsorption("Hormigón", new Dictionary<int, float>
            { {125, 0.01f}, {250, 0.02f}, {500, 0.02f}, {1000, 0.03f}, {2000, 0.04f} }) },
        
        { MATERIALS.PLADUR, new MaterialAbsorption("Pladur", new Dictionary<int, float>
            { {125, 0.10f}, {250, 0.15f}, {500, 0.29f}, {1000, 0.40f}, {2000, 0.45f} }) },
        
        { MATERIALS.MADERA, new MaterialAbsorption("Madera", new Dictionary<int, float>
            { {125, 0.10f}, {250, 0.12f}, {500, 0.15f}, {1000, 0.17f}, {2000, 0.18f} }) },
        
        { MATERIALS.MOQUETA, new MaterialAbsorption("Moqueta", new Dictionary<int, float>
            { {125, 0.35f}, {250, 0.45f}, {500, 0.55f}, {1000, 0.60f}, {2000, 0.65f} }) },
        
        { MATERIALS.PARQUE, new MaterialAbsorption("Parquet", new Dictionary<int, float>
            { {125, 0.05f}, {250, 0.08f}, {500, 0.10f}, {1000, 0.12f}, {2000, 0.13f} }) },
        
        { MATERIALS.VIDRIO, new MaterialAbsorption("Vidrio", new Dictionary<int, float>
            { {125, 0.02f}, {250, 0.03f}, {500, 0.05f}, {1000, 0.06f}, {2000, 0.07f} }) },
    };

    void Start()
    {
        material = MATERIALS.HORMIGON; // Se inicia con el primer material del enum
    }

    public void Done()
    {
        // Asigna el material seleccionado en el Dropdown
        material = (MATERIALS)materialsDropdown.value;

        // Verifica si los campos de entrada contienen valores numéricos válidos
        if (int.TryParse(heightInput.text, out int height) &&
            int.TryParse(wideInput.text, out int wide) &&
            int.TryParse(largeInput.text, out int large))
        {
            // Calcula volumen y área de absorción
            int volume = CalculateVolume(height, wide, large);
            float absorptionArea = CalculateAbsorptionArea(height, wide, large, material);

            // Calcula tiempo de reverberación usando la fórmula de Sabine con coeficiente promedio
            CalculateSabine(volume, absorptionArea, material);
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

    public float CalculateAbsorptionArea(int height, int wide, int large, MATERIALS material)
    {
        // Calcula la superficie total de la sala
        float totalArea = (2 * height * wide) + (2 * height * large) + (wide * large);

        // Obtiene el coeficiente de absorción promedio
        float absorptionCoefficient = materialsData[material].GetAverageAbsorption();

        // Calcula el área de absorción A = S * α
        float absorptionArea = totalArea * absorptionCoefficient;

        Debug.Log("Área total de absorción: " + absorptionArea.ToString("F2") + " m² (Material: " + material + ")");
        return absorptionArea;
    }

    public void CalculateSabine(int volume, float absorptionArea, MATERIALS material)
    {
        if (absorptionArea > 0)
        {
            float reverbTime = (0.161f * volume) / absorptionArea;
            Debug.Log("Tiempo de reverberación (Sabine) con material " + materialsData[material].name + ": " 
                      + reverbTime.ToString("F2") + " segundos.");
        }
        else
        {
            Debug.Log("El área de absorción es 0, no se puede calcular el tiempo de reverberación.");
        }
    }
}
