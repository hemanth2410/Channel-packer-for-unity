using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

public enum ChannelSource
{
    Red, Green, Blue, Alpha, SolidBlack, SolidWhite, InvertRed, InvertGreen, InvertBlue, InvertAlpha
}
public class TextureConverterEditor : EditorWindow
{
    Texture2D redTex;
    Texture2D greenTex;
    Texture2D blueTex;
    Texture2D alphaTex;
    string m_MapName;
    string m_OutputDirectory;
    bool invertAlphaChannel = true;
    string error;
    Vector2 m_TextureSize;
    string saveDirectory;
    string fileName;

    ChannelSource redChannelSource = ChannelSource.Red;
    ChannelSource greenChannelSource = ChannelSource.Green;
    ChannelSource blueChannelSource = ChannelSource.Blue;
    ChannelSource alphaChannelSource = ChannelSource.Alpha;

    Material previewMaterial;
    RenderTexture outputTexture;

    [MenuItem("Tools/ChannelPacker")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(TextureConverterEditor),false, "Channel Packer");
    }
    private void OnEnable()
    {
        Shader shader = Shader.Find("Custom/TextureCombine");
        if (shader != null)
        {
            previewMaterial = new Material(shader);
        }
    }
    void OnGUI()
    {

        EditorGUILayout.LabelField("Input Textures", EditorStyles.boldLabel);

        redTex = (Texture2D)EditorGUILayout.ObjectField("Red Texture", redTex, typeof(Texture2D), false);
        redChannelSource = (ChannelSource)EditorGUILayout.EnumPopup("Red Channel", redChannelSource);

        greenTex = (Texture2D)EditorGUILayout.ObjectField("Green Texture", greenTex, typeof(Texture2D), false);
        greenChannelSource = (ChannelSource)EditorGUILayout.EnumPopup("Green Channel", greenChannelSource);

        blueTex = (Texture2D)EditorGUILayout.ObjectField("Blue Texture", blueTex, typeof(Texture2D), false);
        blueChannelSource = (ChannelSource)EditorGUILayout.EnumPopup("Blue Channel", blueChannelSource);

        alphaTex = (Texture2D)EditorGUILayout.ObjectField("Alpha Texture", alphaTex, typeof(Texture2D), false);
        alphaChannelSource = (ChannelSource)EditorGUILayout.EnumPopup("Alpha Channel", alphaChannelSource);

        if (GUILayout.Button("Generate Preview"))
        {
            RenderOutputTexture();
        }

        if (outputTexture != null)
        {
            GUILayout.Label(outputTexture, GUILayout.Width(256), GUILayout.Height(256));

            if (GUILayout.Button("Save As PNG"))
            {
                SaveTexture();
            }
        }
    }

    void RenderOutputTexture()
    {
        if (previewMaterial == null) return;

        previewMaterial.SetTexture("_RedTexture", redTex);
        previewMaterial.SetTexture("_GreenTexture", greenTex);
        previewMaterial.SetTexture("_BlueTexture", blueTex);
        previewMaterial.SetTexture("_AlphaTexture", alphaTex);

        previewMaterial.SetInt("_RedChannel", (int)redChannelSource);
        previewMaterial.SetInt("_GreenChannel", (int)greenChannelSource);
        previewMaterial.SetInt("_BlueChannel", (int)blueChannelSource);
        previewMaterial.SetInt("_AlphaChannel", (int)alphaChannelSource);

        if (outputTexture == null || outputTexture.width != 2048)
        {
            outputTexture = new RenderTexture(2048, 2048, 0);
        }

        Graphics.Blit(null, outputTexture, previewMaterial);
    }

    void SaveTexture()
    {
        RenderTexture.active = outputTexture;
        Texture2D tex = new Texture2D(outputTexture.width, outputTexture.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, outputTexture.width, outputTexture.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        string path = EditorUtility.SaveFilePanel("Save Texture", Application.dataPath, "MaskMap", "png");

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();
        }
    }
}
