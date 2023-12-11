using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SynchronizeSubtitlesWithVideo : MonoBehaviour
{
    [SerializeField] private double[] m_subsTimestamps;

   private ShowMessageFromList m_closedCaptionText;
    private VideoPlayer m_video;

    private int m_curIndex = 0;
    private void Awake()
    {
        if(GetComponent<ClosedCaptions>()!=null && GetComponent<ClosedCaptions>().ClosedCaption!=null) m_closedCaptionText = GetComponent<ClosedCaptions>().ClosedCaption.GetComponentInChildren<ShowMessageFromList>();
        m_video= GetComponent<VideoPlayer>();
    }

    

    // Update is called once per frame
    void Update()
    {
       
        if (m_video != null && m_closedCaptionText != null && m_video.isPlaying)
        {
            SendMessage("ToggleClosedCaptionsActivity", true, SendMessageOptions.DontRequireReceiver);
            var curTime = m_video.time;//m_video.clip.length * m_video.frame / m_video.clip.frameCount;
            
            var newIndex = GetCurrentSubIndex(curTime);
            
            if (m_curIndex != newIndex)
            {
                m_curIndex = newIndex;
                if(m_closedCaptionText!=null) m_closedCaptionText.ShowMessageAtIndex(m_curIndex);
            }
        }
        /* видео останавливается за какие то миллисекунды до конца (типа 3 фрейма как пишут в инете). можно вставить это параметром в проверке но я забью
        Debug.Log(m_video.time + " " + m_video.length);
        if (m_video != null && m_closedCaptionText != null && m_video.time>= m_video.length)
        {
            SendMessage("ToggleClosedCaptionsActivity", false, SendMessageOptions.DontRequireReceiver);
        }*/
    }

    private int GetCurrentSubIndex(double time)
    {
        var index = 0;
        if (m_subsTimestamps.Length == 0) return 0;
        else if (time < m_subsTimestamps[0]) return 0;
        else if (time > m_subsTimestamps[m_subsTimestamps.Length - 1]) return m_subsTimestamps.Length - 1;
        else index = CheckInterval(0, m_subsTimestamps.Length - 1, time);
        return index;
    }

    private int CheckInterval(int left, int right, double value)
    {
        var center = Mathf.CeilToInt((left + right) / 2);
        if (value>=m_subsTimestamps[center])
        {
            if (value<m_subsTimestamps[center + 1]) return center;
            else return CheckInterval(center+1, right,value);
        }
        else
        {
            if (value>=m_subsTimestamps[center - 1]) return (center-1);
            else return CheckInterval(left, center - 1, value);
        }

    }
}
