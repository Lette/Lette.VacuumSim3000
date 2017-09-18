open Lette.DustSucker2000.Library
open Lette.DustSucker2000.Console.IOHelpers

let printInput input =
    printfn "Input:"
    printfn "--------------------------------------"
    printfn "%s" input
    printfn "--------------------------------------"
    input

let printHeader () =
    [
        "DustSucker2000 Simulator by Christoffer Lette"
        ""
        "This software calculates the end position of the vacuum cleaner,"
        "given an initial configuration and a set of commands."
        ""
        "The input for the simulator is read from <stdin>. Any errors in the"
        "input data or from the running simulation will be directed to <stderr>."
        "The final result will be the last row written to <stdout>."
    ]
        |> List.iter stdout.WriteLine

let printFinalResult (state : State) =
    let heading =
        match state.Heading with
        | North -> "N"
        | East -> "E"
        | South -> "S"
        | West -> "W"

    printfn "Result: %s %i %i" heading state.Location.X state.Location.Y

let readThreeStrings () =
    read strings
        |> Seq.truncate 3
        |> Seq.toList


[<EntryPoint>]
let main argv =

    printHeader ()

    let input = readThreeStrings ()

    input
        |> Lexer.tokenize
        |> Parser.parse
        |> Result.map (Interpreter.run >> printFinalResult)
        |> ignore

    0