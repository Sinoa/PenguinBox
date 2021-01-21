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

namespace Sinoalmond.PenguinBox.Utilities
{
    /// <summary>
    /// ユニークな符号なし64bit整数のIDを生成を提供します。
    /// </summary>
    public class AlmondId
    {
        // AlmondId := MSB [Timestamp] | [WorkerID] | [Sequence] LSB (64bit ID)
        public const ulong TimestampMask = 0x3FFFFFFFFFF; // 42bit
        public const ulong WorkerIdMask = 0x3FF; // 10bit
        public const ulong SequenceMask = 0xFFF; // 12bit

        /// <summary>
        /// AlmondIdが生成開始するオフセットタイムスタンプ（ミリ秒）
        /// </summary>
        /// <remarks>
        /// この値はソースコードをコピーして実装者側で自由に変えて良い。
        /// ただし、内部でタイムスタンプを計算する際に UTC+0 を用いることに注意して下さい。
        /// </remarks>
        public static readonly System.DateTimeOffset OffsetTimestamp = new System.DateTimeOffset(2021, 1, 1, 0, 0, 0, 0, System.TimeSpan.Zero);

        private readonly int workerId;
        private long lastFetchedTimestamp;
        private int sequence;



        /// <summary>
        /// AlmondId クラスのインスタンスを初期化します
        /// </summary>
        /// <param name="workerId">この AlmondId が担当する生成するワーカーID</param>
        /// <exception cref="System.ArgumentOutOfRangeException">workerId が 0 から WorkerIdMask の範囲外です。</exception>
        public AlmondId(int workerId)
        {
            if (workerId < 0 || (int)WorkerIdMask < workerId)
            {
                var message = $"workerId が 0 から WorkerIdMask の範囲外です。 workerId='{workerId}'";
                throw new System.ArgumentOutOfRangeException(message);
            }


            this.workerId = workerId;
            lastFetchedTimestamp = GetTimestamp();
            sequence = 0;
        }


        /// <summary>
        /// ユニークなIDを生成します
        /// </summary>
        /// <returns>生成された符号なし64bit整数のIDを返します</returns>
        public ulong Generate()
        {
            var timestamp = GetTimestamp();
            while (timestamp < lastFetchedTimestamp)
            {
                System.Threading.Thread.Sleep(0);
                timestamp = GetTimestamp();
            }


            if (timestamp == lastFetchedTimestamp)
            {
                if (++sequence <= (int)SequenceMask)
                {
                    return BuildID(timestamp, workerId, sequence);
                }


                while (timestamp <= lastFetchedTimestamp)
                {
                    System.Threading.Thread.Sleep(0);
                    timestamp = GetTimestamp();
                }
            }


            sequence = 0;
            lastFetchedTimestamp = timestamp;
            return BuildID(timestamp, workerId, sequence);
        }


        private long GetTimestamp()
        {
            return (long)(System.DateTimeOffset.UtcNow - OffsetTimestamp).TotalMilliseconds;
        }


        private static ulong BuildID(long timestamp, int workerID, int sequence)
        {
            return (((ulong)timestamp & TimestampMask) << 22) | (((ulong)workerID & WorkerIdMask) << 12) | ((ulong)sequence & SequenceMask);
        }
    }
}