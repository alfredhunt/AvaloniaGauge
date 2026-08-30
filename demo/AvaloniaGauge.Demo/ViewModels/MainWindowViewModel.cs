using Avalonia.Collections;
using Avalonia.Media;
using AvaloniaGauge.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Globalization;

namespace AvaloniaGauge.Demo.ViewModels;

public sealed class MainWindowViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit> AddRegionCommand { get; }

    public ReactiveCommand<Unit, Unit> AddMarkerCommand { get; }

    public ReactiveCommand<GaugeRegionViewModel, Unit> RemoveRegionCommand { get; }

    public ReactiveCommand<GaugeMarkerViewModel, Unit> RemoveMarkerCommand { get; }

    private double _minimum = 0;
    private double _maximum = 100;
    private double _value = 65;

    private double _startAngle = 225;
    private double _sweepAngle = 270;
    private double _regionThickness = 22;

    private double _needleThickness = 4;
    private double _needleLength = 0.85;
    private double _needleTailLength = 0.12;
    private double _needleCenterRadius = 7;

    private double _markerDistance = 0;

    private string _trackColor = "#303030";
    private string _needleColor = "#202020";

    private AvaloniaList<GaugeRegion> _regions = [];
    private AvaloniaList<GaugeMarker> _markers = [];

    public MainWindowViewModel()
    {
        RegionEditors.CollectionChanged += OnRegionsCollectionChanged;
        MarkerEditors.CollectionChanged += OnMarkersCollectionChanged;

        foreach (var region in RegionEditors)
            region.PropertyChanged += OnRegionPropertyChanged;

        foreach (var marker in MarkerEditors)
            marker.PropertyChanged += OnMarkerPropertyChanged;

        AddInitialRegions();
        AddInitialMarkers();

        RebuildRegions();
        RebuildMarkers();

        AddRegionCommand = ReactiveCommand.Create(AddRegion);
        AddMarkerCommand = ReactiveCommand.Create(AddMarker);

        RemoveRegionCommand =
            ReactiveCommand.Create<GaugeRegionViewModel>(RemoveRegion);

        RemoveMarkerCommand =
            ReactiveCommand.Create<GaugeMarkerViewModel>(RemoveMarker);

        BuildAxaml();

        this.WhenAnyValue(x => x.Value)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.Minimum)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.Maximum)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.StartAngle)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.SweepAngle)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.RegionThickness)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.MarkerDistance)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.NeedleThickness)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.NeedleLength)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.NeedleTailLength)
            .Subscribe(_ => BuildAxaml());

        this.WhenAnyValue(x => x.NeedleCenterRadius)
            .Subscribe(_ => BuildAxaml());
    }

    // =====================================================================
    // Gauge properties
    // =====================================================================

    public double Minimum
    {
        get => _minimum;
        set => this.RaiseAndSetIfChanged(ref _minimum, value);
    }

    public double Maximum
    {
        get => _maximum;
        set => this.RaiseAndSetIfChanged(ref _maximum, value);
    }

    public double Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public double StartAngle
    {
        get => _startAngle;
        set => this.RaiseAndSetIfChanged(ref _startAngle, value);
    }

    public double SweepAngle
    {
        get => _sweepAngle;
        set => this.RaiseAndSetIfChanged(ref _sweepAngle, value);
    }

    public double RegionThickness
    {
        get => _regionThickness;
        set => this.RaiseAndSetIfChanged(ref _regionThickness, value);
    }

    public double NeedleThickness
    {
        get => _needleThickness;
        set => this.RaiseAndSetIfChanged(ref _needleThickness, value);
    }

    public double NeedleLength
    {
        get => _needleLength;
        set => this.RaiseAndSetIfChanged(ref _needleLength, value);
    }

    public double NeedleTailLength
    {
        get => _needleTailLength;
        set => this.RaiseAndSetIfChanged(ref _needleTailLength, value);
    }

    public double NeedleCenterRadius
    {
        get => _needleCenterRadius;
        set => this.RaiseAndSetIfChanged(ref _needleCenterRadius, value);
    }

    public double MarkerDistance
    {
        get => _markerDistance;
        set => this.RaiseAndSetIfChanged(ref _markerDistance, value);
    }

    // =====================================================================
    // Colors
    // =====================================================================

    public string TrackColor
    {
        get => _trackColor;
        set
        {
            this.RaiseAndSetIfChanged(ref _trackColor, value);
        }
    }

    public string NeedleColor
    {
        get => _needleColor;
        set
        {
            this.RaiseAndSetIfChanged(ref _needleColor, value);
        }
    }

    public IBrush TrackBrush =>
        ParseBrush(TrackColor);

    public IBrush NeedleBrush =>
        ParseBrush(NeedleColor);

    // =====================================================================
    // Region editor collection
    // =====================================================================

    public ObservableCollection<GaugeRegionViewModel> RegionEditors { get; } = [];

    public AvaloniaList<GaugeRegion> Regions
    {
        get => _regions;
        private set
        {
            if (ReferenceEquals(_regions, value))
                return;

            this.RaiseAndSetIfChanged(
                ref _regions,
                value);
        }
    }

    private void UpdateRegions()
    {
        Regions.Clear();

        foreach (var editor in RegionEditors)
        {
            Regions.Add(
                new GaugeRegion
                {
                    Start = editor.Start,
                    End = editor.End,
                    Color = new SolidColorBrush(editor.Color),
                    Thickness = editor.Thickness
                });
        }
    }

    // =====================================================================
    // Marker editor collection
    // =====================================================================

    public ObservableCollection<GaugeMarkerViewModel> MarkerEditors { get; } = [];

    public AvaloniaList<GaugeMarker> Markers
    {
        get => _markers;
        private set
        {
            if (ReferenceEquals(_markers, value))
                return;

            this.RaiseAndSetIfChanged(
                ref _markers,
                value);
        }
    }

    // =====================================================================
    // Region commands
    // =====================================================================

    public void AddRegion()
    {
        var start =
            RegionEditors.Count == 0
                ? Minimum
                : RegionEditors[^1].End;

        var end =
            Math.Min(
                Maximum,
                start + (Maximum - Minimum) * 0.25);

        var region =
            new GaugeRegionViewModel
            {
                Start = start,
                End = end,
                Color = Colors.Gray
            };

        RegionEditors.Add(region);
    }

    public void RemoveRegion(
        GaugeRegionViewModel region)
    {
        if (!RegionEditors.Remove(region))
            return;
    }

    // =====================================================================
    // Marker commands
    // =====================================================================

    public void AddMarker()
    {
        var marker =
            new GaugeMarkerViewModel
            {
                Value = Minimum,
                Text = Minimum.ToString("0"),
                Placement = GaugeMarkerPlacement.Outside
            };

        MarkerEditors.Add(marker);
    }

    public void RemoveMarker(
        GaugeMarkerViewModel marker)
    {
        if (!MarkerEditors.Remove(marker))
            return;
    }

    // =====================================================================
    // Collection monitoring
    // =====================================================================

    private void OnRegionsCollectionChanged(
        object? sender,
        System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (GaugeRegionViewModel region in e.OldItems)
                region.PropertyChanged -= OnRegionPropertyChanged;
        }

        if (e.NewItems is not null)
        {
            foreach (GaugeRegionViewModel region in e.NewItems)
                region.PropertyChanged += OnRegionPropertyChanged;
        }

        RebuildRegions();
        BuildAxaml();
    }

    private void OnMarkersCollectionChanged(
        object? sender,
        System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (GaugeMarkerViewModel marker in e.OldItems)
                marker.PropertyChanged -= OnMarkerPropertyChanged;
        }

        if (e.NewItems is not null)
        {
            foreach (GaugeMarkerViewModel marker in e.NewItems)
                marker.PropertyChanged += OnMarkerPropertyChanged;
        }

        RebuildMarkers();
        BuildAxaml();
    }

    private readonly List<IDisposable> _regionSubscriptions = new();
    private readonly List<IDisposable> _markerSubscriptions = new();

    private void OnRegionPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        RebuildRegions();
        BuildAxaml();
    }

    private void OnMarkerPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        RebuildMarkers();
        BuildAxaml();
    }

    // =====================================================================
    // Gauge collection generation
    // =====================================================================

    private void RebuildRegions()
    {
        Regions =
            new AvaloniaList<GaugeRegion>(
                RegionEditors.Select(
                    region =>
                        new GaugeRegion
                        {
                            Start = region.Start,
                            End = region.End,
                            Color = new SolidColorBrush(region.Color),
                            Thickness = region.Thickness
                        }));
    }

    private void RebuildMarkers()
    {
        Markers =
            new AvaloniaList<GaugeMarker>(
                MarkerEditors.Select(
                    marker =>
                        new GaugeMarker
                        {
                            Value = marker.Value,
                            Text = marker.Text,

                            Placement =
                                marker.Placement,

                            Gap =
                                marker.Gap,

                            Offset =
                                marker.Offset,

                            Foreground =
                                marker.ForegroundBrush,

                            FontSize =
                                marker.FontSize,

                            ShowLine =
                                marker.ShowLine,

                            LineThickness =
                                marker.LineThickness,

                            LineBrush =
                                marker.LineBrush
                        }));
    }

    // =====================================================================
    // Initial demo configuration
    // =====================================================================

    private void AddInitialRegions()
    {
        RegionEditors.Add(
            new GaugeRegionViewModel
            {
                Start = 0,
                End = 30,
                Color = Colors.Green
            });

        RegionEditors.Add(
            new GaugeRegionViewModel
            {
                Start = 30,
                End = 70,
                Color = Colors.Yellow
            });

        RegionEditors.Add(
            new GaugeRegionViewModel
            {
                Start = 70,
                End = 100,
                Color = Colors.Red
            });
    }

    private void AddInitialMarkers()
    {
        MarkerEditors.Add(
            new GaugeMarkerViewModel
            {
                Value = 0,
                Text = "0",
                Placement = GaugeMarkerPlacement.Outside,
                ShowLine = true
            });

        MarkerEditors.Add(
            new GaugeMarkerViewModel
            {
                Value = 50,
                Text = "50",
                Placement = GaugeMarkerPlacement.Outside,
                ShowLine = true
            });

        MarkerEditors.Add(
            new GaugeMarkerViewModel
            {
                Value = 100,
                Text = "100",
                Placement = GaugeMarkerPlacement.Outside,
                ShowLine = true
            });
    }

    // =====================================================================
    // Helpers
    // =====================================================================

    private static IBrush ParseBrush(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Brushes.Transparent;

        try
        {
            return Brush.Parse(value);
        }
        catch (FormatException)
        {
            return Brushes.Transparent;
        }
    }

    private string _generatedAxaml = string.Empty;

    public string GeneratedAxaml
    {
        get => _generatedAxaml;
        private set => this.RaiseAndSetIfChanged(
            ref _generatedAxaml,
            value);
    }

    private void BuildAxaml()
    {
        var sb = new StringBuilder();

        sb.AppendLine("<controls:Gauge");
        sb.AppendLine("    Minimum=\"{Binding Minimum}\"");
        sb.AppendLine("    Maximum=\"{Binding Maximum}\"");
        sb.AppendLine("    Value=\"{Binding Value}\"");
        sb.AppendLine();
        sb.AppendLine($"    StartAngle=\"{Format(StartAngle)}\"");
        sb.AppendLine($"    SweepAngle=\"{Format(SweepAngle)}\"");
        sb.AppendLine();
        sb.AppendLine($"    RegionThickness=\"{Format(RegionThickness)}\"");
        sb.AppendLine($"    MarkerDistance=\"{Format(MarkerDistance)}\"");
        sb.AppendLine();
        sb.AppendLine($"    NeedleThickness=\"{Format(NeedleThickness)}\"");
        sb.AppendLine($"    NeedleLength=\"{Format(NeedleLength)}\"");
        sb.AppendLine($"    NeedleTailLength=\"{Format(NeedleTailLength)}\"");
        sb.AppendLine($"    NeedleCenterRadius=\"{Format(NeedleCenterRadius)}\">");

        BuildRegionsAxaml(sb);
        BuildMarkersAxaml(sb);

        sb.AppendLine();
        sb.AppendLine("</controls:Gauge>");

        GeneratedAxaml = sb.ToString();
    }

    private static string Format(double value)
    {
        return value.ToString(
            "0.####",
            CultureInfo.InvariantCulture);
    }

    private void BuildRegionsAxaml(StringBuilder sb)
    {
        if (RegionEditors.Count == 0)
            return;

        sb.AppendLine();
        sb.AppendLine("    <controls:Gauge.Regions>");

        foreach (var region in RegionEditors)
        {
            sb.AppendLine();
            sb.AppendLine("        <controls:GaugeRegion");
            sb.AppendLine($"            Start=\"{Format(region.Start)}\"");
            sb.AppendLine($"            End=\"{Format(region.End)}\"");
            sb.AppendLine($"            Color=\"{region.HexColor}\" />");
        }

        sb.AppendLine();
        sb.AppendLine("    </controls:Gauge.Regions>");
    }

    private static string BrushToHex(IBrush? brush)
    {
        if (brush is SolidColorBrush solid)
        {
            var color = solid.Color;

            return color.A == 255
                ? $"#{color.R:X2}{color.G:X2}{color.B:X2}"
                : $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        return "#000000";
    }

    private void BuildMarkersAxaml(StringBuilder sb)
    {
        if (MarkerEditors.Count == 0)
            return;

        sb.AppendLine();
        sb.AppendLine("    <controls:Gauge.Markers>");

        foreach (var marker in MarkerEditors)
        {
            sb.AppendLine();
            sb.AppendLine("        <controls:GaugeMarker");
            sb.AppendLine($"            Value=\"{Format(marker.Value)}\"");
            sb.AppendLine($"            Text=\"{EscapeXml(marker.Text)}\"");
            sb.AppendLine($"            Placement=\"{marker.Placement}\"");
            sb.AppendLine($"            Gap=\"{Format(marker.Gap)}\"");
            sb.AppendLine($"            Offset=\"{Format(marker.Offset)}\"");
            sb.AppendLine($"            FontSize=\"{Format(marker.FontSize)}\" />");
        }

        sb.AppendLine();
        sb.AppendLine("    </controls:Gauge.Markers>");
    }

    private static string EscapeXml(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value
            .Replace("&", "&amp;")
            .Replace("\"", "&quot;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("'", "&apos;");
    }
}
