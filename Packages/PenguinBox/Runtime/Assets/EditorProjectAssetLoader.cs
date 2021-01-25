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

using System.Threading;
using System.Threading.Tasks;
using Sinoalmond.PenguinBox.Cores;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Sinoalmond.PenguinBox.Assets
{
    public class EditorProjectAssetLoader : AssetLoader
    {
        protected override bool CanUriSupport(UriInfo assetUri)
        {
            if (assetUri.Uri.Host == "assetbundle")
            {
                return assetUri.ContainsKey("name");
            }

            return true;
        }


        protected override Task<T> LoadAssetCoreAsync<T>(UriInfo assetUri, IAssetLoadEventListener listener, CancellationToken token)
        {
#if !UNITY_EDITOR
            var message = "このロード関数はエディタ以外では動作しません";
            var error = new AssetLoadException(message, AssetLoadErrorReason.LoadEditorOnly);
            return Task.FromException<T>(error);
#else
            UnityObject asset;
            var assetPath = GetAssetPath(assetUri);
            if (typeof(T) == typeof(MultiSprite))
            {
                var sprites = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
                if (sprites == null)
                {
                    var message = $"指定されたアセット '{assetPath}' が見つかりませんでした。";
                    var error = new AssetLoadException(message, AssetLoadErrorReason.AssetNotFound);
                    return Task.FromException<T>(error);
                }


                var multiSprite = ScriptableObject.CreateInstance<MultiSprite>();
                multiSprite.SetSprites(sprites);
                asset = multiSprite;
            }
            else
            {
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset == null)
                {
                    var message = $"指定されたアセット '{assetPath}' が見つかりませんでした。";
                    var error = new AssetLoadException(message, AssetLoadErrorReason.AssetNotFound);
                    return Task.FromException<T>(error);
                }
            }


            return Task.FromResult<T>((T)asset);
#endif
        }


        private string GetAssetPath(UriInfo assetUri)
        {
            if (assetUri.Uri.Host == "assetbundle")
            {
                return assetUri.GetQuery("name");
            }


            return assetUri.Uri.AbsolutePath.TrimStart('/');
        }
    }
}