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
using System.Threading.Tasks;

namespace PenguinBox.Fetcher
{
    /// <summary>
    /// HTTPまたはHTTPSを使用したコンテンツフェッチを行うクラスです
    /// </summary>
    public class HttpFetcher : IFetcher
    {
        /// <summary>
        /// リモートからコンテンツをフェッチします
        /// </summary>
        /// <param name="remoteUri">フェッチするリモートURI</param>
        /// <param name="outStream">フェッチしたデータを出力する出力先ストリーム</param>
        /// <param name="listener">フェッチイベントを監視するリスナー</param>
        /// <param name="token">キャンセルを受け付けるためのトークン</param>
        public void Fetch(Uri remoteUri, Stream outStream, IFetcherEventListener listener, CancellationToken token)
        {
            ThrowIfArgumentNull(remoteUri, nameof(remoteUri));
            ThrowIfArgumentNull(outStream, nameof(outStream));
            ThrowIfArgumentNull(listener, nameof(listener));


            if (token.IsCancellationRequested)
            {
                listener.OnError(FetchErrorReason.Cancel, null);
                return;
            }


            var request = WebRequest.CreateHttp(remoteUri);
            request.ContinueTimeout = 1000;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException error)
            {
                if (error.Status == WebExceptionStatus.Timeout)
                {
                    listener.OnError(FetchErrorReason.Timeout, error);
                    return;
                }


                return;
            }


            using (var stream = response.GetResponseStream())
            {
                var buffer = new byte[1024];
                int readSize = 0;
                while ((readSize = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, readSize);
                }
            }
        }


        private bool TryGetResponse(HttpWebRequest request, out HttpWebResponse response, out WebException error)
        {
            throw new NotImplementedException();
        }


        private void ThrowIfArgumentNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}