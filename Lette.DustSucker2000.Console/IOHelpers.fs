namespace Lette.VacuumSim3000.Console

[<AutoOpen>]
module IOHelpers =

    let readInputWithHeaders headers =

        let rec trRead (headers : string list) values =
            match headers with
            | [] -> values
            | (h :: hs) ->
                stdout.WriteLine h
                match stdin.ReadLine() with
                | null -> values
                | value ->
                    // The following statement makes this code platform dependant.
                    // Generally, there is no way to distinguish between redirected and
                    // not redirected input. This is only used to show data coming from
                    // redirected input on screen.
                    if System.Console.IsInputRedirected then stdout.WriteLine value
                    trRead hs (value :: values)

        trRead headers []
            |> List.rev
