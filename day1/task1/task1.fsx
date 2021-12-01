open System.IO
open System

let lines = File.ReadAllLines("input.txt")

let list =
    lines |> Array.map (fun x -> Int32.Parse(x))

for i in 0..(list.Length - 1) do
    printfn "%d" list[i]

let inc =
    Array.map2 (fun x y -> x < y) list[0..list.Length - 2] list[1..]

for i in 0..(inc.Length - 1) do
    printfn "%d %b" list[i+1] inc[i]

let result =
    inc
    |> Array.filter (fun x -> x)
    |> Array.length

printfn "%d" result