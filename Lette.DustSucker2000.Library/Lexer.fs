namespace Lette.DustSucker2000.Library

type Position = { Row : int; Column : int }

[<AutoOpen>]
module Position =
    let startPosition = { Row = 1; Column = 1 }
    let nextColumn position = { position with Column = position.Column + 1 }
    let nextRow position = { position with Row = position.Row + 1; Column = 1 }


type Token =
    | Number of int
    | Letter of char
    | Space
    | CarriageReturn
    | LineFeed
    | Unknown of char * Position

module Lexer =

    let private knownLetters = "AELNRSW" :> char seq

    let private (|IsNumber|_|) (c : char) =
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

        let rec parseChars chars currentPosition tokens =
            match chars with
            | [] -> tokens
            | (c :: cs) ->
                match c with
                | IsNumber n      ->
                    match tokens with
                    | (Number m :: ts) -> parseChars cs (currentPosition |> nextColumn) (Number (m * 10 + n) :: ts)
                    | _                -> parseChars cs (currentPosition |> nextColumn) (Number n            :: tokens)
                | IsKnownLetter l -> parseChars cs (currentPosition |> nextColumn) (Letter l                     :: tokens)
                | c when c = ' '  -> parseChars cs (currentPosition |> nextColumn) (Space                        :: tokens)
                | c when c = '\r' -> parseChars cs (currentPosition |> nextColumn) (CarriageReturn               :: tokens)
                | c when c = '\n' -> parseChars cs (currentPosition |> nextRow)    (LineFeed                     :: tokens)
                | c               -> parseChars cs (currentPosition |> nextColumn) (Unknown (c, currentPosition) :: tokens)

        parseChars (input |> Seq.toList) startPosition []
            |> List.rev
