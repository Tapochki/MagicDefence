using GrandDevs.AppName.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GrandDevs.AppName
{
    public class DataManager : IService, IDataManager
    {
        private IAppStateManager _appStateManager;
        private ILocalizationManager _localizationManager;

        private Dictionary<Enumerators.CacheDataType, string> _cacheDataPathes;

        public UserLocalData CachedUserLocalData { get; set; }

        public DataManager()
        {
            CachedUserLocalData = new UserLocalData();
        }
        public void Dispose()
        {
            SaveAllCache();
        }
        public void Init()
        {
            _appStateManager = GameClient.Get<IAppStateManager>();
            _localizationManager = GameClient.Get<ILocalizationManager>();

            FillCacheDataPathes();
        }
        public void StartLoadCache()
        {

            int count = Enum.GetNames(typeof(Enumerators.CacheDataType)).Length;
            for (int i = 0; i < count; i++)
                LoadCachedData((Enumerators.CacheDataType)i);

            _localizationManager.ApplyLocalization();

            _appStateManager.ChangeAppState(Enumerators.AppState.MAIN_MENU);
        }
        public void Update()
        {

        }
        public void SaveAllCache()
        {
            int count = Enum.GetNames(typeof(Enumerators.CacheDataType)).Length;
            for (int i = 0; i < count; i++)
                SaveCache((Enumerators.CacheDataType)i);
        }
        public Sprite GetSpriteFromTexture(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2f);
        }
        public void SaveCache(Enumerators.CacheDataType type)
        {
            switch (type)
            {
                case Enumerators.CacheDataType.USER_LOCAL_DATA:
                    {
                        if (!File.Exists(_cacheDataPathes[type]))
                            File.Create(_cacheDataPathes[type]).Close();

                        File.WriteAllText(_cacheDataPathes[type], JsonConvert.SerializeObject(CachedUserLocalData));
                    }
                    break;
                default: break;
            }
        }
        private void LoadCachedData(Enumerators.CacheDataType type)
        {
            switch (type)
            {
                case Enumerators.CacheDataType.USER_LOCAL_DATA:
                    {
                        if (File.Exists(_cacheDataPathes[type]))
                            CachedUserLocalData = DeserializeObjectFromPath<UserLocalData>(_cacheDataPathes[type]);
                    }
                    break;
                default: break;
            }
        }
        private void FillCacheDataPathes()
        {
            _cacheDataPathes = new Dictionary<Enumerators.CacheDataType, string>();
            _cacheDataPathes.Add(Enumerators.CacheDataType.USER_LOCAL_DATA, Path.Combine(Application.persistentDataPath, Constants.LOCAL_USER_DATA_FILE_PATH));
        }

        public T DeserializeObjectFromPath<T>(string path)
        {
           return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}