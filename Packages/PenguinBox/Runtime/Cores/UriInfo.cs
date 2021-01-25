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
    /// Uri と クエリ文字列テーブル を保持したクラスです
    /// </summary>
    public class UriInfo : IEquatable<UriInfo>
    {
        private static readonly char[] StringSplitPattern = new char[] { '&' };

        private readonly Uri uri;
        private readonly Dictionary<string, string> queryTable;



        /// <summary>
        /// 保持している Uri への実体
        /// </summary>
        public Uri Uri => uri;



        /// <summary>
        /// UriInfo クラスのインスタンスを初期化します
        /// </summary>
        /// <param name="uriText">URIを表す文字列</param>
        /// <exception cref="ArgumentNullException">uriText が null です。</exception>
        /// <exception cref="ArgumentException">uriText が使用可能なURI形式ではありません。</exception>
        public UriInfo(string uriText)
        {
            queryTable = new Dictionary<string, string>();
            if (!Uri.TryCreate(uriText ?? throw new ArgumentNullException(nameof(uriText)), UriKind.Absolute, out uri))
            {
                var message = $"{nameof(uriText)} が使用可能なURI形式ではありません。";
                throw new ArgumentException(message, nameof(uriText));
            }


            InitializeCommon();
        }


        /// <summary>
        /// UriInfo クラスのインスタンスを初期化します
        /// </summary>
        /// <param name="originalUri">URIを表すURIインスタンス</param>
        /// <exception cref="ArgumentException">指定された URI は絶対URI形式ではありません。</exception>
        public UriInfo(Uri originalUri)
        {
            queryTable = new Dictionary<string, string>();
            if (!originalUri.IsAbsoluteUri)
            {
                var message = "指定された URI は絶対URI形式ではありません。";
                throw new ArgumentException(message, nameof(originalUri));
            }


            uri = originalUri;
            InitializeCommon();
        }


        private void InitializeCommon()
        {
            var queryString = Uri.UnescapeDataString(uri.Query.TrimStart('?'));
            var queries = queryString.Split(StringSplitPattern, StringSplitOptions.RemoveEmptyEntries);
            foreach (var query in queries)
            {
                var keyValue = query.Split('=');
                queryTable[keyValue[0]] = keyValue[1];
            }
        }


        /// <summary>
        /// 指定されたキー名がクエリに含まれるか否かを確認します
        /// </summary>
        /// <param name="keyName">確認するキー名</param>
        /// <returns>キーが含まれる場合は true を、含まれない場合は false を返します</returns>
        public bool ContainsKey(string keyName)
        {
            return queryTable.ContainsKey(keyName);
        }


        /// <summary>
        /// クエリからキー名に対応する値を取得します
        /// </summary>
        /// <param name="keyName">取得する値のキー</param>
        /// <returns>キーを見つけた時はその値を返しますが、見つからなかった時は null を返します</returns>
        public string GetQuery(string keyName)
        {
            queryTable.TryGetValue(keyName, out var value);
            return value;
        }


        /// <summary>
        /// クエリからキー名に対応する値の取得を試みます
        /// </summary>
        /// <param name="keyName">取得する値のキー</param>
        /// <param name="value">キーを見つけた場合は対応する値を設定します</param>
        /// <returns>キーを見つけた場合は true を、見つけられなかった場合は false を返します</returns>
        public bool TryGetQuery(string keyName, out string value)
        {
            return queryTable.TryGetValue(keyName, out value);
        }


        #region GetHashCode and Equal functions
        public override int GetHashCode()
        {
            return uri.OriginalString.GetHashCode();
        }

        public bool Equals(UriInfo other)
        {
            return uri.OriginalString == other.uri.OriginalString;
        }

        public override bool Equals(object obj)
        {
            return obj is UriInfo info && Equals(info);
        }

        public static bool operator ==(UriInfo left, UriInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UriInfo left, UriInfo right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}