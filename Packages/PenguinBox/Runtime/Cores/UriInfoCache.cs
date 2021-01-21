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

using System;
using System.Collections.Generic;

namespace Sinoalmond.PenguinBox.Cores
{
    /// <summary>
    /// UriInfo インスタンスの生成コストを回避するために、極力キャッシュから取り出せるようにするためのキャッシュクラスです。
    /// </summary>
    internal class UriInfoCache
    {
        private const int DefaultCapacity = 2 << 10;

        private readonly Dictionary<string, UriInfo> uriInfoTable;



        /// <summary>
        /// UriInfoCache クラスのインスタンスを初期化します
        /// </summary>
        public UriInfoCache()
        {
            uriInfoTable = new Dictionary<string, UriInfo>(DefaultCapacity);
        }


        /// <summary>
        /// 内部キャッシュテーブルをクリアします
        /// </summary>
        public void ClearCache()
        {
            uriInfoTable.Clear();
        }


        /// <summary>
        /// 指定されたURI文字列から UriInfo のインスタンスを取得します。
        /// もし存在しない場合は、新しい UriInfo を生成しそのキャッシュを取得します。
        /// </summary>
        /// <param name="uriText">取得したい UriInfo のURI文字列</param>
        /// <returns>取得または生成した UriInfo を返します</returns>
        /// <exception cref="ArgumentNullException">uriText が null です</exception>
        /// <exception cref="ArgumentException">uriText が 有効なURI書式ではありません</exception>
        public UriInfo GetOrCreateUriInfo(string uriText)
        {
            if (uriInfoTable.TryGetValue(uriText ?? throw new ArgumentNullException(nameof(uriText)), out var uriInfo))
            {
                return uriInfo;
            }


            if (!Uri.TryCreate(uriText, UriKind.Absolute, out var uri))
            {
                var message = $"指定されたURIは有効な書式ではありません urlText='{uriText}'";
                throw new ArgumentException(message, nameof(uriText));
            }


            uriInfo = new UriInfo(uri);
            uriInfoTable[uriText] = uriInfo;
            return uriInfo;
        }


        /// <summary>
        /// 指定されたURIから UriInfo のインスタンスを取得します。
        /// もし存在しない場合は、新しい UriInfo を生成しそのキャッシュを取得します。
        /// </summary>
        /// <param name="uri">取得したい UriInfo のURI</param>
        /// <returns>取得または生成した UriInfo を返します</returns>
        /// <exception cref="ArgumentNullException">uri が null です</exception>
        public UriInfo GetOrCreateUriInfo(Uri uri)
        {
            var uriText = (uri ?? throw new ArgumentNullException(nameof(uri))).OriginalString;
            if (uriInfoTable.TryGetValue(uriText, out var uriInfo))
            {
                return uriInfo;
            }


            uriInfo = new UriInfo(uri);
            uriInfoTable[uriText] = uriInfo;
            return uriInfo;
        }
    }
}