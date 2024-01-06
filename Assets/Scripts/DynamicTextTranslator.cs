using UnityEngine;

public class DynamicTextTranslator : TextTranslator
{
    public string[] rusTexts;
    public string[] engTexts;

    public void SetText(int id)
    {
        if (isRus) txt.text = rusTexts[id];
        else txt.text = engTexts[id];
    }
}
