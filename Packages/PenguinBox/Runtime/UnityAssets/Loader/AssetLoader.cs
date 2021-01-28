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
using Sinoalmond.PenguinBox.Cores;
using UnityObject = UnityEngine.Object;

namespace Sinoalmond.PenguinBox.Assets
{
    /// <summary>
    /// IAssetLoader を実装するアセットロード抽象クラスです
    /// </summary>
    public abstract class AssetLoader : IAssetLoader
    {
        public Task<T> LoadAssetAsync<T>(UriInfo assetUri, IAssetLoadEventListener listener, CancellationToken token) where T : UnityObject
        {
            ThrowIfArgumentNull(assetUri, nameof(assetUri));
            ThrowIfArgumentNull(listener, nameof(listener));


            if (!CanUriSupport(assetUri))
            {
                var message = $"指定されたURI '{assetUri.Uri.OriginalString}' は対応していません。";
                var error = new AssetLoadException(message, AssetLoadErrorReason.NotSupportedUri);
                return Task.FromException<T>(error);
            }


            if (token.IsCancellationRequested)
            {
                var message = "ロード操作はキャンセルされました。";
                var error = new AssetLoadException(message, AssetLoadErrorReason.Cancel);
                return Task.FromException<T>(error);
            }


            try
            {
                return LoadAssetCoreAsync<T>(assetUri, listener, token);
            }
            catch (Exception innerError)
            {
                // general exception are handling by fetch context exception handler.
                var message = "ロード中に不明なエラーが発生しました。";
                var error = new AssetLoadException(message, AssetLoadErrorReason.Unknown, innerError);
                return Task.FromException<T>(error);
            }
        }


        protected abstract bool CanUriSupport(UriInfo assetUri);


        protected abstract Task<T> LoadAssetCoreAsync<T>(UriInfo assetUri, IAssetLoadEventListener listener, CancellationToken token) where T : UnityObject;


        #region Exception thrower and builder
        private void ThrowIfArgumentNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
        #endregion
    }
}