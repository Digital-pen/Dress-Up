using TMPro;
using UnityEngine;

public class ProfileUiElement : MonoBehaviour
{
    public ProfileDataType dataType;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI totalValueText;

    private void Start()
    {
        if (inputField != null)
            inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        if (int.TryParse(value, out int result))
        {
            totalValueText.text = value;
        }
    }

    public int GetValue()
    {
        return int.Parse(totalValueText.text);
    }

    public void SetValue(int value)
    {
        totalValueText.text = value.ToString();
    }
}
