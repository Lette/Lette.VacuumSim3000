﻿namespace Lette.VacuumSim3000.Library

type RoomSize = { Width : int; Height : int }

type Heading =
    | North
    | East
    | South
    | West

type Location = { X : int; Y : int }

type Instruction =
    | SetRoomSize of RoomSize
    | SetInitialHeading of Heading
    | SetInitialLocation of Location
    | Advance
    | TurnLeft
    | TurnRight

module Parser =

    let private defaultRoomSize = { Width = 10; Height = 10 }
    let private defaultHeading = North
    let private defaultLocation = { X = 0; Y = 0 }

    let private parseSetRoomSize tokens =
        match tokens with
        | (Number 0 :: Space :: Number 0 :: _) -> "Room width and height must be greater than zero." |> Error
        | (Number 0 :: Space :: Number _ :: _) -> "Room width must be greater than zero." |> Error
        | (Number _ :: Space :: Number 0 :: _) -> "Room height must be greater than zero." |> Error
        | (Number width :: Space :: Number height :: ts) -> ([ SetRoomSize { Width = width; Height = height } ], ts) |> Ok
        | (EndOfLine :: _) -> ([ SetRoomSize defaultRoomSize ], tokens) |> Ok
        | _ -> sprintf "Failed to parse room size. Tokens given: %A" tokens |> Error

    let private parseEndOfLine tokens =
        match tokens with
        | (EndOfLine :: ts) -> ([], ts) |> Ok
        | _ -> sprintf "Expected end of line. Tokens given: %A" tokens |> Error

    let private parseSetInitialHeading tokens =
        match tokens with
        | (Letter 'N' :: ts) -> ([ SetInitialHeading North ], ts) |> Ok
        | (Letter 'E' :: ts) -> ([ SetInitialHeading East] , ts) |> Ok
        | (Letter 'S' :: ts) -> ([ SetInitialHeading South ], ts) |> Ok
        | (Letter 'W' :: ts) -> ([ SetInitialHeading West ], ts) |> Ok
        | (EndOfLine :: _) -> ([ SetInitialHeading defaultHeading ], tokens) |> Ok
        | _ -> sprintf "Failed to parse initial heading. Tokens given: %A" tokens |> Error

    let private parseSpace tokens =
        match tokens with
        | (Space :: ts) -> ([], ts) |> Ok
        | (EndOfLine :: _) -> ([], tokens) |> Ok
        | _ -> sprintf "Expected a space. Tokens given: %A" tokens |> Error

    let private parseSetInitialLocation tokens =
        match tokens with
        | (Number x :: Space :: Number y :: ts) -> ([ SetInitialLocation { X = x; Y = y } ], ts) |> Ok
        | (EndOfLine :: _) -> ([ SetInitialLocation defaultLocation ], tokens) |> Ok
        | _ -> sprintf "Failed to parse initial location. Tokens given: %A" tokens |> Error

    let private parseCommands tokens =
        let rec trParseCommand tokens instructions =
            match tokens with
            | (Letter 'A' :: ts) -> trParseCommand ts (Advance :: instructions)
            | (Letter 'L' :: ts) -> trParseCommand ts (TurnLeft :: instructions)
            | (Letter 'R' :: ts) -> trParseCommand ts (TurnRight :: instructions)
            | _ -> (instructions, tokens)

        trParseCommand tokens []
            |> (fun (is, ts) -> (is |> List.rev, ts))
            |> Ok

    let private orderedParsers =
        [
            parseSetRoomSize
            parseEndOfLine
            parseSetInitialHeading
            parseSpace
            parseSetInitialLocation
            parseEndOfLine
            parseCommands
            parseEndOfLine
        ]

    let private printInstructions instructions =
        printfn "Generated instructions:"
        instructions |> List.iter (printfn "%A")

    let private printParseResult instructions =
        match instructions with
        | Ok is  -> printInstructions is
        | Error msg -> printfn "%s" msg
        instructions

    let parse tokens =

        let rec trParse tokens parsers instructions =
            match parsers with
            | [] -> Ok instructions
            | (p :: ps) ->
                match tokens with
                | [] -> Error "The input stream ended unexpectedly."
                | _ ->
                    match p tokens with
                    | Ok (is, ts) -> trParse ts ps (instructions @ is)
                    | Error msg -> Error msg

        trParse tokens orderedParsers []
#if DEBUG
            |> printParseResult
#endif
