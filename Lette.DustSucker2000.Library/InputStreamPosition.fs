namespace Lette.DustSucker2000.Library

type InputStreamPosition = { Row : int; Column : int }

[<AutoOpen>]
module InputStreamPosition =
    let startPosition = { Row = 1; Column = 1 }
    let nextColumn position = { position with Column = position.Column + 1 }
    let nextRow position = { position with Row = position.Row + 1; Column = 1 }
