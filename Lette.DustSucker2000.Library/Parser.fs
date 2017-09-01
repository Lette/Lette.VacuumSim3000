namespace Lette.DustSucker2000.Library

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

    let private parseSetRoomSize tokens =
        match tokens with
        | (Number width :: Space :: Number height :: ts) -> ([ SetRoomSize { Width = width; Height = height } ], ts) |> Success
        | _ -> sprintf "Failed to parse room size. Tokens given: %A" tokens |> Failure

    let private parseNewLine tokens =
        match tokens with
        | (CarriageReturn :: LineFeed :: ts) -> ([], ts) |> Success
        | _ -> sprintf "Expected a new line. Tokens given: %A" tokens |> Failure

    let private parseSetInitialHeading tokens =
        match tokens with
        | (Letter 'N' :: ts) -> ([ SetInitialHeading North ], ts) |> Success
        | (Letter 'E' :: ts) -> ([ SetInitialHeading East] , ts) |> Success
        | (Letter 'S' :: ts) -> ([ SetInitialHeading South ], ts) |> Success
        | (Letter 'W' :: ts) -> ([ SetInitialHeading West ], ts) |> Success
        | _ -> sprintf "Failed to parse initial heading. Tokens given: %A" tokens |> Failure

    let private parseSpace tokens =
        match tokens with
        | (Space :: ts) -> ([], ts) |> Success
        | _ -> sprintf "Expected a space. Tokens given: %A" tokens |> Failure

    let private parseSetInitialLocation tokens =
        match tokens with
        | (Number x :: Space :: Number y :: ts) -> ([ SetInitialLocation { X = x; Y = y } ], ts) |> Success
        | _ -> sprintf "Failed to parse initial location. Tokens given: %A" tokens |> Failure

    let private parseCommands tokens =
        let rec trParseCommand tokens commands =
            match tokens with
            | (Letter 'A' :: ts) -> trParseCommand ts (Advance :: commands)
            | (Letter 'L' :: ts) -> trParseCommand ts (TurnLeft :: commands)
            | (Letter 'R' :: ts) -> trParseCommand ts (TurnRight :: commands)
            | _ -> (commands, tokens)

        trParseCommand tokens []
            |> (fun (cs, ts) -> (cs |> List.rev, ts))
            |> Success

    let private orderedParsers =
        [
            parseSetRoomSize
            parseNewLine
            parseSetInitialHeading
            parseSpace
            parseSetInitialLocation
            parseNewLine
            parseCommands
            parseNewLine
        ]

    let parse tokens =

        let rec trParse tokens parsers instructions =
            match parsers with
            | [] -> Success instructions
            | (p :: ps) ->
                match p tokens with
                | Success (is, ts) -> trParse ts ps (instructions @ is)
                | Failure msg -> Failure msg

        trParse tokens orderedParsers []
