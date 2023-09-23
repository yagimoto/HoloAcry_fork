using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveController : MonoBehaviour
{
    // 作品をセーブする関数
    public void SaveElements()
    {
        // データを格納するクラスをインスタンス化
        Work WorkData = new Work();

        // 作品内のすべてのElementのデータをWorkDataに格納
        StoreElementData(GlobalVariables.CurrentWork, WorkData);
        
        // WorkDataをJson文字列に変換
        string InsertData = JsonUtility.ToJson(WorkData);

        // すでに保存されているデータを取得
        string OriginalFileData = File.ReadAllText(GlobalVariables.SaveFilePath);

        // WorkDataのインサート位置を定義
        int position = OriginalFileData.Length - 2;

        // WorkDataをインサートする
        string NewFileData = OriginalFileData.Substring(0, position) + InsertData + OriginalFileData.Substring(position);

        // ファイルへの書き込み
        File.WriteAllText(GlobalVariables.SaveFilePath, NewFileData);

        Debug.Log("書き込んだよ");
    }

    // 格納したい作品、格納先を引数として作品内のすべてのオブジェクトを格納する関数
    private void StoreElementData(GameObject CurrentWork, Work WorkData)
    {
        // 作品名を格納
        WorkData.work_name = CurrentWork.transform.name;

        // WorkDataクラス内のelementsを初期化
        WorkData.elements = new List<Element>();
        
        // 作品内のすべてのオブジェクトのデータを格納
        foreach (Transform child in CurrentWork.transform)
        {
            GameObject element = child.gameObject;
            Debug.Log("Elementの名前 " + element.transform.name);

            // Elementの色の情報を取得
            Color ElementColor = element.GetComponent<Renderer>().material.color;

            // Elementのデータを格納
            Element elementData = new Element
            {
                name     = element.transform.name,
                scale    = element.transform.localScale,
                position = element.transform.localPosition,
                color_R  = ElementColor.r,
                color_G  = ElementColor.g,
                color_B  = ElementColor.b,
                color_A  = ElementColor.a,
                rotate   = element.transform.localEulerAngles
            };
            
            // Elementのデータが入ったelementDataをWorkDataに格納
            WorkData.elements.Add(elementData);
        }
    }
}