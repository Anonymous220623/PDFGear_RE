// Decompiled with JetBrains decompiler
// Type: QRCoder.QRCodeData
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using QRCoder.Framework4._0Methods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

#nullable disable
namespace QRCoder;

public class QRCodeData : IDisposable
{
  public List<BitArray> ModuleMatrix { get; set; }

  public QRCodeData(int version)
  {
    this.Version = version;
    int length = QRCodeData.ModulesPerSideFromVersion(version);
    this.ModuleMatrix = new List<BitArray>();
    for (int index = 0; index < length; ++index)
      this.ModuleMatrix.Add(new BitArray(length));
  }

  public QRCodeData(string pathToRawData, QRCodeData.Compression compressMode)
    : this(File.ReadAllBytes(pathToRawData), compressMode)
  {
  }

  public QRCodeData(byte[] rawData, QRCodeData.Compression compressMode)
  {
    List<byte> byteList = new List<byte>((IEnumerable<byte>) rawData);
    switch (compressMode)
    {
      case QRCodeData.Compression.Deflate:
        using (MemoryStream memoryStream = new MemoryStream(byteList.ToArray()))
        {
          using (MemoryStream output = new MemoryStream())
          {
            using (DeflateStream input = new DeflateStream((Stream) memoryStream, CompressionMode.Decompress))
              Stream4Methods.CopyTo((Stream) input, (Stream) output);
            byteList = new List<byte>((IEnumerable<byte>) output.ToArray());
            break;
          }
        }
      case QRCodeData.Compression.GZip:
        using (MemoryStream memoryStream = new MemoryStream(byteList.ToArray()))
        {
          using (MemoryStream output = new MemoryStream())
          {
            using (GZipStream input = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
              Stream4Methods.CopyTo((Stream) input, (Stream) output);
            byteList = new List<byte>((IEnumerable<byte>) output.ToArray());
            break;
          }
        }
    }
    int num1 = byteList[0] == (byte) 81 && byteList[1] == (byte) 82 && byteList[2] == (byte) 82 ? (int) byteList[4] : throw new Exception("Invalid raw data file. Filetype doesn't match \"QRR\".");
    byteList.RemoveRange(0, 5);
    this.Version = (num1 - 21 - 8) / 4 + 1;
    Queue<bool> boolQueue = new Queue<bool>(8 * byteList.Count);
    foreach (byte num2 in byteList)
    {
      BitArray bitArray = new BitArray(new byte[1]{ num2 });
      for (int index = 7; index >= 0; --index)
        boolQueue.Enqueue(((uint) num2 & (uint) (1 << index)) > 0U);
    }
    this.ModuleMatrix = new List<BitArray>(num1);
    for (int index1 = 0; index1 < num1; ++index1)
    {
      this.ModuleMatrix.Add(new BitArray(num1));
      for (int index2 = 0; index2 < num1; ++index2)
        this.ModuleMatrix[index1][index2] = boolQueue.Dequeue();
    }
  }

  public byte[] GetRawData(QRCodeData.Compression compressMode)
  {
    List<byte> byteList = new List<byte>();
    byteList.AddRange((IEnumerable<byte>) new byte[4]
    {
      (byte) 81,
      (byte) 82,
      (byte) 82,
      (byte) 0
    });
    byteList.Add((byte) this.ModuleMatrix.Count);
    Queue<int> intQueue = new Queue<int>();
    foreach (BitArray bitArray in this.ModuleMatrix)
    {
      foreach (object obj in bitArray)
        intQueue.Enqueue((bool) obj ? 1 : 0);
    }
    for (int index = 0; index < 8 - this.ModuleMatrix.Count * this.ModuleMatrix.Count % 8; ++index)
      intQueue.Enqueue(0);
    while (intQueue.Count > 0)
    {
      byte num = 0;
      for (int index = 7; index >= 0; --index)
        num += (byte) (intQueue.Dequeue() << index);
      byteList.Add(num);
    }
    byte[] array = byteList.ToArray();
    switch (compressMode)
    {
      case QRCodeData.Compression.Deflate:
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (DeflateStream deflateStream = new DeflateStream((Stream) memoryStream, CompressionMode.Compress))
            deflateStream.Write(array, 0, array.Length);
          array = memoryStream.ToArray();
          break;
        }
      case QRCodeData.Compression.GZip:
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress, true))
            gzipStream.Write(array, 0, array.Length);
          array = memoryStream.ToArray();
          break;
        }
    }
    return array;
  }

  public void SaveRawData(string filePath, QRCodeData.Compression compressMode)
  {
    File.WriteAllBytes(filePath, this.GetRawData(compressMode));
  }

  public int Version { get; private set; }

  private static int ModulesPerSideFromVersion(int version) => 21 + (version - 1) * 4;

  public void Dispose()
  {
    this.ModuleMatrix = (List<BitArray>) null;
    this.Version = 0;
  }

  public enum Compression
  {
    Uncompressed,
    Deflate,
    GZip,
  }
}
