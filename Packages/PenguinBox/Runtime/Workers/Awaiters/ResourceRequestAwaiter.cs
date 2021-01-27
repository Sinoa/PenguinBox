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

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Sinoalmond.PenguinBox.Workers.Awaiters
{
    /// <summary>
    /// ResourceRequest クラスの待機構造体です
    /// </summary>
    public struct ResourceRequestAwaiter : INotifyCompletion
    {
        private readonly ResourceRequest request;
        private Action continuation;



        /// <summary>
        /// タスクが完了しているか否か
        /// </summary>
        public bool IsCompleted => request.isDone;



        /// <summary>
        /// ResourceRequestAwaiter 構造体のインスタンスを初期化します
        /// </summary>
        /// <param name="request">待機する ResourceRequest のインスタンス</param>
        public ResourceRequestAwaiter(ResourceRequest request)
        {
            this.request = request;
            continuation = null;
        }


        /// <summary>
        /// タスクの完了処理をします
        /// </summary>
        /// <param name="continuation">処理の継続関数</param>
        public void OnCompleted(Action continuation)
        {
            if (request.isDone)
            {
                continuation();
                return;
            }


            this.continuation = continuation;
            request.completed += OnCompleted;
        }


        private void OnCompleted(AsyncOperation asyncOperation)
        {
            request.completed -= OnCompleted;
            continuation();
        }


        /// <summary>
        /// タスクの結果を取得します
        /// </summary>
        /// <returns>タスクの結果を返します</returns>
        public UnityObject GetResult()
        {
            return request.asset;
        }
    }
}