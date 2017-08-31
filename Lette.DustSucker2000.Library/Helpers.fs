namespace Lette.DustSucker2000.Library

open System

[<AutoOpen>]
module Helpers =

    let splitString (delimeters : string list) (s : string) =
        s.Split (delimeters |> List.toArray, StringSplitOptions.RemoveEmptyEntries)