// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfNumber
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfNumber : IPdfPrimitive
{
  private int m_intValue;
  private float m_floatValue;
  private bool m_isInteger;
  private bool m_IsLong;
  private ObjectStatus m_status;
  private bool m_isSaving;
  private int m_index;
  private int m_position = -1;
  private long m_longValue;

  internal long LongValue
  {
    get => this.m_longValue;
    set
    {
      this.m_isInteger = false;
      this.m_IsLong = true;
      this.m_longValue = value;
      this.m_intValue = (int) value;
      this.m_floatValue = (float) value;
    }
  }

  public int IntValue
  {
    get => this.m_intValue;
    set
    {
      this.m_isInteger = true;
      this.m_intValue = value;
      this.m_longValue = (long) value;
      this.m_floatValue = (float) value;
    }
  }

  public float FloatValue
  {
    get => this.m_floatValue;
    set
    {
      this.m_isInteger = false;
      this.m_floatValue = value;
      this.m_intValue = (int) value;
    }
  }

  public bool IsInteger
  {
    get => this.m_isInteger;
    set => this.m_isInteger = value;
  }

  internal bool IsLong
  {
    get => this.m_IsLong;
    set => this.m_IsLong = value;
  }

  public ObjectStatus Status
  {
    get => this.m_status;
    set => this.m_status = value;
  }

  public bool IsSaving
  {
    get => this.m_isSaving;
    set => this.m_isSaving = value;
  }

  public int ObjectCollectionIndex
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public IPdfPrimitive ClonedObject => (IPdfPrimitive) null;

  internal PdfNumber(int value) => this.IntValue = value;

  internal PdfNumber(long value) => this.LongValue = value;

  internal PdfNumber(float value) => this.FloatValue = value;

  internal PdfNumber(double value) => this.FloatValue = (float) value;

  public static string FloatToString(float number)
  {
    return number.ToString("######################.00######", (IFormatProvider) CultureInfo.InvariantCulture);
  }

  public static float Min(float x, float y, float z)
  {
    float val2 = Math.Min(x, y);
    return Math.Min(z, val2);
  }

  public static float Max(float x, float y, float z)
  {
    float val2 = Math.Max(x, y);
    return Math.Max(z, val2);
  }

  public void Save(IPdfWriter writer)
  {
    if (this.IsInteger)
      writer.Write(this.IntValue.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    else if (this.IsLong)
      writer.Write(this.LongValue.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    else if ((double) this.FloatValue - Math.Truncate((double) this.FloatValue) == 0.0)
      writer.Write(this.IntValue.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    else
      writer.Write(PdfNumber.FloatToString(this.FloatValue));
  }

  public IPdfPrimitive Clone(PdfCrossTable crossTable)
  {
    return !this.IsInteger ? (!this.IsLong ? (IPdfPrimitive) new PdfNumber(this.FloatValue) : (IPdfPrimitive) new PdfNumber(this.LongValue)) : (IPdfPrimitive) new PdfNumber(this.IntValue);
  }
}
