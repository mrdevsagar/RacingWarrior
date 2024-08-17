using UnityEngine;

public class ToastMessage : MonoBehaviour
{
    public static void ShowToast(string message)
    {
        // Check if we are running on Android
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.LogWarning("Toast messages are only supported on Android.");
            return;
        }

        // Call the Android Toast method
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (activity != null)
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    using (AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast"))
                    {
                        using (AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext"))
                        {
                            using (AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", message))
                            {
                                using (AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", context, javaString, toastClass.GetStatic<int>("LENGTH_SHORT")))
                                {
                                    toastObject.Call("show");
                                }
                            }
                        }
                    }
                }));
            }
        }
    }
}
