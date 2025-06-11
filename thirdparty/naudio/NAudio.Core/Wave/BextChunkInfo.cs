// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.BextChunkInfo
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class BextChunkInfo
{
  public BextChunkInfo() => this.Reserved = new byte[190];

  public string Description { get; set; }

  public string Originator { get; set; }

  public string OriginatorReference { get; set; }

  public DateTime OriginationDateTime { get; set; }

  public string OriginationDate => this.OriginationDateTime.ToString("yyyy-MM-dd");

  public string OriginationTime => this.OriginationDateTime.ToString("HH:mm:ss");

  public long TimeReference { get; set; }

  public ushort Version => 1;

  public string UniqueMaterialIdentifier { get; set; }

  public byte[] Reserved { get; }

  public string CodingHistory { get; set; }
}
