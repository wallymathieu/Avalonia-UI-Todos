# Android version of GUI

## Stumbles

- Google Play Store can be daunting at first. You need to fill in a lot of documentation and render screenshots in exact sizes.
- The default version of this app uses a Grid with a textbox at the bottom. The virtual keyboard hides the bottom content, making it hard to enter anything. Testing your design is a must.
- There is no shutdown event as seen in desktop apps, so you need to persist changes while the app is running.
- Google Play Store requires you to package as 'aab' instead of 'apk'. The 'aab' format is not used when debugging locally... Note the addition of release specific format in the csproj.

## Unrelated notes

The below code is more for reference. You should not execute it, but rather be aware that
you might need to run similar commands.

```sh
export AcceptAndroidSDKLicenses=true
dotnet build -t:InstallAndroidDependencies -f net8.0-android "-p:AndroidSdkDirectory=$HOME/Library/Developer/Xamarin/android-sdk-macosx"
$HOME/Library/Developer/Xamarin/android-sdk-macosx/cmdline-tools/11.0/bin/sdkmanager --install "platform-tools"
```
