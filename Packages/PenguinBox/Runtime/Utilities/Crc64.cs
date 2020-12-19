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

namespace PenguinBox.Utilities
{
    /// <summary>
    /// CRC64実装を提供します
    /// </summary>
    public static class Crc64
    {
        private const ulong Polynomial = 0xC96C5795D7870F42UL;
        private readonly static ulong[] table = new ulong[256];



        /// <summary>
        /// CRC64 クラスの初期化をします
        /// </summary>
        static Crc64()
        {
            for (ulong i = 0UL; i < (ulong)table.Length; ++i)
            {
                ulong num = ((i & 1) * Polynomial) ^ (i >> 1);
                num = ((num & 1) * Polynomial) ^ (num >> 1);
                num = ((num & 1) * Polynomial) ^ (num >> 1);
                num = ((num & 1) * Polynomial) ^ (num >> 1);
                num = ((num & 1) * Polynomial) ^ (num >> 1);
                num = ((num & 1) * Polynomial) ^ (num >> 1);
                num = ((num & 1) * Polynomial) ^ (num >> 1);
                num = ((num & 1) * Polynomial) ^ (num >> 1);
                table[i] = num;
            }
        }


        /// <summary>
        /// 指定されたバッファ全体を、CRCの計算をします
        /// </summary>
        /// <remarks>
        /// この関数は、継続的にCRC計算をするのではなく、この関数の呼び出し一回で終了される事を想定します。
        /// </remarks>
        /// <param name="buffer">計算する対象のバッファ</param>
        /// <returns>計算された結果を返します</returns>
        /// <exception cref="ArgumentNullException">buffer が null です</exception>
        public static unsafe ulong Calculate(byte[] buffer)
        {
            fixed (byte* p = buffer)
            {
                return Calculate(ulong.MaxValue, p, buffer.Length) ^ ulong.MaxValue;
            }
        }


        /// <summary>
        /// 指定されたバッファの範囲を、CRCの計算を行います
        /// </summary>
        /// <remarks>
        /// この関数は、継続的にCRC計算をするのではなく、この関数の呼び出し一回で終了される事を想定します。
        /// </remarks>
        /// <param name="buffer">計算する対象のバッファ</param>
        /// <param name="index">バッファの開始位置</param>
        /// <param name="count">バッファから取り出す量</param>
        /// <returns>計算された結果を返します</returns>
        /// <exception cref="ArgumentNullException">buffer が null です</exception>
        /// <exception cref="ArgumentOutOfRangeException">index または index, count 合計値がbufferの範囲を超えます</exception>
        public static unsafe ulong Calculate(byte[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }


            if (index < 0 || buffer.Length <= index + count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(index)} or {nameof(count)}", $"指定された範囲では {nameof(buffer)} の範囲を超えます");
            }


            fixed (byte* p = buffer)
            {
                return Calculate(ulong.MaxValue, p + index, count) ^ ulong.MaxValue;
            }
        }


        /// <summary>
        /// 指定されたバッファの範囲を、CRCの計算を行います
        /// </summary>
        /// <remarks>
        /// この関数は、継続的にCRC計算をするのではなく、この関数の呼び出し一回で終了される事を想定します。
        /// </remarks>
        /// <param name="buffer">計算する対象のポインタ</param>
        /// <param name="count">計算するバイトの数</param>
        /// <returns>計算された結果を返します</returns>
        /// <exception cref="ArgumentNullException">buffer が null です</exception>
        public static unsafe ulong Calculate(byte* buffer, int count)
        {
            return Calculate(ulong.MaxValue, buffer, count) ^ ulong.MaxValue;
        }


        /// <summary>
        /// 指定されたバッファ全体を、CRCの計算を行います
        /// </summary>
        /// <remarks>
        /// バッファが複数に分かれて、継続して計算する場合は、この関数が返したハッシュ値をそのまま continusHash パラメータに渡して計算を行って下さい。
        /// また、初回の計算をする前に continusHash へ ulong.MaxValue をセットし、すべてのバッファ処理が終了後 ulong.MaxValue の XOR 反転を行って下さい。
        /// </remarks>
        /// <param name="continusHash">前回計算したハッシュ値、存在しない場合は既定値を指定</param>
        /// <param name="buffer">計算する対象のバッファ</param>
        /// <returns>CRC計算された結果を返します</returns>
        /// <exception cref="ArgumentNullException">buffer が null です</exception>
        public static unsafe ulong Calculate(ulong continusHash, byte[] buffer)
        {
            fixed (byte* p = buffer)
            {
                return Calculate(continusHash, p, buffer.Length);
            }
        }


        /// <summary>
        /// 指定されたバッファの範囲を、CRCの計算を行います
        /// </summary>
        /// <remarks>
        /// バッファが複数に分かれて継続して計算する場合は、この関数が返したハッシュ値をそのまま continusHash パラメータに渡して計算を行って下さい。
        /// また、初回の計算をする前に continusHash へ ulong.MaxValue をセットし、すべてのバッファ処理が終了後 ulong.MaxValue の XOR 反転を行って下さい。
        /// </remarks>
        /// <param name="continusHash">前回計算したハッシュ値、存在しない場合は既定値を指定</param>
        /// <param name="buffer">計算する対象のバッファ</param>
        /// <param name="index">バッファの開始位置</param>
        /// <param name="count">バッファから取り出す量</param>
        /// <returns>CRC計算された結果を返します</returns>
        /// <exception cref="ArgumentNullException">buffer が null です</exception>
        /// <exception cref="ArgumentOutOfRangeException">index または index, count 合計値がbufferの範囲を超えます</exception>
        public static unsafe ulong Calculate(ulong continusHash, byte[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }


            if (index < 0 || buffer.Length <= index + count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(index)} or {nameof(count)}", $"指定された範囲では {nameof(buffer)} の範囲を超えます");
            }


            fixed (byte* p = buffer)
            {
                return Calculate(continusHash, p + index, count);
            }
        }


        /// <summary>
        /// 指定されたバッファの範囲を、CRCの計算を行います
        /// </summary>
        /// <remarks>
        /// バッファが複数に分かれて継続して計算する場合は、この関数が返したハッシュ値をそのまま continusHash パラメータに渡して計算を行って下さい。
        /// また、初回の計算をする前に continusHash へ ulong.MaxValue をセットし、すべてのバッファ処理が終了後 ulong.MaxValue の XOR 反転を行って下さい。
        /// </remarks>
        /// <param name="continusHash">前回計算したハッシュ値、存在しない場合は既定値を指定</param>
        /// <param name="buffer">計算する対象のバッファのポインタ</param>
        /// <param name="count">バッファから取り出す量</param>
        /// <returns>CRC計算された結果を返します</returns>
        /// <exception cref="ArgumentNullException">buffer が null です</exception>
        public static unsafe ulong Calculate(ulong continusHash, byte* buffer, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }


            for (int i = 0; i < count; ++i)
            {
                continusHash = table[(*buffer ^ continusHash) & 0xFF] ^ (continusHash >> 8);
                ++buffer;
            }


            return continusHash;
        }
    }
}