// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfListLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Lists;

internal class PdfListLayouter(PdfList element) : ElementLayouter((PdfLayoutElement) element)
{
  private PdfGraphics m_graphics;
  private bool m_finish;
  private PdfList m_curList;
  private Stack<ListInfo> m_info = new Stack<ListInfo>();
  private int m_index;
  private float m_indent;
  private float m_resultHeight;
  private RectangleF m_bounds;
  private PdfPage currentPage;
  private SizeF size;
  private bool usePaginateBounds = true;
  private PdfBrush currentBrush;
  private PdfPen currentPen;
  private PdfFont currentFont;
  private PdfStringFormat currentFormat;
  private float markerMaxWidth;

  public PdfList Element => base.Element as PdfList;

  public void Layout(PdfGraphics graphics, float x, float y)
  {
    RectangleF boundaries = new RectangleF(new PointF(x, y), SizeF.Empty);
    this.Layout(graphics, boundaries);
  }

  public void Layout(PdfGraphics graphics, PointF point)
  {
    RectangleF boundaries = new RectangleF(point, SizeF.Empty);
    this.Layout(graphics, boundaries);
  }

  public void Layout(PdfGraphics graphics, RectangleF boundaries)
  {
    this.m_graphics = graphics != null ? graphics : throw new ArgumentNullException(nameof (graphics));
    PdfLayoutParams pdfLayoutParams = new PdfLayoutParams()
    {
      Bounds = boundaries,
      Format = new PdfLayoutFormat()
    };
    pdfLayoutParams.Format.Layout = PdfLayoutType.OnePage;
    this.LayoutInternal(pdfLayoutParams);
  }

