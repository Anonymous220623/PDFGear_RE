// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BiffRecordPosAttribute
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[AttributeUsage(AttributeTargets.Field)]
public sealed class BiffRecordPosAttribute : Attribute, IComparable
{
  private int m_iPos;
  private int m_iSize;
  private bool m_bIsBit;
  private bool m_bIsString;
  private bool m_bIsString16Bit;
  private bool m_bIsOEMString;
  private bool m_bIsOEMString16Bit;
  private bool m_bIsFloat;
  private bool m_bSigned;

  public int Position => this.m_iPos;

  public int SizeOrBitPosition => this.m_iSize;

  public bool IsBit => this.m_bIsBit;

  public bool IsSigned => this.m_bSigned;

  public bool IsString => this.m_bIsString;

  public bool IsString16Bit => this.m_bIsString16Bit;

  public bool IsFloat => this.m_bIsFloat;

  public bool IsOEMString => this.m_bIsOEMString;

  public bool IsOEMString16Bit => this.m_bIsOEMString16Bit;

  public BiffRecordPosAttribute(int pos, int size, bool isSigned, TFieldType type)
  {
    this.m_iPos = pos;
    this.m_iSize = size;
    this.m_bSigned = isSigned;
    this.m_bIsBit = type == TFieldType.Bit;
    this.m_bIsString = type == TFieldType.String;
    this.m_bIsString16Bit = type == TFieldType.String16Bit;
    this.m_bIsOEMString = type == TFieldType.OEMString;
    this.m_bIsOEMString16Bit = type == TFieldType.OEMString16Bit;
    this.m_bIsFloat = type == TFieldType.Float;
  }

  public BiffRecordPosAttribute(int pos, int size, bool isSigned)
    : this(pos, size, isSigned, TFieldType.Integer)
  {
  }

  public BiffRecordPosAttribute(int pos, int size, TFieldType type)
    : this(pos, size, false, type)
  {
  }

  public BiffRecordPosAttribute(int pos, TFieldType type)
    : this(pos, 0, false, type)
  {
  }

  public BiffRecordPosAttribute(int pos, int size)
    : this(pos, size, false)
  {
  }

  public int CompareTo(object obj)
  {
    return new RecordsPosComparer().Compare(this, obj as BiffRecordPosAttribute);
  }
}
