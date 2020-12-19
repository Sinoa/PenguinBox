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
    public static class EndiannessConverter
    {
        #region ToLittleEndian
        #region signed
        public static void ToLittleEndian(this sbyte value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(sbyte), index);
            RawConvert((byte)value, buffer, index);
        }


        public static void ToLittleEndian(this short value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(short), index);
            RawConvert(BitConverter.IsLittleEndian, (ushort)value, buffer, index);
        }


        public static void ToLittleEndian(this int value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(int), index);
            RawConvert(BitConverter.IsLittleEndian, (uint)value, buffer, index);
        }


        public static void ToLittleEndian(this long value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(long), index);
            RawConvert(BitConverter.IsLittleEndian, (ulong)value, buffer, index);
        }
        #endregion


        #region unsigned
        public static void ToLittleEndian(this byte value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(byte), index);
            RawConvert(value, buffer, index);
        }


        public static void ToLittleEndian(this ushort value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(ushort), index);
            RawConvert(BitConverter.IsLittleEndian, value, buffer, index);
        }


        public static void ToLittleEndian(this uint value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(uint), index);
            RawConvert(BitConverter.IsLittleEndian, value, buffer, index);
        }


        public static void ToLittleEndian(this ulong value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(ulong), index);
            RawConvert(BitConverter.IsLittleEndian, value, buffer, index);
        }
        #endregion


        #region floating point
        private static unsafe int SingleToInt(float value)
        {
            return *(int*)&value;
        }


        private static unsafe long DoubleToLong(double value)
        {
            return *(long*)&value;
        }


        public static void ToLittleEndian(this float value, byte[] buffer, int index = 0)
        {
            ToLittleEndian(SingleToInt(value), buffer, index);
        }


        public static void ToLittleEndian(this double value, byte[] buffer, int index = 0)
        {
            ToLittleEndian(DoubleToLong(value), buffer, index);
        }
        #endregion


        #region special primitive
        // bool 8bit (force convert to true = 1, false = 0)
        public static void ToLittleEndian(this bool value, byte[] buffer, int index = 0)
        {
            ToLittleEndian((byte)(value ? 1 : 0), buffer, index);
        }


        // char(Unicode) 16bit
        public static void ToLittleEndian(this char value, byte[] buffer, int index = 0)
        {
            ToLittleEndian((ushort)value, buffer, index);
        }
        #endregion
        #endregion


        #region ToBigEndian
        #region signed
        public static void ToBigEndian(this sbyte value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(sbyte), index);
            RawConvert((byte)value, buffer, index);
        }


        public static void ToBigEndian(this short value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(short), index);
            RawConvert(!BitConverter.IsLittleEndian, (ushort)value, buffer, index);
        }


        public static void ToBigEndian(this int value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(int), index);
            RawConvert(!BitConverter.IsLittleEndian, (uint)value, buffer, index);
        }


        public static void ToBigEndian(this long value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(long), index);
            RawConvert(!BitConverter.IsLittleEndian, (ulong)value, buffer, index);
        }
        #endregion


        #region unsigned
        public static void ToBigEndian(this byte value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(byte), index);
            RawConvert(value, buffer, index);
        }


        public static void ToBigEndian(this ushort value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(ushort), index);
            RawConvert(!BitConverter.IsLittleEndian, value, buffer, index);
        }


        public static void ToBigEndian(this uint value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(uint), index);
            RawConvert(!BitConverter.IsLittleEndian, value, buffer, index);
        }


        public static void ToBigEndian(this ulong value, byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(ulong), index);
            RawConvert(!BitConverter.IsLittleEndian, value, buffer, index);
        }
        #endregion


        #region floating point
        public static void ToBigEndian(this float value, byte[] buffer, int index = 0)
        {
            ToBigEndian(SingleToInt(value), buffer, index);
        }


        public static void ToBigEndian(this double value, byte[] buffer, int index = 0)
        {
            ToBigEndian(DoubleToLong(value), buffer, index);
        }
        #endregion


        #region special primitive
        // bool 8bit (force convert to true = 1, false = 0)
        public static void ToBigEndian(this bool value, byte[] buffer, int index = 0)
        {
            ToBigEndian((byte)(value ? 1 : 0), buffer, index);
        }


        // char(Unicode) 16bit
        public static void ToBigEndian(this char value, byte[] buffer, int index = 0)
        {
            ToBigEndian((ushort)value, buffer, index);
        }
        #endregion
        #endregion


        #region RawConvert
        private static void RawConvert(byte value, byte[] buffer, int index)
        {
            buffer[index] = value;
        }


        private static void RawConvert(bool isLittleEndian, ushort value, byte[] buffer, int index)
        {
            if (isLittleEndian)
            {
                buffer[index + 0] = (byte)((value >> 0) & 0xFF);
                buffer[index + 1] = (byte)((value >> 8) & 0xFF);
            }
            else
            {
                buffer[index + 0] = (byte)((value >> 8) & 0xFF);
                buffer[index + 1] = (byte)((value >> 0) & 0xFF);
            }
        }


        private static void RawConvert(bool isLittleEndian, uint value, byte[] buffer, int index)
        {
            if (isLittleEndian)
            {
                buffer[index + 0] = (byte)((value >> 0) & 0xFF);
                buffer[index + 1] = (byte)((value >> 8) & 0xFF);
                buffer[index + 2] = (byte)((value >> 16) & 0xFF);
                buffer[index + 3] = (byte)((value >> 24) & 0xFF);
            }
            else
            {
                buffer[index + 0] = (byte)((value >> 24) & 0xFF);
                buffer[index + 1] = (byte)((value >> 16) & 0xFF);
                buffer[index + 2] = (byte)((value >> 8) & 0xFF);
                buffer[index + 3] = (byte)((value >> 0) & 0xFF);
            }
        }


        private static void RawConvert(bool isLittleEndian, ulong value, byte[] buffer, int index)
        {
            if (isLittleEndian)
            {
                buffer[index + 0] = (byte)((value >> 0) & 0xFF);
                buffer[index + 1] = (byte)((value >> 8) & 0xFF);
                buffer[index + 2] = (byte)((value >> 16) & 0xFF);
                buffer[index + 3] = (byte)((value >> 24) & 0xFF);
                buffer[index + 4] = (byte)((value >> 32) & 0xFF);
                buffer[index + 5] = (byte)((value >> 40) & 0xFF);
                buffer[index + 6] = (byte)((value >> 48) & 0xFF);
                buffer[index + 7] = (byte)((value >> 56) & 0xFF);
            }
            else
            {
                buffer[index + 0] = (byte)((value >> 56) & 0xFF);
                buffer[index + 1] = (byte)((value >> 48) & 0xFF);
                buffer[index + 2] = (byte)((value >> 40) & 0xFF);
                buffer[index + 3] = (byte)((value >> 32) & 0xFF);
                buffer[index + 4] = (byte)((value >> 24) & 0xFF);
                buffer[index + 5] = (byte)((value >> 16) & 0xFF);
                buffer[index + 6] = (byte)((value >> 8) & 0xFF);
                buffer[index + 7] = (byte)((value >> 0) & 0xFF);
            }
        }
        #endregion


        #region ToValue
        #region signed
        public static sbyte ToInt8(this byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(sbyte), index);
            return (sbyte)RawValue8(buffer, index);
        }


        public static short ToInt16(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(short), index);
            return (short)RawValue16(BitConverter.IsLittleEndian, isLittleEndianBuffer, buffer, index);
        }


        public static int ToInt32(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(int), index);
            return (int)RawValue32(BitConverter.IsLittleEndian, isLittleEndianBuffer, buffer, index);
        }


        public static long ToInt64(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(long), index);
            return (long)RawValue64(BitConverter.IsLittleEndian, isLittleEndianBuffer, buffer, index);
        }
        #endregion


        #region unsigned
        public static byte ToUInt8(this byte[] buffer, int index = 0)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(byte), index);
            return RawValue8(buffer, index);
        }


        public static ushort ToUInt16(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(ushort), index);
            return RawValue16(BitConverter.IsLittleEndian, isLittleEndianBuffer, buffer, index);
        }


        public static uint ToUInt32(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(uint), index);
            return RawValue32(BitConverter.IsLittleEndian, isLittleEndianBuffer, buffer, index);
        }


        public static ulong ToUInt64(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            ThrowIfBufferIsNotSatisfyCondition(buffer, sizeof(ulong), index);
            return RawValue64(BitConverter.IsLittleEndian, isLittleEndianBuffer, buffer, index);
        }
        #endregion


        #region floating point
        private static unsafe float IntToSingle(int value)
        {
            return *(float*)&value;
        }


        private static unsafe double LongToDouble(long value)
        {
            return *(double*)&value;
        }


        public static float ToSingle(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            return IntToSingle(ToInt32(buffer, index, isLittleEndianBuffer));
        }


        public static double ToDouble(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            return LongToDouble(ToInt64(buffer, index, isLittleEndianBuffer));
        }
        #endregion


        #region special primitive
        public static bool ToBoolean(this byte[] buffer, int index = 0)
        {
            return ToUInt8(buffer, index) != 0;
        }


        public static char ToChar(this byte[] buffer, int index = 0, bool isLittleEndianBuffer = true)
        {
            return (char)ToInt16(buffer, index, isLittleEndianBuffer);
        }
        #endregion
        #endregion


        #region RawValue
        private static byte RawValue8(byte[] buffer, int index)
        {
            return buffer[index];
        }


        private static ushort RawValue16(bool isLittleEndian, bool fromLittleEndian, byte[] buffer, int index)
        {
            if (isLittleEndian == fromLittleEndian)
            {
                return (ushort)(
                    (buffer[index + 0] << 0) |
                    (buffer[index + 1] << 8));
            }
            else
            {
                return (ushort)(
                    (buffer[index + 0] << 8) |
                    (buffer[index + 1] << 0));
            }
        }


        private static uint RawValue32(bool isLittleEndian, bool fromLittleEndian, byte[] buffer, int index)
        {
            if (isLittleEndian == fromLittleEndian)
            {
                return (uint)(
                    (buffer[index + 0] << 0) |
                    (buffer[index + 1] << 8) |
                    (buffer[index + 2] << 16) |
                    (buffer[index + 3] << 24));
            }
            else
            {
                return (uint)(
                    (buffer[index + 0] << 24) |
                    (buffer[index + 1] << 16) |
                    (buffer[index + 2] << 8) |
                    (buffer[index + 3] << 0));
            }
        }


        private static ulong RawValue64(bool isLittleEndian, bool fromLittleEndian, byte[] buffer, int index)
        {
            if (isLittleEndian == fromLittleEndian)
            {
                return
                    ((ulong)buffer[index + 0] << 0) |
                    ((ulong)buffer[index + 1] << 8) |
                    ((ulong)buffer[index + 2] << 16) |
                    ((ulong)buffer[index + 3] << 24) |
                    ((ulong)buffer[index + 4] << 32) |
                    ((ulong)buffer[index + 5] << 40) |
                    ((ulong)buffer[index + 6] << 48) |
                    ((ulong)buffer[index + 7] << 56);
            }
            else
            {
                return
                    ((ulong)buffer[index + 0] << 56) |
                    ((ulong)buffer[index + 1] << 48) |
                    ((ulong)buffer[index + 2] << 40) |
                    ((ulong)buffer[index + 3] << 32) |
                    ((ulong)buffer[index + 4] << 24) |
                    ((ulong)buffer[index + 5] << 16) |
                    ((ulong)buffer[index + 6] << 8) |
                    ((ulong)buffer[index + 7] << 0);
            }
        }
        #endregion


        #region Exception thrower
        private static void ThrowIfBufferIsNotSatisfyCondition(byte[] buffer, int needSize, int index)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }


            if (buffer.Length < needSize)
            {
                var message = $"エンディアン変換にバッファの容量が足りません";
                throw new ArgumentException(message);
            }


            if (index < 0 || buffer.Length < index + needSize)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
        #endregion
    }
}