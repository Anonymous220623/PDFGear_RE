// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfFileInformationHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace PDFKit.Utils;

public static class PdfFileInformationHelper
{
  private static readonly byte[] PdfHeaderStartWith = new byte[5]
  {
    (byte) 37,
    (byte) 80 /*0x50*/,
    (byte) 68,
    (byte) 70,
    (byte) 45
  };

  public static async Task<PdfFileInformationHelper.PdfVersion?> GetPdfVersionAsync(
    Stream stream,
    CancellationToken cancellationToken)
  {
    if (stream.CanRead && stream.CanSeek && stream.Position != 0L)
      stream.Seek(0L, SeekOrigin.Begin);
    byte[] buffer = ArrayPool<byte>.Shared.Rent(16 /*0x10*/);
    try
    {
      int count = await stream.ReadAsync(buffer, 0, 16 /*0x10*/, cancellationToken);
      if (count >= 8)
      {
        if (buffer.AsSpan<byte>().StartsWith<byte>((ReadOnlySpan<byte>) PdfFileInformationHelper.PdfHeaderStartWith))
        {
          int majorVersion = 0;
          int minorVersion = 0;
          for (int i = PdfFileInformationHelper.PdfHeaderStartWith.Length; i < count; ++i)
          {
            char ch = (char) buffer[i];
            if (ch >= '0' && ch <= '9')
              minorVersion = minorVersion * 10 + ((int) ch - 48 /*0x30*/);
            else if (ch == '.')
            {
              majorVersion = minorVersion;
              minorVersion = 0;
            }
            else
            {
              if (ch != '\n' && ch != '\r')
                return new PdfFileInformationHelper.PdfVersion?();
              break;
            }
          }
          if (majorVersion == 0)
            return new PdfFileInformationHelper.PdfVersion?();
          return new PdfFileInformationHelper.PdfVersion?(new PdfFileInformationHelper.PdfVersion()
          {
            Major = majorVersion,
            Minor = minorVersion
          });
        }
      }
    }
    catch
    {
    }
    finally
    {
      ArrayPool<byte>.Shared.Return(buffer);
    }
    return new PdfFileInformationHelper.PdfVersion?();
  }

  public struct PdfVersion
  {
    public int Major;
    public int Minor;

    public override string ToString() => $"{this.Major}.{this.Minor}";

    public static implicit operator Version(PdfFileInformationHelper.PdfVersion version)
    {
      return new Version(version.Major, version.Minor);
    }
  }
}
