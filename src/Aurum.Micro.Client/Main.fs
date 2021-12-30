module Aurum.Micro.Client.Main

open System
open Aurum.Micro.Client.Services
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting.Client
open Bolero.Templating.Client
open Microsoft.AspNetCore.Components
open Microsoft.Fast.Components.FluentUI
open Aurum.Micro.Client.Model.Main

type Message =
    | SetPage of Page
    | GetCoreVersion
    | AsyncFailure of Exception
    | AsyncCoreVersion of string
    | Empty

let init _ =
    { Model.page = Home
      coreVersion = None },
    Cmd.ofMsg GetCoreVersion

let update (tauri: ITauriInteropService) msg model =
    match msg with
    | SetPage page -> { model with page = page }, Cmd.none
    | GetCoreVersion -> model, Cmd.OfTask.either (fun () -> tauri.GetCoreVersion()) () AsyncCoreVersion AsyncFailure
    | AsyncCoreVersion version ->
        { model with
              coreVersion = Some version },
        Cmd.none
    | AsyncFailure ``exception`` ->
        model, Cmd.OfFunc.perform (fun ``exception`` -> printf $"{``exception``}") ``exception`` (fun _ -> Empty)
    | Empty -> model, Cmd.none

let view model dispatch =
    concat [ div [ attr.classes [ "content" ] ] [
                 text "Content Placeholder"
             ]
             div [ attr.classes [ "sidebar" ] ] [
                 div [ attr.classes [ "sidebar-icon" ] ] [
                     text "Aurum.Micro"
                 ]
                 div [ attr.classes [ "sidebar-menu" ] ] [
                     text "Menu Placeholder"
                 ]
                 div [ attr.classes [ "sidebar-footer" ] ] [
                     text "Core"
                     br []
                     text
                         $"""{match model.coreVersion with
                              | Some version -> version
                              | None -> "_version"}"""
                     br []
                     text "Client"
                     br []
                     text $"{Constants.version}"
                 ]
             ] ]

let router: Router<Page, Model, Message> =
    { getEndPoint = fun m -> m.page
      setRoute =
          fun path ->
              match path.Trim('/').Split('/') with
              | [||] -> Some(SetPage Home)
              | [| "group" |] -> Some(SetPage Group)
              | [| "settings" |] -> Some(SetPage Settings)
              | [| "help" |] -> Some(SetPage Help)
              | _ -> None
      getRoute =
          function
          | Home -> "/"
          | Group -> "/group"
          | Settings -> "/settings"
          | Help -> "/help" }

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    [<Inject>]
    member val TauriInterop = Unchecked.defaultof<ITauriInteropService> with get, set

    override this.Program =
        let update = update this.TauriInterop

        Program.mkProgram init update view
        |> Program.withRouter router
