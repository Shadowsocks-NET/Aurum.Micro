module Aurum.Micro.Server.Index

open Bolero
open Bolero.Html
open Bolero.Server.Html
open Aurum.Micro

let page =
    doctypeHtml [] [
        head [] [
            meta [ attr.charset "UTF-8" ]
            meta [ attr.name "viewport"
                   attr.content "width=device-width, initial-scale=1.0" ]
            title [] [ text "Bolero Application" ]
            ``base`` [ attr.href "/" ]
            script [ attr.``type`` "module"
                     attr.src "wwwroot/script/web-components.min.js" ] []
        ]
        body [] [
            div [ attr.id "main" ] [
                rootComp<Client.Main.MyApp>
            ]
            boleroScript
        ]
    ]
