// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.ICompoundFile
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation;

internal interface ICompoundFile : IDisposable
{
  ICompoundStorage RootStorage { get; }

  void Flush();

  void Save(Stream stream);

  void Save(string fileName);
}
