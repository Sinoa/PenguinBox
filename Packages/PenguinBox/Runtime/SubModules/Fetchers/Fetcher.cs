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
using System.Threading;

namespace PenguinBox.SubModules.Fetchers
{
    /// <summary>
    /// IFetcher インターフェイスを実装するフェッチャーの抽象クラスです。
    /// </summary>
    public abstract class Fetcher : IFetcher
    {
        /// <summary>
        /// リモートからアセットをフェッチします
        /// </summary>
        /// <param name="remoteUri">フェッチするリモートURI</param>
        /// <param name="outStream">フェッチしたデータを出力する出力先ストリーム</param>
        /// <param name="listener">フェッチイベントを監視するリスナー</param>
        /// <param name="token">キャンセルを受け付けるためのトークン</param>
        /// <exception cref="ArgumentNullException">remoteUri または outStream または listener が null です</exception>
        /// <exception cref="NotSupportedException">出力ストリームの書き込みが出来ません</exception>
        /// <exception cref="NotSupportedException">指定されたスキームをサポートしていません</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<保留中>")]
        public void Fetch(Uri remoteUri, Stream outStream, IFetcherEventListener listener, CancellationToken token)
        {
            ThrowIfArgumentNull(remoteUri, nameof(remoteUri));
            ThrowIfArgumentNull(outStream, nameof(outStream));
            ThrowIfArgumentNull(listener, nameof(listener));
            ThrowIfOutStreamCanNotWrite(outStream);
            ThrowIfSchemeNotSupported(remoteUri);


            if (token.IsCancellationRequested)
            {
                listener.OnError(FetchErrorReason.Cancel, null);
                return;
            }


            try
            {
                FetchCore(remoteUri, outStream, listener, token);
            }
            catch (Exception error)
            {
                // general exception are handling by fetch context exception handler.
                listener.OnError(FetchErrorReason.Unknown, error);
            }
        }


        protected abstract bool CanSchemeSupport(Uri remoteUri);


        protected abstract void FetchCore(Uri remoteUri, Stream outStream, IFetcherEventListener listener, CancellationToken token);


        #region Exception thrower and builder
        private void ThrowIfArgumentNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }


        private void ThrowIfOutStreamCanNotWrite(Stream stream)
        {
            if (!stream.CanWrite)
            {
                var message = $"出力ストリームの書き込みが出来ません";
                throw new NotSupportedException(message);
            }
        }


        private void ThrowIfSchemeNotSupported(Uri uri)
        {
            if (!CanSchemeSupport(uri))
            {
                var message = $"指定されたスキーム '{uri.Scheme}' は対応していません。";
                throw new NotSupportedException(message);
            }
        }
        #endregion
    }
}