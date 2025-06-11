// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.LayoutParser
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.Layouts;

internal static class LayoutParser
{
  internal static LayoutRenderer[] CompileLayout(
    ConfigurationItemFactory configurationItemFactory,
    SimpleStringReader sr,
    bool? throwConfigExceptions,
    bool isNested,
    out string text)
  {
    List<LayoutRenderer> layoutRendererList = new List<LayoutRenderer>();
    StringBuilder literalBuf = new StringBuilder();
    int position1 = sr.Position;
    int ch1;
    while ((ch1 = sr.Peek()) != -1)
    {
      if (isNested)
      {
        if (ch1 == 92)
        {
          sr.Read();
          int ch2 = sr.Peek();
          if (LayoutParser.EndOfLayout(ch2))
          {
            sr.Read();
            literalBuf.Append((char) ch2);
            continue;
          }
          literalBuf.Append('\\');
          continue;
        }
        if (LayoutParser.EndOfLayout(ch1))
          break;
      }
      sr.Read();
      if (ch1 == 36 && sr.Peek() == 123)
      {
        LayoutParser.AddLiteral(literalBuf, layoutRendererList);
        LayoutRenderer layoutRenderer = LayoutParser.ParseLayoutRenderer(configurationItemFactory, sr, throwConfigExceptions);
        if (LayoutParser.CanBeConvertedToLiteral(layoutRenderer))
          layoutRenderer = LayoutParser.ConvertToLiteral(layoutRenderer);
        layoutRendererList.Add(layoutRenderer);
      }
      else
        literalBuf.Append((char) ch1);
    }
    LayoutParser.AddLiteral(literalBuf, layoutRendererList);
    int position2 = sr.Position;
    LayoutParser.MergeLiterals(layoutRendererList);
    text = sr.Substring(position1, position2);
    return layoutRendererList.ToArray();
  }

  private static void AddLiteral(StringBuilder literalBuf, List<LayoutRenderer> result)
  {
    if (literalBuf.Length <= 0)
      return;
    result.Add((LayoutRenderer) new LiteralLayoutRenderer(literalBuf.ToString()));
    literalBuf.Length = 0;
  }

  private static bool EndOfLayout(int ch) => ch == 125 || ch == 58;

  private static string ParseLayoutRendererName(SimpleStringReader sr)
  {
    return sr.ReadUntilMatch((Func<int, bool>) (ch => LayoutParser.EndOfLayout(ch)));
  }

  private static string ParseParameterName(SimpleStringReader sr)
  {
    int num1 = 0;
    StringBuilder stringBuilder = new StringBuilder();
    int num2;
    while ((num2 = sr.Peek()) != -1 && (num2 != 61 && num2 != 125 && num2 != 58 || num1 != 0))
    {
      if (num2 == 36)
      {
        sr.Read();
        stringBuilder.Append('$');
        if (sr.Peek() == 123)
        {
          stringBuilder.Append('{');
          ++num1;
          sr.Read();
        }
      }
      else
      {
        if (num2 == 125)
          --num1;
        if (num2 == 92)
        {
          sr.Read();
          if (num1 != 0)
            stringBuilder.Append((char) num2);
          stringBuilder.Append((char) sr.Read());
        }
        else
        {
          stringBuilder.Append((char) num2);
          sr.Read();
        }
      }
    }
    return stringBuilder.ToString();
  }

  private static string ParseParameterValue(SimpleStringReader sr)
  {
    string parameterValue = sr.ReadUntilMatch((Func<int, bool>) (ch => LayoutParser.EndOfLayout(ch) || ch == 92));
    if (sr.Peek() != 92)
      return parameterValue;
    StringBuilder nameBuf = new StringBuilder();
    nameBuf.Append(parameterValue);
    LayoutParser.ParseParameterUnicodeValue(sr, nameBuf);
    return nameBuf.ToString();
  }

