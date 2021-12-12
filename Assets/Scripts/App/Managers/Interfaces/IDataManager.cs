using UnityEngine;

namespace GrandDevs.AppName
{
    public interface IDataManager
    {
        UserLocalData CachedUserLocalData { get; set; }
        void StartLoadCache();
        void SaveAllCache();
        Sprite GetSpriteFromTexture(Texture2D texture);
    }
}