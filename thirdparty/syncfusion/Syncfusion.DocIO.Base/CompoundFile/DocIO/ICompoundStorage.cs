// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.ICompoundStorage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO;

public interface ICompoundStorage : IDisposable
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
