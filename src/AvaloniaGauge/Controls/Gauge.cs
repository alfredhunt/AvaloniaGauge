using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System.Collections.Generic;

namespace AvaloniaGauge.Controls;

public class Gauge : TemplatedControl
{
    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(Minimum),
            0);

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(Maximum),
            100);

    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(Value),
            0);

    public static readonly StyledProperty<double> StartAngleProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(StartAngle),
            135);

    public static readonly StyledProperty<double> SweepAngleProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(SweepAngle),
            270);

    public static readonly StyledProperty<double> RegionThicknessProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(RegionThickness),
            22);

    public static readonly StyledProperty<IBrush?> TrackBrushProperty =
        AvaloniaProperty.Register<Gauge, IBrush?>(
            nameof(TrackBrush));

    public static readonly StyledProperty<IBrush?> NeedleBrushProperty =
        AvaloniaProperty.Register<Gauge, IBrush?>(
            nameof(NeedleBrush));

    public static readonly StyledProperty<double> NeedleThicknessProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(NeedleThickness),
            4);

    public static readonly StyledProperty<double> NeedleLengthProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(NeedleLength),
            0.85);

    public static readonly StyledProperty<double> NeedleTailLengthProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(NeedleTailLength),
            0.12);

    public static readonly StyledProperty<double> NeedleCenterRadiusProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(NeedleCenterRadius),
            7);

    public static readonly StyledProperty<double> MarkerDistanceProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(MarkerDistance),
            0);

    public static readonly StyledProperty<double> MarkerMarginProperty =
        AvaloniaProperty.Register<Gauge, double>(
            nameof(MarkerMargin),
            24);

    public static readonly StyledProperty<AvaloniaList<GaugeRegion>?> RegionsProperty =
        AvaloniaProperty.Register<Gauge, AvaloniaList<GaugeRegion>?>(
            nameof(Regions),
            defaultValue: new AvaloniaList<GaugeRegion>());

    public static readonly StyledProperty<AvaloniaList<GaugeMarker>?> MarkersProperty =
        AvaloniaProperty.Register<Gauge, AvaloniaList<GaugeMarker>?>(
            nameof(Markers),
            defaultValue: new AvaloniaList<GaugeMarker>());

    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double StartAngle
    {
        get => GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    public double SweepAngle
    {
        get => GetValue(SweepAngleProperty);
        set => SetValue(SweepAngleProperty, value);
    }

    public double RegionThickness
    {
        get => GetValue(RegionThicknessProperty);
        set => SetValue(RegionThicknessProperty, value);
    }

    public IBrush? TrackBrush
    {
        get => GetValue(TrackBrushProperty);
        set => SetValue(TrackBrushProperty, value);
    }

    public IBrush? NeedleBrush
    {
        get => GetValue(NeedleBrushProperty);
        set => SetValue(NeedleBrushProperty, value);
    }

    public double NeedleThickness
    {
        get => GetValue(NeedleThicknessProperty);
        set => SetValue(NeedleThicknessProperty, value);
    }

    public double NeedleLength
    {
        get => GetValue(NeedleLengthProperty);
        set => SetValue(NeedleLengthProperty, value);
    }

    public double NeedleTailLength
    {
        get => GetValue(NeedleTailLengthProperty);
        set => SetValue(NeedleTailLengthProperty, value);
    }

    public double NeedleCenterRadius
    {
        get => GetValue(NeedleCenterRadiusProperty);
        set => SetValue(NeedleCenterRadiusProperty, value);
    }

    public double MarkerDistance
    {
        get => GetValue(MarkerDistanceProperty);
        set => SetValue(MarkerDistanceProperty, value);
    }

    public double MarkerMargin
    {
        get => GetValue(MarkerMarginProperty);
        set => SetValue(MarkerMarginProperty, value);
    }

    public IList<GaugeRegion>? Regions
    {
        get => GetValue(RegionsProperty);
        set => SetValue(RegionsProperty, value);
    }

    public IList<GaugeMarker>? Markers
    {
        get => GetValue(MarkersProperty);
        set => SetValue(MarkersProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var width = availableSize.Width;
        var height = availableSize.Height;

        if (double.IsInfinity(width) && double.IsInfinity(height))
        {
            return new Size(300, 300);
        }

        if (double.IsInfinity(width))
        {
            return new Size(height, height);
        }

        if (double.IsInfinity(height))
        {
            return new Size(width, width);
        }

        var size = Math.Min(width, height);

        return new Size(size, size);
    }
}