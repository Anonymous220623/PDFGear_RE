// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HeaderInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class HeaderInformation
{
  public HeaderInformation.SIZ sizValue;
  public Dictionary<object, object> sotValue = new Dictionary<object, object>();
  public Dictionary<object, object> codValue = new Dictionary<object, object>();
  public Dictionary<object, object> cocValue = new Dictionary<object, object>();
  public Dictionary<object, object> rgnValue = new Dictionary<object, object>();
  public Dictionary<object, object> qcdValue = new Dictionary<object, object>();
  public Dictionary<object, object> qccValue = new Dictionary<object, object>();
  public Dictionary<object, object> pocValue = new Dictionary<object, object>();
  public HeaderInformation.CRG crgValue;
  public Dictionary<object, object> comValue = new Dictionary<object, object>();
  private int ncom;

  public virtual HeaderInformation.SIZ NewSIZ => new HeaderInformation.SIZ(this);

  public virtual HeaderInformation.SOT NewSOT => new HeaderInformation.SOT(this);

  public virtual HeaderInformation.COD NewCOD => new HeaderInformation.COD(this);

  public virtual HeaderInformation.COC NewCOC => new HeaderInformation.COC(this);

  public virtual HeaderInformation.RGN NewRGN => new HeaderInformation.RGN(this);

  public virtual HeaderInformation.QCD NewQCD => new HeaderInformation.QCD(this);

  public virtual HeaderInformation.QCC NewQCC => new HeaderInformation.QCC(this);

  public virtual HeaderInformation.POC NewPOC => new HeaderInformation.POC(this);

  public virtual HeaderInformation.CRG NewCRG => new HeaderInformation.CRG(this);

  public virtual HeaderInformation.COM NewCOM
  {
    get
    {
      ++this.ncom;
      return new HeaderInformation.COM(this);
    }
  }

  public virtual int NumCOM => this.ncom;

  public virtual string toStringMainHeader()
  {
    int csiz = this.sizValue.csiz;
    string stringMainHeader = string.Concat((object) this.sizValue);
    if (this.codValue[(object) "main"] != null)
      stringMainHeader += (string) (object) (HeaderInformation.COD) this.codValue[(object) "main"];
    for (int index = 0; index < csiz; ++index)
    {
      if (this.cocValue[(object) ("main_c" + (object) index)] != null)
        stringMainHeader += (string) (object) (HeaderInformation.COC) this.cocValue[(object) ("main_c" + (object) index)];
    }
    if (this.qcdValue[(object) "main"] != null)
      stringMainHeader += (string) (object) (HeaderInformation.QCD) this.qcdValue[(object) "main"];
    for (int index = 0; index < csiz; ++index)
    {
      if (this.qccValue[(object) ("main_c" + (object) index)] != null)
        stringMainHeader += (string) (object) (HeaderInformation.QCC) this.qccValue[(object) ("main_c" + (object) index)];
    }
    for (int index = 0; index < csiz; ++index)
    {
      if (this.rgnValue[(object) ("main_c" + (object) index)] != null)
        stringMainHeader += (string) (object) (HeaderInformation.RGN) this.rgnValue[(object) ("main_c" + (object) index)];
    }
    if (this.pocValue[(object) "main"] != null)
      stringMainHeader += (string) (object) (HeaderInformation.POC) this.pocValue[(object) "main"];
    if (this.crgValue != null)
      stringMainHeader += (string) (object) this.crgValue;
    for (int index = 0; index < this.ncom; ++index)
    {
      if (this.comValue[(object) ("main_" + (object) index)] != null)
        stringMainHeader += (string) (object) (HeaderInformation.COM) this.comValue[(object) ("main_" + (object) index)];
    }
    return stringMainHeader;
  }

  public virtual string toStringTileHeader(int t, int ntp)
  {
    int csiz = this.sizValue.csiz;
    string stringTileHeader = "";
    for (int index = 0; index < ntp; ++index)
      stringTileHeader = $"{stringTileHeader}Tile-part {(object) index}, tile {(object) t}:\n" + (object) (HeaderInformation.SOT) this.sotValue[(object) $"t{(object) t}_tp{(object) index}"];
    if (this.codValue[(object) (nameof (t) + (object) t)] != null)
      stringTileHeader += (string) (object) (HeaderInformation.COD) this.codValue[(object) (nameof (t) + (object) t)];
    for (int index = 0; index < csiz; ++index)
    {
      if (this.cocValue[(object) $"t{(object) t}_c{(object) index}"] != null)
        stringTileHeader += (string) (object) (HeaderInformation.COC) this.cocValue[(object) $"t{(object) t}_c{(object) index}"];
    }
    if (this.qcdValue[(object) (nameof (t) + (object) t)] != null)
      stringTileHeader += (string) (object) (HeaderInformation.QCD) this.qcdValue[(object) (nameof (t) + (object) t)];
    for (int index = 0; index < csiz; ++index)
    {
      if (this.qccValue[(object) $"t{(object) t}_c{(object) index}"] != null)
        stringTileHeader += (string) (object) (HeaderInformation.QCC) this.qccValue[(object) $"t{(object) t}_c{(object) index}"];
    }
    for (int index = 0; index < csiz; ++index)
    {
      if (this.rgnValue[(object) $"t{(object) t}_c{(object) index}"] != null)
        stringTileHeader += (string) (object) (HeaderInformation.RGN) this.rgnValue[(object) $"t{(object) t}_c{(object) index}"];
    }
    if (this.pocValue[(object) (nameof (t) + (object) t)] != null)
      stringTileHeader += (string) (object) (HeaderInformation.POC) this.pocValue[(object) (nameof (t) + (object) t)];
    return stringTileHeader;
  }

  public virtual string toStringThNoSOT(int t, int ntp)
  {
    int csiz = this.sizValue.csiz;
    string stringThNoSot = "";
    if (this.codValue[(object) (nameof (t) + (object) t)] != null)
      stringThNoSot += (string) (object) (HeaderInformation.COD) this.codValue[(object) (nameof (t) + (object) t)];
    for (int index = 0; index < csiz; ++index)
    {
      if (this.cocValue[(object) $"t{(object) t}_c{(object) index}"] != null)
        stringThNoSot += (string) (object) (HeaderInformation.COC) this.cocValue[(object) $"t{(object) t}_c{(object) index}"];
    }
    if (this.qcdValue[(object) (nameof (t) + (object) t)] != null)
      stringThNoSot += (string) (object) (HeaderInformation.QCD) this.qcdValue[(object) (nameof (t) + (object) t)];
    for (int index = 0; index < csiz; ++index)
    {
      if (this.qccValue[(object) $"t{(object) t}_c{(object) index}"] != null)
        stringThNoSot += (string) (object) (HeaderInformation.QCC) this.qccValue[(object) $"t{(object) t}_c{(object) index}"];
    }
    for (int index = 0; index < csiz; ++index)
    {
      if (this.rgnValue[(object) $"t{(object) t}_c{(object) index}"] != null)
        stringThNoSot += (string) (object) (HeaderInformation.RGN) this.rgnValue[(object) $"t{(object) t}_c{(object) index}"];
    }
    if (this.pocValue[(object) (nameof (t) + (object) t)] != null)
      stringThNoSot += (string) (object) (HeaderInformation.POC) this.pocValue[(object) (nameof (t) + (object) t)];
    return stringThNoSot;
  }

  public virtual HeaderInformation getCopy(int nt)
  {
    HeaderInformation copy = (HeaderInformation) null;
    try
    {
      copy = (HeaderInformation) this.Clone();
    }
    catch (Exception ex)
    {
    }
    copy.sizValue = this.sizValue.Copy;
    if (this.codValue[(object) "main"] != null)
    {
      HeaderInformation.COD cod = (HeaderInformation.COD) this.codValue[(object) "main"];
      copy.codValue[(object) "main"] = (object) cod.Copy;
    }
    for (int index = 0; index < nt; ++index)
    {
      if (this.codValue[(object) ("t" + (object) index)] != null)
      {
        HeaderInformation.COD cod = (HeaderInformation.COD) this.codValue[(object) ("t" + (object) index)];
        copy.codValue[(object) ("t" + (object) index)] = (object) cod.Copy;
      }
    }
    return copy;
  }

  public virtual object Clone() => (object) null;

  internal class SIZ
  {
    private HeaderInformation enclosingInstance;
    public int lsiz;
    public int rsiz;
    public int xsiz;
    public int ysiz;
    public int x0siz;
    public int y0siz;
    public int xtsiz;
    public int ytsiz;
    public int xt0siz;
    public int yt0siz;
    public int csiz;
    public int[] ssiz;
    public int[] xrsiz;
    public int[] yrsiz;
    private int[] compWidth;
    private int maxCompWidth = -1;
    private int[] compHeight;
    private int maxCompHeight = -1;
    private int numTiles = -1;
    private bool[] origSigned;
    private int[] origBitDepth;

    public SIZ(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public virtual int MaxCompWidth
    {
      get
      {
        if (this.compWidth == null)
        {
          this.compWidth = new int[this.csiz];
          for (int index = 0; index < this.csiz; ++index)
            this.compWidth[index] = (int) (Math.Ceiling((double) this.xsiz / (double) this.xrsiz[index]) - Math.Ceiling((double) this.x0siz / (double) this.xrsiz[index]));
        }
        if (this.maxCompWidth == -1)
        {
          for (int index = 0; index < this.csiz; ++index)
          {
            if (this.compWidth[index] > this.maxCompWidth)
              this.maxCompWidth = this.compWidth[index];
          }
        }
        return this.maxCompWidth;
      }
    }

    public virtual int MaxCompHeight
    {
      get
      {
        if (this.compHeight == null)
        {
          this.compHeight = new int[this.csiz];
          for (int index = 0; index < this.csiz; ++index)
            this.compHeight[index] = (int) (Math.Ceiling((double) this.ysiz / (double) this.yrsiz[index]) - Math.Ceiling((double) this.y0siz / (double) this.yrsiz[index]));
        }
        if (this.maxCompHeight == -1)
        {
          for (int index = 0; index < this.csiz; ++index)
          {
            if (this.compHeight[index] != this.maxCompHeight)
              this.maxCompHeight = this.compHeight[index];
          }
        }
        return this.maxCompHeight;
      }
    }

    public virtual int NumTiles
    {
      get
      {
        if (this.numTiles == -1)
          this.numTiles = (this.xsiz - this.xt0siz + this.xtsiz - 1) / this.xtsiz * ((this.ysiz - this.yt0siz + this.ytsiz - 1) / this.ytsiz);
        return this.numTiles;
      }
    }

    public virtual HeaderInformation.SIZ Copy
    {
      get
      {
        HeaderInformation.SIZ copy = (HeaderInformation.SIZ) null;
        try
        {
          copy = (HeaderInformation.SIZ) this.Clone();
        }
        catch (Exception ex)
        {
        }
        return copy;
      }
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public virtual int getCompImgWidth(int c)
    {
      if (this.compWidth == null)
      {
        this.compWidth = new int[this.csiz];
        for (int index = 0; index < this.csiz; ++index)
          this.compWidth[index] = (int) (Math.Ceiling((double) this.xsiz / (double) this.xrsiz[index]) - Math.Ceiling((double) this.x0siz / (double) this.xrsiz[index]));
      }
      return this.compWidth[c];
    }

    public virtual int getCompImgHeight(int c)
    {
      if (this.compHeight == null)
      {
        this.compHeight = new int[this.csiz];
        for (int index = 0; index < this.csiz; ++index)
          this.compHeight[index] = (int) (Math.Ceiling((double) this.ysiz / (double) this.yrsiz[index]) - Math.Ceiling((double) this.y0siz / (double) this.yrsiz[index]));
      }
      return this.compHeight[c];
    }

    public virtual bool isOrigSigned(int c)
    {
      if (this.origSigned == null)
      {
        this.origSigned = new bool[this.csiz];
        for (int index = 0; index < this.csiz; ++index)
          this.origSigned[index] = SupportClass.URShift(this.ssiz[index], 7) == 1;
      }
      return this.origSigned[c];
    }

    public virtual int getOrigBitDepth(int c)
    {
      if (this.origBitDepth == null)
      {
        this.origBitDepth = new int[this.csiz];
        for (int index = 0; index < this.csiz; ++index)
          this.origBitDepth[index] = (this.ssiz[index] & (int) sbyte.MaxValue) + 1;
      }
      return this.origBitDepth[c];
    }

    public override string ToString()
    {
      string str1 = $"{$"{$"{$"{$"\n --- SIZ ({(object) this.lsiz} bytes) ---\n"} Capabilities : {(object) this.rsiz}\n"} Image dim.   : {(object) (this.xsiz - this.x0siz)}x{(object) (this.ysiz - this.y0siz)}, (off={(object) this.x0siz},{(object) this.y0siz})\n"} Tile dim.    : {(object) this.xtsiz}x{(object) this.ytsiz}, (off={(object) this.xt0siz},{(object) this.yt0siz})\n"} Component(s) : {(object) this.csiz}\n" + " Orig. depth  : ";
      for (int c = 0; c < this.csiz; ++c)
        str1 = $"{str1}{(object) this.getOrigBitDepth(c)} ";
      string str2 = str1 + "\n" + " Orig. signed : ";
      for (int c = 0; c < this.csiz; ++c)
        str2 = $"{str2}{(object) this.isOrigSigned(c)} ";
      string str3 = str2 + "\n" + " Subs. factor : ";
      for (int index = 0; index < this.csiz; ++index)
        str3 = $"{str3}{(object) this.xrsiz[index]},{(object) this.yrsiz[index]} ";
      return str3 + "\n";
    }

    public virtual object Clone() => (object) null;
  }

  internal class SOT
  {
    private HeaderInformation enclosingInstance;
    public int lsot;
    public int isot;
    public int psot;
    public int tpsot;
    public int tnsot;

    public SOT(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      return $"{$"{$"{$"{$"\n --- SOT ({(object) this.lsot} bytes) ---\n"}Tile index         : {(object) this.isot}\n"}Tile-part length   : {(object) this.psot} bytes\n"}Tile-part index    : {(object) this.tpsot}\n"}Num. of tile-parts : {(object) this.tnsot}\n" + "\n";
    }
  }

  internal class COD
  {
    private HeaderInformation enclosingInstance;
    public int lcod;
    public int scod;
    public int sgcod_po;
    public int sgcod_nl;
    public int sgcod_mct;
    public int spcod_ndl;
    public int spcod_cw;
    public int spcod_ch;
    public int spcod_cs;
    public int[] spcod_t = new int[1];
    public int[] spcod_ps;

    public COD(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public virtual HeaderInformation.COD Copy
    {
      get
      {
        HeaderInformation.COD copy = (HeaderInformation.COD) null;
        try
        {
          copy = (HeaderInformation.COD) this.Clone();
        }
        catch (Exception ex)
        {
        }
        return copy;
      }
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      string str1 = $"\n --- COD ({(object) this.lcod} bytes) ---\n" + " Coding style   : ";
      if (this.scod == 0)
      {
        str1 += "Default";
      }
      else
      {
        if ((this.scod & 1) != 0)
          str1 += "Precints ";
        if ((this.scod & 2) != 0)
          str1 += "SOP ";
        if ((this.scod & 4) != 0)
          str1 += "EPH ";
        int num1 = (this.scod & 8) != 0 ? 1 : 0;
        int num2 = (this.scod & 16 /*0x10*/) != 0 ? 1 : 0;
        if (num1 != 0 || num2 != 0)
          str1 = $"{str1 + "Code-blocks offset"}\n Cblk partition : {(object) num1},{(object) num2}";
      }
      string str2 = str1 + "\n" + " Cblk style     : ";
      if (this.spcod_cs == 0)
      {
        str2 += "Default";
      }
      else
      {
        if ((this.spcod_cs & 1) != 0)
          str2 += "Bypass ";
        if ((this.spcod_cs & 2) != 0)
          str2 += "Reset ";
        if ((this.spcod_cs & 4) != 0)
          str2 += "Terminate ";
        if ((this.spcod_cs & 8) != 0)
          str2 += "Vert_causal ";
        if ((this.spcod_cs & 16 /*0x10*/) != 0)
          str2 += "Predict ";
        if ((this.spcod_cs & 32 /*0x20*/) != 0)
          str2 += "Seg_symb ";
      }
      string str3 = $"{str2 + "\n"} Num. of levels : {(object) this.spcod_ndl}\n";
      switch (this.sgcod_po)
      {
        case 0:
          str3 += " Progress. type : LY_RES_COMP_POS_PROG\n";
          break;
        case 1:
          str3 += " Progress. type : RES_LY_COMP_POS_PROG\n";
          break;
        case 2:
          str3 += " Progress. type : RES_POS_COMP_LY_PROG\n";
          break;
        case 3:
          str3 += " Progress. type : POS_COMP_RES_LY_PROG\n";
          break;
        case 4:
          str3 += " Progress. type : COMP_POS_RES_LY_PROG\n";
          break;
      }
      string str4 = $"{$"{str3} Num. of layers : {(object) this.sgcod_nl}\n"} Cblk dimension : {(object) (1 << this.spcod_cw + 2)}x{(object) (1 << this.spcod_ch + 2)}\n";
      switch (this.spcod_t[0])
      {
        case 0:
          str4 += " Filter         : 9-7 irreversible\n";
          break;
        case 1:
          str4 += " Filter         : 5-3 reversible\n";
          break;
      }
      string str5 = $"{str4} Multi comp tr. : {(object) (this.sgcod_mct == 1)}\n";
      if (this.spcod_ps != null)
      {
        str5 += " Precincts      : ";
        for (int index = 0; index < this.spcod_ps.Length; ++index)
          str5 = $"{str5}{(object) (1 << (this.spcod_ps[index] & 15))}x{(object) (1 << ((this.spcod_ps[index] & 240 /*0xF0*/) >> 4))} ";
      }
      return str5 + "\n";
    }

    public virtual object Clone() => (object) null;
  }

  internal class COC
  {
    private HeaderInformation enclosingInstance;
    public int lcoc;
    public int ccoc;
    public int scoc;
    public int spcoc_ndl;
    public int spcoc_cw;
    public int spcoc_ch;
    public int spcoc_cs;
    public int[] spcoc_t = new int[1];
    public int[] spcoc_ps;

    public COC(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      string str1 = $"{$"\n --- COC ({(object) this.lcoc} bytes) ---\n"} Component      : {(object) this.ccoc}\n" + " Coding style   : ";
      if (this.scoc == 0)
      {
        str1 += "Default";
      }
      else
      {
        if ((this.scoc & 1) != 0)
          str1 += "Precints ";
        if ((this.scoc & 2) != 0)
          str1 += "SOP ";
        if ((this.scoc & 4) != 0)
          str1 += "EPH ";
      }
      string str2 = str1 + "\n" + " Cblk style     : ";
      if (this.spcoc_cs == 0)
      {
        str2 += "Default";
      }
      else
      {
        if ((this.spcoc_cs & 1) != 0)
          str2 += "Bypass ";
        if ((this.spcoc_cs & 2) != 0)
          str2 += "Reset ";
        if ((this.spcoc_cs & 4) != 0)
          str2 += "Terminate ";
        if ((this.spcoc_cs & 8) != 0)
          str2 += "Vert_causal ";
        if ((this.spcoc_cs & 16 /*0x10*/) != 0)
          str2 += "Predict ";
        if ((this.spcoc_cs & 32 /*0x20*/) != 0)
          str2 += "Seg_symb ";
      }
      string str3 = $"{$"{str2 + "\n"} Num. of levels : {(object) this.spcoc_ndl}\n"} Cblk dimension : {(object) (1 << this.spcoc_cw + 2)}x{(object) (1 << this.spcoc_ch + 2)}\n";
      switch (this.spcoc_t[0])
      {
        case 0:
          str3 += " Filter         : 9-7 irreversible\n";
          break;
        case 1:
          str3 += " Filter         : 5-3 reversible\n";
          break;
      }
      if (this.spcoc_ps != null)
      {
        str3 += " Precincts      : ";
        for (int index = 0; index < this.spcoc_ps.Length; ++index)
          str3 = $"{str3}{(object) (1 << (this.spcoc_ps[index] & 15))}x{(object) (1 << ((this.spcoc_ps[index] & 240 /*0xF0*/) >> 4))} ";
      }
      return str3 + "\n";
    }
  }

  internal class RGN
  {
    private HeaderInformation enclosingInstance;
    public int lrgn;
    public int crgn;
    public int srgn;
    public int sprgn;

    public RGN(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      string str = $"{$"\n --- RGN ({(object) this.lrgn} bytes) ---\n"} Component : {(object) this.crgn}\n";
      return $"{(this.srgn != 0 ? (object) (str + " ROI style : Unsupported\n") : (object) (str + " ROI style : Implicit\n"))} ROI shift : {(object) this.sprgn}\n" + "\n";
    }
  }

  internal class QCD
  {
    private HeaderInformation enclosingInstance;
    public int lqcd;
    public int sqcd;
    public int[][] spqcd;
    private int qType = -1;
    private int gb = -1;

    public QCD(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public virtual int QuantType
    {
      get
      {
        if (this.qType == -1)
          this.qType = this.sqcd & -225;
        return this.qType;
      }
    }

    public virtual int NumGuardBits
    {
      get
      {
        if (this.gb == -1)
          this.gb = this.sqcd >> 5 & 7;
        return this.gb;
      }
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      string str1 = $"\n --- QCD ({(object) this.lqcd} bytes) ---\n" + " Quant. type    : ";
      int quantType = this.QuantType;
      switch (quantType)
      {
        case 0:
          str1 += "No quantization \n";
          break;
        case 1:
          str1 += "Scalar derived\n";
          break;
        case 2:
          str1 += "Scalar expounded\n";
          break;
      }
      string str2 = $"{str1} Guard bits     : {(object) this.NumGuardBits}\n";
      string str3;
      if (quantType == 0)
      {
        str3 = str2 + " Exponents   :\n";
        for (int index1 = 0; index1 < this.spqcd.Length; ++index1)
        {
          for (int index2 = 0; index2 < this.spqcd[index1].Length; ++index2)
          {
            if (index1 == 0 && index2 == 0)
            {
              int num = this.spqcd[0][0] >> 3 & 31 /*0x1F*/;
              str3 = $"{str3}\tr=0 : {(object) num}\n";
            }
            else if (index1 != 0 && index2 > 0)
            {
              int num = this.spqcd[index1][index2] >> 3 & 31 /*0x1F*/;
              str3 = $"{str3}\tr={(object) index1},s={(object) index2} : {(object) num}\n";
            }
          }
        }
      }
      else
      {
        str3 = str2 + " Exp / Mantissa : \n";
        for (int index3 = 0; index3 < this.spqcd.Length; ++index3)
        {
          for (int index4 = 0; index4 < this.spqcd[index3].Length; ++index4)
          {
            if (index3 == 0 && index4 == 0)
            {
              int num1 = this.spqcd[0][0] >> 11 & 31 /*0x1F*/;
              double num2 = (-1.0 - (double) (this.spqcd[0][0] & 2047 /*0x07FF*/) / 2048.0) / (double) (-1 << num1);
              str3 = $"{str3}\tr=0 : {(object) num1} / {(object) num2}\n";
            }
            else if (index3 != 0 && index4 > 0)
            {
              int num3 = this.spqcd[index3][index4] >> 11 & 31 /*0x1F*/;
              double num4 = (-1.0 - (double) (this.spqcd[index3][index4] & 2047 /*0x07FF*/) / 2048.0) / (double) (-1 << num3);
              str3 = $"{str3}\tr={(object) index3},s={(object) index4} : {(object) num3} / {(object) num4}\n";
            }
          }
        }
      }
      return str3 + "\n";
    }
  }

  internal class QCC
  {
    private HeaderInformation enclosingInstance;
    public int lqcc;
    public int cqcc;
    public int sqcc;
    public int[][] spqcc;
    private int qType = -1;
    private int gb = -1;

    public QCC(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public virtual int QuantType
    {
      get
      {
        if (this.qType == -1)
          this.qType = this.sqcc & -225;
        return this.qType;
      }
    }

    public virtual int NumGuardBits
    {
      get
      {
        if (this.gb == -1)
          this.gb = this.sqcc >> 5 & 7;
        return this.gb;
      }
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      string str1 = $"{$"\n --- QCC ({(object) this.lqcc} bytes) ---\n"} Component      : {(object) this.cqcc}\n" + " Quant. type    : ";
      int quantType = this.QuantType;
      switch (quantType)
      {
        case 0:
          str1 += "No quantization \n";
          break;
        case 1:
          str1 += "Scalar derived\n";
          break;
        case 2:
          str1 += "Scalar expounded\n";
          break;
      }
      string str2 = $"{str1} Guard bits     : {(object) this.NumGuardBits}\n";
      string str3;
      if (quantType == 0)
      {
        str3 = str2 + " Exponents   :\n";
        for (int index1 = 0; index1 < this.spqcc.Length; ++index1)
        {
          for (int index2 = 0; index2 < this.spqcc[index1].Length; ++index2)
          {
            if (index1 == 0 && index2 == 0)
            {
              int num = this.spqcc[0][0] >> 3 & 31 /*0x1F*/;
              str3 = $"{str3}\tr=0 : {(object) num}\n";
            }
            else if (index1 != 0 && index2 > 0)
            {
              int num = this.spqcc[index1][index2] >> 3 & 31 /*0x1F*/;
              str3 = $"{str3}\tr={(object) index1},s={(object) index2} : {(object) num}\n";
            }
          }
        }
      }
      else
      {
        str3 = str2 + " Exp / Mantissa : \n";
        for (int index3 = 0; index3 < this.spqcc.Length; ++index3)
        {
          for (int index4 = 0; index4 < this.spqcc[index3].Length; ++index4)
          {
            if (index3 == 0 && index4 == 0)
            {
              int num1 = this.spqcc[0][0] >> 11 & 31 /*0x1F*/;
              double num2 = (-1.0 - (double) (this.spqcc[0][0] & 2047 /*0x07FF*/) / 2048.0) / (double) (-1 << num1);
              str3 = $"{str3}\tr=0 : {(object) num1} / {(object) num2}\n";
            }
            else if (index3 != 0 && index4 > 0)
            {
              int num3 = this.spqcc[index3][index4] >> 11 & 31 /*0x1F*/;
              double num4 = (-1.0 - (double) (this.spqcc[index3][index4] & 2047 /*0x07FF*/) / 2048.0) / (double) (-1 << num3);
              str3 = $"{str3}\tr={(object) index3},s={(object) index4} : {(object) num3} / {(object) num4}\n";
            }
          }
        }
      }
      return str3 + "\n";
    }
  }

  internal class POC
  {
    private HeaderInformation enclosingInstance;
    public int lpoc;
    public int[] rspoc;
    public int[] cspoc;
    public int[] lyepoc;
    public int[] repoc;
    public int[] cepoc;
    public int[] ppoc;

    public POC(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      string str = $"\n --- POC ({(object) this.lpoc} bytes) ---\n" + " Chg_idx RSpoc CSpoc LYEpoc REpoc CEpoc Ppoc\n";
      for (int index = 0; index < this.rspoc.Length; ++index)
      {
        str = $"{str}   {(object) index}      {(object) this.rspoc[index]}     {(object) this.cspoc[index]}     {(object) this.lyepoc[index]}      {(object) this.repoc[index]}     {(object) this.cepoc[index]}";
        switch (this.ppoc[index])
        {
          case 0:
            str += "  LY_RES_COMP_POS_PROG\n";
            break;
          case 1:
            str += "  RES_LY_COMP_POS_PROG\n";
            break;
          case 2:
            str += "  RES_POS_COMP_LY_PROG\n";
            break;
          case 3:
            str += "  POS_COMP_RES_LY_PROG\n";
            break;
          case 4:
            str += "  COMP_POS_RES_LY_PROG\n";
            break;
        }
      }
      return str + "\n";
    }
  }

  internal class CRG
  {
    private HeaderInformation enclosingInstance;
    public int lcrg;
    public int[] xcrg;
    public int[] ycrg;

    public CRG(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      string str = $"\n --- CRG ({(object) this.lcrg} bytes) ---\n";
      for (int index = 0; index < this.xcrg.Length; ++index)
        str = $"{str} Component {(object) index} offset : {(object) this.xcrg[index]},{(object) this.ycrg[index]}\n";
      return str + "\n";
    }
  }

  internal class COM
  {
    private HeaderInformation enclosingInstance;
    public int lcom;
    public int rcom;
    public byte[] ccom;

    public COM(HeaderInformation enclosingInstance) => this.InitBlock(enclosingInstance);

    private void InitBlock(HeaderInformation enclosingInstance)
    {
      this.enclosingInstance = enclosingInstance;
    }

    public HeaderInformation Enclosing_Instance => this.enclosingInstance;

    public override string ToString()
    {
      string str = $"\n --- COM ({(object) this.lcom} bytes) ---\n";
      return (this.rcom != 0 ? (this.rcom != 1 ? str + " Registration : Unknown\n" : str + " Registration : General use (IS 8859-15:1999 (Latin) values)\n") : str + " Registration : General use (binary values)\n") + "\n";
    }
  }
}
