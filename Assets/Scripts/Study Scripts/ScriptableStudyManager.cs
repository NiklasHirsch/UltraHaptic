using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "New ScriptableStudyManager",menuName = "Scriptable Objects/ScriptableStudyManager")]
public class ScriptableStudyManager : ScriptableObject
{
    #region participant or important study variables / data

    public int participantNumber = 0;

    [RangeEx(6, 60, 6)]
    public int numberOfParticipants = 30;

    [NonSerialized]
    public List<PhysicalState> currentParticipantList = new List<PhysicalState>();

    public int initalTrials = 5;
    [NonSerialized]
    public int trial;

    // 0 = start scene, 1 = first physical state, 2 = second, 3 = third state
    [NonSerialized]
    public int currentStudyBlock = 0;

    [NonSerialized]
    public (ColorSelection, bool) currentSceneConfig;
    #endregion

    #region csv writer variables
    public StreamWriter writer;
    public string writePath;
    public string _csvSubFolder = "TestFiles";
    public string _csvFileName = "example";
    public string _dataSeperator = ";";
    #endregion

    #region setup data
    // 30 Elements per List
    public List<(ColorSelection, bool)> trialSolidConfigList = new List<(ColorSelection, bool)>
      {
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
      };
    public List<(ColorSelection, bool)> trialLiquidConfigList = new List<(ColorSelection, bool)>
      {
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
      };
    public List<(ColorSelection, bool)> trialGasConfigList = new List<(ColorSelection, bool)>
      {
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
          (ColorSelection.Blue, true),
          (ColorSelection.Blue, false),
          (ColorSelection.Neutral, true),
          (ColorSelection.Neutral, false),
          (ColorSelection.Red, true),
          (ColorSelection.Red, false),
      };

    public List<List<PhysicalState>> latinSquareList = new List<List<PhysicalState>>();
    public List<PhysicalState> latinSquareList1 = new List<PhysicalState>();
    public List<PhysicalState> latinSquareList2 = new List<PhysicalState>();
    public List<PhysicalState> latinSquareList3 = new List<PhysicalState>();
    public List<PhysicalState> latinSquareList4 = new List<PhysicalState>();
    public List<PhysicalState> latinSquareList5 = new List<PhysicalState>();
    public List<PhysicalState> latinSquareList6 = new List<PhysicalState>();
    #endregion

    #region testing
    public bool disableHapticFeedbackElements = false;
    #endregion

    public void SetupParticipantList()
    {
        currentParticipantList = latinSquareList[participantNumber - 1];
    }
    public void SetupBasicLatinSquareLists()
    {
        latinSquareList1.Add(PhysicalState.Solid);
        latinSquareList1.Add(PhysicalState.Liquid);
        latinSquareList1.Add(PhysicalState.Gas);

        latinSquareList2.Add(PhysicalState.Solid);
        latinSquareList2.Add(PhysicalState.Gas);
        latinSquareList2.Add(PhysicalState.Liquid);

        latinSquareList3.Add(PhysicalState.Gas);
        latinSquareList3.Add(PhysicalState.Solid);
        latinSquareList3.Add(PhysicalState.Liquid);

        latinSquareList4.Add(PhysicalState.Gas);
        latinSquareList4.Add(PhysicalState.Liquid);
        latinSquareList4.Add(PhysicalState.Solid);

        latinSquareList5.Add(PhysicalState.Liquid);
        latinSquareList5.Add(PhysicalState.Gas);
        latinSquareList5.Add(PhysicalState.Solid);

        latinSquareList6.Add(PhysicalState.Liquid);
        latinSquareList6.Add(PhysicalState.Solid);
        latinSquareList6.Add(PhysicalState.Gas);
    }

