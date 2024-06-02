using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    public Color[] m_NewColor;

    void Start()
    {
        RandomColorChange();
    }
    //Sorteio de Cor
    public void RandomColorChange()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        int index = Random.Range(0, m_NewColor.Length);

        //m_SpriteRenderer.color = m_NewColor[index]; // Escolher em uma lista de cores

        m_SpriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1); // Sorteio RGBa

        Debug.Log("A cor escolhida foi = " + index);
    }
}
