// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.CalcQuickBase
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace Syncfusion.Calculate;

public class CalcQuickBase : ISheetData, ICalcData, IDisposable
{
  private const string LEFTBRACE = "{";
  private const char LEFTBRACKET = '[';
  private const char RIGHTBRACKET = ']';
  private int _calcQuickID;
  private Hashtable _controlModifiedFlags;
  private FormulaInfoHashtable _dataStore;
  private CalcEngine _engine;
  private Hashtable _keyToRowsMap;
  private Hashtable _keyToVectors;
  private Hashtable _nameToControlMap;
  private Hashtable _rowsToKeyMap;
  private bool autoCalc;
  private string cellPrefix = "!0!A";
  private bool checkKeys = true;
  private bool disposeEngineResource = true;
  protected bool ignoreChanges;
  private string TIC = "\"";
  private string VALIDLEFTCHARS = "+-*/><=^(&" + (object) CalcEngine.ParseArgumentSeparator;
  private string VALIDRIGHTCHARS = "+-*/><=^)&" + (object) CalcEngine.ParseArgumentSeparator;

  public CalcQuickBase() => this.InitCalcQuick(false);

  public CalcQuickBase(bool resetStaticMembers) => this.InitCalcQuick(resetStaticMembers);

  public event ValueChangedEventHandler ValueChanged;

  public event QuickValueSetEventHandler ValueSet;

  public bool AutoCalc
  {
    get => this.autoCalc;
    set
    {
      this.autoCalc = value;
      this.Engine.CalculatingSuspended = !value;
      this.Engine.IgnoreValueChanged = !value;
      this.Engine.UseDependencies = value;
      if (!value)
        return;
      this.SetDirty();
    }
  }

  private int calcQuickID
  {
    get
    {
      ++this._calcQuickID;
      if (this._calcQuickID == int.MaxValue)
        this._calcQuickID = 1;
      return this._calcQuickID;
    }
  }

  public bool CheckKeys
  {
    get => this.checkKeys;
    set => this.checkKeys = value;
  }

  protected Hashtable ControlModifiedFlags => this._controlModifiedFlags;

  protected FormulaInfoHashtable DataStore => this._dataStore;

  public bool DisposeEngineResource
  {
    get => this.disposeEngineResource;
    set => this.disposeEngineResource = value;
  }

  public CalcEngine Engine => this._engine;

  public char FormulaCharacter
  {
    get => CalcEngine.FormulaCharacter;
    set => CalcEngine.FormulaCharacter = value;
  }

  protected Hashtable KeyToRowsMap => this._keyToRowsMap;

  protected Hashtable KeyToVectors => this._keyToVectors;

  protected Hashtable NameToControlMap => this._nameToControlMap;

  protected Hashtable RowsToKeyMap => this._rowsToKeyMap;

  public void ResetKeys()
  {
    this.DataStore.Clear();
    this.KeyToRowsMap.Clear();
    this.RowsToKeyMap.Clear();
    this.KeyToVectors.Clear();
    this.NameToControlMap.Clear();
  }

