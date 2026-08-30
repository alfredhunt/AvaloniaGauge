using Avalonia.Media;

namespace AvaloniaGauge.Controls;

public sealed class DialMarker
{
    public double Value { get; set; }

    public string Text { get; set; } = string.Empty;

    public DialMarkerPlacement Placement { get; set; } =
        DialMarkerPlacement.Outside;

    /// <summary>
    /// Additional radial distance from the selected placement.
    /// Positive values move farther from the gauge center.
    /// Negative values move toward the gauge center.
    /// </summary>
    public double Offset { get; set; }

    public FontFamily? FontFamily { get; set; }

    public IBrush? Foreground { get; set; }

    public double? FontSize { get; set; }

    public FontWeight? FontWeight { get; set; }

    public FontStyle? FontStyle { get; set; }

    public FontStretch? FontStretch { get; set; }

    public bool ShowLine { get; set; } = true;

    public double LineThickness { get; set; } = 2;

    public IBrush? LineBrush { get; set; }
    public double Gap { get; set; } = 4;
}
