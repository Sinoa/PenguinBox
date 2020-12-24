// Zlib license
//
// Copyright (c) 2020 Sinoa
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

using System.Collections.Generic;
using UnityEngine;

namespace Sinoalmond.PenguinBox.Assets
{
    /// <summary>
    /// AssetBundle のオープンコストを回避するため可能な限りオープンを維持するためのアセットバンドルキャッシュクラスです
    /// </summary>
    internal class AssetBundleCache
    {
        private const int DefaultCapacity = 512;

        private readonly Dictionary<string, AssetBundle> assetBundleTable;



        /// <summary>
        /// AssetBundleCache クラスのインスタンスを初期化します
        /// </summary>
        public AssetBundleCache()
        {
            assetBundleTable = new Dictionary<string, AssetBundle>(DefaultCapacity);
        }


        /// <summary>
        /// すべてのアセットバンドルのアンロードとクローズを行います。
        /// 既にロードされたアセットの参照までは解放されません。
        /// </summary>
        public void Clear()
        {
            assetBundleTable.Clear();
            AssetBundle.UnloadAllAssetBundles(false);
        }
    }
}