using UnityEngine;
using UnityEngine.Profiling;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class AutomatedPerformanceTest : MonoBehaviour
{
    [System.Serializable]
    public class TestScenario
    {
        public string name;
        public float testDuration = 10f; // 테스트 지속 시간
        public int targetFPS = 30;
        public float maxMemoryMB = 200f;
    }

    [Header("테스트 설정")]
    public TestScenario[] testScenarios;
    
    private List<PerformanceTestResult> allResults = new List<PerformanceTestResult>();
    private bool showUIReport = false;
    private string finalReportText = "";

    [System.Serializable]
    public class PerformanceTestResult
    {
        public string scenarioName;
        public float averageFPS;
        public float minFPS;
        public float maxMemoryMB;
        public bool passed;
        public List<string> issues = new List<string>();
    }
    
    public static AutomatedPerformanceTest Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 씬이 바뀌어도 이 오브젝트를 파괴하지 않음
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // 중복 생성된 경우 파괴
            Destroy(gameObject);
        }
    }

    // 버튼 등에 연결하여 테스트 시작
    public void RunAllTests()
    {
        // 1. 모든 상태 완전 초기화
        showUIReport = false;
        finalReportText = "";
        allResults.Clear(); 
        statusMessage = "초기화 중...";

        // 2. 실행 중인 모든 코루틴 정지 후 새로 시작
        StopAllCoroutines();
        StartCoroutine(RunTestSequence());
    }

    IEnumerator RunTestSequence()
    {
        statusMessage = "시작하는 중...";
        allResults.Clear();

        if (testScenarios == null || testScenarios.Length == 0)
        {
            Debug.LogError("❌ 테스트 시나리오가 설정되지 않았습니다! Inspector에서 추가해주세요.");
            yield break;
        }

        foreach (var scenario in testScenarios)
        {
            statusMessage = $"측정 중: {scenario.name}";
            Debug.Log($"[Test] 시작: {scenario.name}");
            
            float elapsedTime = 0f;
            float totalFPS = 0f;
            int frameCount = 0;
            float minFPS = float.MaxValue;
            float maxMem = 0f;

            PerformanceTestResult currentResult = new PerformanceTestResult { scenarioName = scenario.name };

            while (elapsedTime < scenario.testDuration)
            {
                // 1. FPS 측정 (unscaledDeltaTime으로 정지 상태 대응)
                float currentFPS = 1.0f / Time.unscaledDeltaTime;
                totalFPS += currentFPS;
                frameCount++;
                minFPS = Mathf.Min(minFPS, currentFPS);

                // 2. 메모리 측정
                float memoryMB = Profiler.GetTotalAllocatedMemoryLong() / (1024f * 1024f);
                maxMem = Mathf.Max(maxMem, memoryMB);

                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            // 결과 데이터 정리
            currentResult.averageFPS = totalFPS / frameCount;
            currentResult.minFPS = minFPS;
            currentResult.maxMemoryMB = maxMem;

            // 통과 여부 판정
            if (currentResult.averageFPS < scenario.targetFPS)
                currentResult.issues.Add($"평균 FPS 미달 ({currentResult.averageFPS:F1})");
            if (currentResult.maxMemoryMB > scenario.maxMemoryMB)
                currentResult.issues.Add($"메모리 초과 ({currentResult.maxMemoryMB:F1}MB)");

            currentResult.passed = (currentResult.issues.Count == 0);
            allResults.Add(currentResult);

            Debug.Log($"[Test] 완료: {scenario.name} ({(currentResult.passed ? "PASS" : "FAIL")})");
            
            yield return new WaitForSecondsRealtime(0.5f); // 시나리오 간 짧은 휴식
        }
        statusMessage = "리포트 생성 완료";
        GenerateFinalReport();
    }

    void GenerateFinalReport()
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine("=== 📊 성능 테스트 결과 리포트 ===");
        report.AppendLine($"기기: {SystemInfo.deviceModel}");
        report.AppendLine("-----------------------------------");
        
        int passCount = 0;
        foreach (var res in allResults)
        {
            string status = res.passed ? "✅ PASS" : "❌ FAIL";
            if (res.passed) passCount++;

            report.AppendLine($"[{res.scenarioName}] {status}");
            report.AppendLine($"- FPS: 평균 {res.averageFPS:F1} / 최소 {res.minFPS:F1}");
            report.AppendLine($"- Max Memory: {res.maxMemoryMB:F1} MB");
            
            if (!res.passed)
            {
                report.AppendLine($"- 이슈: {string.Join(", ", res.issues)}");
            }
            report.AppendLine("-----------------------------------");
        }

        report.AppendLine($"최종 결과: {passCount} / {allResults.Count} 통과");
        
        finalReportText = report.ToString();
        Debug.Log(finalReportText); // 콘솔에 출력
        showUIReport = true; // 화면 GUI 활성화
    }

    private string statusMessage = "대기 중...";
    void OnGUI()
    {
        if (!showUIReport && statusMessage == "대기 중...") return;

        float scale = Screen.width / 400.0f;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1));

        // 박스 크기를 약간 더 키움 (가로 320, 세로 300)
        Rect windowRect = new Rect(40, 50, 320, 300); 
        GUI.backgroundColor = new Color(0, 0, 0, 0.9f);
        GUI.Box(windowRect, "<b>[ TEST RESULT ]</b>");

        GUIStyle reportStyle = new GUIStyle(GUI.skin.label);
        reportStyle.fontSize = 12;
        reportStyle.normal.textColor = Color.white;
        reportStyle.wordWrap = true;
        reportStyle.alignment = TextAnchor.UpperLeft;
    
        // --- 글자 깨짐/사라짐 방지 핵심 설정 ---
        reportStyle.clipping = TextClipping.Overflow; // 영역을 넘쳐도 일단 그리도록 설정
        reportStyle.richText = true;

        string displayBody = showUIReport ? finalReportText : $"<color=yellow>테스트 중: {statusMessage}</color>";

        // 텍스트 출력 영역의 높이를 충분히 (250 -> 220으로 조절하되 여유있게)
        Rect textRect = new Rect(windowRect.x + 10, windowRect.y + 35, windowRect.width - 20, 220);
        GUI.Label(textRect, displayBody, reportStyle);

        if (showUIReport)
        {
            if (GUI.Button(new Rect(windowRect.center.x - 50, windowRect.yMax - 40, 100, 30), "CLOSE"))
            {
                showUIReport = false;
                statusMessage = "대기 중...";
            }
        }
    }
}