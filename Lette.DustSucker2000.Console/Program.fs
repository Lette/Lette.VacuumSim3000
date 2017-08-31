open Lette.DustSucker2000.Library

[<EntryPoint>]
let main argv =

    let input = "6 7
N 3 2
RARAARARA"

    printfn "Input:
--------------------------------------
%s
--------------------------------------" input

    input
        |> Lexer.tokenize
        |> printfn "Found tokens:
%A"

    0