  public string this[string key]
  {
    get
    {
      key = key.ToUpper();
      if (this.DataStore.ContainsKey((object) key))
      {
        FormulaInfo formula1 = this.DataStore[(object) key];
        string formulaText = formula1.FormulaText;
        if (formulaText.Length > 0 && (int) formulaText[0] == (int) CalcEngine.FormulaCharacter && formula1.calcID != this.Engine.GetCalcID())
        {
          this.Engine.cell = this.cellPrefix + this.KeyToRowsMap[(object) key].ToString();
          string formula2 = formulaText.Substring(1);
          try
          {
            formula1.ParsedFormula = this.Engine.ParseFormula(this.MarkKeys(formula2));
          }
          catch (Exception ex)
          {
            if (this.CheckKeys)
            {
              formula1.FormulaValue = ex.Message;
              formula1.calcID = this.Engine.GetCalcID();
              if (this.ValueSet != null)
                this.ValueSet((object) this, new QuickValueSetEventArgs(key, formula1.FormulaValue, FormulaInfoSetAction.CalculatedValueSet));
              return this.DataStore[(object) key].FormulaValue;
            }
          }
          try
          {
            formula1.FormulaValue = this.Engine.ComputeFormula(formula1.ParsedFormula);
          }
          catch (Exception ex)
          {
            if (this.ThrowCircularException)
            {
              if (ex.Message.StartsWith(this.Engine.FormulaErrorStrings[this.Engine.circular_reference_]))
                throw ex;
            }
          }
          if (this.ValueSet != null)
            this.ValueSet((object) this, new QuickValueSetEventArgs(key, formula1.FormulaValue, FormulaInfoSetAction.CalculatedValueSet));
        }
        if (this.Engine.ThrowCircularException && this.Engine.IterationMaxCount > 0)
          formula1.FormulaValue = this.Engine.HandleIteration(this.Engine.cell, formula1);
        return this.DataStore[(object) key].FormulaValue;
      }
      return this.KeyToVectors.ContainsKey((object) key) ? this.KeyToVectors[(object) key].ToString() : string.Empty;
    }
    set
    {
      key = key.ToUpper();
      string str1 = value.ToString().Trim();
      if (!this.DataStore.ContainsKey((object) key) || str1.StartsWith("{"))
      {
        if (str1.StartsWith("{"))
        {
          if (!this.KeyToVectors.ContainsKey((object) key))
            this.KeyToVectors.Add((object) key, (object) string.Empty);
          string str2 = str1.Substring(1, str1.Length - 2);
          int num = this.KeyToRowsMap.Count + 1;
          string[] strArray = str2.Split(CalcEngine.ParseArgumentSeparator);
          string str3 = $"A{num}:A{num + strArray.GetLength(0) - 1}";
          this.KeyToVectors[(object) key] = (object) str3;
          foreach (string formulaText in strArray)
          {
            string key1 = $"Q_{this.KeyToRowsMap.Count + 1}";
            this.DataStore.Add((object) key1, (object) new FormulaInfo());
            this.KeyToRowsMap.Add((object) key1, (object) (this.KeyToRowsMap.Count + 1));
            this.RowsToKeyMap.Add((object) (this.RowsToKeyMap.Count + 1), (object) key1);
            FormulaInfo formulaInfo = this.DataStore[(object) key1];
            formulaInfo.FormulaText = string.Empty;
            formulaInfo.ParsedFormula = string.Empty;
            formulaInfo.FormulaValue = this.ParseAndCompute(formulaText);
          }
          return;
        }
        this.DataStore.Add((object) key, (object) new FormulaInfo());
        this.KeyToRowsMap.Add((object) key, (object) (this.KeyToRowsMap.Count + 1));
        this.RowsToKeyMap.Add((object) (this.RowsToKeyMap.Count + 1), (object) key);
      }
      if (this.KeyToVectors.ContainsKey((object) key))
        this.KeyToVectors.Remove((object) key);
      FormulaInfo formulaInfo1 = this.DataStore[(object) key];
      if (!this.ignoreChanges && formulaInfo1.FormulaText != null && formulaInfo1.FormulaText.Length > 0 && formulaInfo1.FormulaText != str1)
      {
        string str4 = this.cellPrefix + this.KeyToRowsMap[(object) key].ToString();
        if (this.Engine.DependentFormulaCells.ContainsKey((object) str4) && this.Engine.DependentFormulaCells[(object) str4] != null)
          this.Engine.ClearFormulaDependentCells(str4);
      }
      if (str1.Length > 0 && (int) str1[0] == (int) CalcEngine.FormulaCharacter)
      {
        formulaInfo1.FormulaText = str1;
        if (this.ValueSet != null)
          this.ValueSet((object) this, new QuickValueSetEventArgs(key, str1, FormulaInfoSetAction.FormulaSet));
      }
      else if (formulaInfo1.FormulaValue != str1)
      {
        formulaInfo1.FormulaText = string.Empty;
        formulaInfo1.ParsedFormula = string.Empty;
        formulaInfo1.FormulaValue = str1;
        if (this.ValueSet != null)
          this.ValueSet((object) this, new QuickValueSetEventArgs(key, str1, FormulaInfoSetAction.NonFormulaSet));
      }
      if (!this.AutoCalc)
        return;
      this.UpdateDependencies(key);
    }
  }

