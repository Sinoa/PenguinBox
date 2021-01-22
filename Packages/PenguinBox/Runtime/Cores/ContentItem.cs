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

namespace Sinoalmond.PenguinBox.Cores
{
    /// <summary>
    /// 単位コンテンツデータの情報を保持した構造体です
    /// </summary>
    public readonly struct ContentItem
    {
        /// <summary>
        /// コンテンツ名
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// コンテンツの配信サイズ
        /// </summary>
        public readonly long Length;

        /// <summary>
        /// コンテンツの配信タイムスタンプ（ミリ秒）
        /// </summary>
        /// <remarks>タイムスタンプ形式はミリ秒であればユーザーアプリケーションで定義しても構いません</remarks>
        public readonly long Timestamp;

        /// <summary>
        /// コンテンツが存在するリモートURI
        /// </summary>
        public readonly string RemoteUri;

        /// <summary>
        /// ローカルに配置される先のローカルURI
        /// </summary>
        public readonly string LocalUri;

        /// <summary>
        /// コンテンツのダイジェスト
        /// </summary>
        /// <remarks>使用されるハッシュメッセージダイジェストはSHA512になります</remarks>
        public readonly byte[] Digest;

        /// <summary>
        /// このコンテンツが依存するコンテンツの名前
        /// </summary>
        public readonly string[] DependentContentNames;



        /// <summary>
        /// ContentItem 構造体のインスタンスを初期化します
        /// </summary>
        /// <param name="name">コンテンツ名</param>
        /// <param name="length">コンテンツの配信サイズ</param>
        /// <param name="timestamp">コンテンツ配信タイムスタンプ</param>
        /// <param name="remoteUri">コンテンツリモートURI</param>
        /// <param name="localUri">コンテンツ配置ローカルURI</param>
        /// <param name="digest">コンテンツメッセージダイジェスト</param>
        /// <param name="dependentContentName">依存するコンテンツ名</param>
        public ContentItem(string name, long length, long timestamp, string remoteUri, string localUri, byte[] digest, string[] dependentContentName)
        {
            Name = name;
            Length = length;
            Timestamp = timestamp;
            RemoteUri = remoteUri;
            LocalUri = localUri;
            Digest = digest;
            DependentContentNames = dependentContentName;
        }
    }
}