// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.Constants
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;

#nullable disable
namespace Syncfusion.Compression.Zip;

[CLSCompliant(false)]
public sealed class Constants
{
  public const int HeaderSignature = 67324752;
  public const int HeaderSignatureBytes = 4;
  public const int BufferSize = 4096 /*0x1000*/;
  public const short VersionNeededToExtract = 20;
  public const short VersionMadeBy = 45;
  public const int ShortSize = 2;
  public const int IntSize = 4;
  public const int CentralHeaderSignature = 33639248;
  public const int CentralDirectoryEndSignature = 101010256;
  public const uint StartCrc = 4294967295 /*0xFFFFFFFF*/;
  public const int CentralDirSizeOffset = 12;
  public const int HeaderSignatureStartByteValue = 80 /*0x50*/;

  private Constants()
  {
  }
}
