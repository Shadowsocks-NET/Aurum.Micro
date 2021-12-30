namespace Aurum.Micro.Client.Services

open System.Threading.Tasks
open Microsoft.JSInterop

type ITauriInteropService =
    interface
        abstract GetCoreVersion : unit -> string Task
    end

type TauriInteropService(JSRuntime: IJSRuntime) =
    let tauriModule =
        task {
            try
                let! tauri =
                    JSRuntime
                        .InvokeAsync<IJSObjectReference>("import", "tauri")
                        .AsTask()

                return Some tauri
            with
            | exn ->
                printf $"{exn}"
                return None
        }

    interface ITauriInteropService with

        member this.GetCoreVersion() =
            task {
                let! tauri = tauriModule

                match tauri with
                | Some tauri -> return! tauri.InvokeAsync("invoke", "getCoreVersion")
                | None -> return "4.44.0"
            }