  private static void ParseParameterUnicodeValue(SimpleStringReader sr, StringBuilder nameBuf)
  {
    int ch1;
    while ((ch1 = sr.Peek()) != -1 && !LayoutParser.EndOfLayout(ch1))
    {
      if (ch1 == 92)
      {
        sr.Read();
        char ch2 = (char) sr.Peek();
        switch (ch2)
        {
          case '"':
          case '\'':
          case ':':
          case '\\':
          case '{':
          case '}':
            sr.Read();
            nameBuf.Append(ch2);
            continue;
          case '0':
            sr.Read();
            nameBuf.Append(char.MinValue);
            continue;
          case 'U':
            sr.Read();
            char unicode1 = LayoutParser.GetUnicode(sr, 8);
            nameBuf.Append(unicode1);
            continue;
          case 'a':
            sr.Read();
            nameBuf.Append('\a');
            continue;
          case 'b':
            sr.Read();
            nameBuf.Append('\b');
            continue;
          case 'f':
            sr.Read();
            nameBuf.Append('\f');
            continue;
          case 'n':
            sr.Read();
            nameBuf.Append('\n');
            continue;
          case 'r':
            sr.Read();
            nameBuf.Append('\r');
            continue;
          case 't':
            sr.Read();
            nameBuf.Append('\t');
            continue;
          case 'u':
            sr.Read();
            char unicode2 = LayoutParser.GetUnicode(sr, 4);
            nameBuf.Append(unicode2);
            continue;
          case 'v':
            sr.Read();
            nameBuf.Append('\v');
            continue;
          case 'x':
            sr.Read();
            char unicode3 = LayoutParser.GetUnicode(sr, 4);
            nameBuf.Append(unicode3);
            continue;
          default:
            continue;
        }
      }
      else
      {
        nameBuf.Append((char) ch1);
        sr.Read();
      }
    }
  }

  private static char GetUnicode(SimpleStringReader sr, int maxDigits)
  {
    int unicode = 0;
    for (int index = 0; index < maxDigits; ++index)
    {
      int num1 = sr.Peek();
      int num2;
      if (num1 >= 48 /*0x30*/ && num1 <= 57)
        num2 = num1 - 48 /*0x30*/;
      else if (num1 >= 97 && num1 <= 102)
        num2 = num1 - 97 + 10;
      else if (num1 >= 65 && num1 <= 70)
        num2 = num1 - 65 + 10;
      else
        break;
      sr.Read();
      unicode = unicode * 16 /*0x10*/ + num2;
    }
    return (char) unicode;
  }

  private static LayoutRenderer ParseLayoutRenderer(
    ConfigurationItemFactory configurationItemFactory,
    SimpleStringReader stringReader,
    bool? throwConfigExceptions)
  {
    stringReader.Read();
    string layoutRendererName = LayoutParser.ParseLayoutRendererName(stringReader);
    LayoutRenderer layoutRenderer1 = LayoutParser.GetLayoutRenderer(configurationItemFactory, layoutRendererName, throwConfigExceptions);
    Dictionary<Type, LayoutRenderer> dictionary = (Dictionary<Type, LayoutRenderer>) null;
    List<LayoutRenderer> orderedWrappers = (List<LayoutRenderer>) null;
    for (int index = stringReader.Read(); index != -1 && index != 125; index = stringReader.Read())
    {
      string str = LayoutParser.ParseParameterName(stringReader).Trim();
      if (stringReader.Peek() == 61)
      {
        stringReader.Read();
        LayoutRenderer layoutRenderer2 = layoutRenderer1;
        PropertyInfo result1;
        Type result2;
        if (!PropertyHelper.TryGetPropertyInfo((object) layoutRenderer1, str, out result1) && configurationItemFactory.AmbientProperties.TryGetDefinition(str, out result2))
        {
          dictionary = dictionary ?? new Dictionary<Type, LayoutRenderer>();
          orderedWrappers = orderedWrappers ?? new List<LayoutRenderer>();
          LayoutRenderer instance;
          if (!dictionary.TryGetValue(result2, out instance))
          {
            instance = configurationItemFactory.AmbientProperties.CreateInstance(str);
            dictionary[result2] = instance;
            orderedWrappers.Add(instance);
          }
          if (!PropertyHelper.TryGetPropertyInfo((object) instance, str, out result1))
            result1 = (PropertyInfo) null;
          else
            layoutRenderer2 = instance;
        }
        if (result1 == (PropertyInfo) null)
        {
          string parameterValue = LayoutParser.ParseParameterValue(stringReader);
          if (!string.IsNullOrEmpty(str) || !StringHelpers.IsNullOrWhiteSpace(parameterValue))
            InternalLogger.Warn("Skipping unrecognized property '{0}={1}` for ${{{2}}} ({3})", (object) str, (object) parameterValue, (object) layoutRendererName, (object) layoutRenderer1?.GetType());
        }
        else if (typeof (Layout).IsAssignableFrom(result1.PropertyType))
        {
          string text;
          SimpleLayout simpleLayout = new SimpleLayout(LayoutParser.CompileLayout(configurationItemFactory, stringReader, throwConfigExceptions, true, out text), text, configurationItemFactory);
          result1.SetValue((object) layoutRenderer2, (object) simpleLayout, (object[]) null);
        }
        else if (typeof (ConditionExpression).IsAssignableFrom(result1.PropertyType))
        {
          ConditionExpression expression = ConditionParser.ParseExpression(stringReader, configurationItemFactory);
          result1.SetValue((object) layoutRenderer2, (object) expression, (object[]) null);
        }
        else
        {
          string parameterValue = LayoutParser.ParseParameterValue(stringReader);
          PropertyHelper.SetPropertyFromString((object) layoutRenderer2, str, parameterValue, configurationItemFactory);
        }
      }
      else
        LayoutParser.SetDefaultPropertyValue(configurationItemFactory, layoutRenderer1, str);
    }
    if (orderedWrappers != null)
      layoutRenderer1 = LayoutParser.ApplyWrappers(configurationItemFactory, layoutRenderer1, orderedWrappers);
    return layoutRenderer1;
  }

