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
    /// ファイルシステムを使用した汎用的なフェッチを行うクラスです
    /// </summary>
    public class FileSystemFetcher : Fetcher
    {
        protected override bool CanSchemeSupport(Uri remoteUri)
        {
            return remoteUri.Scheme == Uri.UriSchemeFile;
        }


        protected override void FetchCore(Uri remoteUri, Stream outStream, IFetcherEventListener listener, CancellationToken token)
        {
            var filePath = remoteUri.LocalPath;
            if (!File.Exists(filePath))
            {
                listener.OnError(FetchErrorReason.RemoteContentNotFound, null);
                return;
            }


            var fileInfo = new FileInfo(filePath);
            listener.OnContentLengthDetected(fileInfo.Length);


            using (var inputStream = fileInfo.OpenRead())
            {
                var readSize = 0;
                var buffer = new byte[1 << 20];
                while ((readSize = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, readSize);
                    listener.OnContentReceiving(buffer, 0, readSize);


                    if (token.IsCancellationRequested)
                    {
                        listener.OnError(FetchErrorReason.Cancel, null);
                        return;
                    }
                }
            }
        }
    }
}