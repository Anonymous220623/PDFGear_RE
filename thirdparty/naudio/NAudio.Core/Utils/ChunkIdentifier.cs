// Decompiled with JetBrains decompiler
// Type: NAudio.Utils.ChunkIdentifier
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Text;

#nullable disable
namespace NAudio.Utils;

public class ChunkIdentifier
{
  public static int ChunkIdentifierToInt32(string s)
  {
    byte[] numArray = s.Length == 4 ? Encoding.UTF8.GetBytes(s) : throw new ArgumentException("Must be a four character string");
    return numArray.Length == 4 ? BitConverter.ToInt32(numArray, 0) : throw new ArgumentException("Must encode to exactly four bytes");
  }
}
