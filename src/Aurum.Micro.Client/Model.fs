namespace Aurum.Micro.Client.Model

open Bolero

module Main =
    type Page =
        | [<EndPoint "/">] Home
        | [<EndPoint "/group">] Group
        | [<EndPoint "/settings">] Settings
        | [<EndPoint "/help">] Help

    type Model =
        { page: Page
          coreVersion: string option }
