using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace AvaloniaGauge.Controls;

public sealed class GaugePresenter : TemplatedControl
{
    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(Minimum),
            0);

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(Maximum),
            100);

    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(Value),
            0);

    public static readonly StyledProperty<double> StartAngleProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(StartAngle),
            135);

    public static readonly StyledProperty<double> SweepAngleProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(SweepAngle),
            270);

    public static readonly StyledProperty<double> RegionThicknessProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(RegionThickness),
            22);

    public static readonly StyledProperty<IBrush?> TrackBrushProperty =
        AvaloniaProperty.Register<GaugePresenter, IBrush?>(
            nameof(TrackBrush));

    public static readonly StyledProperty<IBrush?> NeedleBrushProperty =
        AvaloniaProperty.Register<GaugePresenter, IBrush?>(
            nameof(NeedleBrush));

    public static readonly StyledProperty<double> NeedleThicknessProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(NeedleThickness),
            4);

    public static readonly StyledProperty<double> NeedleLengthProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(NeedleLength),
            0.85);

    public static readonly StyledProperty<double> NeedleTailLengthProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(NeedleTailLength),
            0.12);

    public static readonly StyledProperty<double> NeedleCenterRadiusProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(NeedleCenterRadius),
            7);

    public static readonly StyledProperty<double> MarkerDistanceProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(MarkerDistance),
            0);

    /// <summary>
    /// Reserves space between the Gauge geometry and the edge of the control
    /// for markers positioned outside the colored region.
    /// </summary>
    public static readonly StyledProperty<double> MarkerMarginProperty =
        AvaloniaProperty.Register<GaugePresenter, double>(
            nameof(MarkerMargin),
            24);

    public static readonly StyledProperty<IList<GaugeRegion>?> RegionsProperty =
        AvaloniaProperty.Register<GaugePresenter, IList<GaugeRegion>?>(
            nameof(Regions));

    public static readonly StyledProperty<IList<GaugeMarker>?> MarkersProperty =
        AvaloniaProperty.Register<GaugePresenter, IList<GaugeMarker>?>(
            nameof(Markers));

    private readonly List<CachedRegion> _regions = new();
    private readonly List<CachedMarker> _markers = new();

    private StreamGeometry? _trackGeometry;
    private Pen? _needlePen;

    private INotifyCollectionChanged? _regionsCollection;
    private INotifyCollectionChanged? _markersCollection;

    private Point _center;

    private double _trackRadius;
    private double _trackInnerRadius;
    private double _trackOuterRadius;

    private bool _cacheDirty = true;

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

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty)
        {
            InvalidateVisual();
            return;
        }

        if (change.Property == RegionsProperty)
        {
            UnsubscribeRegions();

            SubscribeRegions(
                change.GetNewValue<IList<GaugeRegion>?>());

            InvalidateCache();
            return;
        }

        if (change.Property == MarkersProperty)
        {
            UnsubscribeMarkers();

            SubscribeMarkers(
                change.GetNewValue<IList<GaugeMarker>?>());

            InvalidateCache();
            return;
        }

        if (change.Property == MinimumProperty ||
            change.Property == MaximumProperty ||
            change.Property == StartAngleProperty ||
            change.Property == SweepAngleProperty ||
            change.Property == RegionThicknessProperty ||
            change.Property == TrackBrushProperty ||
            change.Property == NeedleBrushProperty ||
            change.Property == NeedleThicknessProperty ||
            change.Property == NeedleLengthProperty ||
            change.Property == NeedleTailLengthProperty ||
            change.Property == NeedleCenterRadiusProperty ||
            change.Property == MarkerDistanceProperty ||
            change.Property == MarkerMarginProperty ||
            change.Property == FontFamilyProperty ||
            change.Property == FontSizeProperty ||
            change.Property == FontWeightProperty ||
            change.Property == FontStyleProperty ||
            change.Property == FontStretchProperty ||
            change.Property == ForegroundProperty)
        {
            InvalidateCache();
        }
    }

    protected override void OnSizeChanged(
        SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        if (e.PreviousSize != e.NewSize)
            InvalidateCache();
    }

    protected override void OnDetachedFromVisualTree(
        VisualTreeAttachmentEventArgs e)
    {
        UnsubscribeRegions();
        UnsubscribeMarkers();

        base.OnDetachedFromVisualTree(e);
    }

    public override void Render(
        DrawingContext context)
    {
        base.Render(context);

        if (Bounds.Width <= 0 ||
            Bounds.Height <= 0)
        {
            return;
        }

        if (_cacheDirty)
            RebuildCache();

        DrawStatic(context);
        DrawNeedle(context);
    }

    private void RebuildCache()
    {
        _cacheDirty = false;

        _trackGeometry = null;
        _needlePen = null;

        _regions.Clear();
        _markers.Clear();

        _center = new Point(
            Bounds.Width / 2.0,
            Bounds.Height / 2.0);

        var diameter =
            Math.Min(
                Bounds.Width,
                Bounds.Height);

        if (diameter <= 0)
            return;

        /*
         * MarkerMargin deliberately reduces the usable Gauge diameter.
         *
         * This gives outside markers physical room inside the control
         * instead of allowing their text to be clipped by the bounds.
         */
        var markerMargin =
            Math.Max(
                0,
                MarkerMargin);

        var usableDiameter =
            Math.Max(
                0,
                diameter - markerMargin * 2.0);

        var baseThickness =
            Math.Max(
                0,
                RegionThickness);

        if (usableDiameter <= 0 ||
            baseThickness <= 0)
        {
            return;
        }

        var outerRadius =
            Math.Max(
                0,
                usableDiameter / 2.0);

        var innerRadius =
            Math.Max(
                0,
                outerRadius - baseThickness);

        _trackInnerRadius = innerRadius;
        _trackOuterRadius = outerRadius;

        _trackRadius =
            (innerRadius + outerRadius) / 2.0;

        var sweep =
            NormalizeSweep(SweepAngle);

        if (sweep <= 0 ||
            _trackRadius <= 0)
        {
            return;
        }

        var startAngle =
            NormalizeAngle(StartAngle);

        _trackGeometry =
            CreateAnnularArc(
                _center,
                innerRadius,
                outerRadius,
                startAngle,
                sweep);

        BuildRegions(sweep);
        BuildMarkers(sweep);
        BuildNeedlePen();
    }

    private void BuildRegions(
        double sweep)
    {
        var regions = Regions;

        if (regions is null ||
            Maximum <= Minimum)
        {
            return;
        }

        foreach (var region in regions)
        {
            if (region is null ||
                region.Color is null)
            {
                continue;
            }

            var startFraction =
                ValueToFraction(region.Start);

            var endFraction =
                ValueToFraction(region.End);

            if (endFraction <= startFraction)
                continue;

            var regionSweep =
                sweep *
                (endFraction - startFraction);

            if (regionSweep <= 0)
                continue;

            var thickness =
                region.Thickness > 0
                    ? region.Thickness
                    : RegionThickness;

            if (thickness <= 0)
                continue;

            var halfThickness =
                thickness / 2.0;

            var innerRadius =
                Math.Max(
                    0,
                    _trackRadius - halfThickness);

            var outerRadius =
                _trackRadius + halfThickness;

            var geometry =
                CreateAnnularArc(
                    _center,
                    innerRadius,
                    outerRadius,
                    StartAngle +
                    sweep * startFraction,
                    regionSweep);

            _regions.Add(
                new CachedRegion(
                    geometry,
                    region.Color));
        }
    }

    private void BuildMarkers(
        double sweep)
    {
        _markers.Clear();

        var markers = Markers;

        if (markers is null ||
            markers.Count == 0 ||
            Maximum <= Minimum ||
            _trackRadius <= 0)
        {
            return;
        }

        var fontFamily = FontFamily;
        var fontSize = FontSize;
        var fontWeight = FontWeight;
        var fontStyle = FontStyle;
        var fontStretch = FontStretch;
        var foreground = Foreground;

        if (fontFamily is null ||
            fontSize <= 0)
        {
            return;
        }

        foreach (var marker in markers)
        {
            if (marker is null)
                continue;

            var fraction =
                ValueToFraction(marker.Value);

            var angle =
                StartAngle +
                sweep * fraction;

            var effectiveFontFamily =
                marker.FontFamily ??
                fontFamily;

            var effectiveFontSize =
                marker.FontSize ??
                fontSize;

            var effectiveFontWeight =
                marker.FontWeight ??
                fontWeight;

            var effectiveFontStyle =
                marker.FontStyle ??
                fontStyle;

            var effectiveFontStretch =
                marker.FontStretch ??
                fontStretch;

            var effectiveForeground =
                marker.Foreground ??
                foreground;

            if (effectiveFontFamily is null ||
                effectiveFontSize <= 0 ||
                effectiveForeground is null)
            {
                continue;
            }

            var typeface =
                new Typeface(
                    effectiveFontFamily,
                    effectiveFontStyle,
                    effectiveFontWeight,
                    effectiveFontStretch);

            var text =
                new FormattedText(
                    marker.Text ?? string.Empty,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    effectiveFontSize,
                    effectiveForeground);

            var markerRadius =
                GetMarkerTextRadius(marker);

            if (!double.IsFinite(markerRadius))
                continue;

            var position =
                PointOnCircle(
                    _center,
                    markerRadius,
                    angle);

            var origin =
                new Point(
                    position.X -
                    text.Width / 2.0,

                    position.Y -
                    text.Height / 2.0);

            var markerLine =
                BuildMarkerLine(
                    marker,
                    angle,
                    fraction,
                    effectiveForeground);

            _markers.Add(
                new CachedMarker(
                    text,
                    origin,
                    markerLine.Start,
                    markerLine.End,
                    markerLine.Pen));
        }
    }

    private double GetMarkerTextRadius(
        GaugeMarker marker)
    {
        var halfThickness =
            Math.Max(
                0,
                RegionThickness) / 2.0;

        var innerRadius =
            Math.Max(
                0,
                _trackRadius - halfThickness);

        var outerRadius =
            _trackRadius + halfThickness;

        var radius =
            marker.Placement switch
            {
                GaugeMarkerPlacement.Inside =>
                    innerRadius -
                    marker.Gap,

                GaugeMarkerPlacement.Center =>
                    _trackRadius,

                GaugeMarkerPlacement.Outside =>
                    outerRadius +
                    marker.Gap,

                _ =>
                    _trackRadius
            };

        /*
         * MarkerDistance is retained as the global raGauge adjustment.
         *
         * Offset is the marker-specific adjustment.
         */
        radius +=
            MarkerDistance +
            marker.Offset;

        return radius;
    }

    private MarkerLine BuildMarkerLine(
        GaugeMarker marker,
        double angle,
        double fraction,
        IBrush? fallbackBrush)
    {
        if (!marker.ShowLine ||
            marker.LineThickness <= 0)
        {
            return default;
        }

        var brush =
            marker.LineBrush ??
            fallbackBrush;

        if (brush is null)
            return default;

        var thickness =
            GetRegionThicknessAt(fraction);

        if (thickness <= 0)
            return default;

        var halfThickness =
            thickness / 2.0;

        var innerRadius =
            Math.Max(
                0,
                _trackRadius - halfThickness);

        var outerRadius =
            _trackRadius + halfThickness;

        var pen =
            new Pen(
                brush,
                marker.LineThickness);

        return new MarkerLine(
            PointOnCircle(
                _center,
                innerRadius,
                angle),

            PointOnCircle(
                _center,
                outerRadius,
                angle),

            pen);
    }

    private double GetRegionThicknessAt(
        double fraction)
    {
        if (Regions is not null)
        {
            var value =
                Minimum +
                (Maximum - Minimum) *
                fraction;

            foreach (var region in Regions)
            {
                if (region is null)
                    continue;

                if (value >= region.Start &&
                    value <= region.End)
                {
                    return region.Thickness > 0
                        ? region.Thickness
                        : RegionThickness;
                }
            }
        }

        return RegionThickness;
    }

    private void BuildNeedlePen()
    {
        if (NeedleBrush is null ||
            NeedleThickness <= 0)
        {
            return;
        }

        _needlePen =
            new Pen(
                NeedleBrush,
                NeedleThickness);
    }

    private void DrawStatic(
        DrawingContext context)
    {
        if (_trackGeometry is not null &&
            TrackBrush is not null)
        {
            context.DrawGeometry(
                TrackBrush,
                null,
                _trackGeometry);
        }

        foreach (var region in _regions)
        {
            context.DrawGeometry(
                region.Brush,
                null,
                region.Geometry);
        }

        foreach (var marker in _markers)
        {
            if (marker.LinePen is not null)
            {
                context.DrawLine(
                    marker.LinePen,
                    marker.LineStart,
                    marker.LineEnd);
            }

            context.DrawText(
                marker.Text,
                marker.Origin);
        }
    }

    private void DrawNeedle(
        DrawingContext context)
    {
        if (_needlePen is null ||
            _trackRadius <= 0)
        {
            return;
        }

        var fraction =
            ValueToFraction(Value);

        var sweep =
            NormalizeSweep(SweepAngle);

        if (sweep <= 0)
            return;

        var angle =
            NormalizeAngle(
                StartAngle +
                sweep * fraction);

        var needleLength =
            Math.Clamp(
                NeedleLength,
                0,
                1);

        var tailLength =
            Math.Clamp(
                NeedleTailLength,
                0,
                1);

        var tip =
            PointOnCircle(
                _center,
                _trackRadius *
                needleLength,
                angle);

        var tail =
            PointOnCircle(
                _center,
                -_trackRadius *
                tailLength,
                angle);

        context.DrawLine(
            _needlePen,
            tail,
            tip);

        if (NeedleBrush is not null &&
            NeedleCenterRadius > 0)
        {
            context.DrawEllipse(
                NeedleBrush,
                null,
                _center,
                NeedleCenterRadius,
                NeedleCenterRadius);
        }
    }

    private double ValueToFraction(
        double value)
    {
        if (!double.IsFinite(Minimum) ||
            !double.IsFinite(Maximum) ||
            !double.IsFinite(value) ||
            Maximum <= Minimum)
        {
            return 0;
        }

        return Math.Clamp(
            (value - Minimum) /
            (Maximum - Minimum),
            0,
            1);
    }

    private static double NormalizeSweep(
        double sweep)
    {
        if (!double.IsFinite(sweep))
            return 0;

        return Math.Clamp(
            sweep,
            0,
            360);
    }

    private static double NormalizeAngle(
        double angle)
    {
        if (!double.IsFinite(angle))
            return 0;

        angle %= 360;

        if (angle < 0)
            angle += 360;

        return angle;
    }

    private static Point PointOnCircle(
        Point center,
        double radius,
        double angle)
    {
        var radians =
            angle *
            Math.PI /
            180.0;

        return new Point(
            center.X +
            Math.Sin(radians) * radius,

            center.Y -
            Math.Cos(radians) * radius);
    }

    private static StreamGeometry CreateAnnularArc(
        Point center,
        double innerRadius,
        double outerRadius,
        double startAngle,
        double sweepAngle)
    {
        var geometry =
            new StreamGeometry();

        using var context =
            geometry.Open();

        if (outerRadius <= 0 ||
            sweepAngle <= 0)
        {
            return geometry;
        }

        if (sweepAngle >= 359.999999)
        {
            AddAnnularArc(
                context,
                center,
                innerRadius,
                outerRadius,
                startAngle,
                180);

            AddAnnularArc(
                context,
                center,
                innerRadius,
                outerRadius,
                startAngle + 180,
                180);

            return geometry;
        }

        AddAnnularArc(
            context,
            center,
            innerRadius,
            outerRadius,
            startAngle,
            sweepAngle);

        return geometry;
    }

    private static void AddAnnularArc(
        StreamGeometryContext context,
        Point center,
        double innerRadius,
        double outerRadius,
        double startAngle,
        double sweepAngle)
    {
        if (sweepAngle <= 0 ||
            outerRadius <= 0)
        {
            return;
        }

        var outerStart =
            PointOnCircle(
                center,
                outerRadius,
                startAngle);

        var outerEnd =
            PointOnCircle(
                center,
                outerRadius,
                startAngle +
                sweepAngle);

        var innerEnd =
            PointOnCircle(
                center,
                innerRadius,
                startAngle +
                sweepAngle);

        var innerStart =
            PointOnCircle(
                center,
                innerRadius,
                startAngle);

        var largeArc =
            sweepAngle > 180;

        context.BeginFigure(
            outerStart,
            true);

        context.ArcTo(
            outerEnd,
            new Size(
                outerRadius,
                outerRadius),
            0,
            largeArc,
            SweepDirection.Clockwise);

        context.LineTo(
            innerEnd);

        if (innerRadius > 0)
        {
            context.ArcTo(
                innerStart,
                new Size(
                    innerRadius,
                    innerRadius),
                0,
                largeArc,
                SweepDirection.CounterClockwise);
        }
        else
        {
            context.LineTo(center);
        }

        context.LineTo(
            outerStart);

        context.EndFigure(true);
    }

    private void SubscribeRegions(
        IList<GaugeRegion>? regions)
    {
        if (regions is not INotifyCollectionChanged collection)
            return;

        _regionsCollection = collection;

        collection.CollectionChanged +=
            OnRegionsCollectionChanged;
    }

    private void UnsubscribeRegions()
    {
        if (_regionsCollection is null)
            return;

        _regionsCollection.CollectionChanged -=
            OnRegionsCollectionChanged;

        _regionsCollection = null;
    }

    private void SubscribeMarkers(
        IList<GaugeMarker>? markers)
    {
        if (markers is not INotifyCollectionChanged collection)
            return;

        _markersCollection = collection;

        collection.CollectionChanged +=
            OnMarkersCollectionChanged;
    }

    private void UnsubscribeMarkers()
    {
        if (_markersCollection is null)
            return;

        _markersCollection.CollectionChanged -=
            OnMarkersCollectionChanged;

        _markersCollection = null;
    }

    private void OnRegionsCollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        InvalidateCache();
    }

    private void OnMarkersCollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        InvalidateCache();
    }

    private void InvalidateCache()
    {
        _cacheDirty = true;
        InvalidateVisual();
    }

    private readonly record struct MarkerLine(
        Point Start,
        Point End,
        Pen? Pen);

    private sealed class CachedRegion
    {
        public CachedRegion(
            StreamGeometry geometry,
            IBrush brush)
        {
            Geometry = geometry;
            Brush = brush;
        }

        public StreamGeometry Geometry { get; }

        public IBrush Brush { get; }
    }

    private sealed class CachedMarker
    {
        public CachedMarker(
            FormattedText text,
            Point origin,
            Point lineStart,
            Point lineEnd,
            Pen? linePen)
        {
            Text = text;
            Origin = origin;
            LineStart = lineStart;
            LineEnd = lineEnd;
            LinePen = linePen;
        }

        public FormattedText Text { get; }

        public Point Origin { get; }

        public Point LineStart { get; }

        public Point LineEnd { get; }

        public Pen? LinePen { get; }
    }
}