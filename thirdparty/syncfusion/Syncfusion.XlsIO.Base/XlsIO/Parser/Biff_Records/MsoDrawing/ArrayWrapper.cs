// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.ArrayWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

public class ArrayWrapper
{
  private byte[] m_arrBuffer;
  private int m_iHash;

  private ArrayWrapper()
  {
  }

  public ArrayWrapper(byte[] arrBuffer)
  {
    this.m_arrBuffer = arrBuffer != null ? arrBuffer : throw new ArgumentNullException(nameof (arrBuffer));
    this.EvaluateHash();
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    byte[] array2 = (byte[]) null;
    if (obj is ArrayWrapper)
      array2 = ((ArrayWrapper) obj).m_arrBuffer;
    else if (obj is byte[])
      array2 = (byte[]) obj;
    return array2 != null && BiffRecordRaw.CompareArrays(this.m_arrBuffer, array2);
  }

  public override int GetHashCode() => this.m_iHash;

  private void EvaluateHash()
  {
    int num1 = this.m_arrBuffer.Length / 4;
    this.m_iHash = 0;
    int num2 = 0;
    int startIndex = 0;
    while (num2 < num1)
    {
      this.m_iHash |= BitConverter.ToInt32(this.m_arrBuffer, startIndex);
      ++num2;
      startIndex += 4;
    }
  }
}
