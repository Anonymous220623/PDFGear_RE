// Decompiled with JetBrains decompiler
// Type: XmpCore.Options.Options
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace XmpCore.Options;

public abstract class Options
{
  private int _options;
  private IDictionary<int, string> _optionNames;

  protected Options()
  {
  }

  protected Options(int options)
  {
    this.AssertOptionsValid(options);
    this.SetOptions(options);
  }

  public void Clear() => this._options = 0;

  public bool IsExactly(int optionBits) => this.GetOptions() == optionBits;

  public bool ContainsAllOptions(int optionBits) => (this.GetOptions() & optionBits) == optionBits;

  public bool ContainsOneOf(int optionBits) => (this.GetOptions() & optionBits) != 0;

  protected bool GetOption(int optionBit) => (this._options & optionBit) != 0;

  public void SetOption(int optionBits, bool value)
  {
    this._options = value ? this._options | optionBits : this._options & ~optionBits;
  }

  public int GetOptions() => this._options;

  public void SetOptions(int options)
  {
    this.AssertOptionsValid(options);
    this._options = options;
  }

  public override bool Equals(object obj)
  {
    return obj is XmpCore.Options.Options options && this.GetOptions() == options.GetOptions();
  }

  public override int GetHashCode() => this.GetOptions();

  public string GetOptionsString()
  {
    if (this._options == 0)
      return "<none>";
    StringBuilder stringBuilder = new StringBuilder();
    int num;
    for (int index = this._options; index != 0; index = num)
    {
      num = index & index - 1;
      string optionName = this.GetOptionName(index ^ num);
      stringBuilder.Append(optionName);
      if (num != 0)
        stringBuilder.Append(" | ");
    }
    return stringBuilder.ToString();
  }

  public override string ToString() => $"0x{this._options:X}";

  protected abstract int GetValidOptions();

  protected abstract string DefineOptionName(int option);

  internal virtual void AssertConsistency(int options)
  {
  }

  private void AssertOptionsValid(int options)
  {
    int num = options & ~this.GetValidOptions();
    if (num != 0)
      throw new XmpException($"The option bit(s) 0x{num:X} are invalid!", XmpErrorCode.BadOptions);
    this.AssertConsistency(options);
  }

  private string GetOptionName(int option)
  {
    IDictionary<int, string> dictionary = this.ProcureOptionNames();
    string optionName;
    dictionary.TryGetValue(option, out optionName);
    if (optionName == null)
    {
      optionName = this.DefineOptionName(option);
      if (optionName != null)
        dictionary[option] = optionName;
      else
        optionName = "<option name not defined>";
    }
    return optionName;
  }

  private IDictionary<int, string> ProcureOptionNames()
  {
    return this._optionNames ?? (this._optionNames = (IDictionary<int, string>) new Dictionary<int, string>());
  }
}