    public void SetupLatinSquareList()
    {
        Debug.Log("<color=#0000FF>" + numberOfParticipants + "</color>");
        for (int p = 0; p < numberOfParticipants; p++)
        {
            var num = (p % 6) + 1;
            //Debug.Log("Modulo Num: " + num + ", p: " + p);
            switch (num)
            {
                case 1:
                    latinSquareList.Add(latinSquareList1);
                    break;
                case 2:
                    latinSquareList.Add(latinSquareList2);
                    break;
                case 3:
                    latinSquareList.Add(latinSquareList3);
                    break;
                case 4:
                    latinSquareList.Add(latinSquareList4);
                    break;
                case 5:
                    latinSquareList.Add(latinSquareList5);
                    break;
                case 6:
                    latinSquareList.Add(latinSquareList6);
                    break;
            }
        }

        LogList(latinSquareList);
    }

    public void LogList(List<List<PhysicalState>> list)
    {
        // result String of list content
        string listContent = "";

        int listIndex = 0;
        foreach (var listX in list)
        {
            listContent += $"Participant {listIndex + 1}: ";
            foreach (var pState in listX)
            {
                
                listContent += $"{pState}, ";
            }
            listContent += "\n";
            listIndex++;
        }

        Debug.Log("<color=#00FFFF>" + listContent + "</color>");
    }

    #region CSV Writer
    // ----------- CSV Writer -------------
    public void SetupWriter()
    {
        // Set the parent folder path
        string parentFolderPath = Application.dataPath + "/CSV/" + _csvSubFolder + "/";

        // Create a subfolder for the current day if it doesn't already exist
        string folderName = DateTime.Now.ToString("MM-dd");
        string folderPath = Path.Combine(parentFolderPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string timeStamp = DateTime.Now.ToString("HH-mm-ss");

        // Set the file path and name
        string fileName = _csvFileName + " t_" + timeStamp + " p_" + participantNumber + ".csv";
        string filePath = Path.Combine(folderPath, fileName);

        writePath = filePath;

        // Create a new file
        writer = new StreamWriter(filePath);

        Debug.Log("< color =#00FF00> File created successfully. </color>");

        //writer.WriteLine($"Date:{_dataSeperator}" +
        //    $"{DateTime.Now:yyyyddHHmmss}");

        //writer.WriteLine($"Participant:{_dataSeperator}" + participantNumber);

        // Main Line
        writer.WriteLine($"Block{_dataSeperator}Trial{_dataSeperator}Haptic{_dataSeperator}Color{_dataSeperator}A1{_dataSeperator}A2");
       /*
       writer.WriteLine($"B-LS Block:{_dataSeperator}" +
           $"{latinSquareList[participantNumber - 1][0]}{_dataSeperator}" +
           $"{latinSquareList[participantNumber - 1][1]}{_dataSeperator}" +
           $"{latinSquareList[participantNumber - 1][2]}");
       */

       CloseWriter();
    }

    public void AppendCSVLine(string input)
    {
        writer = new StreamWriter(writePath, true);

        // Write some data to the file
        writer.WriteLine(input);

        CloseWriter();
    }

    public void CloseWriter()
    {
        // Close the file
        writer.Close();
    }
    #endregion

    public (ColorSelection, bool) GetRandomElementOfTrialList(PhysicalState state)
    {
        Random.InitState(participantNumber);
        try
        {
            switch (state)
            {
                case PhysicalState.Solid:
                    var indexS = UnityEngine.Random.Range(0, trialSolidConfigList.Count - 1);
                    var elementS = trialSolidConfigList[indexS];
                    trialSolidConfigList.RemoveAt(indexS);
                    return elementS;
                case PhysicalState.Liquid:
                    var indexL = UnityEngine.Random.Range(0, trialSolidConfigList.Count - 1);
                    var elementL = trialSolidConfigList[indexL];
                    trialSolidConfigList.RemoveAt(indexL);
                    return elementL;
                case PhysicalState.Gas:
                    var indexG = UnityEngine.Random.Range(0, trialSolidConfigList.Count - 1);
                    var elementG = trialSolidConfigList[indexG];
                    trialSolidConfigList.RemoveAt(indexG);
                    return elementG;
                default:
                    return (ColorSelection.Blue, true);
            }
        } catch (IndexOutOfRangeException ex) {
            Debug.LogError(ex);
            return (ColorSelection.Blue, true);
        }
    }
}
