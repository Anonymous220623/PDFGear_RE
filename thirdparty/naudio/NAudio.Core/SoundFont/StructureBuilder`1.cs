// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.StructureBuilder`1
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NAudio.SoundFont;

internal abstract class StructureBuilder<T>
{
  protected List<T> data;

  public StructureBuilder() => this.Reset();

  public abstract T Read(BinaryReader br);

  public abstract void Write(BinaryWriter bw, T o);

  public abstract int Length { get; }

  public void Reset() => this.data = new List<T>();

  public T[] Data => this.data.ToArray();
}
