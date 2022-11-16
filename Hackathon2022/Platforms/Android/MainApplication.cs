using Android.App;
using Android.Runtime;

namespace Hackathon2022;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr Handle, JniHandleOwnership Ownership)
        : base(Handle, Ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
