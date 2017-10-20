namespace Lette.VacuumSim3000.Library

type State = { Heading : Heading; Location : Location; RoomSize : RoomSize}

[<AutoOpen>]
module State =
    let initialState =
        {
            Heading = North
            Location = { X = 0; Y = 0 }
            RoomSize = { Width = 1; Height = 1 }
        }

    let setRoomSize roomSize state = { state with RoomSize = roomSize } |> Ok

    let setHeading heading state = { state with Heading = heading } |> Ok

    let private isOutsideOfRoom state location =
        location.X >= state.RoomSize.Width || location.Y >= state.RoomSize.Height

    let setLocation location state =
        if location |> isOutsideOfRoom state then
            let modifiedLocation =
                { location with
                    X = min location.X (state.RoomSize.Width - 1)
                    Y = min location.Y (state.RoomSize.Height - 1)
                }
            ({ state with Location = modifiedLocation }, "Location was set to outside of room. Location has beed updated.") |> Error
        else
            { state with Location = location } |> Ok

    let private moveNorth state =
        if state.Location.Y = state.RoomSize.Height - 1 then
            (state, "Bumped into the northern wall.") |> Error
        else
            { state with Location = { state.Location with Y = state.Location.Y + 1 } } |> Ok

    let private moveEast state =
        if state.Location.X = state.RoomSize.Width - 1 then
            (state, "Bumped into the eastern wall.") |> Error
        else
            { state with Location = { state.Location with X = state.Location.X + 1 } } |> Ok

    let private moveSouth state =
        if state.Location.Y = 0 then
            (state, "Bumped into the southern wall.") |> Error
        else
            { state with Location = { state.Location with Y = state.Location.Y - 1 } } |> Ok

    let private moveWest state =
        if state.Location.X = 0 then
            (state, "Bumped into the western wall.") |> Error
        else
            { state with Location = { state.Location with X = state.Location.X - 1 } } |> Ok

    let advance (state : State) =
        match state.Heading with
        | North -> moveNorth state
        | East  -> moveEast state
        | South -> moveSouth state
        | West  -> moveWest state

    let turnLeft state =
        match state.Heading with
        | North -> { state with Heading = West } |> Ok
        | East  -> { state with Heading = North } |> Ok
        | South -> { state with Heading = East } |> Ok
        | West  -> { state with Heading = South } |> Ok

    let turnRight state =
        match state.Heading with
        | North -> { state with Heading = East } |> Ok
        | East  -> { state with Heading = South } |> Ok
        | South -> { state with Heading = West } |> Ok
        | West  -> { state with Heading = North } |> Ok


module Interpreter =

    let run instructions =

        let rec trRun instructions states =

            let addState command =
                match states with
                | []       -> (State.initialState |> command) :: []
                | (Ok s :: _) -> (s |> command) :: states
                | (Error (s, _) :: _) -> (s |> command) :: states

            match instructions with
            | [] -> states
            | (i :: is) ->
                match i with
                | SetRoomSize rs       -> setRoomSize rs |> addState |> trRun is
                | SetInitialHeading h  -> setHeading h   |> addState |> trRun is
                | SetInitialLocation l -> setLocation l  |> addState |> trRun is
                | Advance              -> advance        |> addState |> trRun is
                | TurnLeft             -> turnLeft       |> addState |> trRun is
                | TurnRight            -> turnRight      |> addState |> trRun is

        trRun instructions []
            |> List.rev
