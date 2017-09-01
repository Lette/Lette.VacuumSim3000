open Lette.DustSucker2000.Library

[<EntryPoint>]
let main argv =

    let input = "6 7
N 3 2
RARAARARA
"

    printfn "Input:
--------------------------------------
%s
--------------------------------------" input

    let tokens = input |> Lexer.tokenize

    tokens |> printfn "Found tokens:
%A"

    let instructions = tokens |> Parser.parse

    instructions |> printfn "Generated instructions:
%A"

    0