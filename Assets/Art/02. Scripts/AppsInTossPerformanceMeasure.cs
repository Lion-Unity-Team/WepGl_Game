using UnityEngine;
using UnityEngine.Profiling;
using System.Collections.Generic;
using System.Text;

public class AppsInTossPerformanceMeasure : MonoBehaviour
{
    [Header("측정 설정")]
    public bool enableMeasurement = true;
    public float measureInterval = 1.0f;
    public int maxSampleCount = 300; // 5분간 데이터
    
    [Header("성능 임계값")]
    public float fpsWarningThreshold = 30f;
    public float memoryWarningThreshold = 200f; // MB
    public float startupTimeLimit = 5f; // 초
    
    private Dictionary<string, PerformanceMetric> metrics = new Dictionary<string, PerformanceMetric>();
    private float startTime;
    private bool startupMeasured = false;
    
    [System.Serializable]
    public class PerformanceMetric
    {
        public string name;
        public Queue<float> samples = new Queue<float>();
        public float currentValue;
        public float averageValue;
        public float minValue = float.MaxValue;
        public float maxValue = float.MinValue;
        
        public void AddSample(float value)
        {
            samples.Enqueue(value);
            if (samples.Count > 300) samples.Dequeue(); // 최대 샘플 수 유지
            
            currentValue = value;
            minValue = Mathf.Min(minValue, value);
            maxValue = Mathf.Max(maxValue, value);
            
            // 평균 계산
            float sum = 0;
            foreach (float sample in samples)
                sum += sample;
            averageValue = sum / samples.Count;
        }
    }
    
    void Awake()
    {
        startTime = Time.realtimeSinceStartup;
        InitializeMetrics(); //
    }
    
    void Start()
    {
        if (enableMeasurement)
        {
            // 지정된 간격으로 측정 실행
            InvokeRepeating(nameof(MeasurePerformance), 0f, measureInterval);
        }
    }
    
    void InitializeMetrics()
    {
        metrics["FPS"] = new PerformanceMetric { name = "FPS" };
        metrics["Memory"] = new PerformanceMetric { name = "Memory (MB)" };
        metrics["DrawCalls"] = new PerformanceMetric { name = "Draw Calls" };
        metrics["CPUFrameTime"] = new PerformanceMetric { name = "CPU Frame Time (ms)" }; //
    }
    
    void MeasurePerformance()
    {
        // 1. FPS 측정 (unscaledDeltaTime 사용으로 일시정지 중에도 측정 가능)
        float fps = 1.0f / Time.unscaledDeltaTime;
        metrics["FPS"].AddSample(fps);
        
        // 2. 메모리 측정 (MB 단위로 변환, Long API 사용하여 에러 방지)
        long allocatedMemory = Profiler.GetTotalAllocatedMemoryLong();
        float memoryMB = allocatedMemory / (1024f * 1024f);
        metrics["Memory"].AddSample(memoryMB);
        
        // 3. 드로우 콜 측정 (런타임 에러 방지를 위해 에디터에서만 측정하거나 0 처리)
        int drawCalls = 0;
#if UNITY_EDITOR
        drawCalls = UnityEditor.UnityStats.drawCalls; 
#endif
        metrics["DrawCalls"].AddSample(drawCalls);
        
        // 4. CPU 프레임 시간 (ms)
        float cpuTime = Time.unscaledDeltaTime * 1000f;
        metrics["CPUFrameTime"].AddSample(cpuTime);
        
        // 시작 시간 측정 (최초 1회)
        if (!startupMeasured && Time.realtimeSinceStartup > 1f)
        {
            float startupTime = Time.realtimeSinceStartup - startTime;
            LogStartupTime(startupTime);
            startupMeasured = true;
        }
        
        // 경고 임계값 체크
        CheckPerformanceWarnings();
    }
    
    void CheckPerformanceWarnings()
    {
        // FPS 저하 감지
        if (metrics["FPS"].currentValue < fpsWarningThreshold)
        {
            Debug.LogWarning($"[Performance] Low FPS: {metrics["FPS"].currentValue:F1}");
            SendWarningToAppsInToss("low_fps", metrics["FPS"].currentValue);
        }
        
        // 메모리 과다 사용 감지
        if (metrics["Memory"].currentValue > memoryWarningThreshold)
        {
            Debug.LogWarning($"[Performance] High Memory: {metrics["Memory"].currentValue:F1}MB");
            SendWarningToAppsInToss("high_memory", metrics["Memory"].currentValue);
        }
    }
    
    void LogStartupTime(float startupTime)
    {
        Debug.Log($"[Performance] Startup time: {startupTime:F2}s");
        
        if (startupTime > startupTimeLimit)
        {
            Debug.LogWarning($"Startup time exceeds limit: {startupTime:F2}s");
        }
        
        SendMetricToAppsInToss("startup_time", startupTime); //
    }
    
    void SendWarningToAppsInToss(string warningType, float value)
    {
        var data = new Dictionary<string, object>
        {
            {"warning_type", warningType},
            {"value", value},
            {"timestamp", System.DateTime.UtcNow.ToString("o")},
            {"device_model", SystemInfo.deviceModel}
        };
        
        // 토스 앱 자바스크립트 엔진으로 데이터 전송
        Application.ExternalCall("SendPerformanceWarning", JsonUtility.ToJson(data));
    }
    
    void SendMetricToAppsInToss(string metricName, float value)
    {
        var data = new Dictionary<string, object>
        {
            {"metric_name", metricName},
            {"value", value},
            {"timestamp", System.DateTime.UtcNow.ToString("o")}
        };
        
        Application.ExternalCall("SendPerformanceMetric", JsonUtility.ToJson(data)); //
    }
    
    void OnGUI()
    {
        if (!enableMeasurement) return;
        
        float scale = Screen.width / 400.0f; // 기준 너비를 400으로 잡고 배율 계산
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1));
        
        // 화면 왼쪽 상단에 실시간 성능 정보 표시
        GUILayout.BeginArea(new Rect(10, 10, 350, 200));
        GUI.color = Color.black;
        GUILayout.Box("AppsInToss Performance Monitor");
        GUI.color = Color.white;
        
        GUILayout.Label($"FPS: {metrics["FPS"].currentValue:F1} (Avg: {metrics["FPS"].averageValue:F1})");
        GUILayout.Label($"Memory: {metrics["Memory"].currentValue:F1} MB");
        
        if (metrics["FPS"].currentValue < fpsWarningThreshold)
        {
            GUI.color = Color.red;
            GUILayout.Label("⚠ LOW FPS WARNING");
        }
        
        if (metrics["Memory"].currentValue > memoryWarningThreshold)
        {
            GUI.color = Color.red;
            GUILayout.Label("⚠ HIGH MEMORY WARNING");
        }
        
        GUI.color = Color.white;
        GUILayout.EndArea();
    }
}