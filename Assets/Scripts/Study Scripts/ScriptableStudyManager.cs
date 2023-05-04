using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;

[CreateAssetMenu(fileName = "New ScriptableStudyManager",menuName = "Scriptable Objects/ScriptableStudyManager")]
public class ScriptableStudyManager : ScriptableObject
{
    #region participant or important study variables / data

    public int participantNumber = 0;

    [RangeEx(6, 60, 6)] public int numberOfParticipants = 30;

    [NonSerialized] public List<PhysicalState> currentParticipantList = new List<PhysicalState>();

    public int startWithStep = 0;

    public int initalTrials = 30;

    [NonSerialized] public int trial;

    // 0 = start scene, 1 = first physical state, 2 = second, 3 = third state
    [NonSerialized] public int currentStudyBlock = 0;

    [NonSerialized] public (ColorSelection, bool) currentSceneConfig;

    [NonSerialized] public Vector3 participantPos = new Vector3(0, -0.83f, -0.37f);
    [NonSerialized] public Quaternion participantRot;
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
        writer.WriteLine($"ID{_dataSeperator}Block{_dataSeperator}Trial{_dataSeperator}Haptic{_dataSeperator}Color{_dataSeperator}A1{_dataSeperator}A2{_dataSeperator}Participant number");
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

    public void DeleteLastCSVLine()
    {
        var linesList = File.ReadAllLines(writePath).ToList();
        linesList.RemoveAt(linesList.Count - 1);
        File.WriteAllLines(writePath, linesList.ToArray());
    }

    public void ChangeLastCSVLine(string newText)
    {
        string[] arrLine = File.ReadAllLines(writePath);
        int lastLineIndex = arrLine.Length - 1;
        arrLine[lastLineIndex] = newText;
        File.WriteAllLines(writePath, arrLine);
    }

    public void CloseWriter()
    {
        // Close the file
        writer.Close();
    }
    #endregion

    /*
    public (ColorSelection, bool) GetRandomElementOfTrialList(PhysicalState state)
    {
        try
        {
            var count = trialSolidConfigList.Count;
            var last = count - 1;

            switch (state)
            {
                case PhysicalState.Solid:
                    var indexS = UnityEngine.Random.Range(0, trialSolidConfigList.Count - 1);
                    var elementS = trialSolidConfigList[indexS];
                    trialSolidConfigList.RemoveAt(indexS);
                    return elementS;
                case PhysicalState.Liquid:
                    var indexL = UnityEngine.Random.Range(0, trialLiquidConfigList.Count - 1);
                    var elementL = trialLiquidConfigList[indexL];
                    trialSolidConfigList.RemoveAt(indexL);
                    return elementL;
                case PhysicalState.Gas:
                    var indexG = UnityEngine.Random.Range(0, trialGasConfigList.Count - 1);
                    var elementG = trialGasConfigList[indexG];
                    trialSolidConfigList.RemoveAt(indexG);
                    return elementG;
                default:
                    return (ColorSelection.Blue, true);
            }
        } catch (IndexOutOfRangeException ex) {
            Debug.LogError(ex);
            return (ColorSelection.Blue, true);
        }
    }*/

    public void SetupRandomizedTrialOrder(List<(ColorSelection, bool)> trialList)
    {
        var count = trialList.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = trialList[i];
            trialList[i] = trialList[r];
            trialList[r] = tmp;
        }
    }

    public (ColorSelection, bool) GetCurrentSceneConfig(PhysicalState physicalState)
    {
        int currentTrial = trial - 1;
        (ColorSelection, bool) config;
        switch (physicalState)
        {
            case PhysicalState.Solid:
                config = trialSolidConfigList[currentTrial];
                break;
            case PhysicalState.Liquid:
                config = trialLiquidConfigList[currentTrial];
                break;
            case PhysicalState.Gas:
                config = trialGasConfigList[currentTrial];
                break;
            default:
                config = (ColorSelection.Neutral, true);
                break;
        }

        currentSceneConfig = config;
        return config;

    }

    public string GetUniqueID()
    {
        string returnString = "";
        // solid = 1; liquid = 2; gas = 3;
        var block = currentParticipantList[currentStudyBlock];

        switch (block)
        {
            case PhysicalState.Solid:
                returnString += "1";
                break;
            case PhysicalState.Liquid:
                returnString += "2";
                break;
            case PhysicalState.Gas:
                returnString += "3";
                break;
        }
        
        // blue = 1; neutral = 2; red = 3;
        var color = currentSceneConfig.Item1;

        switch (color)
        {
            case ColorSelection.Blue:
                returnString += "1";
                break;
            case ColorSelection.Neutral:
                returnString += "2";
                break;
            case ColorSelection.Red:
                returnString += "3";
                break;
        }

        // no haptic = 1; haptic = 2;
        var haptic = currentSceneConfig.Item2;

        if (haptic)
        {
            returnString += "2";
        } else
        {
            returnString += "1";
        }

        var count = "0";
        switch (block)
        {
            case PhysicalState.Solid:
                // in trial block time of combination 1st time = 1 .... 5th time = 5;
                count = CountOccurrencesUpToIndex(trialSolidConfigList, currentSceneConfig, trial - 1).ToString();
                returnString += count;
                break;
            case PhysicalState.Liquid:
                // in trial block time of combination 1st time = 1 .... 5th time = 5;
                count = CountOccurrencesUpToIndex(trialLiquidConfigList, currentSceneConfig, trial - 1).ToString();
                returnString += count;
                break;
            case PhysicalState.Gas:
                // in trial block time of combination 1st time = 1 .... 5th time = 5;
                count = CountOccurrencesUpToIndex(trialGasConfigList, currentSceneConfig, trial - 1).ToString();
                returnString += count;
                break;
        }

        

        return returnString;
    }

    public int CountOccurrencesUpToIndex(List<(ColorSelection, bool)> list, (ColorSelection, bool) element, int index)
    {
        int count = 0;
        for (int i = 0; i <= index && i < list.Count; i++)
        {
            if (list[i].Equals(element))
            {
                count++;
            }
        }
        return count;
    }
}