  public bool ThrowCircularException
  {
    get => this.Engine.ThrowCircularException;
    set => this.Engine.ThrowCircularException = value;
  }

  private bool CheckAdjacentPiece(string s, string validChars, bool first)
  {
    bool flag = true;
    s = s.Trim();
    if (s.Length > 0)
      flag = validChars.IndexOf(s[first ? 0 : s.Length - 1]) > -1;
    return flag;
  }

  public virtual CalcEngine CreateEngine() => new CalcEngine((ICalcData) this);

  public void Dispose()
  {
    this._dataStore = (FormulaInfoHashtable) null;
    this._rowsToKeyMap = (Hashtable) null;
    this._keyToRowsMap = (Hashtable) null;
    this._keyToVectors = (Hashtable) null;
    this._controlModifiedFlags = (Hashtable) null;
    this._nameToControlMap = (Hashtable) null;
    this.ValueChanged -= new ValueChangedEventHandler(this._engine.grid_ValueChanged);
    if (!this.DisposeEngineResource)
      return;
    this._engine.DependentFormulaCells.Clear();
    this._engine.DependentCells.Clear();
    if (this._engine != null)
      this._engine.Dispose();
    this._engine = (CalcEngine) null;
  }

  public string TryParseAndCompute(string formulaText)
  {
    try
    {
      return this.ParseAndCompute(formulaText);
    }
    catch (Exception ex)
    {
      return ex.Message;
    }
  }

  public string GetFormula(string key)
  {
    key = key.ToUpper();
    return this.DataStore.ContainsKey((object) key) ? this.DataStore[(object) key].FormulaText : string.Empty;
  }

  public object GetValueRowCol(int row, int col)
  {
    string valueRowCol = this[this.RowsToKeyMap[(object) row].ToString()].ToString();
    double result;
    if (valueRowCol != null && valueRowCol.EndsWith("%") && valueRowCol.Length > 1 && double.TryParse(valueRowCol.Substring(0, valueRowCol.Length - 1), NumberStyles.Any, (IFormatProvider) null, out result))
      valueRowCol = (result / 100.0).ToString();
    return (object) valueRowCol;
  }

  protected void InitCalcQuick(bool resetStaticMembers)
  {
    this._dataStore = new FormulaInfoHashtable();
    this._rowsToKeyMap = new Hashtable();
    this._keyToRowsMap = new Hashtable();
    this._keyToVectors = new Hashtable();
    this._controlModifiedFlags = new Hashtable();
    this._nameToControlMap = new Hashtable();
    this._engine = this.CreateEngine();
    if (resetStaticMembers)
    {
      CalcEngine.ResetSheetFamilyID();
      this._engine.DependentFormulaCells.Clear();
      this._engine.DependentCells.Clear();
    }
    int sheetFamilyId = CalcEngine.CreateSheetFamilyID();
    this.cellPrefix = $"!{sheetFamilyId}!A";
    this._engine.RegisterGridAsSheet(RangeInfo.GetAlphaLabel(this.calcQuickID), (ICalcData) this, sheetFamilyId);
    this._engine.CalculatingSuspended = true;
    this._engine.IgnoreValueChanged = true;
  }

