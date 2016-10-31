/*********************************************************************************
 *Copyright(C) 2015 by LiYupeng
 *All rights reserved.
 *FileName:     Lua2Utf8.cs
 *Author:       LiYupeng
 *Version:      1.0
 *UnityVersion：5.4.0f3
 *Date:         2016-10-31
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

using UnityEditor;

using System;
using System.IO;
using System.Text;

public class Lua2Utf8
{
    public static string luaPath = "Assets/Lua";
    public static System.Text.Encoding utf8 = System.Text.Encoding.GetEncoding("utf-8");
    [MenuItem("KOL/Lua/Lua2Utf8")]
    public static void Lua2Utf8NoneBom()
    {
        Lua2Utf8NoneBomForFolder(luaPath);
    }

    public static void Lua2Utf8NoneBomForFolder(string folderPath)
    {
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        foreach (DirectoryInfo dirChild in dir.GetDirectories())
        {
            Lua2Utf8NoneBomForFolder(dirChild.FullName);
        }

        foreach (FileInfo file in dir.GetFiles())
        {
            Lua2Utf8NoneBomForFile(file.FullName);
        }
    }

    public static void Lua2Utf8NoneBomForFile(string filePath)
    {
        System.Text.Encoding encoding = GetType(filePath);
        if (encoding == utf8)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            r.Close();
            fs.Close();
            if ((ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                utf8WithBom2Utf8WithoutBom(filePath);
            }            

        }
    }

    public static void utf8WithBom2Utf8WithoutBom(string filePath)
    {
        Encoding utf8WithoutBom = new System.Text.UTF8Encoding(false);
        Encoding utf8WithBom = new System.Text.UTF8Encoding(true);
        //FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        StreamReader sr = new StreamReader(filePath, utf8WithBom);
        string data = sr.ReadToEnd();
        sr.Close();

        StreamWriter sw = new StreamWriter(filePath,false, utf8WithoutBom);
        sw.Write(data);
        sw.Close();
    }

    public static System.Text.Encoding GetType(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        Encoding r = GetType(fs);
        fs.Close();
        return r;
    }

    public static System.Text.Encoding GetType(FileStream fs)
    {
        byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
        byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
        byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
        Encoding reVal = Encoding.Default;

        BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
        int i;
        int.TryParse(fs.Length.ToString(), out i);
        byte[] ss = r.ReadBytes(i);
        if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
        {
            reVal = Encoding.UTF8;
        }
        else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
        {
            reVal = Encoding.BigEndianUnicode;
        }
        else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
        {
            reVal = Encoding.Unicode;
        }
        r.Close();
        return reVal;

    }

    /// <summary>
    /// 判断是否是不带 BOM 的 UTF8 格式
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static bool IsUTF8Bytes(byte[] data)
    {
        int charByteCounter = 1; //计算当前正分析的字符应还有的字节数
        byte curByte; //当前分析的字节.
        for (int i = 0; i < data.Length; i++)
        {
            curByte = data[i];
            if (charByteCounter == 1)
            {
                if (curByte >= 0x80)
                {
                    //判断当前
                    while (((curByte <<= 1) & 0x80) != 0)
                    {
                        charByteCounter++;
                    }
                    //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                    if (charByteCounter == 1 || charByteCounter > 6)
                    {
                        return false;
                    }
                }
            }
            else
            {
                //若是UTF-8 此时第一位必须为1
                if ((curByte & 0xC0) != 0x80)
                {
                    return false;
                }
                charByteCounter--;
            }
        }
        if (charByteCounter > 1)
        {
            throw new Exception("非预期的byte格式");
        }
        return true;
    }

}
