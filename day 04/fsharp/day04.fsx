let withinRange min max input =
    input >= min && input <= max

let numberToSequence x = 
    (string x).ToCharArray() 
    |> Seq.map (string >> int)

let hasAdjacents input =
    let rec hasAdjacent (latter: seq<int>) former =
        if(Seq.isEmpty latter) then false
        else former = (Seq.head latter) || hasAdjacent (Seq.skip 1 latter) (Seq.head latter)

    let intSeq = numberToSequence input
    hasAdjacent (Seq.skip 1 intSeq) (Seq.head intSeq)

let evenNumberedAdjacents input =
    let checkValid count =
        count = 1
        || (count % 2) = 0

    let rec adjacentCountOkay count head (tail: seq<int>) =
        if(Seq.isEmpty tail) then checkValid count
        elif head = (Seq.head tail) then adjacentCountOkay (count + 1) (Seq.head tail) (Seq.skip 1 tail) 
        else (checkValid count) && (adjacentCountOkay 1 (Seq.head tail) (Seq.skip 1 tail))

    let intSeq = numberToSequence input

    adjacentCountOkay 1 (Seq.head intSeq) (Seq.skip 1 intSeq)

let isIncreasing input =
    let rec exceeds (latter: seq<int>) former =
        if(Seq.isEmpty latter) then true
        else former <= (Seq.head latter) && exceeds (Seq.skip 1 latter) (Seq.head latter)

    let intSeq = numberToSequence input
    exceeds (Seq.skip 1 intSeq) (Seq.head intSeq)

let passwords = seq { 200000 .. 800000 }

passwords
|> Seq.filter (withinRange 240920 789857)
|> Seq.filter hasAdjacents
|> Seq.filter evenNumberedAdjacents
|> Seq.filter isIncreasing
|> Seq.length


