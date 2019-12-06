type Program = int list

let jump = 4

let haltCode = 99

let ops =
  [
    1, (+); 
    2, (*);
  ]
  |> Map.ofList

let position (program: Program) index offset =
  program.[index + offset]

let valueAt (program: Program) index offset = 
  program.[position program index offset]

let replace value index (elements: Program) =
  let update = fun i existing -> if i = index then value else existing
  elements |> List.mapi update

let rec reProgram (program: Program) index =
  let getUpdated (program: Program) index =
    let op = ops.[program.[index]]
    let get = valueAt program index
    let value = op (get +1) (get +2)
    let at = position program index +3
    let updated = replace value at program
    reProgram updated (index + jump)

  if program.[index] = haltCode then program
  else getUpdated program index

let fix (program: Program) =
  reProgram program 0

let assertThat condition =
  if condition then "ok"
  else failwith "nope"

assertThat ([2;0;0;0;99] = fix [1;0;0;0;99])
assertThat ([3500;9;10;70;2;3;11;0;99;30;40;50] = fix [1;9;10;3;2;3;11;0;99;30;40;50])
assertThat ([2;3;0;6;99] = fix [2;3;0;3;99])
assertThat ([2;4;4;5;99;9801] = fix [2;4;4;5;99;0])
assertThat ([30;1;1;4;2;5;6;0;99] = fix [1;1;1;4;99;5;6;0;99])

let input = [1;12;2;3;1;1;2;3;1;3;4;3;1;5;0;3;2;10;1;19;2;19;6;23;2;13;23;27;1;9;27;31;2;31;9;35;1;6;35;39;2;10;39;43;1;5;43;47;1;5;47;51;2;51;6;55;2;10;55;59;1;59;9;63;2;13;63;67;1;10;67;71;1;71;5;75;1;75;6;79;1;10;79;83;1;5;83;87;1;5;87;91;2;91;6;95;2;6;95;99;2;10;99;103;1;103;5;107;1;2;107;111;1;6;111;0;99;2;14;0;0]

let partOne = (fix input).[0]

