// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.FormulaParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class FormulaParser
{
  private const string DEF_IF_FUNCTION = "IF";
  private const int DEF_SPACE_OPTIONS = 64 /*0x40*/;
  private const int DEF_SPACE_DATA = 256 /*0x0100*/;
  private const int DDELinkNameOptions = 32738;
  private const char AbsoluteCellReference = '$';
  private const RegexOptions DEF_REGEX = RegexOptions.Compiled;
  private FormulaTokenizer m_tokenizer;
  private List<Ptg> m_arrTokens = new List<Ptg>();
  private Stack<AttrPtg> m_tokenSpaces = new Stack<AttrPtg>();
  private Stack<AttrPtg> m_operandTokenSpaces = new Stack<AttrPtg>();
  private WorkbookImpl m_book;

  public FormulaParser(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
    this.m_tokenizer = new FormulaTokenizer(book);
  }

  public void SetSeparators(char operandsSeparator, char arrayRowsSeparator)
  {
    this.m_tokenizer.ArgumentSeparator = operandsSeparator;
  }

  public void Parse(
    string formula,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    formula = formula != null ? formula.TrimEnd(' ') : throw new ArgumentNullException(nameof (formula));
    if (formula.Length == 0)
      throw new ArgumentException("formula - string cannot be empty.");
    this.m_tokenizer.NumberFormat = this.m_book.AppImplementation.CheckAndApplySeperators().NumberFormat;
    this.m_tokenizer.PreviousTokenType = FormulaToken.None;
    this.m_tokenizer.TokenType = FormulaToken.None;
    this.m_arrTokens.Clear();
    this.m_tokenSpaces.Clear();
    this.m_tokenizer.Prepare(formula);
    this.m_tokenizer.NextToken();
    AttrPtg attrPtg1 = (AttrPtg) null;
    this.ParseExpression(Priority.None, indexes, i, options, arguments);
    if (this.m_tokenSpaces.Count <= 0)
      return;
    AttrPtg attrPtg2 = this.m_tokenSpaces.Pop();
    attrPtg2.SpaceAfterToken = true;
    this.m_arrTokens.Add((Ptg) attrPtg2);
    attrPtg1 = (AttrPtg) null;
  }

  private Ptg ParseExpression(
    Priority priority,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    Ptg expression = this.ParseFirstOperand(priority, indexes, i, ref options, arguments);
    this.m_arrTokens.Add(expression);
    AttrPtg tokenSpace = (AttrPtg) null;
    if (expression == null)
      this.m_tokenizer.RaiseUnexpectedToken("No Expression found");
    while (true)
    {
      FormulaToken tokenType1 = this.m_tokenizer.TokenType;
      switch (tokenType1)
      {
        case FormulaToken.tAdd:
          if (priority < Priority.PlusMinus)
          {
            this.m_tokenizer.HasSubtract = true;
            this.m_tokenizer.NextToken();
            this.ParseExpression(Priority.PlusMinus, indexes, i, options, arguments);
            expression = this.CreateBinaryOperation(tokenType1, ref tokenSpace);
            this.m_tokenizer.HasSubtract = false;
            break;
          }
          goto label_5;
        case FormulaToken.tSub:
          if (priority < Priority.PlusMinus)
          {
            this.m_tokenizer.HasSubtract = true;
            this.m_tokenizer.NextToken();
            this.ParseExpression(Priority.PlusMinus, indexes, i, options, arguments);
            expression = this.CreateBinaryOperation(tokenType1, ref tokenSpace);
            this.m_tokenizer.HasSubtract = false;
            break;
          }
          goto label_10;
        case FormulaToken.tMul:
        case FormulaToken.tDiv:
          if (priority < Priority.MulDiv)
          {
            this.UpdateOptions(ref options);
            this.m_tokenizer.NextToken();
            this.ParseExpression(Priority.MulDiv, indexes, i, options, arguments);
            expression = this.CreateBinaryOperation(tokenType1, ref tokenSpace);
            break;
          }
          goto label_27;
        case FormulaToken.tPower:
          if (priority < Priority.Power)
          {
            this.m_tokenizer.NextToken();
            this.ParseExpression(Priority.Power, indexes, i, options, arguments);
            expression = this.CreateBinaryOperation(FormulaToken.tPower, ref tokenSpace);
            break;
          }
          goto label_16;
        case FormulaToken.tConcat:
          if (priority < Priority.Concat)
          {
            if (this.m_operandTokenSpaces.Contains(tokenSpace) || tokenSpace == null && this.m_operandTokenSpaces.Count > 0)
              tokenSpace = this.m_operandTokenSpaces.Pop();
            this.m_tokenizer.NextToken();
            this.ParseExpression(Priority.Concat, indexes, i, options, arguments);
            expression = this.CreateBinaryOperation(FormulaToken.tConcat, ref tokenSpace);
            break;
          }
          goto label_19;
        case FormulaToken.tLessThan:
        case FormulaToken.tLessEqual:
        case FormulaToken.tEqual:
        case FormulaToken.tGreaterEqual:
        case FormulaToken.tGreater:
        case FormulaToken.tNotEqual:
          if (priority < Priority.Equality)
          {
            FormulaToken tokenType2 = this.m_tokenizer.TokenType;
            this.m_tokenizer.NextToken();
            this.ParseExpression(Priority.Equality, indexes, i, options, arguments);
            expression = this.CreateBinaryOperation(tokenType2, ref tokenSpace);
            break;
          }
          goto label_33;
        case FormulaToken.tCellRange:
          if (priority < Priority.CellRange)
          {
            this.UpdateOptions(ref options);
            this.m_tokenizer.NextToken();
            this.ParseExpression(Priority.CellRange, indexes, i, options, arguments);
            expression = this.CreateBinaryOperation(FormulaToken.tCellRange, ref tokenSpace);
            break;
          }
          goto label_24;
        case FormulaToken.tPercent:
          this.m_tokenizer.NextToken();
          expression = (Ptg) new UnaryOperationPtg("%");
          this.m_arrTokens.Add(expression);
          break;
        case FormulaToken.tParentheses:
          goto label_39;
        case FormulaToken.tError:
          if (tokenSpace == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          expression = this.ParseError(indexes, i, options, arguments.Worksheet);
          this.m_tokenizer.NextToken();
          break;
        case FormulaToken.EndOfFormula:
          goto label_3;
        case FormulaToken.CloseParenthesis:
          goto label_29;
        case FormulaToken.Comma:
          if ((options & (ExcelParseFormulaOptions.ParseOperand | ExcelParseFormulaOptions.ParseComplexOperand)) == ExcelParseFormulaOptions.None)
          {
            this.m_tokenizer.NextToken();
            this.ParseExpression(Priority.None, indexes, i, options, arguments);
            expression = (Ptg) new CellRangeListPtg(this.m_tokenizer.ArgumentSeparator.ToString());
            this.m_arrTokens.Add(expression);
            break;
          }
          goto label_38;
        case FormulaToken.Space:
          tokenSpace = this.ParseSpaces(indexes, i, options, arguments);
          this.m_operandTokenSpaces.Push(tokenSpace);
          continue;
        default:
          this.m_tokenizer.RaiseUnexpectedToken("Unexpected token.");
          break;
      }
      tokenSpace = (AttrPtg) null;
    }
label_3:
    return expression;
label_5:
    if (tokenSpace != null)
    {
      this.m_tokenSpaces.Push(tokenSpace);
      tokenSpace = (AttrPtg) null;
    }
    return expression;
label_10:
    if (tokenSpace != null)
    {
      this.m_tokenSpaces.Push(tokenSpace);
      tokenSpace = (AttrPtg) null;
    }
    return expression;
label_16:
    return expression;
label_19:
    return expression;
label_24:
    return expression;
label_27:
    return expression;
label_29:
    if (tokenSpace != null)
    {
      this.m_tokenSpaces.Push(tokenSpace);
      tokenSpace = (AttrPtg) null;
    }
    return expression;
label_33:
    return expression;
label_38:
    return expression;
label_39:
    return expression;
  }

  private Ptg CreateBinaryOperation(FormulaToken tokenType, ref AttrPtg tokenSpace)
  {
    if (tokenSpace != null)
    {
      this.m_arrTokens.Add((Ptg) tokenSpace);
      tokenSpace = (AttrPtg) null;
    }
    else if (this.m_tokenSpaces.Count > 0)
      this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
    Ptg ptgByType = FormulaUtil.CreatePtgByType(tokenType);
    this.m_arrTokens.Add(ptgByType);
    return ptgByType;
  }

  private AttrPtg ParseSpaces(
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    int length = this.m_tokenizer.TokenString.Length;
    Ptg ptg = (Ptg) null;
    bool flag = true;
    int spaceCount;
    switch (this.m_tokenizer.PreviousTokenType)
    {
      case FormulaToken.tError:
      case FormulaToken.CloseParenthesis:
      case FormulaToken.Identifier:
      case FormulaToken.Identifier3D:
        this.m_tokenizer.NextToken();
        flag = false;
        switch (this.m_tokenizer.TokenType)
        {
          case FormulaToken.tParentheses:
          case FormulaToken.tError:
          case FormulaToken.tFunction1:
          case FormulaToken.Identifier:
          case FormulaToken.Identifier3D:
            spaceCount = length - 1;
            this.ParseExpression(Priority.None, indexes, i, options, arguments);
            ptg = FormulaUtil.CreatePtgByType(FormulaToken.tCellRangeIntersection);
            this.m_arrTokens.Add(ptg);
            break;
          default:
            goto label_3;
        }
        break;
      default:
label_3:
        spaceCount = length;
        if (flag)
        {
          this.m_tokenizer.NextToken();
          break;
        }
        break;
    }
    AttrPtg spaces = (AttrPtg) null;
    if (spaceCount > 0)
    {
      spaces = this.CreateSpaceToken(spaceCount);
      if (ptg != null)
      {
        this.m_arrTokens.Insert(this.m_arrTokens.Count - 1, (Ptg) spaces);
        spaces = (AttrPtg) null;
      }
    }
    return spaces;
  }

  private AttrPtg CreateSpaceToken(int spaceCount)
  {
    AttrPtg ptg = (AttrPtg) FormulaUtil.CreatePtg(FormulaToken.tAttr, (ushort) 64 /*0x40*/, (ushort) 256 /*0x0100*/);
    ptg.SpaceCount = spaceCount;
    return ptg;
  }

  private Ptg ParseFirstOperand(
    Priority priority,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ref ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    Ptg firstOperand = (Ptg) null;
    AttrPtg attrPtg = (AttrPtg) null;
    bool flag;
    do
    {
      flag = false;
      Ptg expression;
      switch (this.m_tokenizer.TokenType)
      {
        case FormulaToken.tAdd:
          this.m_tokenizer.NextToken();
          if (attrPtg == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          if ((options & ExcelParseFormulaOptions.ParseOperand) != ExcelParseFormulaOptions.None)
          {
            options -= ExcelParseFormulaOptions.ParseOperand;
            options |= ExcelParseFormulaOptions.ParseComplexOperand;
          }
          expression = this.ParseExpression(Priority.UnaryMinus, indexes, i, options, arguments);
          firstOperand = (Ptg) new UnaryOperationPtg("+");
          break;
        case FormulaToken.tSub:
          this.m_tokenizer.NextToken();
          if (attrPtg == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          if ((options & ExcelParseFormulaOptions.ParseOperand) != ExcelParseFormulaOptions.None)
          {
            options -= ExcelParseFormulaOptions.ParseOperand;
            options |= ExcelParseFormulaOptions.ParseComplexOperand;
          }
          expression = this.ParseExpression(Priority.UnaryMinus, indexes, i, options, arguments);
          firstOperand = (Ptg) new UnaryOperationPtg("-");
          break;
        case FormulaToken.tParentheses:
          if (attrPtg != null)
            this.m_arrTokens.RemoveAt(this.m_arrTokens.Count - 1);
          this.m_tokenizer.NextToken();
          ExcelParseFormulaOptions options1 = options & ~ExcelParseFormulaOptions.ParseOperand;
          expression = this.ParseExpression(Priority.None, indexes, i, options1, arguments);
          if (this.m_tokenizer.TokenType != FormulaToken.CloseParenthesis)
            this.m_tokenizer.RaiseUnexpectedToken("End parenthesis not found");
          if (attrPtg != null)
            this.m_arrTokens.Add((Ptg) attrPtg);
          else if (this.m_tokenSpaces.Count > 0)
          {
            attrPtg = this.m_tokenSpaces.Pop();
            attrPtg.SpaceAfterToken = true;
            this.m_arrTokens.Add((Ptg) attrPtg);
          }
          firstOperand = (Ptg) new ParenthesesPtg();
          this.m_tokenizer.NextToken();
          break;
        case FormulaToken.tStringConstant:
          if (attrPtg == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          firstOperand = (Ptg) new StringConstantPtg(this.m_tokenizer.TokenString);
          this.m_tokenizer.NextToken();
          break;
        case FormulaToken.tError:
          if (attrPtg == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          firstOperand = this.ParseError(indexes, i, options, arguments.Worksheet);
          this.m_tokenizer.NextToken();
          break;
        case FormulaToken.tInteger:
          ushort result;
          if (ushort.TryParse(this.m_tokenizer.TokenString, NumberStyles.Float, (IFormatProvider) this.m_tokenizer.NumberFormat, out result))
          {
            firstOperand = (Ptg) new IntegerPtg(result);
            if (attrPtg == null && this.m_tokenSpaces.Count > 0)
              this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
            this.m_tokenizer.NextToken();
            this.UpdateOptions(ref options);
            break;
          }
          goto case FormulaToken.tNumber;
        case FormulaToken.tNumber:
          try
          {
            firstOperand = (Ptg) new DoublePtg(double.Parse(this.m_tokenizer.TokenString, NumberStyles.Float, (IFormatProvider) this.m_tokenizer.NumberFormat));
          }
          catch (Exception ex)
          {
            this.m_tokenizer.RaiseException($"Invalid number {this.m_tokenizer.TokenString}", ex);
          }
          if (attrPtg == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          this.m_tokenizer.NextToken();
          this.UpdateOptions(ref options);
          break;
        case FormulaToken.tArray1:
          if (attrPtg == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          firstOperand = this.ParseArray(ArrayPtg.IndexToCode(FormulaUtil.GetIndex(typeof (ArrayPtg), 1, indexes, i, options)), arguments);
          this.m_tokenizer.NextToken();
          break;
        case FormulaToken.tFunction1:
          firstOperand = this.ParseFunction(indexes, i, options, arguments);
          break;
        case FormulaToken.Comma:
          if (attrPtg == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          firstOperand = FormulaUtil.CreatePtg(FormulaToken.tMissingArgument);
          break;
        case FormulaToken.ValueTrue:
          firstOperand = (Ptg) new BooleanPtg(true);
          this.m_tokenizer.NextToken();
          break;
        case FormulaToken.ValueFalse:
          if (attrPtg == null && this.m_tokenSpaces.Count > 0)
            this.m_arrTokens.Add((Ptg) this.m_tokenSpaces.Pop());
          firstOperand = (Ptg) new BooleanPtg(false);
          this.m_tokenizer.NextToken();
          break;
        case FormulaToken.Space:
          attrPtg = (AttrPtg) FormulaUtil.CreatePtg(FormulaToken.tAttr, (ushort) 64 /*0x40*/, (ushort) 256 /*0x0100*/);
          attrPtg.SpaceCount = this.m_tokenizer.TokenString.Length;
          this.m_arrTokens.Add((Ptg) attrPtg);
          this.m_tokenizer.NextToken();
          flag = true;
          firstOperand = (Ptg) attrPtg;
          break;
        case FormulaToken.Identifier:
        case FormulaToken.Identifier3D:
          string tokenString = this.m_tokenizer.TokenString;
          if (this.m_tokenizer.TokenString == "Overview!#REF!")
            this.m_tokenizer.TokenType = FormulaToken.tError;
          this.m_tokenizer.NextToken();
          this.UpdateOptions(ref options);
          firstOperand = this.ParseIdentifier(tokenString, indexes, i, options, arguments);
          break;
        case FormulaToken.DDELink:
          firstOperand = this.ParseDDELink(indexes, i, options, arguments);
          break;
      }
    }
    while (flag);
    return firstOperand;
  }

  private void UpdateOptions(ref ExcelParseFormulaOptions options)
  {
    if ((options & ExcelParseFormulaOptions.ParseOperand) == ExcelParseFormulaOptions.None)
      return;
    switch (this.m_tokenizer.TokenType)
    {
      case FormulaToken.tCellRange:
        break;
      case FormulaToken.EndOfFormula:
        break;
      case FormulaToken.CloseParenthesis:
        break;
      case FormulaToken.Comma:
        break;
      case FormulaToken.Space:
        break;
      default:
        options -= ExcelParseFormulaOptions.ParseOperand;
        options |= ExcelParseFormulaOptions.ParseComplexOperand;
        break;
    }
  }

  private Ptg ParseDDELink(
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    string tokenString1 = this.m_tokenizer.TokenString;
    this.m_tokenizer.NextToken();
    string tokenString2 = this.m_tokenizer.TokenString;
    this.m_tokenizer.NextToken();
    string strName = this.m_tokenizer.TokenString;
    if (!strName.Contains("'"))
      strName = $"'{strName}'";
    this.m_tokenizer.TokenType = FormulaToken.None;
    this.m_tokenizer.NextToken();
    return this.CreateDDELink(tokenString1, tokenString2, strName, indexes, i, options, arguments);
  }

  private Ptg CreateDDELink(
    string strDDELink,
    string strParamName,
    string strName,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    string str = $"{strDDELink}|{strParamName}";
    ExternBookCollection externWorkbooks = ((WorkbookImpl) arguments.Workbook).ExternWorkbooks;
    ExternWorkbookImpl externWorkbookImpl = externWorkbooks[str];
    int index1;
    if (externWorkbookImpl == null)
    {
      index1 = externWorkbooks.AddDDEFile(str);
      externWorkbookImpl = externWorkbooks[index1];
    }
    else
      index1 = externWorkbookImpl.Index;
    ExternNamesCollection externNames = externWorkbookImpl.ExternNames;
    int index2 = externNames.GetNameIndex(strName);
    if (index2 == -1)
      index2 = externNames.Add(strName);
    ExternNameImpl externNameImpl = externNames[index2];
    externNameImpl.Record.Options = (ushort) 32738;
    externNameImpl.isAdvise = true;
    externWorkbookImpl.IsDdeLink = true;
    NameXPtg ptg = (NameXPtg) FormulaUtil.CreatePtg(NameXPtg.IndexToCode(FormulaUtil.GetIndex(typeof (ArrayPtg), 1, indexes, i, options)));
    ptg.NameIndex = (ushort) (index2 + 1);
    ptg.RefIndex = (ushort) index1;
    return (Ptg) ptg;
  }

  private Ptg ParseIdentifier(
    string identifier,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    switch (identifier)
    {
      case null:
        throw new ArgumentNullException(nameof (identifier));
      case "":
        throw new ArgumentException("identifier - string cannot be empty.");
      default:
        Ptg identifier1 = (Ptg) null;
        int iRefIndex = -1;
        if (this.m_tokenizer.PreviousTokenType == FormulaToken.Identifier3D)
        {
          int length1 = identifier.LastIndexOf('!');
          if (length1 <= 0)
          {
            if (arguments.CellRow != 0 && arguments.CellColumn != 0)
              throw new ArgumentOutOfRangeException(nameof (identifier));
          }
          else
          {
            string location = identifier.Substring(0, length1);
            identifier = identifier.Substring(length1 + 1);
            if (identifier == string.Empty && location.LastIndexOf('!') >= 0)
            {
              int length2 = location.LastIndexOf('!');
              identifier = location.Substring(length2 + 1);
              location = location.Substring(0, length2);
            }
            iRefIndex = this.ConvertLocationIntoReference(location, arguments);
          }
        }
        if (!this.TryGetNamedRange(identifier, arguments, indexes, i, options, iRefIndex, out identifier1) && !this.TryCreateRange(identifier, indexes, i, options, arguments, out identifier1, iRefIndex))
          identifier1 = this.CreateNamedRange(identifier, arguments, indexes, i, options, iRefIndex);
        return identifier1;
    }
  }

  private bool TryGetNamedRange(
    string strToken,
    ParseParameters arguments,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    int iRefIndex,
    out Ptg result)
  {
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    if (strToken == null || strToken.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (strToken));
    WorkbookImpl workbook = (WorkbookImpl) arguments.Workbook;
    result = (Ptg) null;
    if (iRefIndex < 0 || workbook.IsLocalReference(iRefIndex))
    {
      IWorksheet worksheet = arguments.Worksheet;
      IName name1;
      if (iRefIndex == -1)
      {
        IName name2;
        name1 = worksheet != null ? worksheet.Names[strToken] : (name2 = workbook.Names[strToken]);
      }
      else
      {
        IWorksheet sheetByReference = workbook.GetSheetByReference(iRefIndex, false);
        name1 = (sheetByReference != null ? sheetByReference.Names : workbook.Names)[strToken];
      }
      if (name1 != null)
        result = this.CreateNameToken(iRefIndex, (name1 as NameImpl).Index, arguments, indexes, i, options);
    }
    else
    {
      int bookIndex = workbook.GetBookIndex(iRefIndex);
      int nameIndex = workbook.ExternWorkbooks[bookIndex].ExternNames.GetNameIndex(strToken);
      if (nameIndex >= 0)
        result = this.CreateNameToken(iRefIndex, nameIndex, arguments, indexes, i, options);
    }
    return result != null;
  }

  private int ConvertLocationIntoReference(string location, ParseParameters arguments)
  {
    int length1 = location.Length;
    if (location[0] == '\'' && location[length1 - 1] == '\'')
      location = location.Substring(1, length1 - 2);
    string str1 = (string) null;
    string strBookPath = (string) null;
    int length2 = location.IndexOf('[');
    int num1 = location.IndexOf(']');
    int startIndex1 = num1 > 0 ? num1 + 1 : 0;
    if (length2 > 0)
      strBookPath = location.Substring(0, length2);
    if (num1 > 0)
      str1 = location.Substring(length2 + 1, num1 - length2 - 1);
    string str2 = location.Substring(startIndex1);
    WorkbookImpl workbook = (WorkbookImpl) arguments.Workbook;
    int result = -1;
    string[] array = new string[6]
    {
      ".xlsx",
      ".xls",
      ".csv",
      ".xltx",
      ".xltm",
      ".xlsm"
    };
    int startIndex2 = str2.IndexOf(".");
    string str3 = string.Empty;
    if (startIndex2 != -1)
      str3 = str2.Substring(startIndex2, str2.Length - startIndex2);
    if (str1 == null && (workbook.GetFileName(workbook.FullFileName) != str2 && Array.IndexOf<string>(array, str3) >= 0 || workbook.ExternWorkbooks[str2] != null))
    {
      str1 = str2;
      str2 = string.Empty;
    }
    int num2;
    if (str1 == null)
    {
      num2 = workbook.AddSheetReference(str2);
    }
    else
    {
      ExternWorkbookImpl externWorkbookImpl;
      if (workbook.Loading && strBookPath == null && int.TryParse(str1, out result))
      {
        if (result == 0)
          result = workbook.ExternWorkbooks.InsertSelfSupbook() + 1;
        externWorkbookImpl = workbook.ExternWorkbooks[result - 1];
      }
      else
      {
        if (string.IsNullOrEmpty(str1))
          str1 = location.Substring(startIndex1);
        if (str1 == this.m_book.FullFileName)
        {
          int index = workbook.ExternWorkbooks.InsertSelfSupbook();
          externWorkbookImpl = workbook.ExternWorkbooks[index];
        }
        else
          externWorkbookImpl = workbook.ExternWorkbooks.FindOrAdd(str1, strBookPath);
      }
      int num3 = str2 == null || str2.Length <= 0 ? 65534 : externWorkbookImpl.FindOrAddSheet(str2);
      num2 = this.m_book.AddSheetReference(externWorkbookImpl.Index, num3, num3);
    }
    return num2;
  }

  private Ptg ParseFunction(
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    if ((options & ExcelParseFormulaOptions.ParseComplexOperand) != ExcelParseFormulaOptions.None)
    {
      options -= ExcelParseFormulaOptions.ParseComplexOperand;
      options |= ExcelParseFormulaOptions.ParseOperand;
    }
    string upper = this.m_tokenizer.TokenString.ToUpper();
    Ptg result = (Ptg) null;
    AttrPtg spaceBeforeIf = (AttrPtg) null;
    if (this.m_tokenizer.PreviousTokenType == FormulaToken.Space)
    {
      int index = this.m_arrTokens.Count - 1;
      if (this.m_arrTokens[index] is AttrPtg)
      {
        AttrPtg arrToken = (AttrPtg) this.m_arrTokens[index];
        if (!arrToken.SpaceAfterToken)
        {
          spaceBeforeIf = arrToken;
          this.m_arrTokens.RemoveAt(index);
        }
      }
    }
    if (upper == "IF")
    {
      result = this.CreateIFFunction(indexes, i, options, arguments, spaceBeforeIf);
      spaceBeforeIf = (AttrPtg) null;
    }
    else
    {
      ExcelFunction functionId;
      if (FormulaUtil.FunctionAliasToId.TryGetValue(upper, out functionId))
        result = !this.IsFunctionSupported(functionId, this.m_book.Version) ? this.CreateCustomFunction(indexes, i, options, arguments, true) : this.CreateFunction(functionId, indexes, i, options, arguments);
      else if (FormulaUtil.IsCustomFunction(this.m_tokenizer.TokenString, arguments.Workbook, out int _, out int _) || !this.TryCreateFunction2007(ref result, indexes, i, options, arguments))
      {
        result = this.CreateCustomFunction(indexes, i, options, arguments, false);
        spaceBeforeIf = (AttrPtg) null;
      }
    }
    if (spaceBeforeIf != null)
      this.m_arrTokens.Add((Ptg) spaceBeforeIf);
    else if (this.m_tokenSpaces.Count > 0)
    {
      AttrPtg attrPtg = this.m_tokenSpaces.Pop();
      attrPtg.AttrData1 = 4;
      this.m_arrTokens.Add((Ptg) attrPtg);
    }
    return result;
  }

  private bool IsFunctionSupported(ExcelFunction functionId, ExcelVersion excelVersion)
  {
    bool flag = true;
    if (excelVersion < ExcelVersion.Excel2016)
    {
      if (FormulaUtil.IsExcel2016Function(functionId))
        flag = false;
      else if (excelVersion < ExcelVersion.Excel2013)
      {
        if (FormulaUtil.IsExcel2013Function(functionId))
          flag = false;
        else if (excelVersion < ExcelVersion.Excel2010)
        {
          if (FormulaUtil.IsExcel2010Function(functionId))
            flag = false;
          else if (excelVersion < ExcelVersion.Excel2007)
          {
            if (FormulaUtil.IsExcel2007Function(functionId))
              flag = false;
            if (this.m_book.ExternWorkbooks.ContainsExternName(functionId.ToString()))
              flag = false;
          }
        }
      }
    }
    return flag;
  }

  private bool TryCreateFunction2007(
    ref Ptg result,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    string upper = this.m_tokenizer.TokenString.ToUpper();
    if (arguments.Workbook.Version == ExcelVersion.Excel97to2003 || !Enum.IsDefined(typeof (Excel2007Function), (object) upper))
      return false;
    Excel2007Function functionId = (Excel2007Function) Enum.Parse(typeof (Excel2007Function), upper, true);
    result = this.CreateFunction((ExcelFunction) functionId, indexes, i, options, arguments);
    return true;
  }

  private bool TryCreateRange(
    string strFormula,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments,
    out Ptg resultToken,
    int iRefIndex)
  {
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    if (strFormula == null)
      throw new ArgumentNullException(nameof (strFormula));
    bool range = true;
    resultToken = (Ptg) null;
    IWorksheet worksheet = arguments.Worksheet;
    WorkbookImpl workbook = (WorkbookImpl) arguments.Workbook;
    bool isR1C1 = arguments.IsR1C1;
    int cellRow = arguments.CellRow;
    int cellColumn = arguments.CellColumn;
    Dictionary<string, string> worksheetNames = arguments.WorksheetNames;
    string str;
    string strParam2;
    string strRow2;
    string strColumn2;
    if (this.m_book.FormulaUtil.IsCellRange(strFormula, isR1C1, out str, out strParam2, out strRow2, out strColumn2))
    {
      FormulaToken code = AreaPtg.IndexToCode(FormulaUtil.GetIndex(typeof (AreaPtg), 0, indexes, i, options));
      resultToken = FormulaUtil.CreatePtg(code, cellRow, cellColumn, str, strParam2, strRow2, strColumn2, isR1C1, (IWorkbook) workbook);
    }
    else if (FormulaUtil.IsCell(strFormula, isR1C1, out str, out strParam2))
    {
      bool flag = false;
      if (strParam2 != null && str != null)
        flag = strParam2.Contains('$'.ToString()) || str.Contains('$'.ToString());
      FormulaToken token = flag || indexes == null || !indexes.ContainsKey(typeof (RefNPtg)) ? RefPtg.IndexToCode(FormulaUtil.GetIndex(typeof (RefPtg), 0, indexes, i, options)) : RefNPtg.IndexToCode(FormulaUtil.GetIndex(typeof (RefNPtg), 0, indexes, i, options) + 1);
      double result = 0.0;
      if (double.TryParse(str, out result) && !isR1C1 && (double) int.MaxValue < result)
      {
        range = false;
        this.m_book.ThrowOnUnknownNames = false;
      }
      else
        resultToken = FormulaUtil.CreatePtg(token, cellRow, cellColumn, str, strParam2, isR1C1);
    }
    else
      range = false;
    if (range && iRefIndex != -1)
      resultToken = (resultToken as IToken3D).Get3DToken(iRefIndex);
    return range;
  }

  private Ptg CreateNamedRange(
    string strToken,
    ParseParameters arguments,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    int iRefIndex)
  {
    WorkbookImpl workbook = (WorkbookImpl) arguments.Workbook;
    return iRefIndex >= 0 && !workbook.IsLocalReference(iRefIndex) ? this.CreateExternalName(iRefIndex, strToken, arguments, indexes, i, options) : this.CreateLocalName(iRefIndex, strToken, arguments, indexes, i, options);
  }

  private Ptg CreateExternalName(
    int iRefIndex,
    string strToken,
    ParseParameters arguments,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options)
  {
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    if (strToken == null)
      throw new ArgumentNullException(nameof (strToken));
    if (iRefIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iRefIndex));
    WorkbookImpl workbook = (WorkbookImpl) arguments.Workbook;
    int bookIndex = workbook.GetBookIndex(iRefIndex);
    ExternNamesCollection externNames = workbook.ExternWorkbooks[bookIndex].ExternNames;
    int iNameIndex = externNames.GetNameIndex(strToken);
    if (iNameIndex < 0)
      iNameIndex = externNames.Add(strToken);
    return this.CreateNameToken(iRefIndex, iNameIndex, arguments, indexes, i, options);
  }

  private Ptg CreateLocalName(
    int iRefIndex,
    string strToken,
    ParseParameters arguments,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options)
  {
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    strToken = strToken != null && strToken.Length != 0 ? strToken.Replace("[]", string.Empty) : throw new ArgumentOutOfRangeException(nameof (strToken));
    WorkbookImpl workbook = (WorkbookImpl) arguments.Workbook;
    IWorksheet worksheet = arguments.Worksheet;
    IName name;
    INames names;
    if (iRefIndex == -1)
    {
      name = worksheet?.Names[strToken] ?? workbook.Names[strToken];
      names = workbook.Names;
    }
    else
    {
      IWorksheet sheetByReference = workbook.GetSheetByReference(iRefIndex, false);
      names = sheetByReference != null ? sheetByReference.Names : workbook.Names;
      name = names[strToken];
    }
    if (name == null)
    {
      bool throwOnUnknownNames = workbook.ThrowOnUnknownNames;
      if (!workbook.Loading && (workbook.IsCellModified || workbook.m_bisCopy || workbook.IsLoaded) && workbook.ThrowOnUnknownNames && FormulaParser.IsTableRange(ref strToken, workbook, arguments.CellRow, arguments.CellColumn))
        workbook.ThrowOnUnknownNames = false;
      if (workbook.ThrowOnUnknownNames)
        throw new ParseException(strToken + " is not valid named range");
      name = names.Add(strToken);
      (name as NameImpl).m_isFormulaNamedRange = true;
      workbook.ThrowOnUnknownNames = throwOnUnknownNames;
    }
    int index = (name as NameImpl).Index;
    return this.CreateNameToken(iRefIndex, index, arguments, indexes, i, options);
  }

  internal static bool IsTableRange(ref string strToken, WorkbookImpl book, int row, int column)
  {
    string pattern1 = "\\[.*?\\]";
    string pattern2 = "\\[.*(\\[.*\\])(:)(\\[.*\\])\\]";
    if (strToken.Contains("\n"))
      pattern1 = "\\[.*\\n.*?\\]";
    bool flag1 = false;
    MatchCollection matchCollection1 = Regex.Matches(strToken, pattern2, RegexOptions.Compiled);
    string str1 = "";
    bool flag2 = false;
    bool flag3 = false;
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) book.Worksheets)
    {
      foreach (IListObject listObject in (IEnumerable<IListObject>) worksheet.ListObjects)
      {
        IRange location = listObject.Location;
        if (matchCollection1.Count > 0)
        {
          str1 = strToken.Substring(0, strToken.Length - matchCollection1[0].Value.Length);
          if (str1 == listObject.Name)
            flag1 = true;
        }
        MatchCollection matchCollection2 = Regex.Matches(strToken, pattern1, RegexOptions.Compiled);
        foreach (Match match1 in matchCollection2)
        {
          if (matchCollection2.Count > 1)
            flag2 = true;
          string input = Regex.Replace(match1.Value, "\\[|\\]|\\@", "");
          string pattern3 = "\\!|\\\"|\\#|\\$|\\%|\\&|\\'|\\(|\\)|\\*|\\+|\\,|\\-|\\/|\\:|\\;|\\<|\\=|\\>|\\?|\\@|\\[|\\|\\\\|\\]|\\^|\\`|\\{|\\||\\}|\\~|\\.|\\\\";
          MatchCollection matchCollection3 = Regex.Matches(input, pattern3);
          int num1 = 0;
          foreach (Capture capture in matchCollection3)
          {
            int num2 = capture.Index - num1;
            if (num2 > 0 && input[num2 - 1] == '\'')
            {
              input = input.Remove(num2 - 1, 1);
              ++num1;
            }
          }
          foreach (IListObjectColumn column1 in (IEnumerable<IListObjectColumn>) listObject.Columns)
          {
            bool flag4 = column1.Name == input;
            if (!flag4)
            {
              string calculatedFormula = column1.CalculatedFormula;
              if (calculatedFormula != null && calculatedFormula.Contains(listObject.Name))
              {
                foreach (Capture match2 in Regex.Matches(calculatedFormula, pattern1, RegexOptions.Compiled))
                {
                  string str2 = Regex.Replace(match2.Value, "\\[|\\]|\\@", "");
                  if (input == str2)
                  {
                    flag4 = true;
                    break;
                  }
                }
              }
            }
            if (flag4 && flag2)
            {
              flag3 = true;
              break;
            }
            if (flag4 && !flag2)
            {
              strToken = strToken.Contains("@") ? (strToken = $"{listObject.Name}[[#This Row],[{input}]]") : (strToken.StartsWith("[") ? listObject.Name + strToken : strToken);
              flag3 = true;
              break;
            }
          }
          if (!flag2)
          {
            if (flag3)
              break;
          }
        }
        if (flag2 && flag3)
        {
          if (flag1)
          {
            if (strToken.Contains("@"))
              strToken = strToken.Replace("@", "[#This Row],");
          }
          else if (strToken.Contains(":"))
          {
            string[] strArray = strToken.Split(':');
            if (strArray[0].Length > 0)
            {
              if (strArray[0].StartsWith(listObject.Name))
                strToken = strArray[0].ToString();
              else if (str1 == string.Empty)
                strToken = listObject.Name + strArray[0].ToString();
            }
            if (strArray[1].Length > 0)
            {
              if (strArray[1].StartsWith(listObject.Name))
                strToken = $"{strToken}:{strArray[1].ToString()}";
              else if (str1 == string.Empty)
                strToken = $"{strToken}:{listObject.Name}{strArray[1].ToString()}";
            }
            if (strToken.Contains("@"))
              strToken = strToken.Replace("@", "[#This Row],");
          }
        }
        if (flag3)
          break;
      }
      if (!flag3)
      {
        foreach (IListObject listObject in (IEnumerable<IListObject>) worksheet.ListObjects)
        {
          if (strToken.Equals(listObject.Name))
            flag3 = true;
          if (strToken.StartsWith(listObject.Name))
          {
            foreach (Capture match in Regex.Matches(strToken, pattern1, RegexOptions.Compiled))
            {
              string str3 = Regex.Replace(match.Value, "\\[|\\]", "");
              switch (str3)
              {
                case "#All":
                case "#Headers":
                case "#Totals":
                case "#Data":
                  flag3 = true;
                  break;
              }
              if (!flag3)
              {
                foreach (IListObjectColumn column2 in (IEnumerable<IListObjectColumn>) listObject.Columns)
                {
                  if (column2.Name == str3)
                  {
                    flag3 = true;
                    break;
                  }
                }
              }
            }
          }
        }
      }
    }
    return flag3;
  }

  private Ptg CreateNameToken(
    int iRefIndex,
    int iNameIndex,
    ParseParameters arguments,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options)
  {
    Ptg ptg;
    if (iRefIndex >= 0)
      ptg = FormulaUtil.CreatePtg(NameXPtg.IndexToCode(FormulaUtil.GetIndex(typeof (NameXPtg), 0, indexes, i, options)), (object) iRefIndex, (object) iNameIndex);
    else
      ptg = FormulaUtil.CreatePtg(NamePtg.IndexToCode(FormulaUtil.GetIndex(typeof (NamePtg), 0, indexes, i, options)), (object) iNameIndex);
    return ptg;
  }

  private Ptg CreateLocalName(
    string strNameLocation,
    string strToken,
    ParseParameters arguments,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options)
  {
    IWorkbook workbook = arguments.Workbook;
    IWorksheet worksheet = arguments.Worksheet;
    if (strNameLocation != null && strNameLocation.Length > 0)
      worksheet = workbook.Worksheets[strNameLocation];
    IName name = worksheet?.Names[strToken] ?? workbook.Names[strToken];
    Ptg localName = (Ptg) null;
    if (name != null)
    {
      int index = (name as NameImpl).Index;
      localName = FormulaUtil.CreatePtg(NamePtg.IndexToCode(FormulaUtil.GetIndex(typeof (NamePtg), 0, indexes, i, options)), (object) index);
    }
    return localName;
  }

  private bool IsExternLocation(IWorkbook book, string strLocation)
  {
    return strLocation != null && strLocation.Length != 0 && book.Worksheets[strLocation] == null;
  }

  private Ptg CreateFunction(
    ExcelFunction functionId,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments)
  {
    List<int> operands = this.ExtractOperands(options, arguments, functionId);
    bool flag = false;
    int num1;
    FunctionPtg ptg;
    if (FormulaUtil.FunctionIdToParamCount.TryGetValue(functionId, out num1))
    {
      int num2;
      if (FormulaUtil.MissingArgumentsCount.TryGetValue(functionId, out num2) && operands.Count - 1 < num2 && operands.Count - 1 > num1)
        flag = true;
      if (functionId == ExcelFunction.RAND)
      {
        operands.Clear();
        if (num1 != operands.Count)
          this.m_tokenizer.RaiseException("Wrong arguments number for function: " + (object) functionId, (Exception) null);
      }
      else if (flag)
        this.m_tokenizer.RaiseException("Wrong arguments number for function: " + (object) functionId, (Exception) null);
      ptg = (FunctionPtg) FormulaUtil.CreatePtg(FunctionPtg.IndexToCode(FormulaUtil.GetIndex(typeof (FunctionPtg), 0, indexes, i, options)), functionId);
    }
    else
    {
      if (functionId == ExcelFunction.GETPIVOTDATA && operands.Count > 4 && operands.Count % 2 == 0)
      {
        this.m_arrTokens.Add(FormulaUtil.CreatePtg(FormulaToken.tMissingArgument));
        operands.Add(this.m_arrTokens.Count);
      }
      if (functionId != ExcelFunction.ROW && functionId != ExcelFunction.COLUMN && FormulaUtil.InfiniteParamCountFunctionId.Contains(functionId) && operands.Count < 2)
        this.m_tokenizer.RaiseException("Wrong arguments number for function: " + (object) functionId, (Exception) null);
      ptg = (FunctionPtg) FormulaUtil.CreatePtg(FunctionVarPtg.IndexToCode(functionId == ExcelFunction.ROW || functionId == ExcelFunction.COLUMN ? 2 : FormulaUtil.GetIndex(typeof (FunctionVarPtg), 0, indexes, i, options)), functionId);
      ptg.NumberOfArguments = (byte) (operands.Count - 1);
    }
    return (Ptg) ptg;
  }

  private Ptg CreateIFFunction(
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments,
    AttrPtg spaceBeforeIf)
  {
    List<int> operands = this.ExtractOperands(options, arguments, ExcelFunction.IF);
    int num1 = operands.Count - 1;
    if (num1 > 3 || num1 < 2)
      this.m_tokenizer.RaiseException("Argument count for IF function must be 2 or 3.", (Exception) null);
    int num2 = operands[1];
    int iStartToken = operands[2];
    int num3 = this.GetTokensSize(num2, operands[2], arguments) + 4;
    int num4 = num1 == 3 ? this.GetTokensSize(iStartToken, operands[3], arguments) + 4 : 0;
    AttrPtg attrPtg = (AttrPtg) null;
    if (this.m_tokenSpaces.Count > 0)
    {
      attrPtg = this.m_tokenSpaces.Pop();
      attrPtg.AttrData1 = 4;
      num4 += attrPtg.GetSize(arguments.Version);
    }
    if (spaceBeforeIf != null)
      num4 += spaceBeforeIf.GetSize(arguments.Version);
    Ptg ptg1 = FormulaUtil.CreatePtg(FormulaToken.tAttr, (object) 2, (object) num3);
    this.m_arrTokens.Insert(num2, ptg1);
    int index = iStartToken + 1;
    int num5 = (options & ExcelParseFormulaOptions.InArray) == ExcelParseFormulaOptions.None ? 8 : 0;
    Ptg ptg2 = FormulaUtil.CreatePtg(FormulaToken.tAttr, (object) num5, (object) (num4 + 3));
    this.m_arrTokens.Insert(index, ptg2);
    if (spaceBeforeIf != null)
      this.m_arrTokens.Add((Ptg) spaceBeforeIf);
    if (attrPtg != null)
      this.m_arrTokens.Add((Ptg) attrPtg);
    if (num1 == 3)
      this.m_arrTokens.Add(FormulaUtil.CreatePtg(FormulaToken.tAttr, (object) num5, (object) 3));
    FunctionPtg ptg3 = (FunctionPtg) FormulaUtil.CreatePtg(FunctionVarPtg.IndexToCode(FormulaUtil.GetIndex(typeof (FunctionVarPtg), 1, indexes, i, options)), ExcelFunction.IF);
    ptg3.NumberOfArguments = (byte) (operands.Count - 1);
    return (Ptg) ptg3;
  }

  private Ptg CreateCustomFunction(
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    ParseParameters arguments,
    bool registerFunction)
  {
    IWorksheet worksheet = arguments.Worksheet;
    int iBookIndex;
    int iNameIndex;
    if (!FormulaUtil.IsCustomFunction(this.m_tokenizer.TokenString, arguments.Workbook, out iBookIndex, out iNameIndex))
    {
      if (registerFunction)
      {
        IName name = this.m_book.Names.Add("_xlfn." + this.m_tokenizer.TokenString);
        (name as NameImpl).IsFunction = true;
        iNameIndex = (name as NameImpl).Index;
        iBookIndex = -1;
      }
      else
        this.m_tokenizer.RaiseException(this.m_tokenizer.TokenString + " isn't custom function.", (Exception) null);
    }
    Ptg ptg1;
    if (iBookIndex == -1)
      ptg1 = FormulaUtil.CreatePtg(FormulaToken.tName1, (object) iNameIndex);
    else
      ptg1 = FormulaUtil.CreatePtg(FormulaToken.tNameX1, (object) iBookIndex, (object) iNameIndex);
    this.m_arrTokens.Add(ptg1);
    List<int> operands = this.ExtractOperands(options, arguments, ExcelFunction.CustomFunction);
    FunctionVarPtg ptg2 = (FunctionVarPtg) FormulaUtil.CreatePtg(FunctionVarPtg.IndexToCode(FormulaUtil.GetIndex(typeof (FunctionVarPtg), 1, indexes, i, options)), ExcelFunction.CustomFunction);
    ptg2.NumberOfArguments = (byte) operands.Count;
    return (Ptg) ptg2;
  }

  private int GetTokensSize(int iStartToken, int iEndToken, ParseParameters arguments)
  {
    if (iStartToken < 0 || iStartToken >= iEndToken)
      throw new ArgumentOutOfRangeException(nameof (iStartToken));
    int tokensSize = 0;
    for (int index = iStartToken; index < iEndToken; ++index)
    {
      Ptg arrToken = this.m_arrTokens[index];
      tokensSize += arrToken.GetSize(arguments.Version);
    }
    return tokensSize;
  }

  private List<int> ExtractOperands(
    ExcelParseFormulaOptions options,
    ParseParameters arguments,
    ExcelFunction functionId)
  {
    Dictionary<Type, ReferenceIndexAttribute> indexes = FormulaUtil.FunctionIdToIndex[functionId];
    ExcelParseFormulaOptions parseFormulaOptions = options;
    if ((options & ExcelParseFormulaOptions.RootLevel) != ExcelParseFormulaOptions.None)
      --parseFormulaOptions;
    ExcelParseFormulaOptions options1 = parseFormulaOptions | ExcelParseFormulaOptions.ParseOperand;
    if (FormulaUtil.IndexOf(FormulaUtil.SemiVolatileFunctions, functionId) != -1)
      this.m_arrTokens.Add(FormulaUtil.CreatePtg(FormulaToken.tAttr, (ushort) 1, (ushort) 0));
    this.m_tokenizer.NextToken();
    if (this.m_tokenizer.TokenType != FormulaToken.tParentheses)
      throw new ArgumentOutOfRangeException("Can't extract function arguments.");
    this.m_tokenizer.NextToken();
    if (functionId == ExcelFunction.PI && this.m_tokenizer.PreviousTokenType == FormulaToken.tParentheses && this.m_tokenizer.TokenType == FormulaToken.Space)
    {
      while (this.m_tokenizer.TokenType == FormulaToken.Space)
        this.m_tokenizer.NextToken();
    }
    int i = 0;
    List<int> operands = new List<int>();
    operands.Add(this.m_arrTokens.Count);
    while (this.m_tokenizer.TokenType != FormulaToken.CloseParenthesis)
    {
      this.ParseExpression(Priority.None, indexes, i, options1, arguments);
      if (this.m_tokenSpaces.Count > 0)
      {
        AttrPtg attrPtg = this.m_tokenSpaces.Pop();
        attrPtg.SpaceAfterToken = true;
        this.m_arrTokens.Add((Ptg) attrPtg);
      }
      operands.Add(this.m_arrTokens.Count);
      if (this.m_tokenizer.TokenType == FormulaToken.Comma)
        this.m_tokenizer.NextToken();
      ++i;
    }
    if (this.m_tokenizer.PreviousTokenType == FormulaToken.Comma)
    {
      this.m_arrTokens.Add((Ptg) new MissingArgumentPtg(""));
      operands.Add(this.m_arrTokens.Count);
    }
    this.m_tokenizer.NextToken();
    return operands;
  }

  private Ptg ParseArray(FormulaToken tokenId, ParseParameters arguments)
  {
    string tokenString = this.m_tokenizer.TokenString;
    return FormulaUtil.CreatePtg(tokenId, (object) tokenString, (object) arguments.FormulaUtility);
  }

  private Ptg ParseError(
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    ExcelParseFormulaOptions options,
    IWorksheet sheet)
  {
    string tokenString1 = this.m_tokenizer.TokenString;
    int length = tokenString1.LastIndexOf('!', tokenString1.Length - 2);
    Ptg error;
    if (length != -1)
    {
      int num = this.m_book.AddSheetReference(tokenString1.Substring(0, length).Trim('\''));
      error = FormulaUtil.CreatePtg(RefError3dPtg.IndexToCode(FormulaUtil.GetIndex(typeof (RefError3dPtg), 1, indexes, i, options)));
      ((Ref3DPtg) error).RefIndex = (ushort) num;
    }
    else
    {
      if (tokenString1.EndsWith("#REF!"))
      {
        int index = FormulaUtil.GetIndex(typeof (RefErrorPtg), 1, indexes, i, options);
        error = FormulaUtil.CreatePtg(sheet != null ? RefErrorPtg.IndexToCode(index) : RefError3dPtg.IndexToCode(index));
      }
      else
      {
        string tokenString2 = this.m_tokenizer.TokenString;
        error = (Ptg) FormulaUtil.ErrorNameToConstructor[tokenString2].Invoke(new object[1]
        {
          (object) tokenString2
        });
      }
      if (error is IReference reference)
        reference.RefIndex = ushort.MaxValue;
    }
    return error;
  }

  public List<Ptg> Tokens => this.m_arrTokens;

  public NumberFormatInfo NumberFormat
  {
    get => this.m_tokenizer.NumberFormat;
    set => this.m_tokenizer.NumberFormat = value;
  }
}
