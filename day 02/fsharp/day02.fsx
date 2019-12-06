type Memory = int list

let jump = 4

let haltCode = 99

let opCodes =
  [
    1, (+); 
    2, (*);
  ]
  |> Map.ofList

let address (memory: Memory) pointer offset =
  memory.[pointer + offset]

let valueAt (memory: Memory) pointer offset = 
  memory.[address memory pointer offset]

let replace value pointer (elements: Memory) =
  let update = fun i existing -> if i = pointer then value else existing
  elements |> List.mapi update

let rec reProgram (memory: Memory) pointer =
  let getUpdated (memory: Memory) pointer =
    let op = opCodes.[memory.[pointer]]
    let parameter = valueAt memory pointer
    let value = op (parameter +1) (parameter +2)
    let at = address memory pointer +3
    let updated = replace value at memory
    reProgram updated (pointer + jump)

  if memory.[pointer] = haltCode then memory
  else getUpdated memory pointer

let fix (memory: Memory) =
  reProgram memory 0

let assertThat condition =
  if condition then "ok"
  else failwith "nope"

assertThat ([2;0;0;0;99] = fix [1;0;0;0;99])
assertThat ([3500;9;10;70;2;3;11;0;99;30;40;50] = fix [1;9;10;3;2;3;11;0;99;30;40;50])
assertThat ([2;3;0;6;99] = fix [2;3;0;3;99])
assertThat ([2;4;4;5;99;9801] = fix [2;4;4;5;99;0])
assertThat ([30;1;1;4;2;5;6;0;99] = fix [1;1;1;4;99;5;6;0;99])

let input = [1;12;2;3;1;1;2;3;1;3;4;3;1;5;0;3;2;10;1;19;2;19;6;23;2;13;23;27;1;9;27;31;2;31;9;35;1;6;35;39;2;10;39;43;1;5;43;47;1;5;47;51;2;51;6;55;2;10;55;59;1;59;9;63;2;13;63;67;1;10;67;71;1;71;5;75;1;75;6;79;1;10;79;83;1;5;83;87;1;5;87;91;2;91;6;95;2;6;95;99;2;10;99;103;1;103;5;107;1;2;107;111;1;6;111;0;99;2;14;0;0]

let partOne input = (fix input).[0]

partOne input // <- part one's answer is here

let partTwo input seeking =
  seq {
    for noun = 0 to 99 do
      for verb = 0 to 99 do
        let replacedNoun = replace noun +1 input
        let replacedBoth = replace verb +2 replacedNoun
        let result = fix replacedBoth
        yield noun, verb, result.[0] = seeking  
  }

let producesExpectedOutput (_, _, found) = found

partTwo input 19690720  // <- part two's answer is here
|> Seq.filter producesExpectedOutput
|> Seq.map (fun (noun, verb, _) -> noun * 100 + verb)
