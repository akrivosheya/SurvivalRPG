using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodEnding : MonoBehaviour
{
    [SerializeField] private Image Font;
    [SerializeField] private List<Text> Texts;
    [SerializeField] private Text Creators;
    [SerializeField] private Button Button;
    [SerializeField] private float BlurSpeed;
    [SerializeField] private float TextSpeed;
    private int _currentTextIndex = 0; 
    private bool _cameraIsWhite = false;
    private bool _textsAreFilled = false;
    private bool _buttonIsFilled = false;

    void Update()
    {
        if(!_cameraIsWhite)
        {
            var color = Font.color;
            color.a += Time.deltaTime * BlurSpeed;
            Font.color = color;
            if(color.a >= 1)
            {
                _cameraIsWhite = true;
            }
        }
        else if(!_textsAreFilled)
        {
            var color = Texts[_currentTextIndex].color;
            color.a += Time.deltaTime * BlurSpeed;
            Texts[_currentTextIndex].color = color;
            var transformPosition = Texts[_currentTextIndex].transform.position;
            transformPosition.y += TextSpeed * Time.deltaTime;
            Texts[_currentTextIndex].transform.position = transformPosition;
            if(color.a >= 1)
            {
                ++_currentTextIndex;
            }
            if(_currentTextIndex == Texts.Count)
            {
                _textsAreFilled = true;
            }
        }
        else if(!_buttonIsFilled)
        {
            var image = Button.GetComponent<Image>();
            var color = image.color;
            color.a += Time.deltaTime * BlurSpeed;
            image.color = color;
            var transformPosition = Button.transform.position;
            transformPosition.y += TextSpeed * Time.deltaTime;
            Button.transform.position = transformPosition;
            if(color.a >= 1)
            {
                _buttonIsFilled = true;
            }
        }
    }
}
