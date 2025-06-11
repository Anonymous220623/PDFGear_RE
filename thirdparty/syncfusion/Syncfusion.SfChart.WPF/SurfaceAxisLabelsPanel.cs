// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SurfaceAxisLabelsPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SurfaceAxisLabelsPanel : ILayoutCalculator
{
  private Panel labelsPanels;
  private Size desiredSize;
  private UIElementsRecycler<TextBlock> textBlockRecycler;
  private UIElementsRecycler<ContentControl> contentControlRecycler;
  private SurfaceAxisLabelLayout labelLayout;
  private List<UIElement> children;

  public Panel Panel => this.labelsPanels;

  public Size DesiredSize => this.desiredSize;

  public SurfaceAxis Axis { get; set; }

  public List<UIElement> Children
  {
    get
    {
      this.children = this.textBlockRecycler.generatedElements.Cast<UIElement>().ToList<UIElement>();
      if (this.children.Count < 1)
        this.children = this.contentControlRecycler.generatedElements.Cast<UIElement>().ToList<UIElement>();
      return this.children;
    }
  }

  public double Left { get; set; }

  public double Top { get; set; }

  public SurfaceAxisLabelsPanel(Panel panel)
  {
    this.labelsPanels = panel;
    this.textBlockRecycler = new UIElementsRecycler<TextBlock>(panel);
    this.contentControlRecycler = new UIElementsRecycler<ContentControl>(panel);
  }

  public Size Measure(Size availableSize)
  {
    this.labelLayout = SurfaceAxisLabelLayout.CreateAxisLayout(this.Axis, this.Children);
    this.desiredSize = this.labelLayout.Measure(availableSize);
    return this.desiredSize;
  }

  public Size Arrange(Size finalSize)
  {
    if (this.labelLayout != null)
    {
      this.labelLayout.Arrange(this.DesiredSize);
      this.labelLayout = (SurfaceAxisLabelLayout) null;
    }
    return finalSize;
  }

  public void DetachElements()
  {
    this.labelsPanels = (Panel) null;
    if (this.textBlockRecycler != null)
      this.textBlockRecycler.Clear();
    if (this.contentControlRecycler == null)
      return;
    this.contentControlRecycler.Clear();
  }

  internal void GenerateContainers()
  {
    int index = 0;
    ObservableCollection<ChartAxisLabel> visibleLabels = this.Axis.VisibleLabels;
    if (this.Axis.LabelTemplate == null)
    {
      this.contentControlRecycler.Clear();
      this.textBlockRecycler.GenerateElements(visibleLabels.Count);
      foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) visibleLabels)
      {
        if (chartAxisLabel.LabelContent != null)
          this.textBlockRecycler[index].Text = chartAxisLabel.LabelContent.ToString();
        ++index;
      }
    }
    else
    {
      this.textBlockRecycler.Clear();
      this.contentControlRecycler.GenerateElements(visibleLabels.Count);
      foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) visibleLabels)
      {
        ContentControl contentControl = this.contentControlRecycler[index];
        contentControl.ContentTemplate = this.Axis.LabelTemplate;
        contentControl.ApplyTemplate();
        contentControl.Content = (object) chartAxisLabel;
        ++index;
      }
    }
  }

  public void UpdateElements() => this.GenerateContainers();
}
