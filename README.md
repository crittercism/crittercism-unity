# Crittercism Unity Plugin

Integration instructions and an API reference are available on the Apteligent docs site.

* [Crittercism Android Unity Documentation](http://docs.crittercism.com/development_platforms/integration_unity_android.html).
* [Crittercism iOS Unity Documentation](http://docs.crittercism.com/development_platforms/integration_unity_ios.html).

# Release Notes

## Version 2.0.0 (April 8, 2016)

*New*

* Unified the old Crittercism iOS and Android plugins with a single Crittercism interface. The CrittercismIOS and CrittercismAndroid classes are still available, but we strongly recommend using the new Crittercism class as it works for both iOS and Android builds.
* The iOS plugin now logs breadcrumbs asynchronously.
* The DemoApp now uses Unity's WWW and UnityWebRequest APIs to demonstrate Crittercism's network insights capabilities.

*Fixes*

* The iOS plugin caused duplicate symbol linker errors when compiling with IL2CPP on iOS
* When compiling for Android with IL2CPP turned on, the iOS plugin would cause compile errors since it was not #ifdef'd properly.
* The iOS plugin would hang during a crash when Script Call Optimizations were set to Fast but no Exceptions and IL2CPP was turned on.
* When compiling on Unity 4.x, a compile error would result because the ``Application.logMessageReceived`` API was not available in 4.x

