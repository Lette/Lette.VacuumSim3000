namespace Lette.DustSucker2000.Library

type State = { Heading : Heading; Location : Location; RoomSize : RoomSize}

[<AutoOpen>]
module State =
    let initialState =
        {
            Heading = North
            Location = { X = 0; Y = 0 }
            RoomSize = { Width = 0; Height = 0 }
        }

    let setRoomSize roomSize state = { state with RoomSize = roomSize }

    let setHeading heading state = { state with Heading = heading }

    let setLocation location state = { state with Location = location }

    let advance (state : State) =
        match state.Heading with
        | North -> if state.Location.Y = state.RoomSize.Height - 1 then state else { state with Location = { state.Location with Y = state.Location.Y + 1 } }
        | East  -> if state.Location.X = state.RoomSize.Width - 1  then state else { state with Location = { state.Location with X = state.Location.X + 1 } }
        | South -> if state.Location.Y = 0  then state else { state with Location = { state.Location with Y = state.Location.Y - 1 } }
        | West  -> if state.Location.X = 0  then state else { state with Location = { state.Location with X = state.Location.X - 1 } }

    let turnLeft state =
        match state.Heading with
        | North -> { state with Heading = West }
        | East  -> { state with Heading = North }
        | South -> { state with Heading = East }
        | West  -> { state with Heading = South }

    let turnRight state =
        match state.Heading with
        | North -> { state with Heading = East }
        | East  -> { state with Heading = South }
        | South -> { state with Heading = West }
        | West  -> { state with Heading = North }


module Interpreter =

    let run instructions =

        let rec trRun instructions state =
            match instructions with
            | [] -> state
            | (i :: is) ->
                match i with
                | SetRoomSize rs       -> trRun is (state |> setRoomSize rs)
                | SetInitialHeading h  -> trRun is (state |> setHeading h)
                | SetInitialLocation l -> trRun is (state |> setLocation l)
                | Advance              -> trRun is (state |> advance)
                | TurnLeft             -> trRun is (state |> turnLeft)
                | TurnRight            -> trRun is (state |> turnRight)

        trRun instructions State.initialState
