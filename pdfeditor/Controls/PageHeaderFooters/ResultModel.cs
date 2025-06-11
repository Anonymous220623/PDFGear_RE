// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageHeaderFooters.ResultModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Patagames.Pdf.Net;
using PDFKit.Utils.PageHeaderFooters;
using PDFKit.Utils.XObjects;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.PageHeaderFooters;

public class ResultModel : ObservableObject
{
  private float fontSize;
  private Color color;
  private ResultModel.PageRangeEnum pageRange;
  private int selectedPagesStart;
  private int selectedPagesEnd;
  private ResultModel.SubsetEnum subset;
  private string pageNumberFormat;
  private int pageNumberOffset;
  private string dateFormat;

  public ResultModel()
  {
    this.Text = new ResultModel.TextModel();
    this.FontSize = 10f;
    this.Color = Colors.Black;
    this.pageRange = ResultModel.PageRangeEnum.None;
    this.Margin = new ResultModel.MarginModel();
  }

  public ResultModel.TextModel Text { get; }

  public float FontSize
  {
    get => this.fontSize;
    set => this.SetProperty<float>(ref this.fontSize, value, nameof (FontSize));
  }

  public Color Color
  {
    get => this.color;
    set => this.SetProperty<Color>(ref this.color, value, nameof (Color));
  }

  public ResultModel.PageRangeEnum PageRange
  {
    get => this.pageRange;
    set
    {
      this.SetProperty<ResultModel.PageRangeEnum>(ref this.pageRange, value, nameof (PageRange));
    }
  }

  public int SelectedPagesStart
  {
    get => this.selectedPagesStart;
    set => this.SetProperty<int>(ref this.selectedPagesStart, value, nameof (SelectedPagesStart));
  }

  public int SelectedPagesEnd
  {
    get => this.selectedPagesEnd;
    set => this.SetProperty<int>(ref this.selectedPagesEnd, value, nameof (SelectedPagesEnd));
  }

  public ResultModel.SubsetEnum Subset
  {
    get => this.subset;
    set => this.SetProperty<ResultModel.SubsetEnum>(ref this.subset, value, nameof (Subset));
  }

  public string PageNumberFormat
  {
    get => this.pageNumberFormat;
    set => this.SetProperty<string>(ref this.pageNumberFormat, value, nameof (PageNumberFormat));
  }

  public int PageNumberOffset
  {
    get => this.pageNumberOffset;
    set => this.SetProperty<int>(ref this.pageNumberOffset, value, nameof (PageNumberOffset));
  }

  public string DateFormat
  {
    get => this.dateFormat;
    set => this.SetProperty<string>(ref this.dateFormat, value, nameof (DateFormat));
  }

  public ResultModel.MarginModel Margin { get; }

  public HeaderFooterSettings ToSettings(PdfDocument doc, int currentPageIndex = -1)
  {
    return ResultModel.ResultModelHelper.ToHeaderFooterSettings(doc, this, currentPageIndex);
  }

  public static ResultModel FromSettings(PdfDocument doc, HeaderFooterSettings settings)
  {
    return ResultModel.ResultModelHelper.FromHeaderFooterSettings(doc, settings);
  }

  public class TextModel : ObservableObject
  {
    private string leftHeaderText = string.Empty;
    private string centerHeaderText = string.Empty;
    private string rightHeaderText = string.Empty;
    private string leftFooterText = string.Empty;
    private string centerFooterText = string.Empty;
    private string rightFooterText = string.Empty;

    public string LeftHeaderText
    {
      get => this.leftHeaderText;
      set
      {
        this.SetProperty<string>(ref this.leftHeaderText, value.TrimEnd('\r', '\n'), nameof (LeftHeaderText));
      }
    }

    public string CenterHeaderText
    {
      get => this.centerHeaderText;
      set
      {
        this.SetProperty<string>(ref this.centerHeaderText, value.TrimEnd('\r', '\n'), nameof (CenterHeaderText));
      }
    }

    public string RightHeaderText
    {
      get => this.rightHeaderText;
      set
      {
        this.SetProperty<string>(ref this.rightHeaderText, value.TrimEnd('\r', '\n'), nameof (RightHeaderText));
      }
    }

    public string LeftFooterText
    {
      get => this.leftFooterText;
      set
      {
        this.SetProperty<string>(ref this.leftFooterText, value.TrimEnd('\r', '\n'), nameof (LeftFooterText));
      }
    }

    public string CenterFooterText
    {
      get => this.centerFooterText;
      set
      {
        this.SetProperty<string>(ref this.centerFooterText, value.TrimEnd('\r', '\n'), nameof (CenterFooterText));
      }
    }

    public string RightFooterText
    {
      get => this.rightFooterText;
      set
      {
        this.SetProperty<string>(ref this.rightFooterText, value.TrimEnd('\r', '\n'), nameof (RightFooterText));
      }
    }
  }

