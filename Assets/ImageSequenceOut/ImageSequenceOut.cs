// Image Sequence Output Utility
// http://github.com/keijiro/SequenceOut
using UnityEngine;

public class ImageSequenceOut : MonoBehaviour
{
    #region Properties

    [SerializeField, Range(1, 60)]
    int _frameRate = 30;

    public int frameRate {
        get { return _frameRate; }
        set { _frameRate = value; }
    }

    [SerializeField, Range(1, 4)]
    public int _superSampling = 1;

    public int superSampling {
        get { return _superSampling; }
        set { _superSampling = value; }
    }

    [SerializeField]
    public bool _recordOnStart;

    public bool isRecording { get; private set; }

    public int frameCount { get; private set; }

    #endregion

    #region Public Methods

    public void StartRecording()
    {
        System.IO.Directory.CreateDirectory("Capture");
        Time.captureFramerate = _frameRate;
        frameCount = -1;
        isRecording = true;
    }

    public void StopRecording()
    {
        Time.captureFramerate = 0;
        isRecording = false;
    }

    #endregion

    #region MonoBehaviour Functions

    void Start()
    {
        if (_recordOnStart) StartRecording();
    }

    void Update()
    {
        if (isRecording)
        {
            if (frameCount > 0)
            {
                var name = "Capture/frame" + frameCount.ToString("0000") + ".png";
                Application.CaptureScreenshot(name, _superSampling);
            }
            frameCount++;
        }
    }

    #endregion
}
