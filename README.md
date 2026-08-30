# AvaloniaGauge

A customizable, resizable gauge control for [Avalonia UI](https://avaloniaui.net/) applications.

AvaloniaGauge provides a reusable `Dial` control with configurable value ranges, colored regions, value markers, marker positioning, typography, and a customizable needle.

## Features

* Resizes automatically to the space provided by its parent
* Configurable minimum and maximum values
* Configurable current value
* Configurable start and sweep angles
* Colored gauge regions
* Configurable region thickness
* Value markers
* Marker text positioned:

  * Inside the gauge
  * Centered on the gauge
  * Outside the gauge
* Marker gap and radial offset
* Custom marker typography
* Custom marker text colors
* Optional marker lines
* Custom marker line colors and thickness
* Configurable needle
* Configurable track brush
* MVVM-friendly property binding
* Dynamic regions and markers
* Templated Avalonia control
* Interactive demonstration application
* AXAML configuration generation in the demo

## Requirements

* .NET 10
* Avalonia UI 12.x

Avalonia 12 currently supports `net10.0`.

## Installation

Install the package from NuGet:

```powershell
dotnet add package AvaloniaGauge
```

Or add it directly to your project:

```xml
<PackageReference Include="AvaloniaGauge" Version="1.0.0" />
```

Use the current published package version in place of `1.0.0`.

## Basic Usage

Import the control namespace:

```xml
xmlns:controls="using:AvaloniaGauge.Controls"
```

A complete example:

```xml
<controls:Dial
    Minimum="{Binding Minimum}"
    Maximum="{Binding Maximum}"
    Value="{Binding Value}"

    StartAngle="225"
    SweepAngle="270"

    RegionThickness="22"
    MarkerDistance="0"

    NeedleThickness="4"
    NeedleLength="0.85"
    NeedleTailLength="0.12"
    NeedleCenterRadius="7">

    <controls:Dial.Regions>

        <controls:DialRegion
            Start="0"
            End="30"
            Color="#008000" />

        <controls:DialRegion
            Start="30"
            End="70"
            Color="#FFFF00" />

        <controls:DialRegion
            Start="70"
            End="100"
            Color="#FF0000" />

    </controls:Dial.Regions>

    <controls:Dial.Markers>

        <controls:DialMarker
            Value="0"
            Text="0"
            Placement="Outside"
            Gap="0"
            Offset="0"
            FontSize="14" />

        <controls:DialMarker
            Value="50"
            Text="50"
            Placement="Outside"
            Gap="0"
            Offset="0"
            FontSize="14" />

        <controls:DialMarker
            Value="100"
            Text="100"
            Placement="Outside"
            Gap="0"
            Offset="0"
            FontSize="14" />

    </controls:Dial.Markers>

</controls:Dial>
```

The `Dial` does not require an explicit `Width` or `Height`. It sizes itself based on the space provided by its parent container.

For example:

```xml
<Grid>
    <controls:Dial
        Minimum="0"
        Maximum="100"
        Value="65" />
</Grid>
```

## Dial Properties

| Property             | Description                                       |
| -------------------- | ------------------------------------------------- |
| `Minimum`            | Minimum value represented by the gauge            |
| `Maximum`            | Maximum value represented by the gauge            |
| `Value`              | Current value                                     |
| `StartAngle`         | Starting angle of the gauge                       |
| `SweepAngle`         | Angular range of the gauge                        |
| `RegionThickness`    | Default thickness of the gauge regions            |
| `MarkerDistance`     | Default marker distance                           |
| `TrackBrush`         | Brush used for the gauge track                    |
| `NeedleBrush`        | Brush used for the needle                         |
| `NeedleThickness`    | Needle thickness                                  |
| `NeedleLength`       | Needle length as a proportion of the gauge radius |
| `NeedleTailLength`   | Needle length extending behind the center         |
| `NeedleCenterRadius` | Radius of the center of the needle                |

### Angles

`StartAngle` controls where the gauge begins.

`SweepAngle` controls how much of the circular path is used.

For example:

```xml
<controls:Dial
    StartAngle="225"
    SweepAngle="270" />
```

creates a 270-degree gauge beginning at 225 degrees.

## Regions

Regions divide the gauge into colored value ranges.

```xml
<controls:Dial.Regions>

    <controls:DialRegion
        Start="0"
        End="30"
        Color="#008000" />

    <controls:DialRegion
        Start="30"
        End="70"
        Color="#FFFF00" />

    <controls:DialRegion
        Start="70"
        End="100"
        Color="#FF0000" />

</controls:Dial.Regions>
```

Each region specifies:

* `Start`
* `End`
* `Color`

A region can optionally override the default gauge thickness:

```xml
<controls:DialRegion
    Start="0"
    End="30"
    Color="#008000"
    Thickness="28" />
```

If a region does not specify its own thickness, the `Dial.RegionThickness` value is used.

## Markers

Markers place text at specific values around the gauge.

```xml
<controls:Dial.Markers>

    <controls:DialMarker
        Value="0"
        Text="0"
        Placement="Outside"
        Gap="0"
        Offset="0"
        FontSize="14" />

    <controls:DialMarker
        Value="50"
        Text="50"
        Placement="Outside"
        Gap="0"
        Offset="0"
        FontSize="14" />

    <controls:DialMarker
        Value="100"
        Text="100"
        Placement="Outside"
        Gap="0"
        Offset="0"
        FontSize="14" />

</controls:Dial.Markers>
```

### Marker Placement

A marker can be positioned relative to the gauge region using `Placement`:

```xml
Placement="Inside"
```

```xml
Placement="Center"
```

```xml
Placement="Outside"
```

This controls whether the marker text is placed inside the region, centered on the region, or outside the region.

### Marker Gap

`Gap` controls the distance between the marker and the gauge region.

```xml
<controls:DialMarker
    Value="50"
    Text="50"
    Placement="Outside"
    Gap="8" />
```

### Marker Offset

`Offset` provides additional radial positioning after the placement and gap have been calculated.

```xml
<controls:DialMarker
    Value="50"
    Text="50"
    Placement="Outside"
    Gap="8"
    Offset="4" />
```

## Marker Text Styling

Markers support individual font settings, allowing different markers to have different appearances.

```xml
<controls:DialMarker
    Value="50"
    Text="50"
    Placement="Outside"
    FontSize="16"
    FontWeight="Bold"
    Foreground="White" />
```

Supported marker typography includes:

* `FontFamily`
* `FontSize`
* `FontWeight`
* `FontStyle`
* `FontStretch`
* `Foreground`

Marker-specific settings override the corresponding default settings.

## Marker Lines

Markers can optionally display a line through the gauge region.

```xml
<controls:DialMarker
    Value="50"
    Text="50"
    Placement="Outside"
    ShowLine="True"
    LineThickness="2"
    LineBrush="White" />
```

Marker lines use the thickness of the applicable gauge region when determining where the line begins and ends.

## Needle

The needle can be independently configured.

```xml
<controls:Dial
    Value="65"
    NeedleThickness="4"
    NeedleLength="0.85"
    NeedleTailLength="0.12"
    NeedleCenterRadius="7"
    NeedleBrush="White" />
```

### Needle Length

`NeedleLength` is expressed as a proportion of the gauge radius.

```text
0.0 = no needle
1.0 = full gauge radius
```

### Needle Tail

`NeedleTailLength` controls the portion of the needle extending behind the center.

```xml
<controls:Dial
    NeedleLength="0.85"
    NeedleTailLength="0.12" />
```

## MVVM

All primary dial properties can be bound from a view model.

```xml
<controls:Dial
    Minimum="{Binding Minimum}"
    Maximum="{Binding Maximum}"
    Value="{Binding Value}"
    StartAngle="{Binding StartAngle}"
    SweepAngle="{Binding SweepAngle}"
    RegionThickness="{Binding RegionThickness}"
    MarkerDistance="{Binding MarkerDistance}"
    NeedleThickness="{Binding NeedleThickness}"
    NeedleLength="{Binding NeedleLength}"
    NeedleTailLength="{Binding NeedleTailLength}"
    NeedleCenterRadius="{Binding NeedleCenterRadius}" />
```

Regions and markers can also be supplied from view-model collections.

```xml
<controls:Dial
    Regions="{Binding Regions}"
    Markers="{Binding Markers}" />
```

This allows the gauge configuration to be generated and modified dynamically at runtime.

## Dynamic Regions and Markers

Regions and markers are designed to work with collections, allowing applications to add, remove, and modify them dynamically.

For example, a view model can maintain a collection of regions and add a new region at runtime.

The `Dial` updates its rendering when its configuration changes.

## Demo Application

The repository contains a demo application that provides an interactive editor for the gauge.

The demo allows the user to modify:

### Dial

* Minimum
* Maximum
* Value
* Start angle
* Sweep angle
* Region thickness
* Marker distance

### Needle

* Thickness
* Length
* Tail length
* Center radius

### Regions

* Add regions
* Remove regions
* Start value
* End value
* Color
* Region thickness

### Markers

* Add markers
* Remove markers
* Value
* Text
* Placement
* Gap
* Offset
* Font size
* Text color
* Marker line color
* Marker line settings

The demo also displays the corresponding AXAML configuration so that a configuration created interactively can be copied into another Avalonia application.

## Project Structure

```text
AvaloniaGauge/
├── src/
│   └── AvaloniaGauge/
│       ├── Controls/
│       │   ├── Dial.axaml
│       │   ├── Dial.axaml.cs
│       │   ├── DialPresenter.cs
│       │   ├── DialRegion.cs
│       │   └── DialMarker.cs
│       │
│       └── Themes/
│           └── Generic.axaml
│
├── demo/
│   └── AvaloniaGauge.Demo/
│       ├── ViewModels/
│       ├── Views/
│       └── App.axaml
│
└── README.md
```

The library project contains the reusable control.

The demo project is separate from the library and exists to demonstrate the control and provide an interactive configuration environment.

## Architecture

AvaloniaGauge uses Avalonia's `TemplatedControl` architecture.

`Dial` is the public control that applications interact with.

The control template is defined in the library's default theme, while rendering is handled by the presenter used by that template.

Applications using the control do not need to interact directly with the presenter.

## Building from Source

Clone the repository and build the solution:

```powershell
git clone https://github.com/alfredhunt/AvaloniaGauge.git
cd AvaloniaGauge
dotnet build
```

Run the demo application using the appropriate project for the repository layout.

## NuGet Package

AvaloniaGauge is distributed as a NuGet package.

```xml
<PackageReference Include="AvaloniaGauge" Version="1.0.0" />
```

The package is intended to contain only the reusable control library and its required Avalonia resources. The demo application is not part of the package.

## Contributing

Issues and pull requests are welcome.

When submitting changes, please include a corresponding update to the demo application when the change affects the public control API or visual behavior.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
