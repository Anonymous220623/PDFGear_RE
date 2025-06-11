// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.ICompoundStorage
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation;

internal interface ICompoundStorage : IDisposable
{
  CompoundStream CreateStream(string streamName);

  CompoundStream OpenStream(string streamName);

  void DeleteStream(string streamName);

  bool ContainsStream(string streamName);

  ICompoundStorage CreateStorage(string storageName);

  ICompoundStorage OpenStorage(string storageName);

  void DeleteStorage(string storageName);

  bool ContainsStorage(string storageName);

  void Flush();

  string[] Streams { get; }

  string[] Storages { get; }

  string Name { get; }

  void InsertCopy(ICompoundStorage storageToCopy);

  void InsertCopy(CompoundStream streamToCopy);
}
