#r "packages/FAKE/tools/FakeLib.dll"
open Fake

Target "Paket Restore" (fun _ ->
    Paket.Restore id
)

Target "Build" (fun _ ->
    let setParams defaults =
        { defaults with
            Verbosity = Some MSBuildVerbosity.Minimal
            Targets = [ "Clean"; "Rebuild" ]
            Properties =
                [
                    "Optimize", "True"
                    "DebugSymbols", "True"
                    "Configuration", "Release"
                ]
        }

    build setParams "Lette.VacuumSim3000.sln"
        |> DoNothing
)

"Paket Restore"
   ==> "Build"

Run "Build"
