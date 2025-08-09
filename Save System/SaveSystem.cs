using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

// #author: Piwka ~
public class SaveSystem : MonoBehaviour
{
    public const string SNAME = "Piw_SV";
    public const string EXT = ".ata";
    public const string FOLDER = "Piwkas_Game";
    public const string PROFILES = "profiles";
    public const string SETTINGS = "settings";
    public const string BACKUP = "BACKUP_settings.bin";
    public const string EMPTYSLOT = "EMPTY SLOT";

    private string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    private string dataPath;
    private string profilesPath;

    private void Awake()
    {
        dataPath = Path.Combine(docsPath, FOLDER);
        profilesPath = Path.Combine(dataPath, PROFILES);
    }

    public void SetData(int index)
    {
        string profileName = SaveName(index);
        DateTime now = DateTime.Now;
        ProfileData profData = new ProfileData() {profileName="Piwka", lastSaveDate=now.ToString("yyyy-MM-dd HH:mm:ss"), playerLocation=Vector3.zero};
        string json = JsonUtility.ToJson(profData);
        string encryptedJson = StrEncryptor.Encrypt(json);

        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
            Debug.Log("[SAVE DATA] Data path created.");
        }
        if(!Directory.Exists(profilesPath))
        {
            Directory.CreateDirectory(profilesPath);
            Debug.Log("[SAVE DATA] Profiles path created.");
        }
        
        string filePath = Path.Combine(profilesPath, profileName);
        File.WriteAllText(filePath, encryptedJson);
        
    }

    public void GetData(int index)
    {
        string jsonPath = Path.Combine(profilesPath + @"\" + SaveName(index));
        if(File.Exists(jsonPath))
        {
            try
            {
                string json = File.ReadAllText(jsonPath);
                string decryptedJson = StrEncryptor.Decrypt(json);
                ProfileData profile = JsonUtility.FromJson<ProfileData>(decryptedJson);

                // SAMPLE
                Debug.Log(profile.profileName + profile.lastSaveDate + profile.playerLocation);
            }
            catch(Exception ex)
            {
                Debug.LogError($"[LOAD ERROR] {ex}");
            }
        }
        else
        {
            Debug.LogError("[LOAD ERROR] File does not exist!");
        }
    }

    public void DeleteSlot(int index)
    {
        string jsonPath = Path.Combine(profilesPath + @"\" + SaveName(index));

        if(File.Exists(jsonPath))
        {
            File.Delete(jsonPath);
        }
        else
        {
            Debug.LogWarning("Save couldn't find.");
        }
    }

    private string SaveName(int index)
    {
        return SNAME + index.ToString() + EXT;
    }

    public void ApplySettings()
    {
        string settingsPath = Path.Combine(dataPath, SETTINGS);
        if(!Directory.Exists(settingsPath))
        {
            Directory.CreateDirectory(settingsPath);
        }

        string backupFile = Path.Combine(settingsPath, BACKUP);

        // SAMPLE
        SettingsData settingsData = new SettingsData();
        string settingsJson = JsonUtility.ToJson(settingsData).ToString();
        File.WriteAllText(backupFile, settingsJson);
    }

    public void LoadSettings()
    {
        // Do it yourself.
    }

    public string GetSlotPath(int index)
    {
        string path = Path.Combine(profilesPath + @"\" + SaveName(index));
        if(!File.Exists(path))
        {
           return EMPTYSLOT;
        }
        return path;
    }
}