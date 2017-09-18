namespace Lette.DustSucker2000.Console

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
                | value -> trRead hs (value :: values)

        trRead headers []
            |> List.rev
