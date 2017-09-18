open Lette.DustSucker2000.Library
open Lette.DustSucker2000.Console.IOHelpers

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
        ""
    ]
        |> List.iter stdout.WriteLine

let printFinalResult (state : State) =
    let heading =
        match state.Heading with
        | North -> "N"
        | East -> "E"
        | South -> "S"
        | West -> "W"

    printfn ""
    printfn "Result: %s %i %i" heading state.Location.X state.Location.Y

let getInput () =

    readInputWithHeaders
        [
            "Enter room size (<Width> <Height>):"
            "Enter initial heading and location (<N|E|S|W> <X> <Y>):"
            "Enter commands (<A|L|R>...):"
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