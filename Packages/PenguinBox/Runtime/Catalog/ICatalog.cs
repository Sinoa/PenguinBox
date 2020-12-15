﻿// Zlib license
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

namespace PenguinBox.Catalog
{
    /// <summary>
    /// 一覧を表現するインターフェイスです
    /// </summary>
    public interface ICatalog
    {
        /// <summary>
        /// カタログに含まれるアイテム数
        /// </summary>
        int ItemCount { get; }



        /// <summary>
        /// 指定された名前のカタログアイテムが含まれているか確認します
        /// </summary>
        /// <param name="name">確認するカタログアイテムの名前</param>
        /// <returns>含まれている場合は true を、含まれていない場合は false を返します</returns>
        bool ContainsItem(string name);


        /// <summary>
        /// 指定した名前のカタログアイテムを取得します
        /// </summary>
        /// <param name="name">取得するアイテム名</param>
        /// <returns>指定された名前からカタログアイテムを取得された場合はインスタンスを返しますが、見つからない場合は null を返します</returns>
        ICatalogItem GetItem(string name);


        /// <summary>
        /// カタログに含まれている全てのカタログアイテムを取得して列挙可能なオブジェクトを取得します
        /// </summary>
        /// <returns>全てのカタログアイテムを列挙可能なオブジェクトを返します</returns>
        IEnumerable<ICatalogItem> GetItemAll();
    }
}