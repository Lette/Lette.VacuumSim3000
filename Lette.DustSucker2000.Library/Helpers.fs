namespace Lette.DustSucker2000.Library

type Result<'a> =
    | Success of 'a
    | Failure of string

module Result =
    let map f result =
      match result with
      | Success s -> Success (f s)
      | Failure msg-> Failure msg

