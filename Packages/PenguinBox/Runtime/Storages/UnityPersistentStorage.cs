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

using System.IO;
using UnityEngine;

namespace Sinoalmond.PenguinBox.Storages
{
    /// <summary>
    /// Unityゲームエンジンが提供する Application.persistentDataPath を基本にファイルの入出力を提供します
    /// </summary>
    public class UnityPersistentStorage : FileSystemStorage
    {
        /// <summary>
        /// このストレージがアクセスするベースディレクトリパス
        /// </summary>
        public string BaseDirectoryPath { get; protected set; }



        /// <summary>
        /// UnityPersistentStorage クラスのインスタンスを初期化します
        /// </summary>
        /// <param name="storageName">生成するインスタンスに設定するストレージの名前</param>
        /// <param name="baseDirectoryPath">このストレージがアクセスするベースディレクトリのパス null または 空文字列 を指定するとルート直下を指します。また前後のスラッシュは削除されます。</param>
        public UnityPersistentStorage(string storageName, string baseDirectoryPath) : base(storageName)
        {
            BaseDirectoryPath = !string.IsNullOrWhiteSpace(baseDirectoryPath) ? baseDirectoryPath.Trim('/') : string.Empty;
        }


        protected override string GetBaseDirectoryPath()
        {
            return Path.Combine(Application.persistentDataPath, BaseDirectoryPath);
        }
    }
}