  public class MarginModel : ObservableObject
  {
    private double top = 36.0;
    private double bottom = 36.0;
    private double left = 36.0;
    private double right = 36.0;

    public double Top
    {
      get => this.top;
      set
      {
        if (!this.SetProperty<double>(ref this.top, value, nameof (Top)))
          return;
        this.OnPropertyChanged("TopCm");
      }
    }

    public double Bottom
    {
      get => this.bottom;
      set
      {
        if (!this.SetProperty<double>(ref this.bottom, value, nameof (Bottom)))
          return;
        this.OnPropertyChanged("BottomCm");
      }
    }

    public double Left
    {
      get => this.left;
      set
      {
        if (!this.SetProperty<double>(ref this.left, value, nameof (Left)))
          return;
        this.OnPropertyChanged("LeftCm");
      }
    }

    public double Right
    {
      get => this.right;
      set
      {
        if (!this.SetProperty<double>(ref this.right, value, nameof (Right)))
          return;
        this.OnPropertyChanged("RightCm");
      }
    }

    public double TopCm
    {
      get => PageHeaderFooterUtils.PdfPointToCm(this.Top);
      set => this.Top = PageHeaderFooterUtils.CmToPdfPoint(value);
    }

    public double BottomCm
    {
      get => PageHeaderFooterUtils.PdfPointToCm(this.Bottom);
      set => this.Bottom = PageHeaderFooterUtils.CmToPdfPoint(value);
    }

    public double LeftCm
    {
      get => PageHeaderFooterUtils.PdfPointToCm(this.Left);
      set => this.Left = PageHeaderFooterUtils.CmToPdfPoint(value);
    }

    public double RightCm
    {
      get => PageHeaderFooterUtils.PdfPointToCm(this.Right);
      set => this.Right = PageHeaderFooterUtils.CmToPdfPoint(value);
    }
  }

  public enum PageRangeEnum
  {
    None,
    AllPages,
    CurrentPage,
    SelectedPages,
  }

  public enum SubsetEnum
  {
    AllPages,
    Odd,
    Even,
  }

  private static class ResultModelHelper
  {
    public static ResultModel FromHeaderFooterSettings(
      PdfDocument doc,
      HeaderFooterSettings hfSettings)
    {
      if (hfSettings == null || doc == null)
        return (ResultModel) null;
      ResultModel resultModel = new ResultModel();
      resultModel.Margin.Left = hfSettings.Margin.Left;
      resultModel.Margin.Top = hfSettings.Margin.Top;
      resultModel.Margin.Right = hfSettings.Margin.Right;
      resultModel.Margin.Bottom = hfSettings.Margin.Bottom;
      if (hfSettings.PageRange.Start == -1 && hfSettings.PageRange.End == -1)
      {
        resultModel.PageRange = ResultModel.PageRangeEnum.AllPages;
      }
      else
      {
        int start = hfSettings.PageRange.Start == -1 ? 0 : hfSettings.PageRange.Start;
        int num = hfSettings.PageRange.End == -1 ? doc.Pages.Count : hfSettings.PageRange.End;
        resultModel.PageRange = ResultModel.PageRangeEnum.SelectedPages;
        resultModel.SelectedPagesStart = start;
        resultModel.SelectedPagesEnd = num;
      }
      resultModel.Subset = !hfSettings.PageRange.Even || !hfSettings.PageRange.Odd ? (!hfSettings.PageRange.Even ? ResultModel.SubsetEnum.Odd : ResultModel.SubsetEnum.Even) : ResultModel.SubsetEnum.AllPages;
      resultModel.FontSize = (float) hfSettings.Font.Size;
      resultModel.Color = Color.FromArgb(byte.MaxValue, (byte) (hfSettings.Color.R * (double) byte.MaxValue), (byte) (hfSettings.Color.G * (double) byte.MaxValue), (byte) (hfSettings.Color.B * (double) byte.MaxValue));
      if (hfSettings.Page != null)
      {
        resultModel.PageNumberFormat = PagePlaceholderFormatter.PageModelToString(hfSettings.Page);
        if (resultModel.PageNumberFormat.StartsWith("<<"))
          resultModel.PageNumberFormat = resultModel.PageNumberFormat.Substring(2, resultModel.PageNumberFormat.Length - 4).Trim();
        resultModel.PageNumberOffset = hfSettings.Page.Offset + 1;
      }
      if (hfSettings.Date != null)
      {
        resultModel.DateFormat = PagePlaceholderFormatter.DateModelToString(hfSettings.Date);
        if (resultModel.DateFormat.StartsWith("<<"))
          resultModel.DateFormat = resultModel.DateFormat.Substring(2, resultModel.DateFormat.Length - 4).Trim();
      }
      resultModel.Text.LeftHeaderText = PagePlaceholderFormatter.LocationToString(hfSettings.Header.Left);
      resultModel.Text.CenterHeaderText = PagePlaceholderFormatter.LocationToString(hfSettings.Header.Center);
      resultModel.Text.RightHeaderText = PagePlaceholderFormatter.LocationToString(hfSettings.Header.Right);
      resultModel.Text.LeftFooterText = PagePlaceholderFormatter.LocationToString(hfSettings.Footer.Left);
      resultModel.Text.CenterFooterText = PagePlaceholderFormatter.LocationToString(hfSettings.Footer.Center);
      resultModel.Text.RightFooterText = PagePlaceholderFormatter.LocationToString(hfSettings.Footer.Right);
      return resultModel;
    }

