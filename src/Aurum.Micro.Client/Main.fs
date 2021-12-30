module Aurum.Micro.Client.Main

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client
open Microsoft.Fast.Components.FluentUI

type Page =
    | [<EndPoint "/">] Home
    | [<EndPoint "/group">] Group
    | [<EndPoint "/settings">] Settings
    | [<EndPoint "/help">] Help

type Model = { count: int; page: Page }

let init _ = { count = 0; page = Home }, Cmd.none

type Message =
    | Increment
    | Decrement
    | SetPage of Page

let update msg model =
    match msg with
    | Increment -> { model with count = model.count + 1 }, Cmd.none
    | Decrement -> { model with count = model.count - 1 }, Cmd.none
    | SetPage page -> { model with page = page }, Cmd.none

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
                     text "core_version_placeholder"
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

    override this.Program =
        Program.mkProgram init update view
        |> Program.withRouter router
