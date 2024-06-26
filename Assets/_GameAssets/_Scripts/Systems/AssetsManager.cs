﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _GameAssets._Scripts.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;


//This class is needed for loading different settings in the future
public class AssetsManager : IDisposable
{
    private const string SETTINGS_KEY = "AssetsSettings";
    private Dictionary<Type, ModuleSettings> _modulesSettings = new();
    private static AsyncOperationHandle<IResourceLocator> _initHandle;
    private AssetsSettings _settings;

    public T GetModuleSettings<T>() where T : ModuleSettings
    {
        var key = typeof(T);
        if (!_modulesSettings.ContainsKey(key))
        {
            Debug.LogError($"Module settings not found : {key}");
            return null;
        }

        return _modulesSettings[typeof(T)] as T;
    }

    public async Task InitializeAsync()
    {
        _initHandle = Addressables.InitializeAsync();
        await _initHandle.Task;
    }

    public async Task LoadModulesSettings()
    {
        AsyncOperationHandle<AssetsSettings> handle = Addressables.LoadAssetAsync<AssetsSettings>(SETTINGS_KEY);
        await handle.Task;
        _settings = handle.Result;

        foreach (var setting in _settings._settingsList)
        {
            var settingHandle = setting.LoadAssetAsync<ModuleSettings>();
            settingHandle.Completed += op => { _modulesSettings.Add(op.Result.GetType(), op.Result); };
            await settingHandle.Task;
        }
    }

    public void Dispose()
    {
        if (_initHandle.IsValid())
        {
            Addressables.Release(_initHandle);
        }

        foreach (var item in _modulesSettings)
        {
            Addressables.Release(item.Value);
        }

        _modulesSettings.Clear();
        Addressables.Release(_settings);
    }
}