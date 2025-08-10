using UnityEngine;
using System.IO;

public class LoadSlot : MonoBehaviour
{
    public SaveSystem saveSystem;
    public int slotIndex = 0;

    DataInfo info;

    void Start()
    {
        info = GetComponent<DataInfo>();
        string slotPath = saveSystem.GetSlotPath(slotIndex);
        if(File.Exists(slotPath))
        {
            string jsonData = File.ReadAllText(slotPath);
            string decryptedData = StrEncryptor.Decrypt(jsonData);
            ProfileData profile = JsonUtility.FromJson<ProfileData>(decryptedData);
            info.profileData = profile;
        }
    }

}
