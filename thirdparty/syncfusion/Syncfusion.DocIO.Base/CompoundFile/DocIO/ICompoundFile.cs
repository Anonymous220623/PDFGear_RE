// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.ICompoundFile
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO;

public interface ICompoundFile : IDisposable
{
  ICompoundStorage RootStorage { get; }

  Syncfusion.CompoundFile.DocIO.Net.Directory Directory { get; }

  void Flush();

  void Save(Stream stream);

  void Save(string fileName);
}
