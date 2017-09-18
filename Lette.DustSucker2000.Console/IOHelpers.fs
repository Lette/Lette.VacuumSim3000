namespace Lette.DustSucker2000.Console

[<AutoOpen>]
module IOHelpers =

    open System

    let private readLine _ =
        stdin.ReadLine()

    let private toOption parseResult =
        match parseResult with
        | (true, s) -> Some s
        | _         -> None

    let ints = Int32.TryParse >> toOption
    let strings (s : string) = s

    let read parser =
        Seq.initInfinite (readLine >> parser)