  private string MarkKeys(string formula)
  {
    int num = formula.IndexOf('[');
    while (num > -1)
    {
      int length = formula.Substring(num).IndexOf(']') - 1;
      string empty = string.Empty;
      if (length > 0)
      {
        string upper = formula.Substring(num + 1, length).ToUpper();
        if (this.KeyToVectors.Contains((object) upper))
        {
          string s1 = num + length + 2 < formula.Length ? formula.Substring(num + length + 2) : string.Empty;
          if (this.CheckKeys && !this.CheckAdjacentPiece(s1, this.VALIDRIGHTCHARS, true))
            throw new ArgumentException($"[{upper}] not followed properly");
          string s2 = num > 0 ? formula.Substring(0, num) : string.Empty;
          if (this.CheckKeys && !this.CheckAdjacentPiece(s2, this.VALIDLEFTCHARS, false))
            throw new ArgumentException($"[{upper}] not preceded properly");
          formula = s2 + this.KeyToVectors[(object) upper].ToString() + s1;
          num = formula.IndexOf('[');
        }
        else if (this.KeyToRowsMap.Contains((object) upper))
        {
          string s3 = num + length + 2 < formula.Length ? formula.Substring(num + length + 2) : string.Empty;
          if (this.CheckKeys && !this.CheckAdjacentPiece(s3, this.VALIDRIGHTCHARS, true))
            throw new ArgumentException($"[{upper}] not followed properly");
          string s4 = num > 0 ? formula.Substring(0, num) : string.Empty;
          if (this.CheckKeys && !this.CheckAdjacentPiece(s4, this.VALIDLEFTCHARS, false))
            throw new ArgumentException($"[{upper}] not preceded properly");
          formula = $"{s4}A{this.KeyToRowsMap[(object) upper].ToString()}{s3}";
          num = formula.IndexOf('[');
        }
        else
        {
          if (formula.ToUpper().IndexOf(this.TIC + (object) '[' + upper + (object) ']' + this.TIC) <= 0)
            throw new ArgumentException("Unknown key: " + upper);
          break;
        }
      }
      else
        num = -1;
    }
    return formula;
  }

  public string ParseAndCompute(string formulaText)
  {
    if (formulaText.Length > 0 && (int) formulaText[0] == (int) CalcEngine.FormulaCharacter)
      formulaText = formulaText.Substring(1);
    return this.Engine.ParseAndComputeFormula(this.MarkKeys(formulaText));
  }

  public void RefreshAllCalculations()
  {
    if (!this.AutoCalc)
      return;
    this.SetDirty();
    this.ignoreChanges = true;
    foreach (string key in (IEnumerable) this.DataStore.Keys)
    {
      FormulaInfo formulaInfo = this.DataStore[(object) key];
      string formulaText = formulaInfo.FormulaText;
      if (formulaText.Length > 0 && (int) formulaText[0] == (int) CalcEngine.FormulaCharacter && formulaInfo.calcID != this.Engine.GetCalcID())
      {
        string formula = formulaText.Substring(1);
        this.Engine.cell = this.cellPrefix + this.KeyToRowsMap[(object) key].ToString();
        formulaInfo.ParsedFormula = this.Engine.ParseFormula(this.MarkKeys(formula));
        formulaInfo.FormulaValue = this.Engine.ComputeFormula(formulaInfo.ParsedFormula);
        formulaInfo.calcID = this.Engine.GetCalcID();
        if (this.ValueChanged != null)
          this.ValueChanged((object) this, new ValueChangedEventArgs((int) this.KeyToRowsMap[(object) key], 1, formulaInfo.FormulaValue));
      }
      if (this.ValueSet != null)
        this.ValueSet((object) this, new QuickValueSetEventArgs(key, formulaInfo.FormulaValue, FormulaInfoSetAction.CalculatedValueSet));
    }
    this.ignoreChanges = false;
  }

  public void SetDirty() => this.Engine.UpdateCalcID();

  public void SetValueRowCol(object value, int row, int col)
  {
  }

  public void UpdateDependencies(string key)
  {
    if (!this.AutoCalc)
      return;
    ArrayList dependentCell = this.Engine.DependentCells[(object) (this.cellPrefix + this.KeyToRowsMap[(object) key].ToString())] as ArrayList;
    this.SetDirty();
    if (dependentCell == null)
      return;
    foreach (string str in dependentCell)
    {
      int num = str.IndexOf('A');
      if (num > -1)
      {
        key = this.RowsToKeyMap[(object) int.Parse(str.Substring(num + 1))].ToString();
        this.ignoreChanges = true;
        this[key] = this[key];
        this.ignoreChanges = false;
      }
    }
  }

  public void WireParentObject()
  {
  }

  public int GetFirstRow() => 1;

  public int GetLastRow() => this.RowsToKeyMap.Count;

  public int GetRowCount() => this.RowsToKeyMap.Count;

  public int GetFirstColumn() => 1;

  public int GetLastColumn() => 25;

  public int GetColumnCount() => 25;
}
