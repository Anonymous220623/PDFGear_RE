// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CBlkSizeSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class CBlkSizeSpec : ModuleSpec
{
  private const string optName = "Cblksiz";
  private int maxCBlkWidth;
  private int maxCBlkHeight;

  public virtual int MaxCBlkWidth => this.maxCBlkWidth;

  public virtual int MaxCBlkHeight => this.maxCBlkHeight;

  public CBlkSizeSpec(int nt, int nc, byte type)
    : base(nt, nc, type)
  {
  }

  internal CBlkSizeSpec(int nt, int nc, byte type, JPXParameters pl)
    : base(nt, nc, type)
  {
    bool flag = true;
    SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(pl.getParameter("Cblksiz"));
    byte num = 0;
    bool[] flagArray1 = (bool[]) null;
    bool[] flagArray2 = (bool[]) null;
    while (tokenizer.HasMoreTokens())
    {
      string str = tokenizer.NextToken();
      switch (str[0])
      {
        case 'c':
          flagArray2 = ModuleSpec.parseIdx(str, this.nComp);
          num = num != (byte) 2 ? (byte) 1 : (byte) 3;
          continue;
        case 't':
          flagArray1 = ModuleSpec.parseIdx(str, this.nTiles);
          num = num != (byte) 1 ? (byte) 2 : (byte) 3;
          continue;
        default:
          if (!char.IsDigit(str[0]))
            throw new ArgumentException("Bad construction for parameter: " + str);
          int[] value_Renamed = new int[2];
          try
          {
            value_Renamed[0] = int.Parse(str);
            if (value_Renamed[0] > StdEntropyCoderOptions.MAX_CB_DIM)
              throw new ArgumentException("'Cblksiz' option : the code-block's width cannot be greater than " + (object) StdEntropyCoderOptions.MAX_CB_DIM);
            if (value_Renamed[0] < StdEntropyCoderOptions.MIN_CB_DIM)
              throw new ArgumentException("'Cblksiz' option : the code-block's width cannot be less than " + (object) StdEntropyCoderOptions.MIN_CB_DIM);
            if (value_Renamed[0] != 1 << MathUtil.log2(value_Renamed[0]))
              throw new ArgumentException("'Cblksiz' option : the code-block's width must be a power of 2");
          }
          catch (FormatException ex)
          {
            throw new ArgumentException("'Cblksiz' option : the code-block's width could not be parsed.");
          }
          string s;
          try
          {
            s = tokenizer.NextToken();
          }
          catch (ArgumentOutOfRangeException ex)
          {
            throw new ArgumentException("'Cblksiz' option : could not parse the code-block's height");
          }
          try
          {
            value_Renamed[1] = int.Parse(s);
            if (value_Renamed[1] > StdEntropyCoderOptions.MAX_CB_DIM)
              throw new ArgumentException("'Cblksiz' option : the code-block's height cannot be greater than " + (object) StdEntropyCoderOptions.MAX_CB_DIM);
            if (value_Renamed[1] < StdEntropyCoderOptions.MIN_CB_DIM)
              throw new ArgumentException("'Cblksiz' option : the code-block's height cannot be less than " + (object) StdEntropyCoderOptions.MIN_CB_DIM);
            if (value_Renamed[1] != 1 << MathUtil.log2(value_Renamed[1]))
              throw new ArgumentException("'Cblksiz' option : the code-block's height must be a power of 2");
            if (value_Renamed[0] * value_Renamed[1] > StdEntropyCoderOptions.MAX_CB_AREA)
              throw new ArgumentException("'Cblksiz' option : The code-block's area (i.e. width*height) cannot be greater than " + (object) StdEntropyCoderOptions.MAX_CB_AREA);
          }
          catch (FormatException ex)
          {
            throw new ArgumentException("'Cblksiz' option : the code-block's height could not be parsed.");
          }
          if (value_Renamed[0] > this.maxCBlkWidth)
            this.maxCBlkWidth = value_Renamed[0];
          if (value_Renamed[1] > this.maxCBlkHeight)
            this.maxCBlkHeight = value_Renamed[1];
          if (flag)
          {
            this.setDefault((object) value_Renamed);
            flag = false;
          }
          switch (num)
          {
            case 0:
              this.setDefault((object) value_Renamed);
              continue;
            case 1:
              for (int c = flagArray2.Length - 1; c >= 0; --c)
              {
                if (flagArray2[c])
                  this.setCompDef(c, (object) value_Renamed);
              }
              continue;
            case 2:
              for (int t = flagArray1.Length - 1; t >= 0; --t)
              {
                if (flagArray1[t])
                  this.setTileDef(t, (object) value_Renamed);
              }
              continue;
            default:
              for (int t = flagArray1.Length - 1; t >= 0; --t)
              {
                for (int c = flagArray2.Length - 1; c >= 0; --c)
                {
                  if (flagArray1[t] && flagArray2[c])
                    this.setTileCompVal(t, c, (object) value_Renamed);
                }
              }
              continue;
          }
      }
    }
  }

  public virtual int getCBlkWidth(byte type, int t, int c)
  {
    int[] numArray = (int[]) null;
    switch (type)
    {
      case 0:
        numArray = (int[]) this.getDefault();
        break;
      case 1:
        numArray = (int[]) this.getCompDef(c);
        break;
      case 2:
        numArray = (int[]) this.getTileDef(t);
        break;
      case 3:
        numArray = (int[]) this.getTileCompVal(t, c);
        break;
    }
    return numArray[0];
  }

  public virtual int getCBlkHeight(byte type, int t, int c)
  {
    int[] numArray = (int[]) null;
    switch (type)
    {
      case 0:
        numArray = (int[]) this.getDefault();
        break;
      case 1:
        numArray = (int[]) this.getCompDef(c);
        break;
      case 2:
        numArray = (int[]) this.getTileDef(t);
        break;
      case 3:
        numArray = (int[]) this.getTileCompVal(t, c);
        break;
    }
    return numArray[1];
  }

  public override void setDefault(object value_Renamed)
  {
    base.setDefault(value_Renamed);
    this.storeHighestDims((int[]) value_Renamed);
  }

  public override void setTileDef(int t, object value_Renamed)
  {
    base.setTileDef(t, value_Renamed);
    this.storeHighestDims((int[]) value_Renamed);
  }

  public override void setCompDef(int c, object value_Renamed)
  {
    base.setCompDef(c, value_Renamed);
    this.storeHighestDims((int[]) value_Renamed);
  }

  public override void setTileCompVal(int t, int c, object value_Renamed)
  {
    base.setTileCompVal(t, c, value_Renamed);
    this.storeHighestDims((int[]) value_Renamed);
  }

  private void storeHighestDims(int[] dim)
  {
    if (dim[0] > this.maxCBlkWidth)
      this.maxCBlkWidth = dim[0];
    if (dim[1] <= this.maxCBlkHeight)
      return;
    this.maxCBlkHeight = dim[1];
  }
}
