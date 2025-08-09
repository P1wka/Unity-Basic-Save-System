using UnityEngine;
using TMPro;
using System.IO;

public class LoadSlot : MonoBehaviour
{
    public TMP_Text slotText;
    // If you are using another Text type, you can change this.
    // This is just an example.
    // =)
    public SaveSystem saveSystem;
    public int slotIndex = 0;

    void Start()
    {
        string slotPath = saveSystem.GetSlotPath(slotIndex);
        if(File.Exists(slotPath))
        {
            string jsonData = File.ReadAllText(slotPath);
            string decryptedData = StrEncryptor.Decrypt(jsonData);
            ProfileData profile = JsonUtility.FromJson<ProfileData>(decryptedData);
            slotText.text = $"{profile.profileName} - {profile.lastSaveDate}";
        }
    }

}
