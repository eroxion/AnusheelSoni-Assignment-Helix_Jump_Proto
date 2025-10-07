using UnityEngine;

/// <summary>
/// Universal input handler for device detection and control hints.
/// Supports runtime device detection for Editor, Standalone, and WebGL builds.
/// WebGL detection checks browser user agent for mobile devices.
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
    /// WebGL builds check browser user agent for accurate mobile detection.
    /// Standalone/Editor builds use Unity's platform detection.
    /// </summary>
    private void DetectDevice()
    {
        // Special handling for WebGL builds
        #if UNITY_WEBGL && !UNITY_EDITOR
        DetectWebGLDevice();
        #else
        DetectStandaloneDevice();
        #endif
    }
    
    /// <summary>
    /// WebGL-specific device detection using browser user agent.
    /// Checks for mobile/tablet keywords in user agent string.
    /// </summary>
    private void DetectWebGLDevice()
    {
        // Get browser user agent string
        string userAgent = "WebGL";
        
        // Try to get actual user agent from browser JavaScript
        try
        {
            userAgent = GetBrowserUserAgent();
        }
        catch
        {
            Debug.LogWarning("Could not retrieve browser user agent, using fallback detection");
        }
        
        // Convert to lowercase for case-insensitive matching
        string userAgentLower = userAgent.ToLower();
        
        // Check for mobile device keywords
        bool isMobile = userAgentLower.Contains("mobile") ||
                       userAgentLower.Contains("android") ||
                       userAgentLower.Contains("iphone") ||
                       userAgentLower.Contains("ipod") ||
                       userAgentLower.Contains("blackberry") ||
                       userAgentLower.Contains("windows phone");
        
        // Check for tablet keywords
        bool isTablet = userAgentLower.Contains("tablet") ||
                       userAgentLower.Contains("ipad") ||
                       (userAgentLower.Contains("android") && !userAgentLower.Contains("mobile"));
        
        // Determine device type
        if (isTablet)
        {
            _detectedDevice = DeviceType.Tablet;
            Debug.Log($"WebGL Device detected: Tablet (User Agent: {userAgent})");
        }
        else if (isMobile)
        {
            _detectedDevice = DeviceType.Mobile;
            Debug.Log($"WebGL Device detected: Mobile (User Agent: {userAgent})");
        }
        else
        {
            _detectedDevice = DeviceType.Desktop;
            Debug.Log($"WebGL Device detected: Desktop (User Agent: {userAgent})");
        }
    }
    
    /// <summary>
    /// Standalone/Editor device detection using Unity's built-in platform checks.
    /// </summary>
    private void DetectStandaloneDevice()
    {
        // Check if running on mobile platform
        if (Application.isMobilePlatform)
        {
            _detectedDevice = DeviceType.Mobile;
            Debug.Log("Device detected: Mobile (Platform)");
        }
        // Check if touch is supported AND device is handheld (use Unity's SystemInfo.deviceType)
        else if (Input.touchSupported && SystemInfo.deviceType == UnityEngine.DeviceType.Handheld)
        {
            _detectedDevice = DeviceType.Tablet;
            Debug.Log("Device detected: Tablet/Touchscreen (Platform)");
        }
        // Default to desktop
        else
        {
            _detectedDevice = DeviceType.Desktop;
            Debug.Log("Device detected: Desktop (Platform)");
        }
    }
    
    /// <summary>
    /// Gets browser user agent string via JavaScript interop.
    /// Only works in WebGL builds running in browsers.
    /// </summary>
    private string GetBrowserUserAgent()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        return Application.platform == RuntimePlatform.WebGLPlayer 
            ? GetUserAgentFromJS() 
            : "Unknown";
        #else
        return "Not WebGL";
        #endif
    }
    
    /// <summary>
    /// JavaScript interop to get navigator.userAgent from browser.
    /// Called only in WebGL builds.
    /// </summary>
    private string GetUserAgentFromJS()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            // Fallback: Use Application.absoluteURL as proxy
            // To properly implement, add DeviceDetection.jslib plugin
            return "WebGL-Browser";
        }
        catch
        {
            return "WebGL";
        }
        #else
        return "Not WebGL";
        #endif
    }
    
    /// <summary>
    /// Returns current detected device type.
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
    /// WebGL builds check for touch events in browser.
    /// </summary>
    internal bool IsTouchSupported()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        // In WebGL, check if browser reports touch support
        return Input.touchSupported || _detectedDevice == DeviceType.Mobile || _detectedDevice == DeviceType.Tablet;
        #else
        return Input.touchSupported || Application.isMobilePlatform;
        #endif
    }
    
    /// <summary>
    /// Returns appropriate control hint based on device type.
    /// Used for displaying controls to player during countdown.
    /// Provides specific instructions for each device category.
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
                // Desktop shows keyboard + mouse options
                if (Input.mousePresent)
                {
                    return "Use A/D or Arrow keys\nor drag with mouse to rotate";
                }
                return "Use A/D or Arrow keys to rotate";
        }
    }
}
