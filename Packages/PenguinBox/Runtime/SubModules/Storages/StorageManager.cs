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

namespace PenguinBox.SubModules.Storages
{
    /// <summary>
    /// PenguinBox システムが持つストレージを管理するクラスです
    /// </summary>
    internal class StorageManager
    {
        private readonly List<IStorage> storageList;



        /// <summary>
        /// StorageManager クラスのインスタンスを初期化します
        /// </summary>
        public StorageManager()
        {
            storageList = new List<IStorage>();
        }


        /// <summary>
        /// ストレージを登録します。ただし同じ名前のストレージは登録できません
        /// </summary>
        /// <param name="storage">追加するストレージ</param>
        /// <exception cref="ArgumentNullException">storage が null です</exception>
        /// <exception cref="ArgumentException">ストレージの名前が null です</exception>
        /// <exception cref="StorageAlreadyExistsException">既に同名のストレージが存在します</exception>
        public void RegisterStorage(IStorage storage)
        {
            ThrowIfStorageNameIsNull(storage ?? throw new ArgumentNullException(nameof(storage)));
            ThrowIfStorageAlreadyExists(storage);


            storageList.Add(storage);
        }


        /// <summary>
        /// 指定された名前のストレージを取得します
        /// </summary>
        /// <param name="storageName">取得するストレージ名</param>
        /// <returns>指定された名前のストレージを返します</returns>
        /// <exception cref="ArgumentNullException">storageName が null です</exception>
        /// <exception cref="StorageNotFoundException">ストレージが見つかりませんでした</exception>
        public IStorage GetStorage(string storageName)
        {
            if (storageName == null)
            {
                throw new ArgumentNullException(nameof(storageName));
            }


            foreach (var storage in storageList)
            {
                if (storage.Name == storageName)
                {
                    return storage;
                }
            }


            throw MakeStorageNotFoundException(storageName);
        }


        #region ExceptionBuilder and thrower
        private void ThrowIfStorageNameIsNull(IStorage storage)
        {
            if (storage.Name == null)
            {
                var message = "ストレージの名前が null です。";
                throw new InvalidStorageException(message);
            }
        }


        private void ThrowIfStorageAlreadyExists(IStorage storage)
        {
            foreach (var myStorage in storageList)
            {
                if (myStorage.Name == storage.Name)
                {
                    var message = $"既に '{storage.Name}' の名前のストレージが存在します。";
                    throw new StorageAlreadyExistsException(message);
                }
            }
        }


        private StorageNotFoundException MakeStorageNotFoundException(string storageName)
        {
            var message = $"ストレージ名 '{storageName}' のストレージが見つかりませんでした。";
            return new StorageNotFoundException(message);
        }
        #endregion
    }
}