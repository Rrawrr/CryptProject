using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : MonoBehaviour
{
    [SerializeField] AssetReference loadablePrefab;
    [SerializeField] Transform parent;

    
    public void LoadPrefab()
    {
        //LoadAsset();
        StartCoroutine(LoadPrefabCoroutine());
    }

    //private async void LoadAsset()
    //{
    //    AsyncOperationHandle<GameObject> handle = loadablePrefab.LoadAssetAsync<GameObject>();
    //    await handle.Task;
    //    Debug.Log("Percent is: " + handle.GetDownloadStatus().Percent);
    //    if(handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        Debug.Log("Loading Succeeded");
    //        GameObject imagePrefab = handle.Result;
    //        Instantiate(imagePrefab, parent);
    //        Addressables.Release(handle);
    //    }
    //}

    IEnumerator LoadPrefabCoroutine()
    {
        AsyncOperationHandle<GameObject> handle = loadablePrefab.LoadAssetAsync<GameObject>();
        yield return handle;

        while(handle.GetDownloadStatus().Percent < 1)
        {
            Debug.Log("Percent is: " + handle.GetDownloadStatus().Percent * 100f);
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Loading Succeeded");
            GameObject imagePrefab = handle.Result;
            Instantiate(imagePrefab, parent);
            Addressables.Release(handle);
        }
    }


}
