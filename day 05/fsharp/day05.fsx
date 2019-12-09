open System.IO
open System.Collections.Generic

type Memory = int[]

type Opcode =
  | Noop = 0
  | Add = 1
  | Multiply = 2
  | Input = 3
  | Output = 4
  | Halt = 99

type Mode =
  | Position = 0
  | Immediate = 1

type Parameter = IDictionary<int,Mode>

type ParsedInstruction = { Opcode: Opcode; Parameter: Parameter }

let parseInstruction (instruction: string) =
  let i =
    instruction
    |> fun x -> x.PadLeft(5, '0')
    |> Seq.rev
    |> Seq.map ((fun x -> x.ToString()) >> int)
    |> Seq.toArray
  
  let pMode x = enum<Mode> x
  {
    Opcode = enum<Opcode> i.[0];
    Parameter = dict [ 1, pMode i.[2]; 2, pMode i.[3]; 3, pMode i.[4] ] 
  }

let replace (memory: Memory) pointer value =
  let update = fun i existing -> if i = pointer then value else existing
  memory |> Array.mapi update

let valueByMode (mode: Mode) (memory: Memory) pointer =
  match mode with
  | Mode.Position -> memory.[memory.[pointer]]
  | Mode.Immediate -> memory.[pointer]
  | _ -> invalidArg "mode" (sprintf "Unrecognised mode %A" mode)

let display pointer value =
  printfn "[%d]=%d" pointer value

let diagnose input (memory: Memory) pointer =
  let i = parseInstruction (string <| memory.[pointer])
  let from index (param:Parameter) = valueByMode param.[index] memory (pointer + index)
  let at offset = memory.[pointer + offset] 
  let perform op = op (from 1 i.Parameter) (from 2 i.Parameter)
  let update op = (replace memory memory.[pointer + 3] (perform op))

  match (i.Opcode) with
  | Opcode.Add -> (update (+), pointer + 4)
  | Opcode.Multiply -> (update (*), pointer + 4)
  | Opcode.Input -> ((replace memory memory.[pointer + 1] input), pointer + 2)
  | Opcode.Output -> display pointer (from 1 i.Parameter); (memory, pointer + 2)
  | Opcode.Halt -> (memory, pointer)
  | _ -> invalidArg "i.Opcode" (sprintf "Unrecognised opcode %A" i.Opcode)

let rec runDiagnostic input (memory: Memory) pointer =
    let reprogrammed, next = diagnose input memory pointer

    if reprogrammed.[next] = 99 then true
    else runDiagnostic input reprogrammed next

let memory =
  File.ReadAllText (Path.Combine(__SOURCE_DIRECTORY__, "input.txt"))
  |> fun x -> x.Split(',')
  |> Array.map int

// runDiagnostic 1 memory 0
