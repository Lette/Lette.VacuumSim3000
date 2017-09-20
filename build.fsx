#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.ILMergeHelper
open System.IO

let rootDir = __SOURCE_DIRECTORY__
let outputDir = Path.Combine (rootDir, "output")

Target "Clean Output Dir" (fun _ ->
    CleanDir outputDir
)

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
                    "OutputPath", outputDir
                ]
        }

    build setParams "Lette.VacuumSim3000.sln"
        |> DoNothing
)

Target "IL Merge" (fun _ ->
    let setParams (defaults : ILMergeParams) =
        let toolPath = Path.Combine (rootDir, "packages\\ilmerge\\tools\\ILMerge.exe")
        let libraryPaths =
            [
                "Lette.VacuumSim3000.Library.dll"
                "System.ValueTuple.dll"
                "FSharp.Core.dll"
            ]
            |> List.map (fun p -> Path.Combine (outputDir, p))

        {
            defaults with
                ToolPath = toolPath
                TargetKind = TargetKind.Exe
                Libraries = libraryPaths
        }

    let outputFile = Path.Combine (outputDir, "vacsim3k.exe")
    let primaryAssembly = Path.Combine (outputDir, "Lette.VacuumSim3000.Console.exe")
            
    ILMerge setParams outputFile primaryAssembly
)

"Clean Output Dir"
    ==> "Paket Restore"
    ==> "Build"
    ==> "IL Merge"

Run "IL Merge"
