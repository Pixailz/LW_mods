# DisplayConfigurationMenuBase

## Preview

<p>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/display_configurations_menu/main.png" alt="Display Configurations Main menu" width="300"/>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/display_configurations_menu/edit_configurations.png" alt="Display Configurations Edit Configurations" width="300"/>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/display_configurations_menu/edit_configuration.png" alt="Display Configurations Edit Configuration" width="300"/>
</p>

## Description

Abstract base class for display configuration menus in Logic World. Provides a standardized interface for components with configurable bits per pixel (BPP) and color configurations.

- **Inherits:** `EditComponentMenu`, implements `IAssignMyFields`
- **Location:** [src/client/MenuUtils/DisplayConfigurationMenuBase.cs](https://github.com/Pixailz/LW_mods/blob/main/logic_utils/src/client/MenuUtils/DisplayConfigurationMenuBase.cs)

## Key Features

- Configurable BPP ranges with slider
- Multiple color/display configuration management
- Advanced configuration editor
- Multi-component editing support
- Automatic UI initialization

## Required Component Data Interface

**Component custom data must implement `IDisplayConfigurationData`** to work with this menu. This interface is required because:

1. **Data Access Contract**: The menu needs to read/write BPP and configuration index from component data. `IDisplayConfigurationData` provides the standardized properties (`BitsPerPixel`, `ConfigurationIndex`) that the menu expects.

2. **Separation of Concerns**: Display configuration logic is isolated in the interface, allowing component data to implement additional interfaces without conflicts.

## Abstract & Virtual Members

**Must Override:**
- `ComponentTypeIDs`: Component type IDs editable by this menu

**Can Override:**
- `MinBPP`: Minimum bits per pixel (default: 1)
- `MaxBPP`: Maximum bits per pixel (default: 9)
- `BitsPerPixelName`: Slider label (default: "Bits Per Pixel")

## UI Components

- `displayConfigContainer`: Main container (assigned via `[AssignMe]`)
- `configurationsList`: Manages configuration list
- `bitsPerPixelSlider`: BPP adjustment slider (hidden if MinBPP == MaxBPP)
- `editConfigurationsButton`: Opens configuration editor

## Key Methods

### `Initialize()`
Sets up UI components (1000x1500 min size), initializes slider and button, registers event handlers.

### `SetupDisplayConfigMenu()`
Refreshes menu with current component's BPP and selected configuration.

### `OnBitsPerPixelChanged(float bpp)`
Updates all edited components' BPP and refreshes menu.

### `OnConfigurationSelected(int index)`
Updates configuration index for all edited components.

### `GetCurrentBitsPerPixel()` / `GetCurrentConfigurationIndex()`
Retrieves current values from first edited component's `IDisplayConfigurationData`.

## Static Utilities

- `getVanillaEditDisplayMenuContent()`: Cached vanilla menu content
- `FindChildByName(GameObject, string)`: Recursive child search
- `ReplaceLocalizedText(GameObject, string, string)`: Replace localized with plain text

## Usage Example

```csharp
public class MyDisplayMenu : DisplayConfigurationMenuBase
{
    protected override int MinBPP => 1;
    protected override int MaxBPP => 4;
    protected override IEnumerable<string> ComponentTypeIDs
        => new[] { "MyCustomDisplay" };
}
```

```csharp
// Component's custom data must implement IDisplayConfigurationData
public class MyDisplayData : IDisplayConfigurationData
{
    public int BitsPerPixel { get; set; }
    public int ConfigurationIndex { get; set; }
}
```
