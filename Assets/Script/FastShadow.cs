using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FastShadow : MonoBehaviour
{
#if UNITY_EDITOR
    
    public void CaptureAndSetAsSprite()
    {
        
        string filename = string.Format("Assets/Screenshots/capture_{0}.png",
            DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));
        if (!Directory.Exists("Assets/Screenshots"))
        {
            Directory.CreateDirectory("Assets/Screenshots");
        }

        int width = Camera.main.pixelWidth;
        int height = Camera.main.pixelHeight;
        Texture2D screenshotTexture = TakeScreenshot(Camera.main, width, height);


        // Encode the resulting output texture to a byte array
        byte[] pngShot = screenshotTexture.EncodeToPNG();

        // Write to the file
        File.WriteAllBytes(filename, pngShot);

        // Import the asset
        AssetDatabase.ImportAsset(filename);
        TextureImporter importer = AssetImporter.GetAtPath(filename) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        AssetDatabase.WriteImportSettingsIfDirty(filename);
        AssetDatabase.Refresh();

        // Load the sprite
        Sprite capturedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(filename);

        // Set the captured sprite to the SpriteRenderer
    }

    public static Texture2D TakeScreenshot(Camera cam, int width, int height)
    {
        
        var bak_cam_targetTexture = cam.targetTexture;
        var bak_cam_clearFlags = cam.clearFlags;
        var bak_RenderTexture_active = RenderTexture.active;

        var render_texture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
        cam.targetTexture = render_texture;
        cam.clearFlags = CameraClearFlags.SolidColor;

        RenderTexture.active = render_texture;
        cam.Render();

        var tex_screenshot = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex_screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex_screenshot.Apply();

        cam.targetTexture = bak_cam_targetTexture;
        cam.clearFlags = bak_cam_clearFlags;
        RenderTexture.active = bak_RenderTexture_active;
        RenderTexture.ReleaseTemporary(render_texture);

        return tex_screenshot;
    }

    public void MakeNonTransparentPixelsBlack(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            // Eğer piksel şeffaf değilse, siyah yap
            if (pixels[i].a > 0)
            {
                //pixels[i] = Color.black;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }
#endif
}
