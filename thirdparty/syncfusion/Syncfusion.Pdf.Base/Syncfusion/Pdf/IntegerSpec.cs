// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IntegerSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class IntegerSpec : ModuleSpec
{
  internal static int MAX_INT = int.MaxValue;

  internal virtual int Max
  {
    get
    {
      int max = (int) this.def;
      for (int t = 0; t < this.nTiles; ++t)
      {
        for (int c = 0; c < this.nComp; ++c)
        {
          int spec = (int) this.getSpec(t, c);
          if (max < spec)
            max = spec;
        }
      }
      return max;
    }
  }

  internal virtual int Min
  {
    get
    {
      int min = (int) this.def;
      for (int t = 0; t < this.nTiles; ++t)
      {
        for (int c = 0; c < this.nComp; ++c)
        {
          int spec = (int) this.getSpec(t, c);
          if (min > spec)
            min = spec;
        }
      }
      return min;
    }
  }

  internal IntegerSpec(int nt, int nc, byte type)
    : base(nt, nc, type)
  {
  }

  internal IntegerSpec(int nt, int nc, byte type, JPXParameters pl, string optName)
    : base(nt, nc, type)
  {
    string parameter1 = pl.getParameter(optName);
    if (parameter1 == null)
    {
      string parameter2 = pl.DefaultParameterList.getParameter(optName);
      try
      {
        this.setDefault((object) int.Parse(parameter2));
      }
      catch (FormatException ex)
      {
        throw new ArgumentException("OptionName not supported");
      }
    }
    else
    {
      SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(parameter1);
      byte num1 = 0;
      bool[] flagArray1 = (bool[]) null;
      bool[] flagArray2 = (bool[]) null;
      while (tokenizer.HasMoreTokens())
      {
        string str = tokenizer.NextToken();
        switch (str[0])
        {
          case 'c':
            flagArray2 = ModuleSpec.parseIdx(str, this.nComp);
            num1 = num1 != (byte) 2 ? (byte) 1 : (byte) 3;
            continue;
          case 't':
            flagArray1 = ModuleSpec.parseIdx(str, this.nTiles);
            num1 = num1 != (byte) 1 ? (byte) 2 : (byte) 3;
            continue;
          default:
            int value_Renamed;
            try
            {
              value_Renamed = int.Parse(str);
            }
            catch (FormatException ex)
            {
              throw new ArgumentException($"Non recognized value for option -{optName}: {str}");
            }
            switch (num1)
            {
              case 0:
                this.setDefault((object) value_Renamed);
                break;
              case 1:
                for (int c = flagArray2.Length - 1; c >= 0; --c)
                {
                  if (flagArray2[c])
                    this.setCompDef(c, (object) value_Renamed);
                }
                break;
              case 2:
                for (int t = flagArray1.Length - 1; t >= 0; --t)
                {
                  if (flagArray1[t])
                    this.setTileDef(t, (object) value_Renamed);
                }
                break;
              default:
                for (int t = flagArray1.Length - 1; t >= 0; --t)
                {
                  for (int c = flagArray2.Length - 1; c >= 0; --c)
                  {
                    if (flagArray1[t] && flagArray2[c])
                      this.setTileCompVal(t, c, (object) value_Renamed);
                  }
                }
                break;
            }
            num1 = (byte) 0;
            flagArray1 = (bool[]) null;
            flagArray2 = (bool[]) null;
            continue;
        }
      }
      if (this.getDefault() != null)
        return;
      int num2 = 0;
      for (int index1 = nt - 1; index1 >= 0; --index1)
      {
        for (int index2 = nc - 1; index2 >= 0; --index2)
        {
          if (this.specValType[index1][index2] == (byte) 0)
            ++num2;
        }
      }
      if (num2 != 0)
      {
        string parameter3 = pl.DefaultParameterList.getParameter(optName);
        try
        {
          this.setDefault((object) int.Parse(parameter3));
        }
        catch (FormatException ex)
        {
          throw new ArgumentException($"Non recognized value for option -{optName}: {parameter3}");
        }
      }
      else
      {
        this.setDefault(this.getTileCompVal(0, 0));
        switch (this.specValType[0][0])
        {
          case 1:
            for (int index = nt - 1; index >= 0; --index)
            {
              if (this.specValType[index][0] == (byte) 1)
                this.specValType[index][0] = (byte) 0;
            }
            this.compDef[0] = (object) null;
            break;
          case 2:
            for (int index = nc - 1; index >= 0; --index)
            {
              if (this.specValType[0][index] == (byte) 2)
                this.specValType[0][index] = (byte) 0;
            }
            this.tileDef[0] = (object) null;
            break;
          case 3:
            this.specValType[0][0] = (byte) 0;
            this.tileCompVal[(object) "t0c0"] = (object) null;
            break;
        }
      }
    }
  }

  internal virtual int getMaxInComp(int c)
  {
    int maxInComp = 0;
    for (int t = 0; t < this.nTiles; ++t)
    {
      int spec = (int) this.getSpec(t, c);
      if (maxInComp < spec)
        maxInComp = spec;
    }
    return maxInComp;
  }

  internal virtual int getMinInComp(int c)
  {
    int minInComp = IntegerSpec.MAX_INT;
    for (int t = 0; t < this.nTiles; ++t)
    {
      int spec = (int) this.getSpec(t, c);
      if (minInComp > spec)
        minInComp = spec;
    }
    return minInComp;
  }

  internal virtual int getMaxInTile(int t)
  {
    int maxInTile = 0;
    for (int c = 0; c < this.nComp; ++c)
    {
      int spec = (int) this.getSpec(t, c);
      if (maxInTile < spec)
        maxInTile = spec;
    }
    return maxInTile;
  }

  internal virtual int getMinInTile(int t)
  {
    int minInTile = IntegerSpec.MAX_INT;
    for (int c = 0; c < this.nComp; ++c)
    {
      int spec = (int) this.getSpec(t, c);
      if (minInTile > spec)
        minInTile = spec;
    }
    return minInTile;
  }
}
