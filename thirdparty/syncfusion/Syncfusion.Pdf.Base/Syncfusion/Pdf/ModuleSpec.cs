// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ModuleSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class ModuleSpec
{
  public const byte SPEC_TYPE_COMP = 0;
  public const byte SPEC_TYPE_TILE = 1;
  public const byte SPEC_TYPE_TILE_COMP = 2;
  public const byte SPEC_DEF = 0;
  public const byte SPEC_COMP_DEF = 1;
  public const byte SPEC_TILE_DEF = 2;
  public const byte SPEC_TILE_COMP = 3;
  internal int specType;
  internal int nTiles;
  internal int nComp;
  internal byte[][] specValType;
  internal object def;
  internal object[] compDef;
  internal object[] tileDef;
  internal Dictionary<object, object> tileCompVal;

  public virtual ModuleSpec Copy => (ModuleSpec) this.Clone();

  public virtual object Clone()
  {
    ModuleSpec moduleSpec = (ModuleSpec) null;
    try
    {
      moduleSpec = (ModuleSpec) this.MemberwiseClone();
    }
    catch (Exception ex)
    {
    }
    moduleSpec.specValType = new byte[this.nTiles][];
    for (int index = 0; index < this.nTiles; ++index)
      moduleSpec.specValType[index] = new byte[this.nComp];
    for (int index1 = 0; index1 < this.nTiles; ++index1)
    {
      for (int index2 = 0; index2 < this.nComp; ++index2)
        moduleSpec.specValType[index1][index2] = this.specValType[index1][index2];
    }
    if (this.tileDef != null)
    {
      moduleSpec.tileDef = new object[this.nTiles];
      for (int index = 0; index < this.nTiles; ++index)
        moduleSpec.tileDef[index] = this.tileDef[index];
    }
    if (this.tileCompVal != null)
    {
      moduleSpec.tileCompVal = new Dictionary<object, object>();
      IEnumerator enumerator = (IEnumerator) this.tileCompVal.Keys.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string current = (string) enumerator.Current;
        object obj = this.tileCompVal[(object) current];
        moduleSpec.tileCompVal[(object) current] = obj;
      }
    }
    return (object) moduleSpec;
  }

  public virtual void rotate90(JPXImageCoordinates anT)
  {
    byte[][] numArray = new byte[this.nTiles][];
    JPXImageCoordinates imageCoordinates = new JPXImageCoordinates(anT.y, anT.x);
    for (int index1 = 0; index1 < imageCoordinates.y; ++index1)
    {
      for (int index2 = 0; index2 < imageCoordinates.x; ++index2)
      {
        int num1 = index2;
        int num2 = imageCoordinates.y - index1 - 1;
        numArray[num1 * anT.x + num2] = this.specValType[index1 * imageCoordinates.x + index2];
      }
    }
    this.specValType = numArray;
    if (this.tileDef != null)
    {
      object[] objArray = new object[this.nTiles];
      for (int index3 = 0; index3 < imageCoordinates.y; ++index3)
      {
        for (int index4 = 0; index4 < imageCoordinates.x; ++index4)
        {
          int num3 = index4;
          int num4 = imageCoordinates.y - index3 - 1;
          objArray[num3 * anT.x + num4] = this.tileDef[index3 * imageCoordinates.x + index4];
        }
      }
      this.tileDef = objArray;
    }
    if (this.tileCompVal == null || this.tileCompVal.Count <= 0)
      return;
    Dictionary<object, object> dictionary = new Dictionary<object, object>();
    IEnumerator enumerator = (IEnumerator) this.tileCompVal.Keys.GetEnumerator();
    while (enumerator.MoveNext())
    {
      string current = (string) enumerator.Current;
      object obj = this.tileCompVal[(object) current];
      int num5 = current.IndexOf('t');
      int startIndex = current.IndexOf('c');
      int num6 = int.Parse(current.Substring(num5 + 1, startIndex - (num5 + 1)));
      int num7 = num6 % imageCoordinates.x;
      int num8 = num6 / imageCoordinates.x;
      int num9 = num7;
      int num10 = imageCoordinates.y - num8 - 1 + num9 * anT.x;
      dictionary[(object) $"t{(object) num10}{current.Substring(startIndex)}"] = obj;
    }
    this.tileCompVal = dictionary;
  }

  public ModuleSpec(int nt, int nc, byte type)
  {
    this.nTiles = nt;
    this.nComp = nc;
    this.specValType = new byte[nt][];
    for (int index = 0; index < nt; ++index)
      this.specValType[index] = new byte[nc];
    switch (type)
    {
      case 0:
        this.specType = 0;
        break;
      case 1:
        this.specType = 1;
        break;
      case 2:
        this.specType = 2;
        break;
    }
  }

  public virtual void setDefault(object value_Renamed) => this.def = value_Renamed;

  public virtual object getDefault() => this.def;

  public virtual void setCompDef(int c, object value_Renamed)
  {
    if (this.specType == 1)
    {
      string str = $"Option whose value is '{value_Renamed}' cannot be specified for components as it is a 'tile only' specific option";
    }
    if (this.compDef == null)
      this.compDef = new object[this.nComp];
    for (int index = 0; index < this.nTiles; ++index)
    {
      if (this.specValType[index][c] < (byte) 1)
        this.specValType[index][c] = (byte) 1;
    }
    this.compDef[c] = value_Renamed;
  }

  public virtual object getCompDef(int c)
  {
    int specType = this.specType;
    return this.compDef == null || this.compDef[c] == null ? this.getDefault() : this.compDef[c];
  }

  public virtual void setTileDef(int t, object value_Renamed)
  {
    if (this.specType == 0)
    {
      string str = $"Option whose value is '{value_Renamed}' cannot be specified for tiles as it is a 'component only' specific option";
    }
    if (this.tileDef == null)
      this.tileDef = new object[this.nTiles];
    for (int index = 0; index < this.nComp; ++index)
    {
      if (this.specValType[t][index] < (byte) 2)
        this.specValType[t][index] = (byte) 2;
    }
    this.tileDef[t] = value_Renamed;
  }

  public virtual object getTileDef(int t)
  {
    int specType = this.specType;
    return this.tileDef == null || this.tileDef[t] == null ? this.getDefault() : this.tileDef[t];
  }

  public virtual void setTileCompVal(int t, int c, object value_Renamed)
  {
    if (this.specType != 2)
    {
      string str1 = $"Option whose value is '{value_Renamed}' cannot be specified for ";
      string str2;
      switch (this.specType)
      {
        case 0:
          str2 = str1 + "tiles as it is a 'component only' specific option";
          break;
        case 1:
          str2 = str1 + "components as it is a 'tile only' specific option";
          break;
      }
    }
    if (this.tileCompVal == null)
      this.tileCompVal = new Dictionary<object, object>();
    this.specValType[t][c] = (byte) 3;
    this.tileCompVal[(object) $"t{(object) t}c{(object) c}"] = value_Renamed;
  }

  public virtual object getTileCompVal(int t, int c)
  {
    int specType = this.specType;
    return this.getSpec(t, c);
  }

  internal virtual object getSpec(int t, int c)
  {
    switch (this.specValType[t][c])
    {
      case 0:
        return this.getDefault();
      case 1:
        return this.getCompDef(c);
      case 2:
        return this.getTileDef(t);
      case 3:
        return this.tileCompVal[(object) $"t{(object) t}c{(object) c}"];
      default:
        throw new ArgumentException("Not recognized spec type");
    }
  }

  public virtual byte getSpecValType(int t, int c) => this.specValType[t][c];

  public virtual bool isCompSpecified(int c) => this.compDef != null && this.compDef[c] != null;

  public virtual bool isTileSpecified(int t) => this.tileDef != null && this.tileDef[t] != null;

  public virtual bool isTileCompSpecified(int t, int c)
  {
    if (this.tileCompVal != null)
    {
      if (this.tileCompVal[(object) $"t{(object) t}c{(object) c}"] != null)
        return true;
    }
    return false;
  }

  public static bool[] parseIdx(string word, int maxIdx)
  {
    int length = word.Length;
    char ch = word[0];
    int index1 = -1;
    int num = -1;
    bool flag = false;
    bool[] idx = new bool[maxIdx];
    for (int index2 = 1; index2 < length; ++index2)
    {
      char c = word[index2];
      if (char.IsDigit(c))
      {
        if (index1 == -1)
          index1 = 0;
        index1 = index1 * 10 + ((int) c - 48 /*0x30*/);
      }
      else
      {
        if (index1 == -1 || c != ',' && c != '-')
          throw new ArgumentException("Bad construction for parameter: " + word);
        if (index1 < 0 || index1 >= maxIdx)
          throw new ArgumentException($"Out of range index in parameter `{word}' : {(object) index1}");
        if (c == ',')
        {
          if (flag)
          {
            for (int index3 = num + 1; index3 < index1; ++index3)
              idx[index3] = true;
          }
          flag = false;
        }
        else
          flag = true;
        idx[index1] = true;
        num = index1;
        index1 = -1;
      }
    }
    if (index1 < 0 || index1 >= maxIdx)
      throw new ArgumentException($"Out of range index in parameter `{word}' : {(object) index1}");
    if (flag)
    {
      for (int index4 = num + 1; index4 < index1; ++index4)
        idx[index4] = true;
    }
    idx[index1] = true;
    return idx;
  }
}
