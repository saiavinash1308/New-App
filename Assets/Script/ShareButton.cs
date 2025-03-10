using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShareButton : MonoBehaviour
{
   public void ClickShareBtn()
    {
        //StartCoroutine(TakeScreenshotAndShare());
        StartCoroutine(APKShare());
    }
    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Klik Games").SetText("Download from here!").SetUrl("https://www.youtube.com/watch?v=vTIBel9X3mQ")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }
    private IEnumerator APKShare()
    {
        yield return new WaitForEndOfFrame();

        // Load the texture from Resources
        Texture2D image = Resources.Load("image", typeof(Texture2D)) as Texture2D;

        // Check if the texture is loaded correctly
        if (image == null)
        {
            Debug.LogError("Failed to load texture from Resources. Make sure 'image' exists in the Resources folder.");
            yield break; // Exit if texture is invalid
        }

        // Create a new Texture2D with an uncompressed format
        Texture2D readableTexture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);

        // Copy the pixel data from the original texture
        readableTexture.SetPixels(image.GetPixels());
        readableTexture.Apply(); // Apply the changes

        // Now you can safely encode the texture to PNG
        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, readableTexture.EncodeToPNG());

        // Cleanup
        Destroy(readableTexture);

        // Share the image file
        new NativeShare().AddFile(filePath)
            .SetSubject("Klik Games")
            .SetText("Download from here!")
            .SetUrl("https://appfolio.vercel.app/")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }


}
