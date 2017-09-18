namespace Lette.DustSucker2000.Console

[<AutoOpen>]
module IOHelpers =

    open System

    let private readLine _ =
        stdin.ReadLine()

    let ints = Int32.TryParse
    let strings (s : string) = (true, s)

    let read parser =

        let toOption parseResult =
            match parseResult with
            | (true, s) -> Some s
            | _         -> None

        let generator =
            readLine >> parser >> toOption

        Seq.initInfinite generator
