#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Testing

let buildDir = "./build/"
let testDir = "./build/test/"
let packagingDir = "./nuget/"

let authors = ["Giacomo Stelluti Scala"]
let projectDescription = "Functional programming and other utilities for C#"
let projectSummary = "CSharp Extra Library"
let buildVersion = "0.0.0.0"

Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir]
)

Target "Default" (fun _ ->
    trace "CSharpx pre-release"
)

Target "BuildLib" (fun _ ->
    !! "src/CSharpx/CSharpx.csproj"
        |> MSBuildRelease buildDir "Build"
        |> Log "LibBuild-Output: "
)

(*
Target "BuildTest" (fun _ ->
    !! "tests/CSharpx.Tests/CSharpx.Tests.csproj"
        |> MSBuildDebug testDir "Build"
        |> Log "TestBuild-Output: "
)

Target "Test" (fun _ ->
    //trace "Running Tests..."
    !! (testDir @@ "\CSharpx.Tests.dll")
      |> xUnit2 (fun p -> {p with HtmlOutputPath = Some(testDir @@ "xunit.html")})
)
*)

// Dependencies
"Clean"
    ==> "BuildLib"
    //==> "BuildTest"
    //==> "Test"
    ==> "Default"

RunTargetOrDefault "Default"
