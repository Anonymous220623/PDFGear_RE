// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ZipArchiveFileCompressor
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.IO;
using System.IO.Compression;

#nullable disable
namespace NLog.Targets;

internal class ZipArchiveFileCompressor : IFileCompressor
{
  public void CompressFile(string fileName, string archiveFileName)
  {
    using (FileStream fileStream1 = new FileStream(archiveFileName, FileMode.Create))
    {
      using (ZipArchive zipArchive = new ZipArchive((Stream) fileStream1, ZipArchiveMode.Create))
      {
        using (FileStream fileStream2 = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          using (Stream destination = zipArchive.CreateEntry(Path.GetFileName(fileName)).Open())
            fileStream2.CopyTo(destination);
        }
      }
    }
  }
}
