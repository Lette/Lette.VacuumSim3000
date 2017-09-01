namespace Lette.DustSucker2000.Library

type Token =
    | Number of int
    | Letter of char
    | Space
    | CarriageReturn
    | LineFeed
    | Unknown of char * Position

module Lexer =

    let private knownLetters = "AELNRSW" :> char seq

    let private (|IsDigit|_|) (c : char) =
        if '0' <= c && c <= '9' then
            int c - int '0' |> Some
        else
            None

    let private (|IsKnownLetter|_|) (c : char) =
        if knownLetters |> Seq.contains c then
            Some c
        else
            None

    let tokenize (input : char seq) =

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
                | c when c = '\r' -> parse cs (position |> nextColumn) (CarriageReturn        :: tokens)
                | c when c = '\n' -> parse cs (position |> nextRow)    (LineFeed              :: tokens)
                | c               -> parse cs (position |> nextColumn) (Unknown (c, position) :: tokens)

            match chars with
            | []        -> tokens
            | (c :: cs) -> addToken c cs

        parse (input |> Seq.toList) startPosition []
            |> List.rev
