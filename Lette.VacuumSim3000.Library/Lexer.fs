namespace Lette.VacuumSim3000.Library

type Token =
    | Number of int
    | Letter of char
    | Space
    | EndOfLine
    | Unknown of char * InputStreamPosition

module Lexer =

    let private knownLetters = "AELNRSW" :> char seq

    let private (|IsDigit|_|) c =
        if '0' <= c && c <= '9' then
            int c - int '0' |> Some
        else
            None

    let private (|IsKnownLetter|_|) c =
        Seq.tryFind ((=) c) knownLetters

    let private printTokens tokens =
        printfn "Found tokens:"
        printfn "%A" tokens
        tokens

    let tokenize (input : string list) =

        let rec parse chars position tokens =

            let addDigit n cs =
                match tokens with
                | (Number m :: ts) -> parse cs (position |> nextColumn) (Number (m * 10 + n) :: ts)
                | ts               -> parse cs (position |> nextColumn) (Number n            :: ts)

            let addToken c cs =
                match c with
                | IsDigit n       -> addDigit n cs
                | IsKnownLetter l -> parse cs (position |> nextColumn) (Letter l              :: tokens)
                | c when c = ' '  -> parse cs (position |> nextColumn) (Space                 :: tokens)
                | c               -> parse cs (position |> nextColumn) (Unknown (c, position) :: tokens)

            match chars with
            | []        -> (EndOfLine :: tokens)
            | (c :: cs) -> addToken c cs

        let rec parseRows rows position tokens =
            match rows with
            | []        -> tokens
            | (r :: rs) -> parseRows rs (position |> nextRow) (parse (r |> Seq.toList) position tokens)

        parseRows input startPosition []
            |> List.rev
#if DEBUG
            |> printTokens
#endif
