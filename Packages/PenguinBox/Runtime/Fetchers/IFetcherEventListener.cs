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

using System;

namespace Sinoalmond.PenguinBox.Fetchers
{
    /// <summary>
    /// IFetcher インターフェイスを実装したクラスからイベントを監視するリスナーのインターフェイスです
    /// </summary>
    public interface IFetcherEventListener
    {
        /// <summary>
        /// フェッチするアセットの長さが検出されたイベントを処理します
        /// </summary>
        /// <param name="contentLength">検出されたアセットのコンテンツ長。不明な場合は -1 が渡されます。</param>
        void OnContentLengthDetected(long contentLength);


        /// <summary>
        /// フェッチ中の受信中イベントを処理します
        /// </summary>
        /// <param name="buffer">フェッチに使用しているバッファ</param>
        /// <param name="index">フェッチした有効データが存在するバッファ内のインデックス</param>
        /// <param name="length">フェッチした有効データが存在するバッファ内の長さ</param>
        /// <exception cref="ArgumentOutOfRangeException">指定された範囲がバッファの境界を超えています</exception>
        void OnContentReceiving(byte[] buffer, int index, int length);


        /// <summary>
        /// フェッチエラーが発生したイベントを処理します
        /// </summary>
        /// <param name="reson">エラーの理由</param>
        /// <param name="error">例外発生時に取得した例外。例外がない場合は null。</param>
        void OnError(FetchErrorReason reson, Exception error);
    }
}