  private static LayoutRenderer GetLayoutRenderer(
    ConfigurationItemFactory configurationItemFactory,
    string name,
    bool? throwConfigExceptions)
  {
    try
    {
      return configurationItemFactory.LayoutRenderers.CreateInstance(name);
    }
    catch (Exception ex)
    {
      if (((int) throwConfigExceptions ?? (int) LogManager.ThrowConfigExceptions ?? (LogManager.ThrowExceptions ? 1 : 0)) != 0)
        throw;
      object[] objArray = new object[1]{ (object) name };
      InternalLogger.Error(ex, "Error parsing layout {0} will be ignored.", objArray);
      return (LayoutRenderer) new LiteralLayoutRenderer(string.Empty);
    }
  }

  private static void SetDefaultPropertyValue(
    ConfigurationItemFactory configurationItemFactory,
    LayoutRenderer layoutRenderer,
    string value)
  {
    PropertyInfo result;
    if (PropertyHelper.TryGetPropertyInfo((object) layoutRenderer, string.Empty, out result))
      PropertyHelper.SetPropertyFromString((object) layoutRenderer, result.Name, value, configurationItemFactory);
    else
      InternalLogger.Warn<Type, string>("{0} has no default property to assign value {1}", layoutRenderer.GetType(), value);
  }

  private static LayoutRenderer ApplyWrappers(
    ConfigurationItemFactory configurationItemFactory,
    LayoutRenderer lr,
    List<LayoutRenderer> orderedWrappers)
  {
    for (int index = orderedWrappers.Count - 1; index >= 0; --index)
    {
      WrapperLayoutRendererBase orderedWrapper = (WrapperLayoutRendererBase) orderedWrappers[index];
      InternalLogger.Trace<Type, Type>("Wrapping {0} with {1}", lr.GetType(), orderedWrapper.GetType());
      if (LayoutParser.CanBeConvertedToLiteral(lr))
        lr = LayoutParser.ConvertToLiteral(lr);
      orderedWrapper.Inner = (Layout) new SimpleLayout(new LayoutRenderer[1]
      {
        lr
      }, string.Empty, configurationItemFactory);
      lr = (LayoutRenderer) orderedWrapper;
    }
    return lr;
  }

  private static bool CanBeConvertedToLiteral(LayoutRenderer lr)
  {
    object[] objArray = new object[1]{ (object) lr };
    foreach (object reachableObject in ObjectGraphScanner.FindReachableObjects<IRenderable>(true, objArray))
    {
      Type type = reachableObject.GetType();
      if (!(type == typeof (SimpleLayout)) && !type.IsDefined(typeof (AppDomainFixedOutputAttribute), false))
        return false;
    }
    return true;
  }

  private static void MergeLiterals(List<LayoutRenderer> list)
  {
    int index = 0;
    while (index + 1 < list.Count)
    {
      LiteralLayoutRenderer literalLayoutRenderer1 = list[index] as LiteralLayoutRenderer;
      LiteralLayoutRenderer literalLayoutRenderer2 = list[index + 1] as LiteralLayoutRenderer;
      if (literalLayoutRenderer1 != null && literalLayoutRenderer2 != null)
      {
        literalLayoutRenderer1.Text += literalLayoutRenderer2.Text;
        list.RemoveAt(index + 1);
      }
      else
        ++index;
    }
  }

  private static LayoutRenderer ConvertToLiteral(LayoutRenderer renderer)
  {
    return (LayoutRenderer) new LiteralLayoutRenderer(renderer.Render(LogEventInfo.CreateNullEvent()));
  }
}
