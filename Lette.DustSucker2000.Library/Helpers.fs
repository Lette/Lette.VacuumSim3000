namespace Lette.DustSucker2000.Library

type Result<'a> =
    | Success of 'a
    | Failure of string
