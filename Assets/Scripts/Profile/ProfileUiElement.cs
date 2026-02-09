using TMPro;
using UnityEngine;

public class ProfileUiElement : MonoBehaviour
{
    public ProfileDataType dataType;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI totalValueText;

    private void Start()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        if (int.TryParse(value, out int result))
        {
            totalValueText.text = value;
        }
    }
}
