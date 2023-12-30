using System.Threading.Tasks;
using Assets.Scripts.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        Task<GameObject> Instantiate(string path);
        Task<GameObject> Instantiate(string path, Vector3 at);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void CleanUp();
        void Initialize();
    }
}