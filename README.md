# Crittercism Unity Plugin

Integration instructions and an API reference are available on the Apteligent docs site.

* [Crittercism Android Unity Documentation](http://docs.crittercism.com/development_platforms/integration_unity_android.html).
* [Crittercism iOS Unity Documentation](http://docs.crittercism.com/development_platforms/integration_unity_ios.html).

# Release Notes

## Version 2.1.1 (Nov 30, 2016)

*New*

* Updated the native iOS SDK to version 5.6.4. Please see iOS SDK release notes for a full outline of the changes: https://docs.apteligent.com/release_notes/release_notes_ios.html

## Version 2.1.0 (Oct 25, 2016)

*New*

* Updated the native iOS SDK to version 5.6.3-rc.3. Please see iOS SDK release notes for a full outline of the changes: https://docs.apteligent.com/release_notes/release_notes_ios.html
* SDK can now generate C# stack traces for unhandled exceptions in ‘Fast but no Exceptions’ mode
* Updated the native Android SDK to 5.8.2. Please see Android SDK release notes for a full outline of the changes: https://docs.apteligent.com/release_notes/release_notes_android.html

## Version 2.0.0 (April 8, 2016)

*New*

* Unified the old Crittercism iOS and Android plugins with a single Crittercism interface. The CrittercismIOS and CrittercismAndroid classes are still available, but we strongly recommend using the new Crittercism class as it works for both iOS and Android builds.
* The iOS plugin now logs breadcrumbs asynchronously.
* The DemoApp now uses Unity's WWW and UnityWebRequest APIs to demonstrate Crittercism's network insights capabilities.

*Fixes*

* The iOS plugin caused duplicate symbol linker errors when compiling with IL2CPP on iOS
* When compiling for Android with IL2CPP turned on, the iOS plugin would cause compile errors since it was not #ifdef'd properly.
* The iOS plugin would hang during a crash when Script Call Optimizations was set to Fast but no Exceptions and IL2CPP was turned on.
* When compiling on Unity 4.x, a compile error would result because the ``Application.logMessageReceived`` API was not available in 4.x

