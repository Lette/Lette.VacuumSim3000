open Lette.DustSucker2000.Library

let printInput input =
    printfn "Input:"
    printfn "--------------------------------------"
    printfn "%s" input
    printfn "--------------------------------------"
    input

let printTokens tokens =
    printfn "Found tokens:"
    printfn "%A" tokens
    tokens

let printInstructions instructions =
    printfn "Generated instructions:"
    instructions |> List.iter (printfn "%A")

let printParseResult instructions =
    match instructions with
    | Ok is  -> printInstructions is
    | Error msg -> printfn "%s" msg
    instructions

let printFinalResult (state : State) =
    let heading =
        match state.Heading with
        | North -> "N"
        | East -> "E"
        | South -> "S"
        | West -> "W"

    printfn "--------------------------------------"
    printfn "Result: %s %i %i" heading state.Location.X state.Location.Y
    printfn "--------------------------------------"
    state

[<EntryPoint>]
let main argv =

    "6 7\r\nN 3 2\r\nRARAARARA\r\n"
        |> printInput
        |> Lexer.tokenize
        |> printTokens
        |> Parser.parse
        |> printParseResult
        |> Result.map (Interpreter.run >> printFinalResult)
        |> ignore

    0