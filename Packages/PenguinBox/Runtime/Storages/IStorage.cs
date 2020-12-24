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
using System.IO;

namespace Sinoalmond.PenguinBox.Storages
{
    /// <summary>
    /// ある特定アセットの貯蔵機能を提供するインターフェイスです
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// このストレージの名前
        /// </summary>
        string Name { get; }



        /// <summary>
        /// 指定されたアセットURIのアセットがストレージに存在するか否か確認をします
        /// </summary>
        /// <param name="assetUri">確認するアセットURI</param>
        /// <returns>アセットが存在する場合は true を、存在しない場合は false を返します</returns>
        bool Exists(Uri assetUri);


        /// <summary>
        /// 指定されたアセットURIのアセットをストリームとして開きます。
        /// </summary>
        /// <param name="assetUri">ストリームとして開くアセットURI</param>
        /// <param name="mode">指定されたアセットURIに対するモード</param>
        /// <param name="access">指定されたアセットURIに対するアクセス方法</param>
        /// <returns>正しくストリームを開けた場合はストリームの参照を返します。開けなかった場合は null を返します</returns>
        Stream Open(Uri assetUri, FileMode mode, FileAccess access);


        /// <summary>
        /// 指定されたアセットURIのアセットを削除をします
        /// </summary>
        /// <param name="assetUri">削除するアセットURI</param>
        void Delete(Uri assetUri);


        /// <summary>
        /// ストレージに含まれるすべてのアセットを削除します
        /// </summary>
        void DeleteAll();


        /// <summary>
        /// アセットURIからストレージが扱うネイティブパスを取得します。
        /// ネイティブパスとはストレージがプラットフォームに直接使用するパスを指します。
        /// </summary>
        /// <param name="assetUri">ネイティブパスとして</param>
        /// <returns>ネイティブパスの表現ができる場合はネイティブパスを返しますが、表現出来ない場合は null を返します</returns>
        string GetNativePath(Uri assetUri);
    }
}