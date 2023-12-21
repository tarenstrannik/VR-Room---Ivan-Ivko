using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_boxCount;
    [SerializeField] private TextMeshProUGUI m_timeSpent;

    private float m_curTime = 0;
    [SerializeField] private int m_curBoxCount = 0;

    [SerializeField] GameObject m_savedMessage;

    private bool m_IsStarted=false;
    public bool IsStarted
    {
        set
        {
            m_IsStarted = value;
        }
    }

    [SerializeField] List<GameObject> m_boxes = new List<GameObject>();


    [System.Serializable]
    class SavedTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    [System.Serializable]
    class SaveData
    {

        public float timeSpent;
        public List<SavedTransform> placement;
    }


    private IEnumerator ShowSavedMessage()
    {
        m_savedMessage.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_savedMessage.SetActive(false);
    }
    public void UpdateTime()
    {
        m_curTime += Time.deltaTime;
        UpdateTimeUI();
    }
    public void UpdateTimeUI()
    {
        m_timeSpent.text = "Time spent: " + m_curTime.ToString("F1");
    }
    public void UpdateBoxCount(int delta)
    {
        m_curBoxCount +=delta;
        UpdateBoxCountUI();

    }
    public void UpdateBoxCountUI()
    {
        
        m_boxCount.text = "Box count: " + m_curBoxCount;

    }
    private void Start()
    {

        m_timeSpent.text = "Time spent: 0.0";
        m_boxCount.text = "Box count: " + m_curBoxCount;
    }
    private void Update()
    {
        if(m_IsStarted)
            UpdateTime();
    }

    public void ResetUI()
    {

        m_curTime = 0;

        UpdateTimeUI();

    }
    public void SavePlayerData()
    {
        var listOfBoxes = new List<SavedTransform>();
        foreach( GameObject obj in m_boxes)
        {
            SavedTransform objTransform = new SavedTransform();
            objTransform.position = obj.transform.position;
            objTransform.rotation = obj.transform.rotation;
            objTransform.scale = obj.transform.localScale;
            listOfBoxes.Add(objTransform);
        }
        

        SaveData data = new SaveData
        {

            timeSpent = m_curTime,
            placement = listOfBoxes
        };


        string json = JsonUtility.ToJson(data);
#if UNITY_WEBGL
        // Выполнить сохранение в PlayerPref

        PlayerPrefs.SetString("PunkVsZombiesSave", json);
        PlayerPrefs.Save();
#else
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
#endif

        StartCoroutine(ShowSavedMessage());
    }

    public void LoadPlayerData()
    {

        string json = "";
#if UNITY_WEBGL
        json = PlayerPrefs.GetString("PunkVsZombiesSave");
        if (json.Trim() == "")
        {
            SetDefaultParams();
            SetDefaultPlayerParams();
        }
#else
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);

        }
        else
        {
           
            m_curTime = 0;
            foreach (GameObject obj in m_boxes)
            {
                obj.GetComponent<TranslateObject>().ResetToStartingPosition();
            }
        }
#endif
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        if (data != null)
        {
            
            m_curTime = data.timeSpent;
            var listOfBoxes = data.placement;
            var i = 0;
            foreach (GameObject obj in m_boxes)
            {
                if (data.placement[i] != null)
                {
                    obj.transform.position= data.placement[i].position;
                    obj.transform.rotation = data.placement[i].rotation;
                    obj.transform.localScale = data.placement[i].scale;
                }
                    else
                    {
                        obj.GetComponent<TranslateObject>().ResetToStartingPosition();
                    }
                i++;
            }
        }
        else
        {
           
            m_curTime = 0;
                foreach (GameObject obj in m_boxes)
                {
                    obj.GetComponent<TranslateObject>().ResetToStartingPosition();
                }
            }

        UpdateBoxCountUI();
        UpdateTimeUI();
    }
}
