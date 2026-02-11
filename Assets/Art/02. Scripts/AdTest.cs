using UnityEngine;

public class AdTest : MonoBehaviour
{
    private AdV2Tester adTester;
    public GUISkin customSkin;

    void Start()
    {
        adTester = GetComponent<AdV2Tester>();
    }

    void OnGUI()
    {
        if (adTester == null) return;

        GUISkin skin = customSkin != null ? customSkin : GUI.skin;

        // --- 스타일 커스텀 시작 ---
        
        // 1. 모든 글자를 전반적으로 크게 만들기 위해 폰트 사이즈 설정
        int bigFontSize = 30; // 원하는 크기로 조절하세요.
        int headerFontSize = 40;

        // 버튼 스타일 커스텀
        GUIStyle bigButtonStyle = new GUIStyle(skin.button);
        bigButtonStyle.fontSize = bigFontSize;
        bigButtonStyle.padding = new RectOffset(20, 20, 10, 10); // 안쪽 여백 추가

        // 라벨(일반 텍스트) 스타일 커스텀
        GUIStyle bigLabelStyle = new GUIStyle(skin.label);
        bigLabelStyle.fontSize = bigFontSize;

        // 제목(헤더) 스타일 커스텀
        GUIStyle bigHeaderStyle = new GUIStyle(skin.label);
        bigHeaderStyle.fontSize = headerFontSize;
        bigHeaderStyle.fontStyle = FontStyle.Bold;

        // 텍스트 필드 및 기타 스타일
        GUIStyle bigTextFieldStyle = new GUIStyle(skin.textField);
        bigTextFieldStyle.fontSize = bigFontSize;

        GUIStyle bigCallbackStyle = new GUIStyle(skin.textArea);
        bigCallbackStyle.fontSize = bigFontSize - 5; // 로그는 조금 작게

        // --- 커스텀 스타일 적용하여 DrawUI 호출 ---
        adTester.DrawUI(
            boxStyle: skin.box,
            groupHeaderStyle: bigHeaderStyle,
            labelStyle: bigLabelStyle,
            buttonStyle: bigButtonStyle,
            textFieldStyle: bigTextFieldStyle,
            fieldLabelStyle: bigLabelStyle,
            callbackLabelStyle: bigCallbackStyle
        );
    }
}
