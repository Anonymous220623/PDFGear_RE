// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.GrammarSpelling
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class GrammarSpelling
{
  private byte[] m_plcfsplData;
  private byte[] m_plcfgramData;
  private List<int> m_gramPositions;
  private List<int> m_spellPositions;

  internal GrammarSpelling()
  {
  }

  internal GrammarSpelling(Fib fib, Stream stream, CharPosTableRecord hfCharPosTable)
  {
    int fcLcb97LcbPlcfSpl = (int) fib.FibRgFcLcb97LcbPlcfSpl;
    int lcb97LcbPlcfGram = (int) fib.FibRgFcLcb97LcbPlcfGram;
    this.m_plcfsplData = new byte[fcLcb97LcbPlcfSpl];
    this.m_plcfgramData = new byte[lcb97LcbPlcfGram];
    this.MakeCorrection(hfCharPosTable, fib, stream);
    stream.Position = (long) fib.FibRgFcLcb97FcPlcfSpl;
    stream.Read(this.m_plcfsplData, 0, fcLcb97LcbPlcfSpl);
    stream.Position = (long) fib.FibRgFcLcb97FcPlcfGram;
    stream.Read(this.m_plcfgramData, 0, lcb97LcbPlcfGram);
  }

  internal byte[] PlcfsplData
  {
    get => this.m_plcfsplData;
    set => this.m_plcfsplData = value;
  }

  internal byte[] PlcfgramData
  {
    get => this.m_plcfgramData;
    set => this.m_plcfgramData = value;
  }

  internal void Write(Fib fib, Stream stream)
  {
    if (this.m_plcfgramData == null || this.m_plcfsplData == null)
      return;
    fib.FibRgFcLcb97FcPlcfSpl = (uint) stream.Position;
    stream.Write(this.m_plcfsplData, 0, this.m_plcfsplData.Length);
    fib.FibRgFcLcb97LcbPlcfSpl = (uint) this.m_plcfsplData.Length;
    fib.FibRgFcLcb97FcPlcfGram = (uint) stream.Position;
    stream.Write(this.m_plcfgramData, 0, this.m_plcfgramData.Length);
    fib.FibRgFcLcb97LcbPlcfGram = (uint) this.m_plcfgramData.Length;
  }

  internal void GetPositions(Fib fib, Stream stream)
  {
    BinaryReader binaryReader = new BinaryReader(stream);
    if (this.m_plcfsplData.Length > 0)
    {
      this.m_spellPositions = new List<int>();
      int num = (this.m_plcfsplData.Length + 2) / 6;
      binaryReader.BaseStream.Position = (long) fib.FibRgFcLcb97FcPlcfSpl;
      for (int index = 0; index < num; ++index)
        this.m_spellPositions.Add(binaryReader.ReadInt32());
    }
    if (this.m_plcfgramData.Length <= 0)
      return;
    this.m_gramPositions = new List<int>();
    int num1 = (this.m_plcfgramData.Length + 2) / 6;
    binaryReader.BaseStream.Position = (long) fib.FibRgFcLcb97FcPlcfGram;
    for (int index = 0; index < num1; ++index)
      this.m_gramPositions.Add(binaryReader.ReadInt32());
  }

  private void MakeCorrection(CharPosTableRecord hfCharPosTable, Fib fib, Stream stream)
  {
    if (fib.CcpHdd <= 0 || hfCharPosTable == null)
      return;
    this.GetPositions(fib, stream);
    if (!this.MakeHeaderCorrection(hfCharPosTable, fib))
      return;
    this.UpdateGramSpellData(stream, fib);
  }

  private bool MakeHeaderCorrection(CharPosTableRecord hfCharPosTable, Fib fib)
  {
    bool flag = false;
    if (fib.CcpHdd > 0 && hfCharPosTable != null)
    {
      int startHeaderCP = fib.CcpText + fib.CcpFtn;
      int position = hfCharPosTable.Positions[6];
      int startShiftCP = startHeaderCP + position;
      if (this.m_gramPositions != null)
        flag = this.ShiftHFPos(true, startHeaderCP, startShiftCP, position);
      if (this.m_spellPositions != null && flag)
        flag = this.ShiftHFPos(false, startHeaderCP, startShiftCP, position);
    }
    return flag;
  }

  private bool ShiftHFPos(bool isGrammar, int startHeaderCP, int startShiftCP, int shiftValue)
  {
    int posIndex1 = this.GetPosIndex(isGrammar, startHeaderCP);
    int posIndex2 = this.GetPosIndex(isGrammar, startShiftCP);
    if (posIndex1 == int.MaxValue || posIndex2 == int.MaxValue)
      return false;
    int num = posIndex2 + 1;
    this.SetHFSeparatorsPos(startHeaderCP, posIndex1, num, isGrammar);
    this.ShiftPositions(num, shiftValue, isGrammar);
    return true;
  }

  private void SetHFSeparatorsPos(int value, int startIndex, int endIndex, bool isGrammar)
  {
    List<int> intList = isGrammar ? this.m_gramPositions : this.m_spellPositions;
    for (int index = startIndex; index < endIndex; ++index)
      intList[index] = value;
  }

  private int GetPosIndex(bool isGrammarArray, int charPos)
  {
    List<int> intList = isGrammarArray ? this.m_gramPositions : this.m_spellPositions;
    int posIndex = int.MaxValue;
    int index = 0;
    for (int count = intList.Count; index < count; ++index)
    {
      if (intList[index] >= charPos)
      {
        posIndex = index;
        break;
      }
    }
    return posIndex;
  }

  private void ShiftPositions(int startIndex, int shiftValue, bool isGrammarArray)
  {
    List<int> intList = isGrammarArray ? this.m_gramPositions : this.m_spellPositions;
    int index = startIndex;
    for (int count = intList.Count; index < count; ++index)
      intList[index] -= shiftValue;
  }

  private void UpdateGramSpellData(Stream stream, Fib fib)
  {
    BinaryWriter binaryWriter = new BinaryWriter(stream);
    if (this.m_spellPositions != null)
    {
      binaryWriter.BaseStream.Position = (long) fib.FibRgFcLcb97FcPlcfSpl;
      int index = 0;
      for (int count = this.m_spellPositions.Count; index < count; ++index)
        binaryWriter.Write(this.m_spellPositions[index]);
    }
    if (this.m_gramPositions == null)
      return;
    binaryWriter.BaseStream.Position = (long) fib.FibRgFcLcb97FcPlcfGram;
    int index1 = 0;
    for (int count = this.m_gramPositions.Count; index1 < count; ++index1)
      binaryWriter.Write(this.m_gramPositions[index1]);
  }

  internal void Close()
  {
    if (this.m_plcfsplData != null)
      this.m_plcfsplData = (byte[]) null;
    if (this.m_plcfgramData != null)
      this.m_plcfgramData = (byte[]) null;
    if (this.m_gramPositions != null)
    {
      this.m_gramPositions.Clear();
      this.m_gramPositions = (List<int>) null;
    }
    if (this.m_spellPositions == null)
      return;
    this.m_spellPositions.Clear();
    this.m_spellPositions = (List<int>) null;
  }
}
