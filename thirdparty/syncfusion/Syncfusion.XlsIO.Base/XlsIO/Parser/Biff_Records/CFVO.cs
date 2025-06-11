// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFVO
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class CFVO
{
  private const ushort DEF_MINIMUM_SIZE = 3;
  private byte m_cfvoType = 1;
  private ushort m_formulaLength;
  private byte[] m_arrFormula = new byte[0];
  private Ptg[] m_arrFormulaParsed;
  private double m_numValue;
  private string m_value;
  internal Ptg[] ref3DPtg;

  public ConditionValueType CFVOType
  {
    get => (ConditionValueType) this.m_cfvoType;
    set => this.m_cfvoType = (byte) value;
  }

  public ushort FormulaSize => this.m_formulaLength;

  public Ptg[] FormulaPtgs
  {
    get => this.m_arrFormulaParsed;
    set
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(value, ExcelVersion.Excel2007);
      this.m_arrFormulaParsed = value;
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
  }

  public byte[] FormulaBytes => this.m_arrFormula;

  public double NumValue
  {
    get => this.m_numValue;
    set => this.m_numValue = value;
  }

  public string Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  internal Ptg[] RefPtg
  {
    get => this.ref3DPtg;
    set => this.ref3DPtg = value;
  }

  public int ParseCFVO(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_cfvoType = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_formulaLength = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_arrFormula = new byte[(int) this.m_formulaLength];
    provider.ReadArray(iOffset, this.m_arrFormula);
    iOffset += (int) this.m_formulaLength;
    this.m_arrFormulaParsed = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(this.m_arrFormula), (int) this.m_formulaLength, version);
    if (version == ExcelVersion.Excel97to2003 && this.m_formulaLength > (ushort) 0)
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFormulaParsed, ExcelVersion.Excel2007);
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
    if (this.CFVOType == ConditionValueType.Number || this.CFVOType == ConditionValueType.Percent || this.CFVOType == ConditionValueType.Percentile)
    {
      if (this.m_formulaLength <= (ushort) 0)
      {
        this.m_value = provider.ReadDouble(iOffset).ToString();
        iOffset += 8;
      }
      else if (this.m_arrFormulaParsed.Length > 0 && this.m_arrFormulaParsed[0].GetType().Name != typeof (StringConstantPtg).Name)
      {
        if (this.m_arrFormulaParsed[0].GetType().Name == typeof (Ref3DPtg).Name)
          this.RefPtg = this.m_arrFormulaParsed;
        this.m_value = this.m_arrFormulaParsed[0].ToString();
      }
    }
    else
      this.m_value = "0";
    return iOffset;
  }

  public int SerializeCFVO(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (this.m_arrFormulaParsed != null && this.m_arrFormulaParsed.Length > 0)
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFormulaParsed, version);
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
    else if (this.RefPtg != null)
    {
      this.m_arrFormulaParsed = this.RefPtg;
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFormulaParsed, version);
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
    else
    {
      this.m_arrFormula = (byte[]) null;
      this.m_formulaLength = (ushort) 0;
    }
    provider.WriteByte(iOffset, this.m_cfvoType);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_formulaLength);
    iOffset += 2;
    provider.WriteBytes(iOffset, this.m_arrFormula, 0, (int) this.m_formulaLength);
    iOffset += (int) this.m_formulaLength;
    if ((this.CFVOType == ConditionValueType.Number || this.CFVOType == ConditionValueType.Percent || this.CFVOType == ConditionValueType.Percentile) && this.m_formulaLength <= (ushort) 0 && this.m_value != null && this.RefPtg == null)
    {
      double num = Convert.ToDouble(this.m_value);
      provider.WriteDouble(iOffset, num);
      iOffset += 8;
    }
    return iOffset;
  }

  public int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 3 + (int) this.m_formulaLength;
    if (this.CFVOType == ConditionValueType.Number || this.CFVOType == ConditionValueType.Percent || this.CFVOType == ConditionValueType.Percentile || this.m_formulaLength < (ushort) 0)
      storeSize += 8;
    return storeSize;
  }

  internal void ClearAll()
  {
    this.m_arrFormula = (byte[]) null;
    this.m_arrFormulaParsed = (Ptg[]) null;
    if (this.RefPtg == null)
      return;
    this.RefPtg = (Ptg[]) null;
  }
}
