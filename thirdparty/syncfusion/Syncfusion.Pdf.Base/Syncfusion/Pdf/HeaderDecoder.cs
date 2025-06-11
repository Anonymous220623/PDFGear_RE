// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HeaderDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class HeaderDecoder
{
  public const char OPT_PREFIX = 'H';
  private const int SIZ_FOUND = 1;
  private const int COD_FOUND = 2;
  private const int COC_FOUND = 4;
  private const int QCD_FOUND = 8;
  private const int TLM_FOUND = 16 /*0x10*/;
  private const int PLM_FOUND = 32 /*0x20*/;
  private const int SOT_FOUND = 64 /*0x40*/;
  private const int PLT_FOUND = 128 /*0x80*/;
  private const int QCC_FOUND = 256 /*0x0100*/;
  private const int RGN_FOUND = 512 /*0x0200*/;
  private const int POC_FOUND = 1024 /*0x0400*/;
  private const int COM_FOUND = 2048 /*0x0800*/;
  public const int SOD_FOUND = 8192 /*0x2000*/;
  public const int PPM_FOUND = 16384 /*0x4000*/;
  public const int PPT_FOUND = 32768 /*0x8000*/;
  public const int CRG_FOUND = 65536 /*0x010000*/;
  private static readonly string[][] pinfo;
  private HeaderInformation hi;
  private string hdStr = "";
  private int nTiles;
  public int[] nTileParts;
  private int nfMarkSeg;
  private int nCOCMarkSeg;
  private int nQCCMarkSeg;
  private int nCOMMarkSeg;
  private int nRGNMarkSeg;
  private int nPPMMarkSeg;
  private int[][] nPPTMarkSeg;
  private Dictionary<object, object> ht;
  private int nComp;
  private int cb0x = -1;
  private int cb0y = -1;
  private DecodeHelper decSpec;
  internal bool precinctPartitionIsUsed;
  public int mainHeadOff;
  private List<object> tileOfTileParts;
  private byte[][] pPMMarkerData;
  private byte[][][][] tilePartPkdPktHeaders;
  private MemoryStream[] pkdPktHeaders;

  public virtual int MaxCompImgHeight => this.hi.sizValue.MaxCompHeight;

  public virtual int MaxCompImgWidth => this.hi.sizValue.MaxCompWidth;

  public virtual int ImgWidth => this.hi.sizValue.xsiz - this.hi.sizValue.x0siz;

  public virtual int ImgHeight => this.hi.sizValue.ysiz - this.hi.sizValue.y0siz;

  public virtual int ImgULX => this.hi.sizValue.x0siz;

  public virtual int ImgULY => this.hi.sizValue.y0siz;

  public virtual int NomTileWidth => this.hi.sizValue.xtsiz;

  public virtual int NomTileHeight => this.hi.sizValue.ytsiz;

  public virtual int NumComps => this.nComp;

  public virtual int CbULX => this.cb0x;

  public virtual int CbULY => this.cb0y;

  public virtual DecodeHelper DecoderHelper => this.decSpec;

  public static string[][] ParameterInfo => HeaderDecoder.pinfo;

  public virtual int NumTiles => this.nTiles;

  public virtual int TileOfTileParts
  {
    set
    {
      if (this.nPPMMarkSeg == 0)
        return;
      this.tileOfTileParts.Add((object) value);
    }
  }

  public virtual int NumFoundMarkSeg => this.nfMarkSeg;

  public JPXImageCoordinates getTilingOrigin(JPXImageCoordinates co)
  {
    if (co == null)
      return new JPXImageCoordinates(this.hi.sizValue.xt0siz, this.hi.sizValue.yt0siz);
    co.x = this.hi.sizValue.xt0siz;
    co.y = this.hi.sizValue.yt0siz;
    return co;
  }

  public bool isOriginalSigned(int c) => this.hi.sizValue.isOrigSigned(c);

  public int GetActualBitDepth(int c) => this.hi.sizValue.getOrigBitDepth(c);

  public int getCompSubsX(int c) => this.hi.sizValue.xrsiz[c];

  public int getCompSubsY(int c) => this.hi.sizValue.yrsiz[c];

  public Dequantizer createDequantizer(CBlkQuantDataSrcDec src, int[] rb, DecodeHelper decSpec2)
  {
    return (Dequantizer) new StdDequantizer(src, rb, decSpec2);
  }

  public int getPPX(int t, int c, int rl) => this.decSpec.pss.getPPX(t, c, rl);

  public int getPPY(int t, int c, int rl) => this.decSpec.pss.getPPY(t, c, rl);

  public bool precinctPartitionUsed() => this.precinctPartitionIsUsed;

  private SynWTFilter readFilter(BinaryReader ehs, int[] filtIdx)
  {
    int num = filtIdx[0] = (int) ehs.ReadByte();
    if (num >= 128 /*0x80*/)
      throw new ArgumentException("Custom filters are used");
    switch (num)
    {
      case 0:
        return (SynWTFilter) new SynWTFilterFloatLift9x7();
      case 1:
        return (SynWTFilter) new SynWTFilterIntLift5x3();
      default:
        throw new Exception();
    }
  }

  public virtual void checkMarkerLength(BinaryReader ehs, string str)
  {
    long length = ehs.BaseStream.Length;
    long position = ehs.BaseStream.Position;
  }

  private void readSIZ(BinaryReader ehs)
  {
    HeaderInformation.SIZ newSiz = this.hi.NewSIZ;
    this.hi.sizValue = newSiz;
    newSiz.lsiz = (int) ehs.ReadUInt16();
    newSiz.rsiz = (int) ehs.ReadUInt16();
    int rsiz = newSiz.rsiz;
    newSiz.xsiz = ehs.ReadInt32();
    newSiz.ysiz = ehs.ReadInt32();
    if (newSiz.xsiz <= 0 || newSiz.ysiz <= 0)
      throw new IOException("JJ2000 does not support images whose width and/or height not in the range: 1 -- (2^31)-1");
    newSiz.x0siz = ehs.ReadInt32();
    newSiz.y0siz = ehs.ReadInt32();
    if (newSiz.x0siz < 0 || newSiz.y0siz < 0)
      throw new IOException("JJ2000 does not support images offset not in the range: 0 -- (2^31)-1");
    newSiz.xtsiz = ehs.ReadInt32();
    newSiz.ytsiz = ehs.ReadInt32();
    if (newSiz.xtsiz <= 0 || newSiz.ytsiz <= 0)
      throw new IOException("JJ2000 does not support tiles whose width and/or height are not in  the range: 1 -- (2^31)-1");
    newSiz.xt0siz = ehs.ReadInt32();
    newSiz.yt0siz = ehs.ReadInt32();
    if (newSiz.xt0siz < 0 || newSiz.yt0siz < 0)
      throw new IOException("JJ2000 does not support tiles whose offset is not in  the range: 0 -- (2^31)-1");
    this.nComp = newSiz.csiz = (int) ehs.ReadUInt16();
    newSiz.ssiz = this.nComp >= 1 && this.nComp <= 16384 /*0x4000*/ ? new int[this.nComp] : throw new ArgumentException("Number of component out of range 1--16384: " + (object) this.nComp);
    newSiz.xrsiz = new int[this.nComp];
    newSiz.yrsiz = new int[this.nComp];
    for (int index = 0; index < this.nComp; ++index)
    {
      newSiz.ssiz[index] = (int) ehs.ReadByte();
      newSiz.xrsiz[index] = (int) ehs.ReadByte();
      newSiz.yrsiz[index] = (int) ehs.ReadByte();
    }
    this.checkMarkerLength(ehs, "SIZ marker");
    this.nTiles = newSiz.NumTiles;
    this.decSpec = new DecodeHelper(this.nTiles, this.nComp);
  }

  private void readCRG(BinaryReader ehs)
  {
    HeaderInformation.CRG newCrg = this.hi.NewCRG;
    this.hi.crgValue = newCrg;
    newCrg.lcrg = (int) ehs.ReadUInt16();
    newCrg.xcrg = new int[this.nComp];
    newCrg.ycrg = new int[this.nComp];
    for (int index = 0; index < this.nComp; ++index)
    {
      newCrg.xcrg[index] = (int) ehs.ReadUInt16();
      newCrg.ycrg[index] = (int) ehs.ReadUInt16();
    }
    this.checkMarkerLength(ehs, "CRG marker");
  }

  private void readCOM(BinaryReader ehs, bool mainh, int tileIdx, int comIdx)
  {
    HeaderInformation.COM newCom = this.hi.NewCOM;
    newCom.lcom = (int) ehs.ReadUInt16();
    newCom.rcom = (int) ehs.ReadUInt16();
    if (newCom.rcom == 1)
    {
      newCom.ccom = new byte[newCom.lcom - 4];
      for (int index = 0; index < newCom.lcom - 4; ++index)
        newCom.ccom[index] = ehs.ReadByte();
    }
    else
    {
      BinaryReader binaryReader = ehs;
      long position = binaryReader.BaseStream.Position;
      long num = binaryReader.BaseStream.Seek((long) (newCom.lcom - 4), SeekOrigin.Current) - position;
    }
    if (mainh)
      this.hi.comValue[(object) ("main_" + (object) comIdx)] = (object) newCom;
    else
      this.hi.comValue[(object) $"t{(object) tileIdx}_{(object) comIdx}"] = (object) newCom;
    this.checkMarkerLength(ehs, "COM marker");
  }

  private void readQCD(BinaryReader ehs, bool mainh, int tileIdx, int tpIdx)
  {
    HeaderInformation.QCD newQcd = this.hi.NewQCD;
    newQcd.lqcd = (int) ehs.ReadUInt16();
    newQcd.sqcd = (int) ehs.ReadByte();
    int numGuardBits = newQcd.NumGuardBits;
    int quantType = newQcd.QuantType;
    if (mainh)
    {
      this.hi.qcdValue[(object) "main"] = (object) newQcd;
      switch (quantType)
      {
        case 0:
          this.decSpec.qts.setDefault((object) "reversible");
          break;
        case 1:
          this.decSpec.qts.setDefault((object) "derived");
          break;
        case 2:
          this.decSpec.qts.setDefault((object) "expounded");
          break;
      }
    }
    else
    {
      this.hi.qcdValue[(object) ("t" + (object) tileIdx)] = (object) newQcd;
      switch (quantType)
      {
        case 0:
          this.decSpec.qts.setTileDef(tileIdx, (object) "reversible");
          break;
        case 1:
          this.decSpec.qts.setTileDef(tileIdx, (object) "derived");
          break;
        case 2:
          this.decSpec.qts.setTileDef(tileIdx, (object) "expounded");
          break;
      }
    }
    StdDequantizerParams value_Renamed = new StdDequantizerParams();
    if (quantType == 0)
    {
      int num1 = mainh ? (int) this.decSpec.dls.getDefault() : (int) this.decSpec.dls.getTileDef(tileIdx);
      int[][] numArray1 = value_Renamed.exp = new int[num1 + 1][];
      int[][] numArray2 = new int[num1 + 1][];
      for (int index = 0; index < num1 + 1; ++index)
        numArray2[index] = new int[4];
      newQcd.spqcd = numArray2;
      for (int index1 = 0; index1 <= num1; ++index1)
      {
        int num2;
        int length;
        if (index1 == 0)
        {
          num2 = 0;
          length = 1;
        }
        else
        {
          int num3 = 1;
          int num4 = num3 <= num1 - index1 ? 1 : num3 - (num1 - index1);
          num2 = 1 << (num4 - 1 << 1);
          length = 1 << (num4 << 1);
        }
        numArray1[index1] = new int[length];
        for (int index2 = num2; index2 < length; ++index2)
        {
          int num5 = newQcd.spqcd[index1][index2] = (int) ehs.ReadByte();
          numArray1[index1][index2] = num5 >> 3 & 31 /*0x1F*/;
        }
      }
    }
    else
    {
      int num6 = quantType == 1 ? 0 : (mainh ? (int) this.decSpec.dls.getDefault() : (int) this.decSpec.dls.getTileDef(tileIdx));
      int[][] numArray3 = value_Renamed.exp = new int[num6 + 1][];
      float[][] numArray4 = value_Renamed.nStep = new float[num6 + 1][];
      int[][] numArray5 = new int[num6 + 1][];
      for (int index = 0; index < num6 + 1; ++index)
        numArray5[index] = new int[4];
      newQcd.spqcd = numArray5;
      for (int index3 = 0; index3 <= num6; ++index3)
      {
        int num7;
        int length;
        if (index3 == 0)
        {
          num7 = 0;
          length = 1;
        }
        else
        {
          int num8 = 1;
          int num9 = num8 <= num6 - index3 ? 1 : num8 - (num6 - index3);
          num7 = 1 << (num9 - 1 << 1);
          length = 1 << (num9 << 1);
        }
        numArray3[index3] = new int[length];
        numArray4[index3] = new float[length];
        for (int index4 = num7; index4 < length; ++index4)
        {
          int num10 = newQcd.spqcd[index3][index4] = (int) ehs.ReadUInt16();
          numArray3[index3][index4] = num10 >> 11 & 31 /*0x1F*/;
          numArray4[index3][index4] = (float) (-1.0 - (double) (num10 & 2047 /*0x07FF*/) / 2048.0) / (float) (-1 << numArray3[index3][index4]);
        }
      }
    }
    if (mainh)
    {
      this.decSpec.qsss.setDefault((object) value_Renamed);
      this.decSpec.gbs.setDefault((object) numGuardBits);
    }
    else
    {
      this.decSpec.qsss.setTileDef(tileIdx, (object) value_Renamed);
      this.decSpec.gbs.setTileDef(tileIdx, (object) numGuardBits);
    }
    this.checkMarkerLength(ehs, "QCD marker");
  }

  private void readQCC(BinaryReader ehs, bool mainh, int tileIdx, int tpIdx)
  {
    HeaderInformation.QCC newQcc = this.hi.NewQCC;
    newQcc.lqcc = (int) ehs.ReadUInt16();
    int c = this.nComp >= 257 ? (newQcc.cqcc = (int) ehs.ReadUInt16()) : (newQcc.cqcc = (int) ehs.ReadByte());
    if (c >= this.nComp)
      throw new Exception();
    newQcc.sqcc = (int) ehs.ReadByte();
    int numGuardBits = newQcc.NumGuardBits;
    int quantType = newQcc.QuantType;
    if (mainh)
    {
      this.hi.qccValue[(object) ("main_c" + (object) c)] = (object) newQcc;
      switch (quantType)
      {
        case 0:
          this.decSpec.qts.setCompDef(c, (object) "reversible");
          break;
        case 1:
          this.decSpec.qts.setCompDef(c, (object) "derived");
          break;
        case 2:
          this.decSpec.qts.setCompDef(c, (object) "expounded");
          break;
        default:
          throw new Exception();
      }
    }
    else
    {
      this.hi.qccValue[(object) $"t{(object) tileIdx}_c{(object) c}"] = (object) newQcc;
      switch (quantType)
      {
        case 0:
          this.decSpec.qts.setTileCompVal(tileIdx, c, (object) "reversible");
          break;
        case 1:
          this.decSpec.qts.setTileCompVal(tileIdx, c, (object) "derived");
          break;
        case 2:
          this.decSpec.qts.setTileCompVal(tileIdx, c, (object) "expounded");
          break;
        default:
          throw new Exception();
      }
    }
    StdDequantizerParams value_Renamed = new StdDequantizerParams();
    int num1;
    switch (quantType)
    {
      case 0:
        int num2 = mainh ? (int) this.decSpec.dls.getCompDef(c) : (int) this.decSpec.dls.getTileCompVal(tileIdx, c);
        int[][] numArray1 = value_Renamed.exp = new int[num2 + 1][];
        int[][] numArray2 = new int[num2 + 1][];
        for (int index = 0; index < num2 + 1; ++index)
          numArray2[index] = new int[4];
        newQcc.spqcc = numArray2;
        for (int index1 = 0; index1 <= num2; ++index1)
        {
          int num3;
          int length;
          if (index1 == 0)
          {
            num3 = 0;
            length = 1;
          }
          else
          {
            int num4 = 1;
            int num5 = num4 <= num2 - index1 ? 1 : num4 - (num2 - index1);
            num3 = 1 << (num5 - 1 << 1);
            length = 1 << (num5 << 1);
          }
          numArray1[index1] = new int[length];
          for (int index2 = num3; index2 < length; ++index2)
          {
            int num6 = newQcc.spqcc[index1][index2] = (int) ehs.ReadByte();
            numArray1[index1][index2] = num6 >> 3 & 31 /*0x1F*/;
          }
        }
        goto label_40;
      case 1:
        num1 = 0;
        break;
      default:
        num1 = mainh ? (int) this.decSpec.dls.getCompDef(c) : (int) this.decSpec.dls.getTileCompVal(tileIdx, c);
        break;
    }
    int num7 = num1;
    float[][] numArray3 = value_Renamed.nStep = new float[num7 + 1][];
    int[][] numArray4 = value_Renamed.exp = new int[num7 + 1][];
    int[][] numArray5 = new int[num7 + 1][];
    for (int index = 0; index < num7 + 1; ++index)
      numArray5[index] = new int[4];
    newQcc.spqcc = numArray5;
    for (int index3 = 0; index3 <= num7; ++index3)
    {
      int num8;
      int length;
      if (index3 == 0)
      {
        num8 = 0;
        length = 1;
      }
      else
      {
        int num9 = 1;
        int num10 = num9 <= num7 - index3 ? 1 : num9 - (num7 - index3);
        num8 = 1 << (num10 - 1 << 1);
        length = 1 << (num10 << 1);
      }
      numArray4[index3] = new int[length];
      numArray3[index3] = new float[length];
      for (int index4 = num8; index4 < length; ++index4)
      {
        int num11 = newQcc.spqcc[index3][index4] = (int) ehs.ReadUInt16();
        numArray4[index3][index4] = num11 >> 11 & 31 /*0x1F*/;
        numArray3[index3][index4] = (float) (-1.0 - (double) (num11 & 2047 /*0x07FF*/) / 2048.0) / (float) (-1 << numArray4[index3][index4]);
      }
    }
label_40:
    if (mainh)
    {
      this.decSpec.qsss.setCompDef(c, (object) value_Renamed);
      this.decSpec.gbs.setCompDef(c, (object) numGuardBits);
    }
    else
    {
      this.decSpec.qsss.setTileCompVal(tileIdx, c, (object) value_Renamed);
      this.decSpec.gbs.setTileCompVal(tileIdx, c, (object) numGuardBits);
    }
    this.checkMarkerLength(ehs, "QCC marker");
  }

  private void readCOD(BinaryReader ehs, bool mainh, int tileIdx, int tpIdx)
  {
    HeaderInformation.COD newCod = this.hi.NewCOD;
    newCod.lcod = (int) ehs.ReadUInt16();
    int num1 = newCod.scod = (int) ehs.ReadByte();
    if ((num1 & 1) != 0)
    {
      this.precinctPartitionIsUsed = true;
      num1 &= -2;
    }
    else
      this.precinctPartitionIsUsed = false;
    if (mainh)
    {
      this.hi.codValue[(object) "main"] = (object) newCod;
      if ((num1 & 2) != 0)
      {
        this.decSpec.sops.setDefault((object) "true".ToUpper().Equals("TRUE"));
        num1 &= -3;
      }
      else
        this.decSpec.sops.setDefault((object) "false".ToUpper().Equals("TRUE"));
    }
    else
    {
      this.hi.codValue[(object) ("t" + (object) tileIdx)] = (object) newCod;
      if ((num1 & 2) != 0)
      {
        this.decSpec.sops.setTileDef(tileIdx, (object) "true".ToUpper().Equals("TRUE"));
        num1 &= -3;
      }
      else
        this.decSpec.sops.setTileDef(tileIdx, (object) "false".ToUpper().Equals("TRUE"));
    }
    if (mainh)
    {
      if ((num1 & 4) != 0)
      {
        this.decSpec.ephs.setDefault((object) "true".ToUpper().Equals("TRUE"));
        num1 &= -5;
      }
      else
        this.decSpec.ephs.setDefault((object) "false".ToUpper().Equals("TRUE"));
    }
    else if ((num1 & 4) != 0)
    {
      this.decSpec.ephs.setTileDef(tileIdx, (object) "true".ToUpper().Equals("TRUE"));
      num1 &= -5;
    }
    else
      this.decSpec.ephs.setTileDef(tileIdx, (object) "false".ToUpper().Equals("TRUE"));
    int num2 = num1 & 24;
    if ((num1 & 8) != 0)
    {
      this.cb0x = this.cb0x == -1 || this.cb0x != 0 ? 1 : throw new ArgumentException("Code-block partition origin redefined in new COD marker segment. Not supported by JJ2000");
      num1 &= -9;
    }
    else
      this.cb0x = this.cb0x == -1 || this.cb0x != 1 ? 0 : throw new ArgumentException("Code-block partition origin redefined in new COD marker segment. Not supported by JJ2000");
    if ((num1 & 16 /*0x10*/) != 0)
    {
      this.cb0y = this.cb0y == -1 || this.cb0y != 0 ? 1 : throw new ArgumentException("Code-block partition origin redefined in new COD marker segment. Not supported by JJ2000");
      int num3 = num1 & -17;
    }
    else
      this.cb0y = this.cb0y == -1 || this.cb0y != 1 ? 0 : throw new ArgumentException("Code-block partition origin redefined in new COD marker segment. Not supported by JJ2000");
    newCod.sgcod_po = (int) ehs.ReadByte();
    newCod.sgcod_nl = (int) ehs.ReadUInt16();
    if (newCod.sgcod_nl <= 0 || newCod.sgcod_nl > (int) ushort.MaxValue)
      throw new ArgumentException("Number of layers out of range: 1--65535");
    newCod.sgcod_mct = (int) ehs.ReadByte();
    int value_Renamed1 = newCod.spcod_ndl = (int) ehs.ReadByte();
    if (value_Renamed1 > 32 /*0x20*/)
      throw new ArgumentException("Number of decomposition levels out of range: 0--32");
    int[] value_Renamed2 = new int[2];
    newCod.spcod_cw = (int) ehs.ReadByte();
    value_Renamed2[0] = 1 << newCod.spcod_cw + 2;
    if (value_Renamed2[0] < StdEntropyCoderOptions.MIN_CB_DIM || value_Renamed2[0] > StdEntropyCoderOptions.MAX_CB_DIM)
      throw new ArgumentException("Non-valid code-block width in SPcod field, COD marker");
    newCod.spcod_ch = (int) ehs.ReadByte();
    value_Renamed2[1] = 1 << newCod.spcod_ch + 2;
    if (value_Renamed2[1] < StdEntropyCoderOptions.MIN_CB_DIM || value_Renamed2[1] > StdEntropyCoderOptions.MAX_CB_DIM)
      throw new ArgumentException("Non-valid code-block height in SPcod field, COD marker");
    if (value_Renamed2[0] * value_Renamed2[1] > StdEntropyCoderOptions.MAX_CB_AREA)
      throw new ArgumentException("Non-valid code-block area in SPcod field, COD marker");
    if (mainh)
      this.decSpec.cblks.setDefault((object) value_Renamed2);
    else
      this.decSpec.cblks.setTileDef(tileIdx, (object) value_Renamed2);
    int value_Renamed3 = newCod.spcod_cs = (int) ehs.ReadByte();
    if ((value_Renamed3 & ~(StdEntropyCoderOptions.OPT_BYPASS | StdEntropyCoderOptions.OPT_RESET_MQ | StdEntropyCoderOptions.OPT_TERM_PASS | StdEntropyCoderOptions.OPT_VERT_STR_CAUSAL | StdEntropyCoderOptions.OPT_PRED_TERM | StdEntropyCoderOptions.OPT_SEG_SYMBOLS)) != 0)
      throw new ArgumentException("Unknown \"code-block style\" in SPcod field, COD marker: 0x" + Convert.ToString(value_Renamed3, 16 /*0x10*/));
    SynWTFilter[] synWtFilterArray1 = new SynWTFilter[1];
    SynWTFilter[] synWtFilterArray2 = new SynWTFilter[1];
    synWtFilterArray1[0] = this.readFilter(ehs, newCod.spcod_t);
    synWtFilterArray2[0] = synWtFilterArray1[0];
    SynWTFilter[][] value_Renamed4 = new SynWTFilter[2][]
    {
      synWtFilterArray1,
      synWtFilterArray2
    };
    List<object>[] value_Renamed5 = new List<object>[2]
    {
      new List<object>(10),
      new List<object>(10)
    };
    int maxValue = (int) ushort.MaxValue;
    if (!this.precinctPartitionIsUsed)
    {
      int num4 = 1 << (maxValue & 15);
      value_Renamed5[0].Add((object) num4);
      int num5 = 1 << ((maxValue & 240 /*0xF0*/) >> 4);
      value_Renamed5[1].Add((object) num5);
    }
    else
    {
      newCod.spcod_ps = new int[value_Renamed1 + 1];
      for (int index = value_Renamed1; index >= 0; --index)
      {
        int num6 = newCod.spcod_ps[value_Renamed1 - index] = (int) ehs.ReadByte();
        int num7 = 1 << (num6 & 15);
        value_Renamed5[0].Insert(0, (object) num7);
        int num8 = 1 << ((num6 & 240 /*0xF0*/) >> 4);
        value_Renamed5[1].Insert(0, (object) num8);
      }
    }
    if (mainh)
      this.decSpec.pss.setDefault((object) value_Renamed5);
    else
      this.decSpec.pss.setTileDef(tileIdx, (object) value_Renamed5);
    this.precinctPartitionIsUsed = true;
    this.checkMarkerLength(ehs, "COD marker");
    if (mainh)
    {
      this.decSpec.wfs.setDefault((object) value_Renamed4);
      this.decSpec.dls.setDefault((object) value_Renamed1);
      this.decSpec.ecopts.setDefault((object) value_Renamed3);
      this.decSpec.cts.setDefault((object) newCod.sgcod_mct);
      this.decSpec.nls.setDefault((object) newCod.sgcod_nl);
      this.decSpec.pos.setDefault((object) newCod.sgcod_po);
    }
    else
    {
      this.decSpec.wfs.setTileDef(tileIdx, (object) value_Renamed4);
      this.decSpec.dls.setTileDef(tileIdx, (object) value_Renamed1);
      this.decSpec.ecopts.setTileDef(tileIdx, (object) value_Renamed3);
      this.decSpec.cts.setTileDef(tileIdx, (object) newCod.sgcod_mct);
      this.decSpec.nls.setTileDef(tileIdx, (object) newCod.sgcod_nl);
      this.decSpec.pos.setTileDef(tileIdx, (object) newCod.sgcod_po);
    }
  }

  private void readCOC(BinaryReader ehs, bool mainh, int tileIdx, int tpIdx)
  {
    HeaderInformation.COC newCoc = this.hi.NewCOC;
    newCoc.lcoc = (int) ehs.ReadUInt16();
    int c = this.nComp >= 257 ? (newCoc.ccoc = (int) ehs.ReadUInt16()) : (newCoc.ccoc = (int) ehs.ReadByte());
    if (c >= this.nComp)
      throw new ArgumentException("Invalid component index in QCC marker");
    int num1 = newCoc.scoc = (int) ehs.ReadByte();
    if ((num1 & 1) != 0)
    {
      this.precinctPartitionIsUsed = true;
      int num2 = num1 & -2;
    }
    else
      this.precinctPartitionIsUsed = false;
    int value_Renamed1 = newCoc.spcoc_ndl = (int) ehs.ReadByte();
    int[] value_Renamed2 = new int[2];
    newCoc.spcoc_cw = (int) ehs.ReadByte();
    value_Renamed2[0] = 1 << newCoc.spcoc_cw + 2;
    if (value_Renamed2[0] < StdEntropyCoderOptions.MIN_CB_DIM || value_Renamed2[0] > StdEntropyCoderOptions.MAX_CB_DIM)
      throw new ArgumentException("Non-valid code-block width in SPcod field, COC marker");
    newCoc.spcoc_ch = (int) ehs.ReadByte();
    value_Renamed2[1] = 1 << newCoc.spcoc_ch + 2;
    if (value_Renamed2[1] < StdEntropyCoderOptions.MIN_CB_DIM || value_Renamed2[1] > StdEntropyCoderOptions.MAX_CB_DIM)
      throw new ArgumentException("Non-valid code-block height in SPcod field, COC marker");
    if (value_Renamed2[0] * value_Renamed2[1] > StdEntropyCoderOptions.MAX_CB_AREA)
      throw new ArgumentException("Non-valid code-block area in SPcod field, COC marker");
    if (mainh)
      this.decSpec.cblks.setCompDef(c, (object) value_Renamed2);
    else
      this.decSpec.cblks.setTileCompVal(tileIdx, c, (object) value_Renamed2);
    int value_Renamed3 = newCoc.spcoc_cs = (int) ehs.ReadByte();
    if ((value_Renamed3 & ~(StdEntropyCoderOptions.OPT_BYPASS | StdEntropyCoderOptions.OPT_RESET_MQ | StdEntropyCoderOptions.OPT_TERM_PASS | StdEntropyCoderOptions.OPT_VERT_STR_CAUSAL | StdEntropyCoderOptions.OPT_PRED_TERM | StdEntropyCoderOptions.OPT_SEG_SYMBOLS)) != 0)
      throw new ArgumentException("Unknown \"code-block context\" in SPcoc field, COC marker: 0x" + Convert.ToString(value_Renamed3, 16 /*0x10*/));
    SynWTFilter[] synWtFilterArray1 = new SynWTFilter[1];
    SynWTFilter[] synWtFilterArray2 = new SynWTFilter[1];
    synWtFilterArray1[0] = this.readFilter(ehs, newCoc.spcoc_t);
    synWtFilterArray2[0] = synWtFilterArray1[0];
    SynWTFilter[][] value_Renamed4 = new SynWTFilter[2][]
    {
      synWtFilterArray1,
      synWtFilterArray2
    };
    List<object>[] value_Renamed5 = new List<object>[2]
    {
      new List<object>(10),
      new List<object>(10)
    };
    int maxValue = (int) ushort.MaxValue;
    if (!this.precinctPartitionIsUsed)
    {
      int num3 = 1 << (maxValue & 15);
      value_Renamed5[0].Add((object) num3);
      int num4 = 1 << ((maxValue & 240 /*0xF0*/) >> 4);
      value_Renamed5[1].Add((object) num4);
    }
    else
    {
      newCoc.spcoc_ps = new int[value_Renamed1 + 1];
      for (int index = value_Renamed1; index >= 0; --index)
      {
        int num5 = newCoc.spcoc_ps[index] = (int) ehs.ReadByte();
        int num6 = 1 << (num5 & 15);
        value_Renamed5[0].Insert(0, (object) num6);
        int num7 = 1 << ((num5 & 240 /*0xF0*/) >> 4);
        value_Renamed5[1].Insert(0, (object) num7);
      }
    }
    if (mainh)
      this.decSpec.pss.setCompDef(c, (object) value_Renamed5);
    else
      this.decSpec.pss.setTileCompVal(tileIdx, c, (object) value_Renamed5);
    this.precinctPartitionIsUsed = true;
    this.checkMarkerLength(ehs, "COD marker");
    if (mainh)
    {
      this.hi.cocValue[(object) ("main_c" + (object) c)] = (object) newCoc;
      this.decSpec.wfs.setCompDef(c, (object) value_Renamed4);
      this.decSpec.dls.setCompDef(c, (object) value_Renamed1);
      this.decSpec.ecopts.setCompDef(c, (object) value_Renamed3);
    }
    else
    {
      this.hi.cocValue[(object) $"t{(object) tileIdx}_c{(object) c}"] = (object) newCoc;
      this.decSpec.wfs.setTileCompVal(tileIdx, c, (object) value_Renamed4);
      this.decSpec.dls.setTileCompVal(tileIdx, c, (object) value_Renamed1);
      this.decSpec.ecopts.setTileCompVal(tileIdx, c, (object) value_Renamed3);
    }
  }

  private void readPOC(BinaryReader ehs, bool mainh, int t, int tpIdx)
  {
    bool flag = this.nComp >= 256 /*0x0100*/;
    int num1 = 0;
    HeaderInformation.POC newPoc;
    if (mainh || this.hi.pocValue[(object) (nameof (t) + (object) t)] == null)
    {
      newPoc = this.hi.NewPOC;
    }
    else
    {
      newPoc = (HeaderInformation.POC) this.hi.pocValue[(object) (nameof (t) + (object) t)];
      num1 = newPoc.rspoc.Length;
    }
    newPoc.lpoc = (int) ehs.ReadUInt16();
    int length1 = (newPoc.lpoc - 2) / (5 + (flag ? 4 : 2));
    int length2 = num1 + length1;
    int[][] value_Renamed;
    if (num1 != 0)
    {
      int[][] numArray1 = new int[length2][];
      for (int index = 0; index < length2; ++index)
        numArray1[index] = new int[6];
      value_Renamed = numArray1;
      int[] numArray2 = new int[length2];
      int[] numArray3 = new int[length2];
      int[] numArray4 = new int[length2];
      int[] numArray5 = new int[length2];
      int[] numArray6 = new int[length2];
      int[] numArray7 = new int[length2];
      int[][] tileDef = (int[][]) this.decSpec.pcs.getTileDef(t);
      for (int index = 0; index < num1; ++index)
      {
        value_Renamed[index] = tileDef[index];
        numArray2[index] = newPoc.rspoc[index];
        numArray3[index] = newPoc.cspoc[index];
        numArray4[index] = newPoc.lyepoc[index];
        numArray5[index] = newPoc.repoc[index];
        numArray6[index] = newPoc.cepoc[index];
        numArray7[index] = newPoc.ppoc[index];
      }
      newPoc.rspoc = numArray2;
      newPoc.cspoc = numArray3;
      newPoc.lyepoc = numArray4;
      newPoc.repoc = numArray5;
      newPoc.cepoc = numArray6;
      newPoc.ppoc = numArray7;
    }
    else
    {
      int[][] numArray = new int[length1][];
      for (int index = 0; index < length1; ++index)
        numArray[index] = new int[6];
      value_Renamed = numArray;
      newPoc.rspoc = new int[length1];
      newPoc.cspoc = new int[length1];
      newPoc.lyepoc = new int[length1];
      newPoc.repoc = new int[length1];
      newPoc.cepoc = new int[length1];
      newPoc.ppoc = new int[length1];
    }
    for (int index = num1; index < length2; ++index)
    {
      value_Renamed[index][0] = newPoc.rspoc[index] = (int) ehs.ReadByte();
      value_Renamed[index][1] = !flag ? (newPoc.cspoc[index] = (int) ehs.ReadByte()) : (newPoc.cspoc[index] = (int) ehs.ReadUInt16());
      value_Renamed[index][2] = newPoc.lyepoc[index] = (int) ehs.ReadUInt16();
      if (value_Renamed[index][2] < 1)
        throw new ArgumentException($"LYEpoc value must be greater than 1 in POC marker segment of tile {(object) t}, tile-part {(object) tpIdx}");
      value_Renamed[index][3] = newPoc.repoc[index] = (int) ehs.ReadByte();
      if (value_Renamed[index][3] <= value_Renamed[index][0])
        throw new ArgumentException($"REpoc value must be greater than RSpoc in POC marker segment of tile {(object) t}, tile-part {(object) tpIdx}");
      if (flag)
      {
        value_Renamed[index][4] = newPoc.cepoc[index] = (int) ehs.ReadUInt16();
      }
      else
      {
        int num2 = newPoc.cepoc[index] = (int) ehs.ReadByte();
        value_Renamed[index][4] = num2 != 0 ? num2 : 0;
      }
      if (value_Renamed[index][4] <= value_Renamed[index][1])
        throw new ArgumentException($"CEpoc value must be greater than CSpoc in POC marker segment of tile {(object) t}, tile-part {(object) tpIdx}");
      value_Renamed[index][5] = newPoc.ppoc[index] = (int) ehs.ReadByte();
    }
    this.checkMarkerLength(ehs, "POC marker");
    if (mainh)
    {
      this.hi.pocValue[(object) "main"] = (object) newPoc;
      this.decSpec.pcs.setDefault((object) value_Renamed);
    }
    else
    {
      this.hi.pocValue[(object) (nameof (t) + (object) t)] = (object) newPoc;
      this.decSpec.pcs.setTileDef(t, (object) value_Renamed);
    }
  }

  private void readTLM(BinaryReader ehs)
  {
    int num1 = (int) ehs.ReadUInt16();
    BinaryReader binaryReader = ehs;
    long position = binaryReader.BaseStream.Position;
    long num2 = binaryReader.BaseStream.Seek((long) (num1 - 2), SeekOrigin.Current) - position;
  }

  private void readPLM(BinaryReader ehs)
  {
    int num1 = (int) ehs.ReadUInt16();
    BinaryReader binaryReader = ehs;
    long position = binaryReader.BaseStream.Position;
    long num2 = binaryReader.BaseStream.Seek((long) (num1 - 2), SeekOrigin.Current) - position;
  }

  private void readPLTFields(BinaryReader ehs)
  {
    int num1 = (int) ehs.ReadUInt16();
    BinaryReader binaryReader = ehs;
    long position = binaryReader.BaseStream.Position;
    long num2 = binaryReader.BaseStream.Seek((long) (num1 - 2), SeekOrigin.Current) - position;
  }

  private void readRGN(BinaryReader ehs, bool mainh, int tileIdx, int tpIdx)
  {
    HeaderInformation.RGN newRgn = this.hi.NewRGN;
    newRgn.lrgn = (int) ehs.ReadUInt16();
    int c;
    newRgn.crgn = c = this.nComp < 257 ? (int) ehs.ReadByte() : (int) ehs.ReadUInt16();
    if (c >= this.nComp)
      throw new ArgumentException("Invalid component index in RGN marker" + (object) c);
    newRgn.srgn = (int) ehs.ReadByte();
    if (newRgn.srgn != 0)
      throw new ArgumentException("Unknown or unsupported Srgn parameter in ROI marker");
    if (this.decSpec.rois == null)
      this.decSpec.rois = new MaxShiftSpec(this.nTiles, this.nComp, (byte) 2);
    newRgn.sprgn = (int) ehs.ReadByte();
    if (mainh)
    {
      this.hi.rgnValue[(object) ("main_c" + (object) c)] = (object) newRgn;
      this.decSpec.rois.setCompDef(c, (object) newRgn.sprgn);
    }
    else
    {
      this.hi.rgnValue[(object) $"t{(object) tileIdx}_c{(object) c}"] = (object) newRgn;
      this.decSpec.rois.setTileCompVal(tileIdx, c, (object) newRgn.sprgn);
    }
    this.checkMarkerLength(ehs, "RGN marker");
  }

  private void readPPM(BinaryReader ehs)
  {
    if (this.pPMMarkerData == null)
    {
      this.pPMMarkerData = new byte[this.nPPMMarkSeg][];
      this.tileOfTileParts = new List<object>(10);
      this.decSpec.pphs.setDefault((object) true);
    }
    int count = (int) ehs.ReadUInt16() - 3;
    int index = (int) ehs.ReadByte();
    this.pPMMarkerData[index] = new byte[count];
    ehs.BaseStream.Read(this.pPMMarkerData[index], 0, count);
    this.checkMarkerLength(ehs, "PPM marker");
  }

  private void readPPT(BinaryReader ehs, int tile, int tpIdx)
  {
    if (this.tilePartPkdPktHeaders == null)
      this.tilePartPkdPktHeaders = new byte[this.nTiles][][][];
    if (this.tilePartPkdPktHeaders[tile] == null)
      this.tilePartPkdPktHeaders[tile] = new byte[this.nTileParts[tile]][][];
    if (this.tilePartPkdPktHeaders[tile][tpIdx] == null)
      this.tilePartPkdPktHeaders[tile][tpIdx] = new byte[this.nPPTMarkSeg[tile][tpIdx]][];
    int num = (int) ehs.ReadUInt16();
    int index = (int) ehs.ReadByte();
    byte[] buffer = new byte[num - 3];
    ehs.BaseStream.Read(buffer, 0, buffer.Length);
    this.tilePartPkdPktHeaders[tile][tpIdx][index] = buffer;
    this.checkMarkerLength(ehs, "PPT marker");
    this.decSpec.pphs.setTileDef(tile, (object) true);
  }

  private void extractMainMarkSeg(short marker, JPXRandomAccessStream ehs)
  {
    if (this.nfMarkSeg == 0 && marker != (short) -175)
      throw new ArgumentException("First marker after SOC must be SIZ " + Convert.ToString(marker, 16 /*0x10*/));
    string key = "";
    if (this.ht == null)
      this.ht = new Dictionary<object, object>();
    switch (marker)
    {
      case -175:
        if ((this.nfMarkSeg & 1) != 0)
          throw new ArgumentException("More than one SIZ marker segment found in main header");
        this.nfMarkSeg |= 1;
        key = "SIZ";
        break;
      case -174:
        if ((this.nfMarkSeg & 2) != 0)
          throw new ArgumentException("More than one COD marker found in main header");
        this.nfMarkSeg |= 2;
        key = "COD";
        break;
      case -173:
        this.nfMarkSeg |= 4;
        key = "COC" + (object) this.nCOCMarkSeg++;
        break;
      case -171:
        if ((this.nfMarkSeg & 16 /*0x10*/) != 0)
          throw new ArgumentException("More than one TLM marker found in main header");
        this.nfMarkSeg |= 16 /*0x10*/;
        break;
      case -169:
        if ((this.nfMarkSeg & 32 /*0x20*/) != 0)
          throw new ArgumentException("More than one PLM marker found in main header");
        this.nfMarkSeg |= 32 /*0x20*/;
        key = "PLM";
        break;
      case -168:
        throw new ArgumentException("PLT found in main header");
      case -164:
        if ((this.nfMarkSeg & 8) != 0)
          throw new ArgumentException("More than one QCD marker found in main header");
        this.nfMarkSeg |= 8;
        key = "QCD";
        break;
      case -163:
        this.nfMarkSeg |= 256 /*0x0100*/;
        key = "QCC" + (object) this.nQCCMarkSeg++;
        break;
      case -162:
        this.nfMarkSeg |= 512 /*0x0200*/;
        key = "RGN" + (object) this.nRGNMarkSeg++;
        break;
      case -161:
        if ((this.nfMarkSeg & 1024 /*0x0400*/) != 0)
          throw new ArgumentException("More than one POC marker segment found in main header");
        this.nfMarkSeg |= 1024 /*0x0400*/;
        key = "POC";
        break;
      case -160:
        this.nfMarkSeg |= 16384 /*0x4000*/;
        key = "PPM" + (object) this.nPPMMarkSeg++;
        break;
      case -159:
        throw new ArgumentException("PPT found in main header");
      case -157:
        if ((this.nfMarkSeg & 65536 /*0x010000*/) != 0)
          throw new ArgumentException("More than one CRG marker found in main header");
        this.nfMarkSeg |= 65536 /*0x010000*/;
        key = "CRG";
        break;
      case -156:
        this.nfMarkSeg |= 2048 /*0x0800*/;
        key = "COM" + (object) this.nCOMMarkSeg++;
        break;
      case -112:
        if ((this.nfMarkSeg & 64 /*0x40*/) != 0)
          throw new ArgumentException("More than one SOT marker found right after main or tile header");
        this.nfMarkSeg |= 64 /*0x40*/;
        return;
      case -109:
        throw new ArgumentException("SOD found in main header");
      case -39:
        throw new ArgumentException("EOC found in main header");
      default:
        key = "UNKNOWN";
        break;
    }
    if (marker >= (short) -208 && marker <= (short) -193)
      return;
    int length = ehs.readUnsignedShort();
    byte[] b = new byte[length];
    b[0] = (byte) (length >> 8 & (int) byte.MaxValue);
    b[1] = (byte) (length & (int) byte.MaxValue);
    ehs.readFully(b, 2, length - 2);
    if (key.Equals("UNKNOWN"))
      return;
    this.ht[(object) key] = (object) b;
  }

  internal virtual void extractTilePartMarkSeg(
    short marker,
    JPXRandomAccessStream ehs,
    int tileIdx,
    int tilePartIdx)
  {
    if (this.ht == null)
      this.ht = new Dictionary<object, object>();
    string key;
    switch (marker)
    {
      case -175:
        throw new ArgumentException("SIZ found in tile-part header");
      case -174:
        if ((this.nfMarkSeg & 2) != 0)
          throw new ArgumentException("More than one COD marker found in tile-part header");
        this.nfMarkSeg |= 2;
        key = "COD";
        break;
      case -173:
        this.nfMarkSeg |= 4;
        key = "COC" + (object) this.nCOCMarkSeg++;
        break;
      case -171:
        throw new ArgumentException("TLM found in tile-part header");
      case -169:
        throw new ArgumentException("PLM found in tile-part header");
      case -168:
        if ((this.nfMarkSeg & 32 /*0x20*/) != 0)
          throw new ArgumentException("PLT marker found eventhough PLM marker found in main header");
        key = "UNKNOWN";
        break;
      case -164:
        if ((this.nfMarkSeg & 8) != 0)
          throw new ArgumentException("More than one QCD marker found in tile-part header");
        this.nfMarkSeg |= 8;
        key = "QCD";
        break;
      case -163:
        this.nfMarkSeg |= 256 /*0x0100*/;
        key = "QCC" + (object) this.nQCCMarkSeg++;
        break;
      case -162:
        this.nfMarkSeg |= 512 /*0x0200*/;
        key = "RGN" + (object) this.nRGNMarkSeg++;
        break;
      case -161:
        if ((this.nfMarkSeg & 1024 /*0x0400*/) != 0)
          throw new ArgumentException("More than one POC marker segment found in tile-part header");
        this.nfMarkSeg |= 1024 /*0x0400*/;
        key = "POC";
        break;
      case -160:
        throw new ArgumentException("PPM found in tile-part header");
      case -159:
        this.nfMarkSeg |= 32768 /*0x8000*/;
        if (this.nPPTMarkSeg == null)
          this.nPPTMarkSeg = new int[this.nTiles][];
        if (this.nPPTMarkSeg[tileIdx] == null)
          this.nPPTMarkSeg[tileIdx] = new int[this.nTileParts[tileIdx]];
        key = "PPT" + (object) this.nPPTMarkSeg[tileIdx][tilePartIdx]++;
        break;
      case -157:
        throw new ArgumentException("CRG marker found in tile-part header");
      case -156:
        this.nfMarkSeg |= 2048 /*0x0800*/;
        key = "COM" + (object) this.nCOMMarkSeg++;
        break;
      case -112:
        throw new ArgumentException("Second SOT marker segment found in tile-part header");
      case -109:
        this.nfMarkSeg |= 8192 /*0x2000*/;
        return;
      case -39:
        throw new ArgumentException("EOC found in tile-part header");
      default:
        key = "UNKNOWN";
        break;
    }
    int length = ehs.readUnsignedShort();
    byte[] b = new byte[length];
    b[0] = (byte) (length >> 8 & (int) byte.MaxValue);
    b[1] = (byte) (length & (int) byte.MaxValue);
    ehs.readFully(b, 2, length - 2);
    if (key.Equals("UNKNOWN"))
      return;
    this.ht[(object) key] = (object) b;
  }

  private void readFoundMainMarkSeg()
  {
    if ((this.nfMarkSeg & 1) != 0)
      this.readSIZ((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) "SIZ"]), true));
    if ((this.nfMarkSeg & 2048 /*0x0800*/) != 0)
    {
      for (int comIdx = 0; comIdx < this.nCOMMarkSeg; ++comIdx)
        this.readCOM((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("COM" + (object) comIdx)]), true), true, 0, comIdx);
    }
    if ((this.nfMarkSeg & 65536 /*0x010000*/) != 0)
      this.readCRG((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) "CRG"]), true));
    if ((this.nfMarkSeg & 2) != 0)
      this.readCOD((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) "COD"]), true), true, 0, 0);
    if ((this.nfMarkSeg & 4) != 0)
    {
      for (int index = 0; index < this.nCOCMarkSeg; ++index)
        this.readCOC((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("COC" + (object) index)]), true), true, 0, 0);
    }
    if ((this.nfMarkSeg & 512 /*0x0200*/) != 0)
    {
      for (int index = 0; index < this.nRGNMarkSeg; ++index)
        this.readRGN((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("RGN" + (object) index)]), true), true, 0, 0);
    }
    if ((this.nfMarkSeg & 8) != 0)
      this.readQCD((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) "QCD"]), true), true, 0, 0);
    if ((this.nfMarkSeg & 256 /*0x0100*/) != 0)
    {
      for (int index = 0; index < this.nQCCMarkSeg; ++index)
        this.readQCC((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("QCC" + (object) index)]), true), true, 0, 0);
    }
    if ((this.nfMarkSeg & 1024 /*0x0400*/) != 0)
      this.readPOC((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) "POC"]), true), true, 0, 0);
    if ((this.nfMarkSeg & 16384 /*0x4000*/) != 0)
    {
      for (int index = 0; index < this.nPPMMarkSeg; ++index)
        this.readPPM((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("PPM" + (object) index)])));
    }
    this.ht = (Dictionary<object, object>) null;
  }

  public virtual void readFoundTilePartMarkSeg(int tileIdx, int tpIdx)
  {
    if ((this.nfMarkSeg & 2) != 0)
      this.readCOD((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) "COD"]), true), false, tileIdx, tpIdx);
    if ((this.nfMarkSeg & 4) != 0)
    {
      for (int index = 0; index < this.nCOCMarkSeg; ++index)
        this.readCOC((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("COC" + (object) index)]), true), false, tileIdx, tpIdx);
    }
    if ((this.nfMarkSeg & 512 /*0x0200*/) != 0)
    {
      for (int index = 0; index < this.nRGNMarkSeg; ++index)
        this.readRGN((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("RGN" + (object) index)]), true), false, tileIdx, tpIdx);
    }
    if ((this.nfMarkSeg & 8) != 0)
      this.readQCD((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) "QCD"]), true), false, tileIdx, tpIdx);
    if ((this.nfMarkSeg & 256 /*0x0100*/) != 0)
    {
      for (int index = 0; index < this.nQCCMarkSeg; ++index)
        this.readQCC((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("QCC" + (object) index)]), true), false, tileIdx, tpIdx);
    }
    if ((this.nfMarkSeg & 1024 /*0x0400*/) != 0)
      this.readPOC((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) "POC"]), true), false, tileIdx, tpIdx);
    if ((this.nfMarkSeg & 2048 /*0x0800*/) != 0)
    {
      for (int comIdx = 0; comIdx < this.nCOMMarkSeg; ++comIdx)
        this.readCOM((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("COM" + (object) comIdx)]), true), false, tileIdx, comIdx);
    }
    if ((this.nfMarkSeg & 32768 /*0x8000*/) != 0)
    {
      for (int index = 0; index < this.nPPTMarkSeg[tileIdx][tpIdx]; ++index)
        this.readPPT((BinaryReader) new EndianBinaryReader((Stream) new MemoryStream((byte[]) this.ht[(object) ("PPT" + (object) index)]), true), tileIdx, tpIdx);
    }
    this.ht = (Dictionary<object, object>) null;
  }

  internal HeaderDecoder(JPXRandomAccessStream ehs, JPXParameters pl, HeaderInformation hi)
  {
    this.hi = hi;
    pl.checkList('H', JPXParameters.toNameArray(HeaderDecoder.pinfo));
    this.mainHeadOff = ehs.Pos;
    if (ehs.readShort() != (short) -177)
      throw new ArgumentException("SOC marker segment not  found at the beginning of the codestream.");
    this.nfMarkSeg = 0;
    do
    {
      this.extractMainMarkSeg(ehs.readShort(), ehs);
    }
    while ((this.nfMarkSeg & 64 /*0x40*/) == 0);
    ehs.seek(ehs.Pos - 2);
    this.readFoundMainMarkSeg();
  }

  internal virtual EntropyDecoder createEntropyDecoder(CodedCBlkDataSrcDec src, JPXParameters pl)
  {
    pl.checkList('C', JPXParameters.toNameArray(EntropyDecoder.ParameterInfo));
    bool booleanParameter1 = pl.getBooleanParameter("Cer");
    bool booleanParameter2 = pl.getBooleanParameter("Cverber");
    int intParameter = pl.getIntParameter("m_quit");
    return (EntropyDecoder) new StdEntropyDecoder(src, this.decSpec, booleanParameter1, booleanParameter2, intParameter);
  }

  internal virtual DeScalerROI createROIDeScaler(
    CBlkQuantDataSrcDec src,
    JPXParameters pl,
    DecodeHelper decSpec2)
  {
    return DeScalerROI.createInstance(src, pl, decSpec2);
  }

  public virtual void resetHeaderMarkers()
  {
    this.nfMarkSeg &= 16416;
    this.nCOCMarkSeg = 0;
    this.nQCCMarkSeg = 0;
    this.nCOMMarkSeg = 0;
    this.nRGNMarkSeg = 0;
  }

  public override string ToString() => this.hdStr;

  public virtual MemoryStream getPackedPktHead(int tile)
  {
    if (this.pkdPktHeaders == null)
    {
      this.pkdPktHeaders = new MemoryStream[this.nTiles];
      for (int index = this.nTiles - 1; index >= 0; --index)
        this.pkdPktHeaders[index] = new MemoryStream();
      if (this.nPPMMarkSeg != 0)
      {
        int count = this.tileOfTileParts.Count;
        MemoryStream memoryStream1 = new MemoryStream();
        for (int index = 0; index < this.nPPMMarkSeg; ++index)
        {
          byte[] buffer = this.pPMMarkerData[index];
          memoryStream1.Write(buffer, 0, buffer.Length);
        }
        MemoryStream memoryStream2 = new MemoryStream(memoryStream1.ToArray());
        for (int index = 0; index < count; ++index)
        {
          int tileOfTilePart = (int) this.tileOfTileParts[index];
          byte[] buffer1 = new byte[memoryStream2.ReadByte() << 24 | memoryStream2.ReadByte() << 16 /*0x10*/ | memoryStream2.ReadByte() << 8 | memoryStream2.ReadByte()];
          memoryStream2.Read(buffer1, 0, buffer1.Length);
          byte[] buffer2 = buffer1;
          this.pkdPktHeaders[tileOfTilePart].Write(buffer2, 0, buffer2.Length);
        }
      }
      else
      {
        for (int index1 = this.nTiles - 1; index1 >= 0; --index1)
        {
          for (int index2 = 0; index2 < this.nTileParts[index1]; ++index2)
          {
            for (int index3 = 0; index3 < this.nPPTMarkSeg[index1][index2]; ++index3)
            {
              byte[] buffer = this.tilePartPkdPktHeaders[index1][index2][index3];
              this.pkdPktHeaders[index1].Write(buffer, 0, buffer.Length);
            }
          }
        }
      }
    }
    return new MemoryStream(this.pkdPktHeaders[tile].ToArray());
  }
}
