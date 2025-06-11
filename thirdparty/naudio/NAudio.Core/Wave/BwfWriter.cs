// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.BwfWriter
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace NAudio.Wave;

public class BwfWriter : IDisposable
{
  private readonly WaveFormat format;
  private readonly BinaryWriter writer;
  private readonly long dataChunkSizePosition;
  private long dataLength;
  private bool isDisposed;

  public BwfWriter(string filename, WaveFormat format, BextChunkInfo bextChunkInfo)
  {
    this.format = format;
    this.writer = new BinaryWriter((Stream) File.OpenWrite(filename));
    this.writer.Write(Encoding.UTF8.GetBytes("RIFF"));
    this.writer.Write(0);
    this.writer.Write(Encoding.UTF8.GetBytes("WAVE"));
    this.writer.Write(Encoding.UTF8.GetBytes("JUNK"));
    this.writer.Write(28);
    this.writer.Write(0L);
    this.writer.Write(0L);
    this.writer.Write(0L);
    this.writer.Write(0);
    this.writer.Write(Encoding.UTF8.GetBytes("bext"));
    byte[] bytes = Encoding.ASCII.GetBytes(bextChunkInfo.CodingHistory ?? "");
    int num = 602 + bytes.Length;
    if (num % 2 != 0)
      ++num;
    this.writer.Write(num);
    long position = this.writer.BaseStream.Position;
    this.writer.Write(BwfWriter.GetAsBytes(bextChunkInfo.Description, 256 /*0x0100*/));
    this.writer.Write(BwfWriter.GetAsBytes(bextChunkInfo.Originator, 32 /*0x20*/));
    this.writer.Write(BwfWriter.GetAsBytes(bextChunkInfo.OriginatorReference, 32 /*0x20*/));
    this.writer.Write(BwfWriter.GetAsBytes(bextChunkInfo.OriginationDate, 10));
    this.writer.Write(BwfWriter.GetAsBytes(bextChunkInfo.OriginationTime, 8));
    this.writer.Write(bextChunkInfo.TimeReference);
    this.writer.Write(bextChunkInfo.Version);
    this.writer.Write(BwfWriter.GetAsBytes(bextChunkInfo.UniqueMaterialIdentifier, 64 /*0x40*/));
    this.writer.Write(bextChunkInfo.Reserved);
    this.writer.Write(bytes);
    if (bytes.Length % 2 != 0)
      this.writer.Write((byte) 0);
    this.writer.Write(Encoding.UTF8.GetBytes("fmt "));
    format.Serialize(this.writer);
    this.writer.Write(Encoding.UTF8.GetBytes("data"));
    this.dataChunkSizePosition = this.writer.BaseStream.Position;
    this.writer.Write(-1);
  }

  public void Write(byte[] buffer, int offset, int count)
  {
    if (this.isDisposed)
      throw new ObjectDisposedException("This BWF Writer already disposed");
    this.writer.Write(buffer, offset, count);
    this.dataLength += (long) count;
  }

  public void Flush()
  {
    if (this.isDisposed)
      throw new ObjectDisposedException("This BWF Writer already disposed");
    this.writer.Flush();
    this.FixUpChunkSizes(true);
  }

  private void FixUpChunkSizes(bool restorePosition)
  {
    long position = this.writer.BaseStream.Position;
    int num1 = this.dataLength > (long) int.MaxValue ? 1 : 0;
    long num2 = this.writer.BaseStream.Length - 8L;
    if (num1 != 0)
    {
      int num3 = this.format.BitsPerSample / 8 * this.format.Channels;
      this.writer.BaseStream.Position = 0L;
      this.writer.Write(Encoding.UTF8.GetBytes("RF64"));
      this.writer.Write(-1);
      this.writer.BaseStream.Position += 4L;
      this.writer.Write(Encoding.UTF8.GetBytes("ds64"));
      this.writer.BaseStream.Position += 4L;
      this.writer.Write(num2);
      this.writer.Write(this.dataLength);
      this.writer.Write(this.dataLength / (long) num3);
    }
    else
    {
      this.writer.BaseStream.Position = 4L;
      this.writer.Write((uint) num2);
      this.writer.BaseStream.Position = this.dataChunkSizePosition;
      this.writer.Write((uint) this.dataLength);
    }
    if (!restorePosition)
      return;
    this.writer.BaseStream.Position = position;
  }

  public void Dispose()
  {
    if (this.isDisposed)
      return;
    this.FixUpChunkSizes(false);
    this.writer.Dispose();
    this.isDisposed = true;
  }

  private static byte[] GetAsBytes(string message, int byteSize)
  {
    byte[] destinationArray = new byte[byteSize];
    byte[] bytes = Encoding.ASCII.GetBytes(message ?? "");
    Array.Copy((Array) bytes, (Array) destinationArray, Math.Min(bytes.Length, byteSize));
    return destinationArray;
  }
}
