// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.FieldValue
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal struct FieldValue
{
  private object m_value;

  internal FieldValue(object o) => this.m_value = o;

  internal static FieldValue[] FromParams(params object[] list)
  {
    FieldValue[] fieldValueArray = new FieldValue[list.Length];
    for (int index = 0; index < list.Length; ++index)
      fieldValueArray[index] = !(list[index] is FieldValue) ? new FieldValue(list[index]) : new FieldValue(((FieldValue) list[index]).Value);
    return fieldValueArray;
  }

  internal void Set(object o) => this.m_value = o;

  public object Value => this.m_value;

  public byte ToByte() => Convert.ToByte(this.m_value);

  public short ToShort()
  {
    switch (System.Type.GetTypeCode(this.m_value.GetType()))
    {
      case TypeCode.Int16:
        return (short) this.m_value;
      case TypeCode.UInt16:
        return (short) (ushort) this.m_value;
      default:
        return Convert.ToInt16(this.m_value);
    }
  }

  public ushort ToUShort()
  {
    switch (System.Type.GetTypeCode(this.m_value.GetType()))
    {
      case TypeCode.Int16:
        return (ushort) (short) this.m_value;
      case TypeCode.UInt16:
        return (ushort) this.m_value;
      default:
        return Convert.ToUInt16(this.m_value);
    }
  }

  public int ToInt()
  {
    switch (System.Type.GetTypeCode(this.m_value.GetType()))
    {
      case TypeCode.Int32:
        return (int) this.m_value;
      case TypeCode.UInt32:
        return (int) (uint) this.m_value;
      default:
        return Convert.ToInt32(this.m_value);
    }
  }

  public uint ToUInt()
  {
    switch (System.Type.GetTypeCode(this.m_value.GetType()))
    {
      case TypeCode.Int32:
        return (uint) (int) this.m_value;
      case TypeCode.UInt32:
        return (uint) this.m_value;
      default:
        return Convert.ToUInt32(this.m_value);
    }
  }

  public float ToFloat() => Convert.ToSingle(this.m_value);

  public double ToDouble() => Convert.ToDouble(this.m_value);

  public override string ToString()
  {
    return this.m_value is byte[] ? Tiff.Latin1Encoding.GetString(this.m_value as byte[]) : Convert.ToString(this.m_value);
  }

  public byte[] GetBytes()
  {
    if (this.m_value == null)
      return (byte[]) null;
    if (this.m_value.GetType().IsArray)
    {
      if (this.m_value is byte[])
        return this.m_value as byte[];
      if (this.m_value is short[])
      {
        short[] src = this.m_value as short[];
        byte[] dst = new byte[src.Length * 2];
        Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, dst.Length);
        return dst;
      }
      if (this.m_value is ushort[])
      {
        ushort[] src = this.m_value as ushort[];
        byte[] dst = new byte[src.Length * 2];
        Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, dst.Length);
        return dst;
      }
      if (this.m_value is int[])
      {
        int[] src = this.m_value as int[];
        byte[] dst = new byte[src.Length * 4];
        Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, dst.Length);
        return dst;
      }
      if (this.m_value is uint[])
      {
        uint[] src = this.m_value as uint[];
        byte[] dst = new byte[src.Length * 4];
        Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, dst.Length);
        return dst;
      }
      if (this.m_value is float[])
      {
        float[] src = this.m_value as float[];
        byte[] dst = new byte[src.Length * 4];
        Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, dst.Length);
        return dst;
      }
      if (this.m_value is double[])
      {
        double[] src = this.m_value as double[];
        byte[] dst = new byte[src.Length * 8];
        Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, dst.Length);
        return dst;
      }
    }
    else
    {
      if (this.m_value is string)
        return Tiff.Latin1Encoding.GetBytes(this.m_value as string);
      if (this.m_value is int)
        return BitConverter.GetBytes((int) this.m_value);
    }
    return (byte[]) null;
  }

  public byte[] ToByteArray()
  {
    if (this.m_value == null)
      return (byte[]) null;
    if (this.m_value.GetType().IsArray)
    {
      if (this.m_value is byte[])
        return this.m_value as byte[];
      if (this.m_value is short[])
      {
        short[] numArray = this.m_value as short[];
        byte[] byteArray = new byte[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          byteArray[index] = (byte) numArray[index];
        return byteArray;
      }
      if (this.m_value is ushort[])
      {
        ushort[] numArray = this.m_value as ushort[];
        byte[] byteArray = new byte[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          byteArray[index] = (byte) numArray[index];
        return byteArray;
      }
      if (this.m_value is int[])
      {
        int[] numArray = this.m_value as int[];
        byte[] byteArray = new byte[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          byteArray[index] = (byte) numArray[index];
        return byteArray;
      }
      if (this.m_value is uint[])
      {
        uint[] numArray = this.m_value as uint[];
        byte[] byteArray = new byte[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          byteArray[index] = (byte) numArray[index];
        return byteArray;
      }
    }
    else if (this.m_value is string)
      return Tiff.Latin1Encoding.GetBytes(this.m_value as string);
    return (byte[]) null;
  }

  public short[] ToShortArray()
  {
    if (this.m_value == null)
      return (short[]) null;
    if (this.m_value.GetType().IsArray)
    {
      if (this.m_value is short[])
        return this.m_value as short[];
      if (this.m_value is byte[])
      {
        byte[] numArray = this.m_value as byte[];
        if (numArray.Length % 2 != 0)
          return (short[]) null;
        int length = numArray.Length / 2;
        short[] shortArray = new short[length];
        int startIndex = 0;
        for (int index = 0; index < length; ++index)
        {
          short int16 = BitConverter.ToInt16(numArray, startIndex);
          shortArray[index] = int16;
          startIndex += 2;
        }
        return shortArray;
      }
      if (this.m_value is ushort[])
      {
        ushort[] numArray = this.m_value as ushort[];
        short[] shortArray = new short[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          shortArray[index] = (short) numArray[index];
        return shortArray;
      }
      if (this.m_value is int[])
      {
        int[] numArray = this.m_value as int[];
        short[] shortArray = new short[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          shortArray[index] = (short) numArray[index];
        return shortArray;
      }
      if (this.m_value is uint[])
      {
        uint[] numArray = this.m_value as uint[];
        short[] shortArray = new short[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          shortArray[index] = (short) numArray[index];
        return shortArray;
      }
    }
    return (short[]) null;
  }

  public ushort[] ToUShortArray()
  {
    if (this.m_value == null)
      return (ushort[]) null;
    if (this.m_value.GetType().IsArray)
    {
      if (this.m_value is ushort[])
        return this.m_value as ushort[];
      if (this.m_value is byte[])
      {
        byte[] numArray = this.m_value as byte[];
        if (numArray.Length % 2 != 0)
          return (ushort[]) null;
        int length = numArray.Length / 2;
        ushort[] ushortArray = new ushort[length];
        int startIndex = 0;
        for (int index = 0; index < length; ++index)
        {
          ushort uint16 = BitConverter.ToUInt16(numArray, startIndex);
          ushortArray[index] = uint16;
          startIndex += 2;
        }
        return ushortArray;
      }
      if (this.m_value is short[])
      {
        short[] numArray = this.m_value as short[];
        ushort[] ushortArray = new ushort[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          ushortArray[index] = (ushort) numArray[index];
        return ushortArray;
      }
      if (this.m_value is int[])
      {
        int[] numArray = this.m_value as int[];
        ushort[] ushortArray = new ushort[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          ushortArray[index] = (ushort) numArray[index];
        return ushortArray;
      }
      if (this.m_value is uint[])
      {
        uint[] numArray = this.m_value as uint[];
        ushort[] ushortArray = new ushort[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          ushortArray[index] = (ushort) numArray[index];
        return ushortArray;
      }
    }
    return (ushort[]) null;
  }

  public int[] ToIntArray()
  {
    if (this.m_value == null)
      return (int[]) null;
    if (this.m_value.GetType().IsArray)
    {
      if (this.m_value is int[])
        return this.m_value as int[];
      if (this.m_value is byte[])
      {
        byte[] numArray = this.m_value as byte[];
        if (numArray.Length % 4 != 0)
          return (int[]) null;
        int length = numArray.Length / 4;
        int[] intArray = new int[length];
        int startIndex = 0;
        for (int index = 0; index < length; ++index)
        {
          int int32 = BitConverter.ToInt32(numArray, startIndex);
          intArray[index] = int32;
          startIndex += 4;
        }
        return intArray;
      }
      if (this.m_value is short[])
      {
        short[] numArray = this.m_value as short[];
        int[] intArray = new int[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          intArray[index] = (int) numArray[index];
        return intArray;
      }
      if (this.m_value is ushort[])
      {
        ushort[] numArray = this.m_value as ushort[];
        int[] intArray = new int[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          intArray[index] = (int) numArray[index];
        return intArray;
      }
      if (this.m_value is uint[])
      {
        uint[] numArray = this.m_value as uint[];
        int[] intArray = new int[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          intArray[index] = (int) numArray[index];
        return intArray;
      }
    }
    return (int[]) null;
  }

  public uint[] ToUIntArray()
  {
    if (this.m_value == null)
      return (uint[]) null;
    if (this.m_value.GetType().IsArray)
    {
      if (this.m_value is uint[])
        return this.m_value as uint[];
      if (this.m_value is byte[])
      {
        byte[] numArray = this.m_value as byte[];
        if (numArray.Length % 4 != 0)
          return (uint[]) null;
        int length = numArray.Length / 4;
        uint[] uintArray = new uint[length];
        int startIndex = 0;
        for (int index = 0; index < length; ++index)
        {
          uint uint32 = BitConverter.ToUInt32(numArray, startIndex);
          uintArray[index] = uint32;
          startIndex += 4;
        }
        return uintArray;
      }
      if (this.m_value is short[])
      {
        short[] numArray = this.m_value as short[];
        uint[] uintArray = new uint[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          uintArray[index] = (uint) numArray[index];
        return uintArray;
      }
      if (this.m_value is ushort[])
      {
        ushort[] numArray = this.m_value as ushort[];
        uint[] uintArray = new uint[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          uintArray[index] = (uint) numArray[index];
        return uintArray;
      }
      if (this.m_value is int[])
      {
        int[] numArray = this.m_value as int[];
        uint[] uintArray = new uint[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          uintArray[index] = (uint) numArray[index];
        return uintArray;
      }
    }
    return (uint[]) null;
  }

  public float[] ToFloatArray()
  {
    if (this.m_value == null)
      return (float[]) null;
    if (this.m_value.GetType().IsArray)
    {
      if (this.m_value is float[])
        return this.m_value as float[];
      if (this.m_value is double[])
      {
        double[] numArray = this.m_value as double[];
        float[] floatArray = new float[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          floatArray[index] = (float) numArray[index];
        return floatArray;
      }
      if (this.m_value is byte[])
      {
        byte[] numArray = this.m_value as byte[];
        if (numArray.Length % 4 != 0)
          return (float[]) null;
        int startIndex = 0;
        int length = numArray.Length / 4;
        float[] floatArray = new float[length];
        for (int index = 0; index < length; ++index)
        {
          float single = BitConverter.ToSingle(numArray, startIndex);
          floatArray[index] = single;
          startIndex += 4;
        }
        return floatArray;
      }
    }
    return (float[]) null;
  }

  public double[] ToDoubleArray()
  {
    if (this.m_value == null)
      return (double[]) null;
    if (this.m_value.GetType().IsArray)
    {
      if (this.m_value is double[])
        return this.m_value as double[];
      if (this.m_value is float[])
      {
        float[] numArray = this.m_value as float[];
        double[] doubleArray = new double[numArray.Length];
        for (int index = 0; index < numArray.Length; ++index)
          doubleArray[index] = (double) numArray[index];
        return doubleArray;
      }
      if (this.m_value is byte[])
      {
        byte[] numArray = this.m_value as byte[];
        if (numArray.Length % 8 != 0)
          return (double[]) null;
        int startIndex = 0;
        int length = numArray.Length / 8;
        double[] doubleArray = new double[length];
        for (int index = 0; index < length; ++index)
        {
          double num = BitConverter.ToDouble(numArray, startIndex);
          doubleArray[index] = num;
          startIndex += 8;
        }
        return doubleArray;
      }
    }
    return (double[]) null;
  }
}
