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
using System.Net;
using System.Threading;
using PenguinBox.Core;

namespace PenguinBox.Fetcher
{
    /// <summary>
    /// HTTPまたはHTTPSを使用したコンテンツフェッチを行うクラスです
    /// </summary>
    public class HttpFetcher : IFetcher
    {
        /// <summary>
        /// 最低限必要な受信バッファサイズ
        /// </summary>
        public const int MinimumReceiveBufferSize = 1 << 10;

        private const int DefaultTimeout = 5000;
        private const int DefaultReceiveBufferSize = 1 << 20;

        private int timeout;
        private int receiveBufferSize;



        /// <summary>
        /// 要求が返ってくるまでのタイムアウト時間（ms）
        /// </summary>
        public int Timeout { get => timeout; set => SetTimeout(value); }


        /// <summary>
        /// 受信バッファのサイズ
        /// </summary>
        public int ReceiveBufferSize { get => receiveBufferSize; set => SetReceiveBufferSize(value); }



        /// <summary>
        /// HttpFetcher クラスのインスタンスを初期化します
        /// </summary>
        public HttpFetcher()
        {
            Timeout = DefaultTimeout;
            ReceiveBufferSize = DefaultReceiveBufferSize;
        }


        /// <summary>
        /// リモートからコンテンツをフェッチします
        /// </summary>
        /// <param name="remoteUri">フェッチするリモートURI</param>
        /// <param name="outStream">フェッチしたデータを出力する出力先ストリーム</param>
        /// <param name="listener">フェッチイベントを監視するリスナー</param>
        /// <param name="token">キャンセルを受け付けるためのトークン</param>
        /// <exception cref="ArgumentNullException">remoteUri または outStream または listener が null です</exception>
        /// <exception cref="ArgumentException">出力ストリームの書き込みが出来ません</exception>
        /// <exception cref="SchemeNotSupportedException">指定されたスキームをサポートしていません</exception>
        public void Fetch(Uri remoteUri, Stream outStream, IFetcherEventListener listener, CancellationToken token)
        {
            ThrowIfArgumentNull(remoteUri, nameof(remoteUri));
            ThrowIfArgumentNull(outStream, nameof(outStream));
            ThrowIfArgumentNull(listener, nameof(listener));
            ThrowIfOutStreamCanNotWrite(outStream, nameof(outStream));
            ThrowIfSchemeNotSupported(remoteUri);


            if (token.IsCancellationRequested)
            {
                listener.OnError(FetchErrorReason.Cancel, null);
                return;
            }


            var request = CreateRequest(remoteUri);
            if (!TryGetResponse(request, out var response, out var error))
            {
                if (error.Status == WebExceptionStatus.Timeout)
                {
                    listener.OnError(FetchErrorReason.Timeout, error);
                    return;
                }


                // 404 Status = 'NotFound.' can not retryable.
                // 4xx Status = 'Client problem.' can not retryable.
                // 5xx Status = 'Server problem.' can retryable.
                // xxx Status = '??? unknown...' can retryable.
                var errorResponse = (HttpWebResponse)error.Response;
                var errorNumber = (int)errorResponse.StatusCode / 100;
                if (errorNumber == 4)
                {
                    if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        listener.OnError(FetchErrorReason.RemoteContentNotFound, error);
                        return;
                    }
                    else
                    {
                        listener.OnError(FetchErrorReason.RequestError, error);
                        return;
                    }
                }
                else if (errorNumber == 5)
                {
                    listener.OnError(FetchErrorReason.RemoteError, error);
                    return;
                }


                listener.OnError(FetchErrorReason.Unknown, error);
                return;
            }


            listener.OnContentLengthDetected(response.ContentLength);


            using (response)
            using (var stream = response.GetResponseStream())
            {
                var readSize = 0;
                var buffer = new byte[ReceiveBufferSize];
                while ((readSize = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (token.IsCancellationRequested)
                    {
                        listener.OnError(FetchErrorReason.Cancel, null);
                        return;
                    }


                    outStream.Write(buffer, 0, readSize);
                    listener.OnContentReceiving(buffer, 0, readSize);
                }
            }
        }


        #region private utility
        private HttpWebRequest CreateRequest(Uri uri)
        {
            var request = WebRequest.CreateHttp(uri);
            request.Timeout = Timeout;
            return request;
        }


        private void SetTimeout(int timeout)
        {
            ThrowIfTimeoutOutOfRange(timeout, nameof(timeout));
            this.timeout = timeout;
        }


        private void SetReceiveBufferSize(int bufferSize)
        {
            ThrowIfReceiveBufferSizeOutOfRange(bufferSize, nameof(bufferSize));
            receiveBufferSize = bufferSize;
        }


        private bool TryGetResponse(HttpWebRequest request, out HttpWebResponse response, out WebException error)
        {
            response = null;
            error = null;


            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException exception)
            {
                error = exception;
                return false;
            }
        }
        #endregion


        #region Exception thrower and builder
        private void ThrowIfSchemeNotSupported(Uri uri)
        {
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                var message = $"指定されたスキーム '{uri.Scheme}' は対応していません。";
                throw new SchemeNotSupportedException(message);
            }
        }


        private void ThrowIfArgumentNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }


        private void ThrowIfOutStreamCanNotWrite(Stream stream, string name)
        {
            if (!stream.CanWrite)
            {
                var message = $"出力ストリームの書き込みが出来ません";
                throw new ArgumentException(message, name);
            }
        }


        private void ThrowIfTimeoutOutOfRange(int timeout, string name)
        {
            if (timeout < 0 && timeout != System.Threading.Timeout.Infinite)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }


        private void ThrowIfReceiveBufferSizeOutOfRange(int size, string name)
        {
            if (size < MinimumReceiveBufferSize)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
        #endregion
    }
}