// Zlib license
//
// Copyright (c) 2021 Sinoa
//
// This software is provided 'as-is', without any express or implied warranty.
// In no event will the authors be held liable for any damages arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it freely,
// subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software.
//    If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.

using System;
using System.Threading;
using System.Threading.Tasks;
using Sinoalmond.PenguinBox.Utilities;
using Sinoalmond.PenguinBox.Storages;
using UnityObject = UnityEngine.Object;

namespace Sinoalmond.PenguinBox
{
    /// <summary>
    /// PenguinBox 全体を動かすためのエンジンクラスです
    /// </summary>
    public class PenguinBoxEngine
    {
        private readonly UriInfoCache uriInfoCache;
        private readonly StorageManager storageManager;



        public PenguinBoxEngine()
        {
            uriInfoCache = new UriInfoCache();
            storageManager = new StorageManager();
        }


        public void Update()
        {
            throw new NotImplementedException();
        }


        public Task UpdateCatalogAsync()
        {
            return UpdateCatalogAsync(CancellationToken.None);
        }


        public Task UpdateCatalogAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }


        public Task VerifyAsync()
        {
            return VerifyAsync(CancellationToken.None);
        }


        public Task VerifyAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }


        public Task<UnityObject> LoadAssetAsync(string assetUri)
        {
            return LoadAssetAsync(assetUri, false, CancellationToken.None);
        }


        public Task<UnityObject> LoadAssetAsync(string assetUri, bool tryDownload)
        {
            return LoadAssetAsync(assetUri, tryDownload, CancellationToken.None);
        }


        public Task<UnityObject> LoadAssetAsync(string assetUri, bool tryDownload, CancellationToken token)
        {
            throw new NotImplementedException();
        }


        public void UnloadAsset(UnityObject asset)
        {
            throw new NotImplementedException();
        }


        public void UnloadAssetAll()
        {
            throw new NotImplementedException();
        }
    }
}