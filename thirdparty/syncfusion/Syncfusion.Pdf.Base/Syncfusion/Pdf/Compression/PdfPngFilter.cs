// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.PdfPngFilter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class PdfPngFilter
{
  private const byte m_zero = 0;
  private static PdfPngFilter.RowFilter s_subFilter = new PdfPngFilter.RowFilter(PdfPngFilter.CompressSub);
  private static PdfPngFilter.RowFilter s_upFilter = new PdfPngFilter.RowFilter(PdfPngFilter.CompressUp);
  private static PdfPngFilter.RowFilter s_averageFilter = new PdfPngFilter.RowFilter(PdfPngFilter.CompressAverage);
  private static PdfPngFilter.RowFilter s_paethFilter = new PdfPngFilter.RowFilter(PdfPngFilter.CompressPaeth);
  private static PdfPngFilter.RowFilter s_decompressFilter = new PdfPngFilter.RowFilter(PdfPngFilter.Decompress);

  public static byte[] Compress(byte[] data, int bpr, PdfPngFilter.Type type)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (bpr <= 0)
      throw new ArgumentException("There can't be less or equal to zero bytes in a line.", nameof (bpr));
    PdfPngFilter.RowFilter filter;
    switch (type)
    {
      case PdfPngFilter.Type.None:
        return data;
      case PdfPngFilter.Type.Sub:
        filter = PdfPngFilter.s_subFilter;
        break;
      case PdfPngFilter.Type.Up:
        filter = PdfPngFilter.s_upFilter;
        break;
      case PdfPngFilter.Type.Average:
        filter = PdfPngFilter.s_averageFilter;
        break;
      case PdfPngFilter.Type.Paeth:
        filter = PdfPngFilter.s_paethFilter;
        break;
      default:
        throw new ArgumentException("Unsupported PNG filter: " + type.ToString(), nameof (type));
    }
    return PdfPngFilter.Modify(data, bpr, filter, true);
  }

  public static byte[] Decompress(byte[] data, int bpr)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (bpr <= 0)
      throw new ArgumentException("There can't be less or equal to zero bytes in a line.", nameof (bpr));
    return PdfPngFilter.Modify(data, bpr + 1, PdfPngFilter.s_decompressFilter, false);
  }

  private static byte[] Modify(byte[] data, int bpr, PdfPngFilter.RowFilter filter, bool pack)
  {
    long inIndex = 0;
    long length = (long) data.Length;
    long num = length / (long) bpr;
    int resBPR = bpr - (pack ? -1 : 1);
    byte[] result = new byte[pack ? checked ((IntPtr) unchecked (num * (long) resBPR)) : checked ((IntPtr) unchecked (num * (long) resBPR))];
    int resIndex = 0;
    for (; inIndex + (long) bpr <= length; inIndex += (long) bpr)
    {
      filter(data, inIndex, bpr, result, (long) resIndex, resBPR);
      resIndex += resBPR;
    }
    return result;
  }

  private static void CompressSub(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    result[resIndex] = (byte) 1;
    ++resIndex;
    for (int index = 0; index < resBPR; ++index)
    {
      result[(IntPtr) resIndex] = (byte) ((int) data[inIndex] - (index > 0 ? (int) data[inIndex - 1L] : 0));
      ++resIndex;
      ++inIndex;
    }
  }

  private static void CompressUp(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    long index1 = inIndex - (long) inBPR;
    result[resIndex] = (byte) 2;
    ++resIndex;
    for (int index2 = 0; index2 < inBPR; ++index2)
    {
      result[(IntPtr) resIndex] = (byte) ((int) data[inIndex] - (index1 < 0L ? 0 : (int) data[index1]));
      ++resIndex;
      ++inIndex;
      ++index1;
    }
  }

  private static void CompressAverage(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    long index1 = inIndex - (long) inBPR;
    result[resIndex] = (byte) 3;
    ++resIndex;
    for (int index2 = 0; index2 < inBPR; ++index2)
    {
      result[(IntPtr) resIndex] = (byte) ((int) data[inIndex] - ((index2 > 0 ? (int) data[inIndex - 1L] : 0) + (index1 < 0L ? 0 : (int) data[index1])) >> 1);
      ++resIndex;
      ++inIndex;
      ++index1;
    }
  }

  private static void CompressPaeth(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    long index1 = inIndex - (long) inBPR;
    result[resIndex] = (byte) 3;
    ++resIndex;
    for (int index2 = 0; index2 < inBPR; ++index2)
    {
      byte a = index2 > 0 ? data[inIndex - 1L] : (byte) 0;
      byte b = index1 < 0L ? (byte) 0 : data[index1];
      byte c = index1 < 1L ? (byte) 0 : data[index1 - 1L];
      result[resIndex] = (byte) ((uint) data[inIndex] - (uint) PdfPngFilter.PaethPredictor((int) a, (int) b, (int) c));
      ++resIndex;
      ++inIndex;
      ++index1;
    }
  }

  private static void Decompress(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    switch (data[inIndex])
    {
      case 0:
        PdfPngFilter.DecompressNone(data, inIndex + 1L, inBPR, result, resIndex, resBPR);
        break;
      case 1:
        PdfPngFilter.DeompressSub(data, inIndex + 1L, inBPR, result, resIndex, resBPR);
        break;
      case 2:
        PdfPngFilter.DecompressUp(data, inIndex + 1L, inBPR, result, resIndex, resBPR);
        break;
      case 3:
        PdfPngFilter.DecompressAverage(data, inIndex + 1L, inBPR, result, resIndex, resBPR);
        break;
      case 4:
        PdfPngFilter.DecompressPaeth(data, inIndex + 1L, inBPR, result, resIndex, resBPR);
        break;
      default:
        throw new ArgumentException("Unsupported PNG filter: " + data[inIndex].ToString(), "type");
    }
  }

  private static void DecompressNone(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    for (int index = 1; index < inBPR; ++index)
    {
      result[resIndex] = data[inIndex];
      ++resIndex;
      ++inIndex;
    }
  }

  private static void DeompressSub(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    for (int index = 0; index < resBPR; ++index)
    {
      result[(IntPtr) resIndex] = (byte) ((int) data[inIndex] + (index > 0 ? (int) result[resIndex - 1L] : 0));
      ++resIndex;
      ++inIndex;
    }
  }

  private static byte[] DecompressUp(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    long index1 = resIndex - (long) resBPR;
    for (int index2 = 0; index2 < resBPR; ++index2)
    {
      result[(IntPtr) resIndex] = (byte) ((int) data[inIndex] + (index1 < 0L ? 0 : (int) result[index1]));
      ++resIndex;
      ++inIndex;
      ++index1;
    }
    return result;
  }

  private static void DecompressAverage(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    int num = 1;
    long index1 = resIndex - (long) resBPR;
    byte[] numArray = new byte[resBPR];
    for (int index2 = 0; index2 < resBPR; ++index2)
      result[resIndex + (long) index2] = data[inIndex + (long) index2];
    for (int index3 = 0; index3 < 1; ++index3)
    {
      result[resIndex] = index1 >= 0L ? (byte) ((uint) data[inIndex] + (uint) result[index1] / 2U) : (byte) ((uint) data[inIndex] + (uint) numArray[resIndex]);
      ++index1;
      ++resIndex;
    }
    for (int index4 = num; index4 < resBPR; ++index4)
    {
      if (index1 < 0L)
        result[resIndex] += (byte) ((((int) result[resIndex - (long) num] & (int) byte.MaxValue) + ((int) numArray[resIndex] & (int) byte.MaxValue)) / 2);
      else
        result[resIndex] += (byte) ((((int) result[resIndex - (long) num] & (int) byte.MaxValue) + ((int) result[index1] & (int) byte.MaxValue)) / 2);
      ++resIndex;
      ++inIndex;
      ++index1;
    }
  }

  private static void DecompressPaeth(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR)
  {
    int num = 1;
    long index1 = resIndex - (long) resBPR;
    for (int index2 = 0; index2 < resBPR; ++index2)
      result[resIndex + (long) index2] = data[inIndex + (long) index2];
    for (int index3 = 0; index3 < num; ++index3)
    {
      result[resIndex] += result[index1];
      ++resIndex;
      ++index1;
    }
    for (int index4 = num; index4 < resBPR; ++index4)
    {
      int a = (int) result[resIndex - (long) num] & (int) byte.MaxValue;
      int b = (int) result[index1] & (int) byte.MaxValue;
      int c = (int) result[index1 - (long) num] & (int) byte.MaxValue;
      result[resIndex] += PdfPngFilter.PaethPredictor(a, b, c);
      ++resIndex;
      ++inIndex;
      ++index1;
    }
  }

  private static byte PaethPredictor(int a, int b, int c)
  {
    int num1 = a + b - c;
    int num2 = Math.Abs(num1 - a);
    int num3 = Math.Abs(num1 - b);
    int num4 = Math.Abs(num1 - c);
    if (num2 <= num3 && num2 <= num4)
      return (byte) a;
    return num3 <= num4 ? (byte) b : (byte) c;
  }

  internal enum Type
  {
    None,
    Sub,
    Up,
    Average,
    Paeth,
  }

  private delegate void RowFilter(
    byte[] data,
    long inIndex,
    int inBPR,
    byte[] result,
    long resIndex,
    int resBPR);
}
