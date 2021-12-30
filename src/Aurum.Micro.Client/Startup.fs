namespace Aurum.Micro.Client

open Aurum.Micro.Client.Services
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Microsoft.Extensions.DependencyInjection

module Program =

    [<EntryPoint>]
    let Main args =
        let builder =
            WebAssemblyHostBuilder.CreateDefault(args)

        builder.Services.AddSingleton<ITauriInteropService, TauriInteropService>()
        |> ignore

        builder.RootComponents.Add<Main.MyApp>("#main")

        builder.Build().RunAsync() |> ignore
        0
