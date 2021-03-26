using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class RemoteAssetsLoader : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] List<string> keys;

    private void Awake()
    {
        keys = new List<string>();
        keys.Add("ImageBear");
        keys.Add("ImageLight");
        keys.Add("ImageDark");
    }

    public void LoadAssets()
    {
        StartCoroutine(LoadAssetsCoroutine());
    }

    private IEnumerator LoadAssetsCoroutine()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Check the internet connection");
            yield break; 
        }

        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(keys);
        yield return getDownloadSize;
        Debug.Log("Download size is: " + getDownloadSize.Result.ToString());

        if (getDownloadSize.Result > 0)
        {
            AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(keys[0]);
            while (!downloadDependencies.IsDone)
            {
                Debug.Log("Downloadnig assets... " + downloadDependencies.PercentComplete);
                yield return null;
                if (getDownloadSize.Status == AsyncOperationStatus.Failed || Application.internetReachability == NetworkReachability.NotReachable)
                {
                    Debug.LogError("Failed to download remote assets");
                    yield break; 
                }
            }
            downloadDependencies.Completed += ShowCompleteMessage;
            downloadDependencies.Completed += InstantiatePrefabs;
        }
        else
        {
            InstantiatePrefabs();
        }
    }

    private void ShowCompleteMessage(AsyncOperationHandle handle)
    {
        Debug.Log("Complete downloading assets");
    }

    private void InstantiatePrefabs(AsyncOperationHandle handle)
    {
        foreach (string key in keys)
        {
            Addressables.InstantiateAsync(key, parent);
            Debug.Log("Instantiated " + key);
        }
    }

    private void InstantiatePrefabs()
    {
        foreach (string key in keys)
        {
            Addressables.InstantiateAsync(key, parent);
            Debug.Log("Instantiated " + key);
        }
    }


}
