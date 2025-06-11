// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.CompoundFileException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

[Serializable]
internal class CompoundFileException : Exception
{
  private const string DefaultExceptionMessage = "Unable to parse compound file. Wrong file format.";

  public CompoundFileException()
    : base("Unable to parse compound file. Wrong file format.")
  {
  }

  public CompoundFileException(string message)
    : base(message)
  {
  }
}
