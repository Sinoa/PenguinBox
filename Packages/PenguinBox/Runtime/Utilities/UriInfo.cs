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

namespace Sinoalmond.PenguinBox.Utilities
{
    /// <summary>
    /// Uri と クエリ文字列テーブル を保持した構造体です
    /// </summary>
    public readonly struct UriInfo : IEquatable<UriInfo>
    {
        private static readonly char[] StringSplitPattern = new char[] { '&' };

        private readonly Uri uri;
        private readonly Dictionary<string, string> queryTable;



        /// <summary>
        /// 保持している Uri への実体
        /// </summary>
        public Uri Uri => uri;



        /// <summary>
        /// UriInfo 構造体のインスタンスを初期化します
        /// </summary>
        /// <param name="uri"></param>
        public UriInfo(Uri uri)
        {
            this.uri = uri;


            var queryString = Uri.UnescapeDataString(uri.Query.TrimStart('?'));
            var queries = queryString.Split(StringSplitPattern, StringSplitOptions.RemoveEmptyEntries);
            queryTable = new Dictionary<string, string>();
            foreach (var query in queries)
            {
                var keyValue = query.Split('=');
                queryTable[keyValue[0]] = keyValue[1];
            }
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