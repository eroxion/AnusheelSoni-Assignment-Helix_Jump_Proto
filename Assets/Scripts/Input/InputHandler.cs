using UnityEngine;

/// <summary>
/// Universal input handler for device detection and control hints.
/// Main input processing is handled by HelixRotator.
/// </summary>
public class InputHandler : MonoBehaviour
{
    internal enum DeviceType
    {
        Desktop,
        Mobile,
        Tablet
    }
    
    private DeviceType _detectedDevice;
    
    private void Start()
    {
        DetectDevice();
    }
    
    /// <summary>
    /// Detects device type for optimal input handling.
    /// </summary>
    private void DetectDevice()
    {
        if (Application.isMobilePlatform)
        {
            _detectedDevice = DeviceType.Mobile;
            Debug.Log("Device detected: Mobile");
        }
        else if (Input.touchSupported)
        {
            _detectedDevice = DeviceType.Tablet;
            Debug.Log("Device detected: Tablet/Touchscreen");
        }
        else
        {
            _detectedDevice = DeviceType.Desktop;
            Debug.Log("Device detected: Desktop");
        }
    }
    
    /// <summary>
    /// Returns current device type.
    /// </summary>
    internal DeviceType GetDeviceType()
    {
        return _detectedDevice;
    }
    
    /// <summary>
    /// Returns device type as string.
    /// </summary>
    internal string GetDeviceTypeString()
    {
        return _detectedDevice.ToString();
    }
    
    /// <summary>
    /// Checks if touch input is available.
    /// </summary>
    internal bool IsTouchSupported()
    {
        return Input.touchSupported || Application.isMobilePlatform;
    }
    
    /// <summary>
    /// Returns appropriate control hint based on device type.
    /// Used for displaying controls to player during countdown.
    /// </summary>
    internal string GetControlHint()
    {
        switch (_detectedDevice)
        {
            case DeviceType.Mobile:
                return "Swipe left or right to rotate";
                
            case DeviceType.Tablet:
                return "Drag left or right to rotate";
                
            case DeviceType.Desktop:
            default:
                if (Input.mousePresent)
                {
                    return "Use A/D keys or drag with mouse to rotate";
                }
                return "Use A/D or Arrow keys to rotate";
        }
    }
}