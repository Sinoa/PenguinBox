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

namespace PenguinBox.Storage
{
    /// <summary>
    /// 殆どのプラットフォームで動作するファイルシステムを使用したストレージクラスです
    /// </summary>
    public class FileSystemStorage : IStorage
    {
        /// <summary>
        /// ストレージ名
        /// </summary>
        public string Name { get; protected set; }



        /// <summary>
        /// FileSystemStorage クラスのインスタンスを初期化します
        /// </summary>
        /// <param name="storageName">このファイルシステムストレージを表すストレージ名</param>
        /// <exception cref="ArgumentNullException">storageName が null です</exception>
        public FileSystemStorage(string storageName)
        {
            Name = storageName ?? throw new ArgumentNullException(nameof(storageName));
        }


        /// <summary>
        /// 指定されたコンテンツURIのファイルまたはディレクトリが存在するか確認をします
        /// </summary>
        /// <param name="contentUri">確認をするコンテンツのURI</param>
        /// <returns>指定されたコンテンツURIにファイルまたはディレクトリがあれば true を、どちらも存在しない場合は false を返します</returns>
        /// <exception cref="ArgumentNullException">contentUri が null です</exception>
        public bool Exists(Uri contentUri)
        {
            var nativePath = GetNativePath(contentUri);
            return File.Exists(nativePath) || Directory.Exists(nativePath);
        }


        /// <summary>
        /// 指定されたコンテンツURIのファイルをストリームとして開きます。
        /// また、サブディレクトリが指定されている場合は内部でサブディレクトリの生成を行います。
        /// </summary>
        /// <param name="contentUri">ストリームとして開くコンテンツURI</param>
        /// <param name="mode">指定されたコンテンツURIに対するモード</param>
        /// <param name="access">指定されたコンテンツURIに対するアクセス方法</param>
        /// <returns>正しくストリームを開けた場合はストリームの参照を返します。開けなかった場合は null を返します</returns>
        /// <exception cref="ArgumentNullException">contentUri が null です</exception>
        public Stream Open(Uri contentUri, FileMode mode, FileAccess access)
        {
            var nativePath = GetNativePath(contentUri);
            var fileName = Path.GetFileName(nativePath);
            var removeStartIndex = nativePath.LastIndexOf(fileName) - 1;
            var directoryPath = nativePath.Remove(removeStartIndex);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }


            return new FileStream(nativePath, mode, access);
        }


        /// <summary>
        /// 指定されたコンテンツURIのファイルまたはディレクトリを削除します。
        /// ディレクトリとしてのコンテンツURIが指定された場合は、そのディレクトリのコンテンツもすべて削除されます。
        /// </summary>
        /// <param name="contentUri">削除するコンテンツのURI</param>
        /// <exception cref="ArgumentNullException">contentUri が null です</exception>
        public void Delete(Uri contentUri)
        {
            var nativePath = GetNativePath(contentUri);
            if (File.Exists(nativePath))
            {
                File.Delete(nativePath);
                return;
            }


            if (Directory.Exists(nativePath))
            {
                Directory.Delete(nativePath, true);
                return;
            }
        }


        /// <summary>
        /// このファイルシステムストレージが管理するディレクトリに含まれるすべてのコンテンツを削除します
        /// </summary>
        public void DeleteAll()
        {
            var baseDirectoryPath = GetBaseDirectoryPath();
            foreach (var directoryPath in Directory.GetDirectories(baseDirectoryPath))
            {
                Directory.Delete(directoryPath, true);
            }


            foreach (var filePath in Directory.GetFiles(baseDirectoryPath))
            {
                File.Delete(filePath);
            }
        }


        /// <summary>
        /// コンテンツURIからファイルシステムとしてのファイルパスを取得します
        /// </summary>
        /// <param name="contentUri">ファイルパスとして取得するコンテンツURI</param>
        /// <returns>指定されたコンテンツURIからファイルシステムとしてのファイルパスを返します</returns>
        /// <exception cref="ArgumentNullException">contentUri が null です</exception>
        public string GetNativePath(Uri contentUri)
        {
            var uriLocalPath = (contentUri ?? throw new ArgumentNullException(nameof(contentUri))).LocalPath.TrimStart('/');
            return Path.Combine(GetBaseDirectoryPath(), uriLocalPath).Replace("\\", "/");
        }


        /// <summary>
        /// このファイルシステムストレージが管理するディレクトリへのパス
        /// </summary>
        /// <returns>このファイルシステムストレージが管理するディレクトリパスを返します</returns>
        protected virtual string GetBaseDirectoryPath()
        {
            // 既定動作はカレントディレクトリパスを返す
            return Environment.CurrentDirectory;
        }
    }
}