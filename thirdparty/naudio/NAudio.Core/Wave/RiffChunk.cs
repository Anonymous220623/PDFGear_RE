// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.RiffChunk
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Text;

#nullable disable
namespace NAudio.Wave;

public class RiffChunk
{
  public RiffChunk(int identifier, int length, long streamPosition)
  {
    this.Identifier = identifier;
    this.Length = length;
    this.StreamPosition = streamPosition;
  }

  public int Identifier { get; }

  public string IdentifierAsString
  {
    get => Encoding.UTF8.GetString(BitConverter.GetBytes(this.Identifier));
  }

  public int Length { get; private set; }

  public long StreamPosition { get; private set; }
}
