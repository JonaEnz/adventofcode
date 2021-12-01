open System.IO
open System

let lines = File.ReadAllLines("input.txt")

let list =
    lines |> Array.map (fun x -> Int32.Parse(x))

let slide =
    Array.map (fun i -> list[i-2] + list[i-1] + list[i]) [|2..list.Length - 1|]

let inc =
    Array.map2 (fun x y -> x < y) slide[0..slide.Length - 2] slide[1..]

let result =
    inc
    |> Array.filter (fun x -> x)
    |> Array.length

printfn "%d" result