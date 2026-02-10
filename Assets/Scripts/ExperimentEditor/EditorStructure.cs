/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor
{
    [System.Serializable]
    public struct TextOptions
    {
        public Color textColor;
        public float textSize;
        public TMPro.FontStyles textStyle;
        public TMPro.HorizontalAlignmentOptions horizontalAlignment;
        public TMPro.VerticalAlignmentOptions verticalAlignment;

        public TextOptions(Color color, float size, TMPro.FontStyles style = FontStyles.Normal, TMPro.HorizontalAlignmentOptions horizontalAlignment = HorizontalAlignmentOptions.Left, TMPro.VerticalAlignmentOptions verticalAlignment = VerticalAlignmentOptions.Middle)
        {
            textColor = color;
            textSize = size;
            textStyle = style;
            this.horizontalAlignment = horizontalAlignment;
            this.verticalAlignment = verticalAlignment;
        }
    }

    [System.Serializable]
    public struct RadioButtonCreateOption
    {
        public TMP_InputField optionInputText;
        public Toggle optionToggle;
        public Toggle defaultOption;

        public void Reset()
        {
            optionInputText.text = string.Empty;
            optionToggle.isOn = false;
            if(defaultOption != null) defaultOption.isOn = false;
        }
    }

    [System.Serializable]
    public struct RadioOptionValue
    {
        public string optionName;
        public bool isEnabled;
        public bool isDefault;
    }

    [System.Serializable]
    public struct SliderOptions
    {
        public float minValue;
        public float maxValue;
        public float defaultValue;
        public string labelPrefix;
        public string labelSuffix;
        public int decimalPlaces;

        public SliderOptions(float min = 1.0f, float max = 99.0f, float value = 1.0f, string prefix = "", string suffix = "", int decimalPlaces = 0)
        {
            minValue = min;
            maxValue = max;
            defaultValue = value;
            labelPrefix = prefix;
            labelSuffix = suffix;
            this.decimalPlaces = decimalPlaces;
        }

       
    }

    [System.Serializable]
    public struct SliderCreateOption
    {
        public TMP_InputField sliderMinValue;
        public TMP_InputField sliderMaxValue;
        public TMP_InputField sliderDefaultValue;
        public TMP_InputField sliderLabelPrefix;
        public TMP_InputField sliderLabelSuffix;
        public Slider decimalPlaces;

        public void Reset()
        {
            sliderMinValue.text = "1";
            sliderMaxValue.text = "100";
            sliderDefaultValue.text = "10";
            sliderLabelPrefix.text = string.Empty;
            sliderLabelSuffix.text = string.Empty;
            decimalPlaces.value = 0;
        }
    }

    [System.Serializable]
    public struct DropdownOption
    {
        public string value;
        public bool enabled;
    }

    [System.Serializable]
    public struct ExperimentSaveData
    {
        public string experimentName;
        public ExperimentType experimentType;
        public string assignedVideoFile;
        public Color defaultPageColor;
        public Color defaultTextColor;
        public float defaultTextSize;
        public List<ExperimentSaveDataPage> pages;
        public List<ExperimentSaveDataQuestion> questions;
    }

    [System.Serializable]
    public struct ExperimentSaveDataPage
    {
        public string pageName;
        public string pageId;
        public PageType pageType;
        public string pageText;
        public TextOptions textOptions;
        public Color backgroundColor;

        public ExperimentSaveDataPage(string id, string name, PageType type, string text, Color color, TextOptions options)
        {
            pageId = id;
            pageName = name;
            pageType = type;
            pageText = text;
            backgroundColor = color;
            textOptions = options;
        }
    }

    [System.Serializable]
    public struct ExperimentSaveDataQuestion
    {
        public string questionName;
        public string questionId;
        public QuestionType questionType;
        public string referencePageId;
        public string questionText;
        public TextOptions textOptions;
        public RadioOptionValue[] radioOptions;
        public SliderOptions sliderOptions;

        public ExperimentSaveDataQuestion(string id, string name, QuestionType type, string pageId, string text, TextOptions textOptions, RadioOptionValue[] radioOptions, SliderOptions sliderOptions)
        {
            questionId = id;
            questionName = name;
            questionType = type;
            referencePageId = pageId;
            questionText = text;
            this.textOptions = textOptions;
            this.radioOptions = radioOptions;
            this.sliderOptions = sliderOptions;
        }
    }
}

