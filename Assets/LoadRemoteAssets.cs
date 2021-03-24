using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;


public class LoadRemoteAssets : MonoBehaviour
{
    [SerializeField] string label;
    [SerializeField] Transform parent;


    public void LoadAssets()
    {
        Get(label);
    }

    private async Task Get(string label)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        foreach (var location in locations)
        {
            await Addressables.InstantiateAsync(location,parent).Task;
        }
        
    }
}
