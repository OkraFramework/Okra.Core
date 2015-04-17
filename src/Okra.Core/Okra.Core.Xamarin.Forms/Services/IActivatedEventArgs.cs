using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Services
{
    // Summary:
    //     Specifies the type of activation.
    public enum ActivationKind
    {
        // Summary:
        //     The user launched the app or tapped a content tile.
        Launch = 0,
        //
        // Summary:
        //     The user wants to search with the app.
        Search = 1,
        //
        // Summary:
        //     The app is activated as a target for share operations.
        ShareTarget = 2,
        //
        // Summary:
        //     An app launched a file whose file type this app is registered to handle.
        File = 3
    }

    // Summary:
    //     Specifies the execution state of the app.
    public enum ApplicationExecutionState
    {
        // Summary:
        //     The app is not running.
        NotRunning = 0,
        //
        // Summary:
        //     The app is running.
        Running = 1,
        //
        // Summary:
        //     The app is suspended.
        Suspended = 2,
        //
        // Summary:
        //     The app was terminated after being suspended.
        Terminated = 3,
        //
        // Summary:
        //     The app was closed by the user.
        ClosedByUser = 4,
    }

    public interface IActivatedEventArgs
    {
        // Summary:
        //     Gets the reason that this app is being activated.
        //
        // Returns:
        //     One of the enumeration values.
        ActivationKind Kind { get; }
        //
        // Summary:
        //     Gets the execution state of the app before this activation.
        //
        // Returns:
        //     One of the enumeration values.
        ApplicationExecutionState PreviousExecutionState { get; }

        //
        // Summary:
        //     Gets the splash screen object that provides information about the transition
        //     from the splash screen to the activated app.
        //
        // Returns:
        //     The splash screen object.
        //SplashScreen SplashScreen { get; }
    }
}
