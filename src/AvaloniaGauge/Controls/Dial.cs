using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System.Collections.Generic;

namespace AvaloniaGauge.Controls;

public class Dial : TemplatedControl
{
    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(Minimum),
            0);

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(Maximum),
            100);

    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(Value),
            0);

    public static readonly StyledProperty<double> StartAngleProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(StartAngle),
            135);

    public static readonly StyledProperty<double> SweepAngleProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(SweepAngle),
            270);

    public static readonly StyledProperty<double> RegionThicknessProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(RegionThickness),
            22);

    public static readonly StyledProperty<IBrush?> TrackBrushProperty =
        AvaloniaProperty.Register<Dial, IBrush?>(
            nameof(TrackBrush));

    public static readonly StyledProperty<IBrush?> NeedleBrushProperty =
        AvaloniaProperty.Register<Dial, IBrush?>(
            nameof(NeedleBrush));

    public static readonly StyledProperty<double> NeedleThicknessProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(NeedleThickness),
            4);

    public static readonly StyledProperty<double> NeedleLengthProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(NeedleLength),
            0.85);

    public static readonly StyledProperty<double> NeedleTailLengthProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(NeedleTailLength),
            0.12);

    public static readonly StyledProperty<double> NeedleCenterRadiusProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(NeedleCenterRadius),
            7);

    public static readonly StyledProperty<double> MarkerDistanceProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(MarkerDistance),
            0);

    public static readonly StyledProperty<double> MarkerMarginProperty =
        AvaloniaProperty.Register<Dial, double>(
            nameof(MarkerMargin),
            24);

    public static readonly StyledProperty<AvaloniaList<DialRegion>?> RegionsProperty =
        AvaloniaProperty.Register<Dial, AvaloniaList<DialRegion>?>(
            nameof(Regions),
            defaultValue: new AvaloniaList<DialRegion>());

    public static readonly StyledProperty<AvaloniaList<DialMarker>?> MarkersProperty =
        AvaloniaProperty.Register<Dial, AvaloniaList<DialMarker>?>(
            nameof(Markers),
            defaultValue: new AvaloniaList<DialMarker>());

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

    public IList<DialRegion>? Regions
    {
        get => GetValue(RegionsProperty);
        set => SetValue(RegionsProperty, value);
    }

    public IList<DialMarker>? Markers
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