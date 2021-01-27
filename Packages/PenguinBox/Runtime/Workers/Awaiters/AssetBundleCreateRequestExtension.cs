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

using UnityEngine;

namespace Sinoalmond.PenguinBox.Workers.Awaiters
{
    /// <summary>
    /// AssetBundleCreateRequest クラスの拡張関数実装クラスです
    /// </summary>
    public static class AssetBundleCreateRequestExtension
    {
        /// <summary>
        /// AssetBundleCreateRequest の待機オブジェクトを取得します
        /// </summary>
        /// <param name="request">待機する AssetBundleCreateRequest</param>
        /// <returns>AssetBundleCreateRequest の待機オブジェクトを返します</returns>
        public static AssetBundleCreateRequestAwaiter GetAwaiter(this AssetBundleCreateRequest request)
        {
            return new AssetBundleCreateRequestAwaiter(request);
        }
    }
}