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
    | Success is  -> printInstructions is
    | Failure msg -> printfn "%s" msg
    instructions

[<EntryPoint>]
let main argv =

    "6 7\r\nN 3 2\r\nRARAARARA\r\n"
        |> printInput
        |> Lexer.tokenize
        |> printTokens
        |> Parser.parse
        |> printParseResult
        |> ignore

    0