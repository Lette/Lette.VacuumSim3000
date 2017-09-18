namespace Lette.DustSucker2000.Console

[<AutoOpen>]
module IOHelpers =

    let readInputWithHeaders headers =

        let rec trRead (headers : string list) values =
            match headers with
            | [] -> values
            | (h :: hs) ->
                stdout.WriteLine h
                trRead hs (stdin.ReadLine() :: values)

        trRead headers []
            |> List.rev
