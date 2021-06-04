using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider slider;
    public AudioSource audioSource;
    public Button btnStart;
    public Button btnSave;
    public Button btnSubmit;
    public Button btnLoad;
    public Button btnExit;
    //public Dropdown dpScenes;
    public Canvas canvas;
    public GameObject WhiteBall;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = audioSource.volume;
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        //dpScenes.onValueChanged.AddListener(delegate { DropdownValueChanged(dpScenes); });

        btnStart.onClick.AddListener(StartClicked);
        btnSubmit.onClick.AddListener(SubmitClicked);
        btnSave.onClick.AddListener(SaveClicked);
        btnLoad.onClick.AddListener(LoadClicked);
        btnExit.onClick.AddListener(ExitClicked);
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("music_volume"))
        {
            var musicVoume = PlayerPrefs.GetFloat("music_volume");
            audioSource.volume = musicVoume;
            slider.value = musicVoume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameStatus.GameIsRunning)
        {
            GameStatus.GameIsRunning = false;
            canvas.gameObject.SetActive(true);
        }
    }

    public void StartClicked()
    {
        //Debug.Log("Clicked start");
        GameStatus.GameIsRunning = true;
        canvas.gameObject.SetActive(false);
    }

    public void SubmitClicked()
    {
        //Debug.Log("Clicked submit");        
        PlayerPrefs.SetFloat("music_volume", slider.value);
        PlayerPrefs.Save();
    }

    public void SaveClicked()
    {
        //Debug.Log("Clicked save");

        var saveData = CreateSaveGameObject();

        // Guardar data en archivo binario
        BinaryFormatter bf = new BinaryFormatter();
        var filePath = Application.persistentDataPath + "/gamesave.save";
        Debug.Log(filePath);

        FileStream file = File.Create(filePath);
        bf.Serialize(file, saveData);
        file.Close();

        Debug.Log("Game Saved");

        // Save in json
        var filePathJson = Application.persistentDataPath + "/gamesave.json";
        Debug.Log(filePathJson);

        // Serialize to json
        string jsonData = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePathJson, jsonData);

        Debug.Log("Saving as JSON: " + jsonData);
    }

    private SaveData CreateSaveGameObject()
    {
        var whiteBallData = new SaveData();
        whiteBallData.positionX = WhiteBall.transform.position.x;
        whiteBallData.positionY = WhiteBall.transform.position.y;
        whiteBallData.positionZ = WhiteBall.transform.position.z;

        return whiteBallData;
    }

    public void LoadClicked()
    {
        //Debug.Log("Clicked load");

        var filePath = Application.persistentDataPath + "/gamesave.save";
        var filePathJson = Application.persistentDataPath + "/gamesave.json";
        if (File.Exists(filePath))
        {
            /*BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            SaveData save = (SaveData)bf.Deserialize(file);
            file.Close();

            Debug.Log(save);

            nave.transform.position = new Vector3(save.positionX, save.positionY, save.positionZ);
            Debug.Log("Game Loaded");*/

            // Load from json
            var jsonData = File.ReadAllText(filePathJson);
            var save = JsonUtility.FromJson<SaveData>(jsonData);

            Debug.Log(save);

            WhiteBall.transform.position = new Vector3(save.positionX, save.positionY, save.positionZ);
            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    public void ValueChangeCheck()
    {
        //Debug.Log(slider.value);
        audioSource.volume = slider.value;
    }

    public void DropdownValueChanged(Dropdown change)
    {
        Debug.Log("New value" + change.value);
        switch (change.value)
        {
            case 1:
                SceneManager.LoadScene("Disparo");
                break;
            case 2:
                SceneManager.LoadScene("RayCast");
                break;
        }
    }

    public void ExitClicked()
    {
        Application.Quit();
    }
}
