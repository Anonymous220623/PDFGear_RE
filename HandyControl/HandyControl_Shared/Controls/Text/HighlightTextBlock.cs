// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.HighlightTextBlock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class HighlightTextBlock : TextBlock
{
  public static readonly DependencyProperty SourceTextProperty = DependencyProperty.Register(nameof (SourceText), typeof (string), typeof (HighlightTextBlock), new PropertyMetadata((object) null, new PropertyChangedCallback(HighlightTextBlock.OnSourceTextChanged)));
  public static readonly DependencyProperty QueriesTextProperty = DependencyProperty.Register(nameof (QueriesText), typeof (string), typeof (HighlightTextBlock), new PropertyMetadata((object) null, new PropertyChangedCallback(HighlightTextBlock.OnQueriesTextChanged)));
  public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register(nameof (HighlightBrush), typeof (Brush), typeof (HighlightTextBlock));
  public static readonly DependencyProperty HighlightTextBrushProperty = DependencyProperty.Register(nameof (HighlightTextBrush), typeof (Brush), typeof (HighlightTextBlock));

  private static void OnSourceTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((HighlightTextBlock) d).RefreshInlines();
  }

  private static void OnQueriesTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((HighlightTextBlock) d).RefreshInlines();
  }

  public string SourceText
  {
    get => (string) this.GetValue(HighlightTextBlock.SourceTextProperty);
    set => this.SetValue(HighlightTextBlock.SourceTextProperty, (object) value);
  }

  public string QueriesText
  {
    get => (string) this.GetValue(HighlightTextBlock.QueriesTextProperty);
    set => this.SetValue(HighlightTextBlock.QueriesTextProperty, (object) value);
  }

  public Brush HighlightBrush
  {
    get => (Brush) this.GetValue(HighlightTextBlock.HighlightBrushProperty);
    set => this.SetValue(HighlightTextBlock.HighlightBrushProperty, (object) value);
  }

  public Brush HighlightTextBrush
  {
    get => (Brush) this.GetValue(HighlightTextBlock.HighlightTextBrushProperty);
    set => this.SetValue(HighlightTextBlock.HighlightTextBrushProperty, (object) value);
  }

  private void RefreshInlines()
  {
    this.Inlines.Clear();
    if (string.IsNullOrEmpty(this.SourceText))
      return;
    if (string.IsNullOrEmpty(this.QueriesText))
    {
      this.Inlines.Add(this.SourceText);
    }
    else
    {
      string sourceText = this.SourceText;
      List<HighlightTextBlock.Range> mergedIntervals = HighlightTextBlock.MergeIntervals(((IEnumerable<string>) this.QueriesText.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries)).Distinct<string>().SelectMany<string, HighlightTextBlock.Range, HighlightTextBlock.Range>((Func<string, IEnumerable<HighlightTextBlock.Range>>) (query => HighlightTextBlock.GetQueryIntervals(sourceText, query)), (Func<string, HighlightTextBlock.Range, HighlightTextBlock.Range>) ((query, interval) => interval)).ToList<HighlightTextBlock.Range>());
      this.Inlines.AddRange(this.GenerateRunElement(HighlightTextBlock.SplitTextByOrderedDisjointIntervals(sourceText, mergedIntervals)));
    }
  }

  private IEnumerable GenerateRunElement(IEnumerable<HighlightTextBlock.Fragment> fragments)
  {
    return (IEnumerable) fragments.Select<HighlightTextBlock.Fragment, Run>((Func<HighlightTextBlock.Fragment, Run>) (item => !item.IsQuery ? new Run(item.Text) : this.GetHighlightRun(item.Text)));
  }

  private Run GetHighlightRun(string highlightText)
  {
    Run highlightRun = new Run(highlightText);
    highlightRun.SetBinding(TextElement.BackgroundProperty, (BindingBase) new Binding("HighlightBrush")
    {
      Source = (object) this
    });
    highlightRun.SetBinding(TextElement.ForegroundProperty, (BindingBase) new Binding("HighlightTextBrush")
    {
      Source = (object) this
    });
    return highlightRun;
  }

  private static IEnumerable<HighlightTextBlock.Fragment> SplitTextByOrderedDisjointIntervals(
    string sourceText,
    List<HighlightTextBlock.Range> mergedIntervals)
  {
    if (!string.IsNullOrEmpty(sourceText))
    {
      List<HighlightTextBlock.Range> source = mergedIntervals;
      if ((source != null ? (!source.Any<HighlightTextBlock.Range>() ? 1 : 0) : 1) != 0)
      {
        yield return new HighlightTextBlock.Fragment()
        {
          Text = sourceText,
          IsQuery = false
        };
      }
      else
      {
        HighlightTextBlock.Range range1 = mergedIntervals.First<HighlightTextBlock.Range>();
        int start0 = range1.Start;
        int end0 = range1.End;
        if (start0 > 0)
          yield return new HighlightTextBlock.Fragment()
          {
            Text = sourceText.Substring(0, start0),
            IsQuery = false
          };
        yield return new HighlightTextBlock.Fragment()
        {
          Text = sourceText.Substring(start0, end0 - start0),
          IsQuery = true
        };
        int startIndex = end0;
        foreach (HighlightTextBlock.Range range2 in mergedIntervals.Skip<HighlightTextBlock.Range>(1))
        {
          int start = range2.Start;
          int end = range2.End;
          yield return new HighlightTextBlock.Fragment()
          {
            Text = sourceText.Substring(startIndex, start - startIndex),
            IsQuery = false
          };
          yield return new HighlightTextBlock.Fragment()
          {
            Text = sourceText.Substring(start, end - start),
            IsQuery = true
          };
          startIndex = end;
        }
        if (startIndex < sourceText.Length)
          yield return new HighlightTextBlock.Fragment()
          {
            Text = sourceText.Substring(startIndex),
            IsQuery = false
          };
      }
    }
  }

  private static List<HighlightTextBlock.Range> MergeIntervals(
    List<HighlightTextBlock.Range> intervals)
  {
    if ((intervals != null ? (!intervals.Any<HighlightTextBlock.Range>() ? 1 : 0) : 1) != 0)
      return new List<HighlightTextBlock.Range>();
    intervals.Sort((Comparison<HighlightTextBlock.Range>) ((x, y) => x.Start == y.Start ? x.End - y.End : x.Start - y.Start));
    HighlightTextBlock.Range interval = intervals[0];
    int num1 = interval.Start;
    int num2 = interval.End;
    List<HighlightTextBlock.Range> rangeList = new List<HighlightTextBlock.Range>();
    foreach (HighlightTextBlock.Range range in intervals.Skip<HighlightTextBlock.Range>(1))
    {
      int start = range.Start;
      int end = range.End;
      if (start <= num2)
      {
        if (num2 < end)
          num2 = end;
      }
      else
      {
        rangeList.Add(new HighlightTextBlock.Range()
        {
          Start = num1,
          End = num2
        });
        num1 = start;
        num2 = end;
      }
    }
    rangeList.Add(new HighlightTextBlock.Range()
    {
      Start = num1,
      End = num2
    });
    return rangeList;
  }

  private static IEnumerable<HighlightTextBlock.Range> GetQueryIntervals(
    string sourceText,
    string query)
  {
    if (!string.IsNullOrEmpty(sourceText) && !string.IsNullOrEmpty(query))
    {
      int nextStartIndex = 0;
      while (nextStartIndex < sourceText.Length)
      {
        int num = sourceText.IndexOf(query, nextStartIndex, StringComparison.CurrentCultureIgnoreCase);
        if (num == -1)
          break;
        nextStartIndex = num + query.Length;
        yield return new HighlightTextBlock.Range()
        {
          Start = num,
          End = nextStartIndex
        };
      }
    }
  }

  private struct Fragment
  {
    public string Text { get; set; }

    public bool IsQuery { get; set; }
  }

  private struct Range
  {
    public int Start { get; set; }

    public int End { get; set; }
  }
}