    public static HeaderFooterSettings ToHeaderFooterSettings(
      PdfDocument doc,
      ResultModel model,
      int currentPageIndex = -1)
    {
      if (model == null || doc == null)
        return (HeaderFooterSettings) null;
      HeaderFooterSettings headerFooterSettings = new HeaderFooterSettings();
      headerFooterSettings.Margin.Left = model.Margin.Left;
      headerFooterSettings.Margin.Top = model.Margin.Top;
      headerFooterSettings.Margin.Right = model.Margin.Right;
      headerFooterSettings.Margin.Bottom = model.Margin.Bottom;
      if (model.PageRange == ResultModel.PageRangeEnum.AllPages)
      {
        headerFooterSettings.PageRange.Start = -1;
        headerFooterSettings.PageRange.End = -1;
      }
      else if (model.PageRange == ResultModel.PageRangeEnum.SelectedPages)
      {
        headerFooterSettings.PageRange.Start = model.SelectedPagesStart;
        headerFooterSettings.PageRange.End = model.SelectedPagesEnd;
      }
      else if (currentPageIndex != -1)
      {
        headerFooterSettings.PageRange.Start = currentPageIndex;
        headerFooterSettings.PageRange.End = currentPageIndex;
      }
      else
      {
        headerFooterSettings.PageRange.Start = -1;
        headerFooterSettings.PageRange.End = -1;
      }
      if (model.Subset == ResultModel.SubsetEnum.AllPages)
      {
        headerFooterSettings.PageRange.Even = true;
        headerFooterSettings.PageRange.Odd = true;
      }
      else if (model.Subset == ResultModel.SubsetEnum.Even)
      {
        headerFooterSettings.PageRange.Even = true;
        headerFooterSettings.PageRange.Odd = false;
      }
      else if (model.Subset == ResultModel.SubsetEnum.Odd)
      {
        headerFooterSettings.PageRange.Even = false;
        headerFooterSettings.PageRange.Odd = true;
      }
      headerFooterSettings.Font.Size = (double) model.fontSize;
      headerFooterSettings.Color.R = (double) model.Color.R / (double) byte.MaxValue;
      headerFooterSettings.Color.G = (double) model.Color.G / (double) byte.MaxValue;
      headerFooterSettings.Color.B = (double) model.Color.B / (double) byte.MaxValue;
      if (!string.IsNullOrEmpty(model.PageNumberFormat))
      {
        HeaderFooterSettings.PageModel pageModel = PagePlaceholderFormatter.StringToPageModel($"<<{model.PageNumberFormat}>>", model.PageNumberOffset);
        if (pageModel != null)
        {
          int num = pageModel.Offset;
          if (num < 0)
            num = 0;
          headerFooterSettings.Page.Offset = num;
          headerFooterSettings.Page.Clear();
          foreach (object obj in (HeaderFooterSettings.VariableCollection) pageModel)
            headerFooterSettings.Page.Add(obj);
        }
      }
      if (!string.IsNullOrEmpty(model.DateFormat))
      {
        HeaderFooterSettings.DateModel dateModel = PagePlaceholderFormatter.StringToDateModel($"<<{model.DateFormat}>>");
        if (dateModel != null)
        {
          headerFooterSettings.Date.Clear();
          foreach (object obj in (HeaderFooterSettings.VariableCollection) dateModel)
            headerFooterSettings.Date.Add(obj);
        }
      }
      PagePlaceholderFormatter.StringToLocation(headerFooterSettings.Header.Left, model.Text.LeftHeaderText, model.PageNumberOffset);
      PagePlaceholderFormatter.StringToLocation(headerFooterSettings.Header.Center, model.Text.CenterHeaderText, model.PageNumberOffset);
      PagePlaceholderFormatter.StringToLocation(headerFooterSettings.Header.Right, model.Text.RightHeaderText, model.PageNumberOffset);
      PagePlaceholderFormatter.StringToLocation(headerFooterSettings.Footer.Left, model.Text.LeftFooterText, model.PageNumberOffset);
      PagePlaceholderFormatter.StringToLocation(headerFooterSettings.Footer.Center, model.Text.CenterFooterText, model.PageNumberOffset);
      PagePlaceholderFormatter.StringToLocation(headerFooterSettings.Footer.Right, model.Text.RightFooterText, model.PageNumberOffset);
      return headerFooterSettings;
    }
  }
}