  protected override PdfLayoutResult LayoutInternal(PdfLayoutParams param)
  {
    this.currentPage = param.Page;
    this.m_bounds = param.Bounds;
    if (param.Bounds.Size == SizeF.Empty && this.currentPage != null)
    {
      this.m_bounds.Size = this.currentPage.GetClientSize();
      this.m_bounds.Width -= this.m_bounds.X;
      this.m_bounds.Height -= this.m_bounds.Y;
    }
    if (this.currentPage != null)
      this.m_graphics = this.currentPage.Graphics;
    PageLayoutResult pageResult = new PageLayoutResult();
    pageResult.Broken = false;
    pageResult.Y = this.m_bounds.Y;
    this.m_curList = this.Element;
    this.m_indent = this.Element.Indent;
    this.SetCurrentParameters(this.Element);
    if (this.Element.Brush == null)
      this.currentBrush = PdfBrushes.Black;
    if (this.Element.Font == null)
      this.currentFont = PdfDocument.DefaultFont;
    if (this.m_curList is PdfOrderedList)
      this.markerMaxWidth = this.GetMarkerMaxWidth(this.m_curList as PdfOrderedList, this.m_info);
    bool flag1 = param.Format.Layout == PdfLayoutType.OnePage;
    while (!this.m_finish)
    {
      bool flag2 = this.BeforePageLayout(this.m_bounds, this.currentPage, this.m_curList);
      pageResult.Y = this.m_bounds.Y;
      ListEndPageLayoutEventArgs pageLayoutEventArgs = (ListEndPageLayoutEventArgs) null;
      if (!flag2)
      {
        if (param.Page != null && param.Page.Document != null && param.Page.Document.AutoTag || this.m_curList.PdfTag != null)
        {
          PdfCatalog.StructTreeRoot.IsNewList = true;
          if (this.m_curList.PdfTag == null)
            this.m_curList.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.List);
        }
        pageResult = this.LayoutOnPage(pageResult);
        pageLayoutEventArgs = this.AfterPageLayouted(this.m_bounds, this.currentPage, this.m_curList);
        flag2 = pageLayoutEventArgs != null && pageLayoutEventArgs.Cancel;
      }
      if (!flag1 && !flag2)
      {
        if (this.currentPage != null && !this.m_finish)
        {
          this.currentPage = pageLayoutEventArgs == null || pageLayoutEventArgs.NextPage == null ? this.GetNextPage(this.currentPage) : pageLayoutEventArgs.NextPage;
          this.m_graphics = this.currentPage.Graphics;
          if (param.Bounds.Size == SizeF.Empty)
          {
            this.m_bounds.Size = this.currentPage.GetClientSize();
            this.m_bounds.Width -= this.m_bounds.X;
            this.m_bounds.Height -= this.m_bounds.Y;
          }
          if (param.Format != null && param.Format.UsePaginateBounds && this.usePaginateBounds)
            this.m_bounds = param.Format.PaginateBounds;
        }
      }
      else
        break;
    }
    this.m_info.Clear();
    PdfLayoutResult pdfLayoutResult = new PdfLayoutResult(this.currentPage, new RectangleF(this.m_bounds.X, pageResult.Y, this.m_bounds.Width, this.m_resultHeight));
    if (this.currentFormat != null)
      this.currentFormat.m_isList = false;
    return pdfLayoutResult;
  }

  private float GetMarkerMaxWidth(PdfOrderedList list, Stack<ListInfo> info)
  {
    float markerMaxWidth = -1f;
    for (int index = 0; index < list.Items.Count; ++index)
    {
      PdfStringLayoutResult orderedMarkerResult = this.CreateOrderedMarkerResult((PdfList) list, list.Items[index], index + list.Marker.StartNumber, info, true);
      if ((double) markerMaxWidth < (double) orderedMarkerResult.ActualSize.Width)
        markerMaxWidth = orderedMarkerResult.ActualSize.Width;
    }
    return markerMaxWidth;
  }

  private void SetCurrentParameters(PdfList list)
  {
    if (list.Brush != null)
      this.currentBrush = list.Brush;
    if (list.Pen != null)
      this.currentPen = list.Pen;
    if (list.Font != null)
      this.currentFont = list.Font;
    if (list.StringFormat == null)
      return;
    this.currentFormat = list.StringFormat;
    this.currentFormat.m_isList = true;
  }

  private void SetCurrentParameters(PdfListItem item)
  {
    if (item.Brush != null)
      this.currentBrush = item.Brush;
    if (item.Pen != null)
      this.currentPen = item.Pen;
    if (item.Font != null)
      this.currentFont = item.Font;
    if (item.StringFormat == null)
      return;
    this.currentFormat = item.StringFormat;
  }

  private PageLayoutResult LayoutOnPage(PageLayoutResult pageResult)
  {
    float height = 0.0f;
    float num = 0.0f;
    float y = this.m_bounds.Y;
    float x = this.m_bounds.X;
    this.size = this.m_bounds.Size;
    this.size.Width -= this.m_indent;
    while (true)
    {
      for (; this.m_index < this.m_curList.Items.Count; ++this.m_index)
      {
        PdfListItem pdfListItem = this.m_curList.Items[this.m_index];
        if (this.currentPage != null && !pageResult.Broken)
          this.BeforeItemLayout(pdfListItem, this.currentPage);
        if (this.currentPage != null && this.currentPage.Document != null && this.currentPage.Document.AutoTag || pdfListItem.PdfTag != null)
        {
          PdfCatalog.StructTreeRoot.IsNewListItem = true;
          if (pdfListItem.PdfTag == null)
            pdfListItem.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.ListItem);
        }
        this.DrawItem(ref pageResult, x, this.m_curList, this.m_index, this.m_indent, this.m_info, pdfListItem, ref height, ref y);
        num += height;
        if (pageResult.Broken)
          return pageResult;
        if (this.currentPage != null)
          this.AfterItemLayouted(pdfListItem, this.currentPage);
        if (PdfCatalog.StructTreeRoot != null && this.m_index == this.m_curList.Items.Count - 1 && PdfCatalog.StructTreeRoot.m_isSubList)
          PdfCatalog.StructTreeRoot.m_isSubList = false;
        pageResult.MarkerWrote = false;
        if (pdfListItem.SubList != null && pdfListItem.SubList.Items.Count > 0)
        {
          if (this.m_curList is PdfOrderedList)
          {
            PdfOrderedList curList = this.m_curList as PdfOrderedList;
            curList.Marker.CurrentIndex = this.m_index;
            this.m_info.Push(new ListInfo(this.m_curList, this.m_index, curList.Marker.GetNumber())
            {
              Brush = this.currentBrush,
              Font = this.currentFont,
              Format = this.currentFormat,
              Pen = this.currentPen,
              MarkerWidth = this.markerMaxWidth
            });
          }
          else
            this.m_info.Push(new ListInfo(this.m_curList, this.m_index)
            {
              Brush = this.currentBrush,
              Font = this.currentFont,
              Format = this.currentFormat,
              Pen = this.currentPen
            });
          this.m_curList = pdfListItem.SubList;
          if (this.currentPage != null && this.currentPage.Document != null && this.currentPage.Document.AutoTag || pdfListItem.SubList.PdfTag != null)
          {
            PdfCatalog.StructTreeRoot.IsNewList = true;
            PdfCatalog.StructTreeRoot.m_isSubList = true;
            if (pdfListItem.SubList.PdfTag == null)
              pdfListItem.SubList.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.List);
          }
          if (this.m_curList is PdfOrderedList)
            this.markerMaxWidth = this.GetMarkerMaxWidth(this.m_curList as PdfOrderedList, this.m_info);
          this.m_index = -1;
          this.m_indent += this.m_curList.Indent;
          this.size.Width -= this.m_curList.Indent;
          this.SetCurrentParameters(pdfListItem);
          this.SetCurrentParameters(this.m_curList);
        }
      }
      if (this.m_info.Count != 0)
      {
        ListInfo listInfo = this.m_info.Pop();
        this.m_index = listInfo.Index + 1;
        this.m_indent -= this.m_curList.Indent;
        this.size.Width += this.m_curList.Indent;
        this.markerMaxWidth = listInfo.MarkerWidth;
        this.currentBrush = listInfo.Brush;
        this.currentPen = listInfo.Pen;
        this.currentFont = listInfo.Font;
        this.currentFormat = listInfo.Format;
        this.m_curList = listInfo.List;
      }
      else
        break;
    }
    this.m_resultHeight = num;
    this.m_finish = true;
    return pageResult;
  }

  private void DrawItem(
    ref PageLayoutResult pageResult,
    float x,
    PdfList curList,
    int index,
    float indent,
    Stack<ListInfo> info,
    PdfListItem item,
    ref float height,
    ref float y)
  {
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    PdfStringLayoutResult result = (PdfStringLayoutResult) null;
    bool flag1 = false;
    float textIndent = curList.TextIndent;
    float num1 = height + y;
    float num2 = indent + x;
    float num3 = 0.0f;
    SizeF size = this.size;
    string text1 = item.Text;
    string text2 = (string) null;
    PdfBrush brush = this.currentBrush;
    if (item.Brush != null)
      brush = item.Brush;
    PdfPen pen = this.currentPen;
    if (item.Pen != null)
      pen = item.Pen;
    PdfFont font = this.currentFont;
    if (item.Font != null)
      font = item.Font;
    PdfStringFormat format = this.currentFormat;
    if (item.StringFormat != null)
      format = item.StringFormat;
    if (((double) this.size.Width <= 0.0 || (double) this.size.Width < (double) font.Size) && this.currentPage != null)
      throw new Exception("There is not enough space to layout list.");
    this.size.Height -= height;
    PdfMarker marker;
    if (curList is PdfUnorderedList)
    {
      marker = (PdfMarker) (curList as PdfUnorderedList).Marker;
      if ((curList as PdfUnorderedList).PdfTag != null)
        (item.PdfTag as PdfStructureElement).Parent = (curList as PdfUnorderedList).PdfTag as PdfStructureElement;
    }
    else
    {
      marker = (PdfMarker) (curList as PdfOrderedList).Marker;
      if ((curList as PdfOrderedList).PdfTag != null)
        (item.PdfTag as PdfStructureElement).Parent = (curList as PdfOrderedList).PdfTag as PdfStructureElement;
    }
    if (pageResult.Broken)
    {
      text1 = pageResult.ItemText;
      text2 = pageResult.MarkerText;
    }
    bool flag2 = true;
    PdfStringLayoutResult markerResult;
    float num4;
    float height1;
    if (text2 != null && marker is PdfUnorderedMarker && (marker as PdfUnorderedMarker).Style == PdfUnorderedMarkerStyle.CustomString)
    {
      markerResult = pdfStringLayouter.Layout(text2, this.GetMarkerFont(marker, item), this.GetMarkerFormat(marker, item), this.size);
      num4 = num2 + markerResult.ActualSize.Width;
      pageResult.MarkerWidth = markerResult.ActualSize.Width;
      height1 = markerResult.ActualSize.Height;
      flag2 = true;
    }
    else
    {
      markerResult = this.CreateMarkerResult(index, curList, info, item);
      if (markerResult != null)
      {
        if (curList is PdfOrderedList)
        {
          num4 = num2 + this.markerMaxWidth;
          pageResult.MarkerWidth = this.markerMaxWidth;
        }
        else
        {
          num4 = num2 + markerResult.ActualSize.Width;
          pageResult.MarkerWidth = markerResult.ActualSize.Width;
        }
        height1 = markerResult.ActualSize.Height;
        if (this.currentPage != null)
          flag2 = (double) height1 < (double) this.size.Height;
        if (markerResult.Empty)
          flag2 = false;
      }
      else
      {
        num4 = num2 + (marker as PdfUnorderedMarker).Size.Width;
        pageResult.MarkerWidth = (marker as PdfUnorderedMarker).Size.Width;
        height1 = (marker as PdfUnorderedMarker).Size.Height;
        if (this.currentPage != null)
          flag2 = (double) height1 < (double) this.size.Height;
      }
    }
    if (text2 == null || text2 == string.Empty)
      flag2 = true;
    if (text1 != null && flag2)
    {
      size = this.size;
      size.Width -= pageResult.MarkerWidth;
      if ((double) item.TextIndent == 0.0)
        size.Width -= textIndent;
      else
        size.Width -= item.TextIndent;
      if (((double) size.Width <= 0.0 || (double) size.Width < (double) font.Size) && this.currentPage != null)
        throw new Exception("There is not enough space to layout the item text. Marker is too long or there is no enough space to draw it.");
      float num5 = num4;
      float x1;
      if (!marker.RightToLeft)
      {
        x1 = (double) item.TextIndent != 0.0 ? num5 + item.TextIndent : num5 + textIndent;
      }
      else
      {
        x1 = num5 - pageResult.MarkerWidth;
        if (format != null && (format.Alignment == PdfTextAlignment.Right || format.Alignment == PdfTextAlignment.Center))
          x1 -= indent;
      }
      if (this.currentPage == null && format != null)
      {
        format = (PdfStringFormat) format.Clone();
        format.Alignment = PdfTextAlignment.Left;
      }
      result = pdfStringLayouter.Layout(text1, font, format, size);
      RectangleF layoutRectangle = new RectangleF(x1, num1, size.Width, size.Height);
      if (this.currentPage != null && this.currentPage.Document != null && this.currentPage.Document.AutoTag || item.PdfTag != null)
      {
        this.m_graphics.Tag = (PdfTag) new PdfStructureElement(PdfTagType.ListBody);
        (this.m_graphics.Tag as PdfStructureElement).Parent = item.PdfTag as PdfStructureElement;
      }
      this.m_graphics.DrawStringLayoutResult(result, font, pen, brush, layoutRectangle, format);
      y = num1;
      num3 = result.ActualSize.Height;
    }
    height = (double) num3 < (double) height1 ? height1 : num3;
    if (result != null && !PdfListLayouter.IsNullOrEmpty(result.Remainder) || markerResult != null && !PdfListLayouter.IsNullOrEmpty(markerResult.Remainder) || !flag2)
    {
      y = 0.0f;
      height = 0.0f;
      if (result != null)
      {
        pageResult.ItemText = result.Remainder;
        if (result.Remainder == item.Text)
          flag2 = false;
      }
      else
        pageResult.ItemText = flag2 ? (string) null : item.Text;
      pageResult.MarkerText = markerResult == null ? (string) null : markerResult.Remainder;
      pageResult.Broken = true;
      pageResult.Y = 0.0f;
      this.m_bounds.Y = 0.0f;
    }
    else
      pageResult.Broken = false;
    if (result != null)
    {
      pageResult.MarkerX = num4;
      if (format != null)
      {
        switch (format.Alignment)
        {
          case PdfTextAlignment.Center:
            pageResult.MarkerX = (float) ((double) num4 + (double) size.Width / 2.0 - (double) result.ActualSize.Width / 2.0);
            break;
          case PdfTextAlignment.Right:
            pageResult.MarkerX = num4 + size.Width - result.ActualSize.Width;
            break;
        }
      }
      if (marker.RightToLeft)
      {
        pageResult.MarkerX += result.ActualSize.Width;
        if ((double) item.TextIndent == 0.0)
          pageResult.MarkerX += textIndent;
        else
          pageResult.MarkerX += item.TextIndent;
        if (format != null && (format.Alignment == PdfTextAlignment.Right || format.Alignment == PdfTextAlignment.Center))
          pageResult.MarkerX -= indent;
      }
    }
    if (marker is PdfUnorderedMarker && (marker as PdfUnorderedMarker).Style == PdfUnorderedMarkerStyle.CustomString)
    {
      if (markerResult == null)
        return;
      flag1 = this.DrawMarker(curList, item, markerResult, num1, pageResult.MarkerX);
      pageResult.MarkerWrote = true;
      pageResult.MarkerWidth = markerResult.ActualSize.Width;
    }
    else
    {
      if (!flag2 || pageResult.MarkerWrote)
        return;
      bool flag3 = this.DrawMarker(curList, item, markerResult, num1, pageResult.MarkerX);
      pageResult.MarkerWrote = flag3;
      if (curList is PdfOrderedList)
        pageResult.MarkerWidth = markerResult.ActualSize.Width;
      else
        pageResult.MarkerWidth = (marker as PdfUnorderedMarker).Size.Width;
    }
  }

  private static bool IsNullOrEmpty(string text) => text == null || text == string.Empty;

  private void AfterItemLayouted(PdfListItem item, PdfPage page)
  {
    this.Element.OnEndItemLayout(new EndItemLayoutEventArgs(item, page));
  }

  private void BeforeItemLayout(PdfListItem item, PdfPage page)
  {
    this.Element.OnBeginItemLayout(new BeginItemLayoutEventArgs(item, page));
  }

  private ListEndPageLayoutEventArgs AfterPageLayouted(
    RectangleF currentBounds,
    PdfPage currentPage,
    PdfList list)
  {
    ListEndPageLayoutEventArgs e = (ListEndPageLayoutEventArgs) null;
    if (this.Element.RaiseEndPageLayout && currentPage != null)
    {
      e = new ListEndPageLayoutEventArgs(new PdfLayoutResult(currentPage, currentBounds), list);
      this.Element.OnEndPageLayout((EndPageLayoutEventArgs) e);
    }
    return e;
  }

  private bool BeforePageLayout(RectangleF currentBounds, PdfPage currentPage, PdfList list)
  {
    bool flag = false;
    if (this.Element.RaiseBeginPageLayout && currentPage != null)
    {
      ListBeginPageLayoutEventArgs e = new ListBeginPageLayoutEventArgs(currentBounds, currentPage, list);
      this.Element.OnBeginPageLayout((BeginPageLayoutEventArgs) e);
      flag = e.Cancel;
      this.m_bounds = e.Bounds;
      this.usePaginateBounds = false;
    }
    return flag;
  }

  private PdfStringLayoutResult CreateMarkerResult(
    int index,
    PdfList curList,
    Stack<ListInfo> info,
    PdfListItem item)
  {
    PdfStringLayoutResult markerResult;
    if (curList is PdfOrderedList)
    {
      markerResult = this.CreateOrderedMarkerResult(curList, item, index, info, false);
    }
    else
    {
      SizeF empty = SizeF.Empty;
      markerResult = this.CreateUnorderedMarkerResult(curList, item, ref empty);
    }
    return markerResult;
  }

  private PdfStringLayoutResult CreateUnorderedMarkerResult(
    PdfList curList,
    PdfListItem item,
    ref SizeF markerSize)
  {
    PdfUnorderedMarker marker = (curList as PdfUnorderedList).Marker;
    PdfStringLayoutResult unorderedMarkerResult = (PdfStringLayoutResult) null;
    PdfFont markerFont = this.GetMarkerFont((PdfMarker) marker, item);
    PdfStringFormat markerFormat = this.GetMarkerFormat((PdfMarker) marker, item);
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    switch (marker.Style)
    {
      case PdfUnorderedMarkerStyle.CustomString:
        unorderedMarkerResult = pdfStringLayouter.Layout(marker.Text, markerFont, markerFormat, this.size);
        break;
      case PdfUnorderedMarkerStyle.CustomImage:
        markerSize = new SizeF(markerFont.Size, markerFont.Size);
        marker.Size = markerSize;
        break;
      case PdfUnorderedMarkerStyle.CustomTemplate:
        markerSize = new SizeF(markerFont.Size, markerFont.Size);
        marker.Size = markerSize;
        break;
      default:
        PdfStandardFont font = new PdfStandardFont(PdfFontFamily.ZapfDingbats, markerFont.Size);
        unorderedMarkerResult = pdfStringLayouter.Layout(marker.GetStyledText(), (PdfFont) font, (PdfStringFormat) null, this.size);
        marker.Size = unorderedMarkerResult.ActualSize;
        if (marker.Pen != null)
        {
          unorderedMarkerResult.m_actualSize = new SizeF(unorderedMarkerResult.ActualSize.Width + 2f * marker.Pen.Width, unorderedMarkerResult.ActualSize.Height + 2f * marker.Pen.Width);
          break;
        }
        break;
    }
    return unorderedMarkerResult;
  }

  private PdfStringLayoutResult CreateOrderedMarkerResult(
    PdfList list,
    PdfListItem item,
    int index,
    Stack<ListInfo> info,
    bool findMaxWidth)
  {
    PdfOrderedList pdfOrderedList = list as PdfOrderedList;
    pdfOrderedList.Marker.CurrentIndex = index;
    string text = string.Empty;
    if (pdfOrderedList.Marker.Style != PdfNumberStyle.None)
      text = pdfOrderedList.Marker.GetNumber() + pdfOrderedList.Marker.Suffix;
    if (pdfOrderedList.MarkerHierarchy)
    {
      foreach (ListInfo listInfo in (object[]) info.ToArray())
      {
        if (listInfo.List is PdfOrderedList list1 && list1.Marker.Style != PdfNumberStyle.None)
        {
          PdfOrderedMarker marker = list1.Marker;
          text = listInfo.Number + marker.Delimiter + text;
          if (!list1.MarkerHierarchy)
            break;
        }
        else
          break;
      }
    }
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    PdfOrderedMarker marker1 = (list as PdfOrderedList).Marker;
    PdfFont markerFont = this.GetMarkerFont((PdfMarker) marker1, item);
    PdfStringFormat pdfStringFormat = this.GetMarkerFormat((PdfMarker) marker1, item);
    SizeF size = new SizeF(this.size.Width, this.size.Height);
    if (!findMaxWidth)
    {
      size.Width = this.markerMaxWidth;
      pdfStringFormat = this.SetMarkerStringFormat(marker1, pdfStringFormat);
    }
    return pdfStringLayouter.Layout(text, markerFont, pdfStringFormat, size);
  }

  private PdfStringFormat SetMarkerStringFormat(
    PdfOrderedMarker marker,
    PdfStringFormat markerFormat)
  {
    markerFormat = markerFormat != null ? (PdfStringFormat) markerFormat.Clone() : new PdfStringFormat();
    if (marker.StringFormat == null)
    {
      markerFormat.Alignment = PdfTextAlignment.Right;
      if (marker.RightToLeft)
        markerFormat.Alignment = PdfTextAlignment.Left;
    }
    if (this.currentPage == null && markerFormat != null)
    {
      markerFormat = (PdfStringFormat) markerFormat.Clone();
      markerFormat.Alignment = PdfTextAlignment.Left;
    }
    return markerFormat;
  }

  private bool DrawMarker(
    PdfList curList,
    PdfListItem item,
    PdfStringLayoutResult markerResult,
    float posY,
    float posX)
  {
    if (curList is PdfOrderedList)
    {
      if (curList.Font != null && markerResult != null && (double) curList.Font.Size > (double) markerResult.m_actualSize.Height)
      {
        posY += (float) ((double) curList.Font.Size / 2.0 - (double) markerResult.m_actualSize.Height / 2.0);
        markerResult.m_actualSize.Height += posY;
      }
      this.DrawOrderedMarker(curList, markerResult, item, posX, posY);
    }
    else
    {
      if (curList.Font != null && markerResult != null && (double) curList.Font.Size > (double) markerResult.m_actualSize.Height)
      {
        posY += (float) ((double) curList.Font.Size / 2.0 - (double) markerResult.m_actualSize.Height / 2.0);
        markerResult.m_actualSize.Height += posY;
      }
      this.DrawUnorderedMarker(curList, markerResult, item, posX, posY);
    }
    return true;
  }

  private PdfStringLayoutResult DrawUnorderedMarker(
    PdfList curList,
    PdfStringLayoutResult markerResult,
    PdfListItem item,
    float posX,
    float posY)
  {
    PdfUnorderedMarker marker = (curList as PdfUnorderedList).Marker;
    PdfFont markerFont = this.GetMarkerFont((PdfMarker) marker, item);
    PdfPen markerPen = this.GetMarkerPen((PdfMarker) marker, item);
    PdfBrush markerBrush = this.GetMarkerBrush((PdfMarker) marker, item);
    PdfStringFormat markerFormat = this.GetMarkerFormat((PdfMarker) marker, item);
    if (this.currentPage != null && this.currentPage.Document != null && this.currentPage.Document.AutoTag || item.PdfTag != null)
    {
      this.m_graphics.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Label);
      (this.m_graphics.Tag as PdfStructureElement).Parent = item.PdfTag as PdfStructureElement;
    }
    if (markerResult != null)
    {
      PointF pointF = new PointF(posX - markerResult.ActualSize.Width, posY);
      marker.Size = markerResult.ActualSize;
      if (marker.Style == PdfUnorderedMarkerStyle.CustomString)
      {
        RectangleF layoutRectangle = new RectangleF(pointF, markerResult.ActualSize);
        this.m_graphics.DrawStringLayoutResult(markerResult, markerFont, markerPen, markerBrush, layoutRectangle, markerFormat);
      }
      else
      {
        marker.UnicodeFont = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, markerFont.Size);
        marker.Draw(this.m_graphics, pointF, markerBrush, markerPen);
      }
    }
    else
    {
      marker.Size = new SizeF(markerFont.Size, markerFont.Size);
      PointF point = new PointF(posX - markerFont.Size, posY);
      marker.Draw(this.m_graphics, point, markerBrush, markerPen);
    }
    return (PdfStringLayoutResult) null;
  }

  private PdfStringLayoutResult DrawOrderedMarker(
    PdfList curList,
    PdfStringLayoutResult markerResult,
    PdfListItem item,
    float posX,
    float posY)
  {
    PdfOrderedMarker marker = (curList as PdfOrderedList).Marker;
    PdfFont markerFont = this.GetMarkerFont((PdfMarker) marker, item);
    PdfStringFormat markerFormat = this.GetMarkerFormat((PdfMarker) marker, item);
    PdfPen markerPen = this.GetMarkerPen((PdfMarker) marker, item);
    PdfBrush markerBrush = this.GetMarkerBrush((PdfMarker) marker, item);
    RectangleF layoutRectangle = new RectangleF(new PointF(posX - this.markerMaxWidth, posY), markerResult.ActualSize);
    layoutRectangle.Width = this.markerMaxWidth;
    PdfStringFormat format = this.SetMarkerStringFormat(marker, markerFormat);
    if (this.currentPage != null && this.currentPage.Document != null && this.currentPage.Document.AutoTag || item.PdfTag != null)
    {
      this.m_graphics.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Label);
      (this.m_graphics.Tag as PdfStructureElement).Parent = item.PdfTag as PdfStructureElement;
    }
    this.m_graphics.DrawStringLayoutResult(markerResult, markerFont, markerPen, markerBrush, layoutRectangle, format);
    return markerResult;
  }

  private PdfFont GetMarkerFont(PdfMarker marker, PdfListItem item)
  {
    PdfFont markerFont = marker.Font;
    if (marker.Font == null)
    {
      markerFont = item.Font;
      if (item.Font == null)
        markerFont = this.currentFont;
    }
    marker.Font = markerFont;
    return markerFont;
  }

  private PdfStringFormat GetMarkerFormat(PdfMarker marker, PdfListItem item)
  {
    PdfStringFormat markerFormat = marker.StringFormat;
    if (marker.StringFormat == null)
    {
      markerFormat = item.StringFormat;
      if (item.StringFormat == null)
        markerFormat = this.currentFormat;
    }
    return markerFormat;
  }

  private PdfPen GetMarkerPen(PdfMarker marker, PdfListItem item)
  {
    PdfPen markerPen = marker.Pen;
    if (marker.Pen == null)
    {
      markerPen = item.Pen;
      if (item.Pen == null)
        markerPen = this.currentPen;
    }
    return markerPen;
  }

  private PdfBrush GetMarkerBrush(PdfMarker marker, PdfListItem item)
  {
    PdfBrush markerBrush = marker.Brush;
    if (marker.Brush == null)
    {
      markerBrush = item.Brush;
      if (item.Brush == null)
        markerBrush = this.currentBrush;
    }
    return markerBrush;
  }
}
