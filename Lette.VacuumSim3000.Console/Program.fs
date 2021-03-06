﻿open Lette.VacuumSim3000.Library
open Lette.VacuumSim3000.Console.IOHelpers

let printHeader () =
    [
        "VacuumSim3000 by Christoffer Lette"
        ""
        "This software calculates the end position of the vacuum cleaner,"
        "given an initial configuration and a set of commands."
        ""
        "The input for the simulator is read from <stdin>. Any errors in the"
        "input data or from the running simulation will be directed to <stderr>."
        "The final result will be the last row written to <stdout>."
        ""
    ]
        |> List.iter stdout.WriteLine

let printFinalResult states =
    let toString = function
        | North -> "N"
        | East -> "E"
        | South -> "S"
        | West -> "W"

    let errorMessages = function
        | Ok _           -> None
        | Error (_, msg) -> Some msg

    let printError msg =
        printfn "ERROR: %s" msg

    states
        |> List.choose errorMessages
        |> List.iter printError

    let lastState =
        match states |> List.last with
        | Ok s         -> s
        | Error (s, _) -> s

    printfn ""
    printfn "Result: %s %i %i" (lastState.Heading |> toString) lastState.Location.X lastState.Location.Y

let getInput () =

    readInputWithHeaders
        [
            "Enter room size: (Format: <Width> <Height>, Default: 10 10)"
            "Enter initial heading and location: (Format: <N|E|S|W> <X> <Y>, Default: N 0 0)"
            "Enter commands: (Format: <A|L|R>..., Default: {none})"
        ]


[<EntryPoint>]
let main _ =

    printHeader ()

    getInput ()
        |> Lexer.tokenize
        |> Parser.parse
        |> Result.map (Interpreter.run >> printFinalResult)
        |> ignore

    0