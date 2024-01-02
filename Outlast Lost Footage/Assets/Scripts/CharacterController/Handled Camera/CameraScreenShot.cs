using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraScreenShot : MonoBehaviour
{
    public string fileName;
    public RenderTexture RT;

    void GetImage()
    {
        Texture2D texture2D = new Texture2D(RT.width, RT.height, textureFormat:TextureFormat.ARGB32,false);
        RenderTexture.active = RT;
        texture2D.ReadPixels(new Rect(0,0, RT.width, RT.height),0,0);
        texture2D.Apply();

        string MyDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        var folder = Directory.CreateDirectory(MyDocuments + "/OutlastLostFootage");
        if (folder == null)
            File.Create(MyDocuments + "/OutlastLostFootage");

        string PictureIndex = GetImageIndex().ToString();
        string path = folder + "/" + fileName + PictureIndex + ".png";
        byte[] bytes = texture2D.EncodeToPNG();

        File.WriteAllBytes(path, bytes);
        Debug.Log("get");

    }

    void SetImage()
    {
        Texture2D texture2D = new Texture2D(RT.width, RT.height); string 
        MyDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        var folder = Directory.CreateDirectory(MyDocuments + "/OutlastLostFootage");
        if (folder == null)
            File.Create(MyDocuments + "/OutlastLostFootage");

        string PictureIndex = (GetImageIndex()-1).ToString(); 
        string path = folder + "/" + fileName + PictureIndex + ".png";
        byte[] bytes = File.ReadAllBytes(path);

        texture2D.LoadImage(bytes);
        texture2D.Apply();
        Debug.Log("set");

    }

    int GetImageIndex()
    {
        string MyDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string folderPath = MyDocuments + "/OutlastLostFootage";

        if (!Directory.Exists(folderPath))
            return 0;

        string[] files = Directory.GetFiles(folderPath, "*.png");
        int count = files.Length;

        return count;
    }

    IEnumerator RenderProcess()
    {
        GetImage();
        yield return new WaitForSeconds(0.1f);
        SetImage();
        Debug.Log("Image Saved");
    }

    public void GetSetImage_BTM()
    {
        StartCoroutine(RenderProcess());
    }
}
