// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.StringSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class StringSpec : ModuleSpec
{
  internal StringSpec(int nt, int nc, byte type)
    : base(nt, nc, type)
  {
  }

  internal StringSpec(
    int nt,
    int nc,
    byte type,
    string optName,
    string[] list,
    JPXParameters pl)
    : base(nt, nc, type)
  {
    string parameter1 = pl.getParameter(optName);
    bool flag = false;
    if (parameter1 == null)
    {
      string parameter2 = pl.DefaultParameterList.getParameter(optName);
      for (int index = list.Length - 1; index >= 0; --index)
      {
        if (parameter2.ToUpper().Equals(list[index].ToUpper()))
          flag = true;
      }
      if (!flag)
        throw new ArgumentException("The Option name is not supported");
      this.setDefault((object) parameter2);
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
            flag = false;
            for (int index = list.Length - 1; index >= 0; --index)
            {
              if (str.ToUpper().Equals(list[index].ToUpper()))
                flag = true;
            }
            if (!flag)
              throw new ArgumentException("The Option name is not supported");
            switch (num1)
            {
              case 0:
                this.setDefault((object) str);
                break;
              case 1:
                for (int c = flagArray2.Length - 1; c >= 0; --c)
                {
                  if (flagArray2[c])
                    this.setCompDef(c, (object) str);
                }
                break;
              case 2:
                for (int t = flagArray1.Length - 1; t >= 0; --t)
                {
                  if (flagArray1[t])
                    this.setTileDef(t, (object) str);
                }
                break;
              default:
                for (int t = flagArray1.Length - 1; t >= 0; --t)
                {
                  for (int c = flagArray2.Length - 1; c >= 0; --c)
                  {
                    if (flagArray1[t] && flagArray2[c])
                      this.setTileCompVal(t, c, (object) str);
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
        for (int index = list.Length - 1; index >= 0; --index)
        {
          if (parameter3.ToUpper().Equals(list[index].ToUpper()))
            flag = true;
        }
        if (!flag)
          throw new ArgumentException($"Default parameter of option -{optName} not recognized: {parameter3}");
        this.setDefault((object) parameter3);
      }
      else
      {
        this.setDefault(this.getSpec(0, 0));
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
}
