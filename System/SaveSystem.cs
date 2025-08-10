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

    public void SetData(DataInfo info)
    {
        string profileName = SaveName(info.index);
        DateTime now = DateTime.Now;
        ProfileData profData = info.profileData;
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

    public ProfileData GetData(int slotIndex)
    {
        string jsonPath = Path.Combine(profilesPath + @"\" + SaveName(slotIndex));
        if(File.Exists(jsonPath))
        {
            try
            {
                string json = File.ReadAllText(jsonPath);
                string decryptedJson = StrEncryptor.Decrypt(json);
                ProfileData profile = JsonUtility.FromJson<ProfileData>(decryptedJson);
                return profile;
            }
            catch(Exception ex)
            {
                Debug.LogError($"[LOAD ERROR] {ex}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("[LOAD WARNING] File does not exist!");
            return new ProfileData(){profileName="EMPTY SLOT", lastSaveDate=""};
        }
    }

    public void DeleteSlot(int slotIndex)
    {
        string jsonPath = Path.Combine(profilesPath + @"\" + SaveName(slotIndex));

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
        // I didn't develop this yet.
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