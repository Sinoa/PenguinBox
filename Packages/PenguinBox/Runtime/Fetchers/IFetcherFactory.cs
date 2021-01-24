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

using Sinoalmond.PenguinBox.Cores;

namespace Sinoalmond.PenguinBox.Fetchers
{
    /// <summary>
    /// URIから使用するべき Fetcher を生成する生成クラスです
    /// </summary>
    public interface IFetcherFactory
    {
        /// <summary>
        /// URIから使用するべき Fetcher を生成します
        /// </summary>
        /// <param name="uri">フェッチしようとしている URI</param>
        /// <returns>対応する Fetcher が生成されたインスタンスを返しますが、対応する Fetcher がない場合は null を返します</returns>
        IFetcher CreateFetcher(UriInfo uri);
    }
}