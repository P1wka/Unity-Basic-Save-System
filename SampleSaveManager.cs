using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SampleSaveManager : SaveSystem
{
    public TMP_InputField nameInput, dateInput;
    public TMP_Text nameText, dateText;

    [HideInInspector]public DataInfo dataInfo;

    public void SetDatas(int index)
    {
        dataInfo = new DataInfo();
        dataInfo.index = index;
        dataInfo.profileData = new ProfileData() {profileName=nameInput.text, lastSaveDate=dateInput.text, playerLocation=Vector3.zero};
        SetData(dataInfo);
    }

    public void LoadDatas(int index)
    {
        ProfileData profile = GetData(index);
        nameText.text = profile.profileName;
        dateText.text = profile.lastSaveDate;
    }

    public void DeleteDatas(int index)
    {
        DeleteSlot(index);
    }
}
