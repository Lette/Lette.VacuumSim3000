#### Project TODO List

([Rules](#rules) below.)

##### To do

- [ ] Returned states should be either `Ok<State>` or `Error<(state, message)>`
  * I.e. `run: ... -> Result<State, (State, message)> list`
- [ ] Interpreter should return errors for:
  - [ ] Bad initial location (outside of room)
    - [ ] Set initial location to closest location inside the room
  - [ ] Crash into wall
- [ ] Print error messages...
  - [ ] after lexer to stderr (Unknown token)
  - [ ] after parser to stderr (Error msg)
  - [ ] after interpretation to stderr (Error (s, msg))
- [ ] Paket
- [ ] FAKE
- [ ] ILMerge
  * `.Console.exe + .Library.dll + System.ValueTuple.dll => vacsim3k.exe`
- [ ] Investigate: Interpreter could use issue commands...
  * `type Command = State -> State` or something like that.
    * (...or maybe that's exactly what it is doing right now...)
- [ ] Replay all states graphically!

###### Done

- [x] Program should print final return value on a separate, last, line to stdout
- [x] Move debug outputs to respective module, and print only if in DEBUG
- [x] Disallow bad inputs
  - [x] Negative numbers (done by not including '-' in the alphabet)
  - [x] Non-positive room sizes
- [x] Show description before each input
- [x] Add defaults to input
  - [x] Room size defaults to "10 10"
  - [x] Initial location defaults to "N 0 0"
  - [x] Commands defaults to "" (they already do... just dont enter any)
- [x] Handle errors stemming from partial input from redirected stdin
- [x] Show input if it comes from redirected stdin
  * Is it even possible???
    * Well, sort of. But not in a platform-independent way, no.
- [x] Interpreter should return list of states
- [x] Rename to `Lette.VacuumSim3000.*`
  - [x] Project files
  - [x] Folders
  - [x] Outputs
  - [x] Namespaces
  - [x] Header text
  - [x] Search for old names and replace with new
- [x] Push to GitHub

##### Rules

1. Add new tasks to the [To do](#to-do) list and prioritize.
1. The tasks with the highest priorities are at the top of the list.
1. No item can be marked as done, if it has sub-items that are not done.
1. When an item is done, move it to the bottom of the [Done](#done) list.
