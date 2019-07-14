module Extensions

open System.Collections.Generic
open FSharpPlus
open NUnit.Framework
open Validations

[<Test>]
let ``Dict.union gives same dictionary when joined with an empty dictionary (identity)`` () =
  let m1 = dict [1, "2"; 2,"4"; 4,"8"]
  let m2 = dict []
  let r1 = m1 |> Dict.union m2 |> Seq.toList

  areEqual (m1 |> Seq.toList) r1

[<Test>]
let ``Dict.union returns same results independent of the order (commutative)`` () = 
  let m1 = dict [1, "1"; 2,"2"; 3,"3"]
  let m2 = dict [3, "3"; 4,"4"; 5,"6"]

  let r1 = m1 |> Dict.union m2 |> Seq.toList |> List.sortBy (fun x -> x.Key)
  let r2 = m2 |> Dict.union m1 |> Seq.toList |> List.sortBy (fun x -> x.Key)

  areEqual r1 r2

[<Test>]
let ``Dict.union returns same results independent of the grouping (associative)`` () = 
  let m1 = dict [1, "1"; 2,"2"; 3,"3"]
  let m2 = dict [3, "3"; 4,"4"; 5,"6"]
  let m3 = dict [6, "6"; 6,"6"; 8,"8"]

  let r1 = Dict.union (Dict.union m1 m2) m3 |> Seq.toList  
  let r2 = Dict.union m1 (Dict.union m2 m3) |> Seq.toList  

  areEqual r1 r2
  
[<Test>]
let ``Dict.union provides same end result as Dict.unionWith picking the first source value for dupes`` () =
  let m1 = dict [1, "2"; 2,"4"; 4,"8"]
  let m2 = dict [1, "4"; 2,"8"; 4,"16"]

  let r1 = m1 |> Dict.union m2 |> Seq.toList
  let r2 = m1 |> Dict.unionWith konst m2 |> Seq.toList

  areEqual r1 r2
  
[<Test>]
let ``Map.union gives same map when joined with an empty map (identity)`` () =
  let m1 = [1, "2"; 2,"4"; 4,"8"] |> Map.ofList
  let r1 = Map.union m1 Map.empty |> Map.toList

  areEqual [1, "2"; 2,"4"; 4,"8"] r1

[<Test>]
let ``Map.union returns same results independent of the order (associative)`` () = 
  let m1 = [1, "1"; 2,"2"; 3,"3"] |> Map.ofList
  let m2 = [3, "3"; 4,"4"; 5,"6"] |> Map.ofList

  let r1 = m1 |> Map.union m2
  let r2 = m2 |> Map.union m1

  areEqual r1 r2

[<Test>]
let ``Map.union provides same end result as Map.unionWith picking the first source value for dupes`` () =
  let m1 = [1, "2"; 2,"4"; 4,"8"] |> Map.ofList
  let m2 = [1, "4"; 2,"8"; 4,"16"] |> Map.ofList

  let r1 = m1 |> Map.union m2
  let r2 = m1 |> Map.unionWith konst m2
  
  areEqual r1 r2 
  
[<Test>]
let ``Map.intersect returns any dictionary when intersected with the empty map (identity)`` () = 
  let m1 = [1, "2"; 2,"4"; 4,"8"] |> Map.ofList 
  let m2 = Map.empty

  let r1 = m1 |> Map.intersect m2

  areEqual Map.empty r1

[<Test>]
let ``Map.intersect returns same results independent of the order (commutative)`` () = 
  let m1 = [1, "1"; 2,"2"; 3,"3"] |> Map.ofList
  let m2 = [3, "3"; 4,"4"; 5,"5"] |> Map.ofList

  let r1 = m1 |> Map.intersect m2
  let r2 = m2 |> Map.intersect m1

  areEqual r1 r2

[<Test>]
let ``Map.intersect returns same results independent of grouping (associative)`` () = 
  let m1 = [1, "1"; 2,"2"; 3,"3"] |> Map.ofList
  let m2 = [3, "3"; 4,"4"; 5,"5"] |> Map.ofList
  let m3 = [5, "5"; 6,"6"; 7,"7"] |> Map.ofList

  let r1 = Map.intersect (Map.intersect m1 m2) m3
  let r2 =  Map.intersect m1 (Map.intersect m2 m3)

  areEqual r1 r2
  
[<Test>]
let ``Map.intersect provides same end result as Map.intersectWith picking the first source value for dupes`` () =
  let m1 = [1, "2"; 2,"4"; 4,"8"] |> Map.ofList
  let m2 = [1, "4"; 2,"8"; 4,"16"] |> Map.ofList

  let r1 = m1 |> Map.intersect m2
  let r2 = m1 |> Map.intersectWith konst m2

  areEqual r1 r2

[<Test>]
let ``Dict.intersect returns any dictionary when intersected with the empty dictionary (identity)`` () = 
  let m1 = dict [1, "2"; 2,"4"; 4,"8"]
  let m2 = Dictionary<int,string>() :> IDictionary<int,string>

  let r1 = m1 |> Dict.intersect m2 |> Seq.toList

  areEqual [] r1

[<Test>]
let ``Dict.intersect returns same results independent of the order (commutative)`` () = 
  let m1 = dict [1, "1"; 2,"2"; 3,"3"]
  let m2 = dict [3, "3"; 4,"4"; 5,"5"]

  let r1 = m1 |> Dict.intersect m2 |> Seq.toList
  let r2 = m2 |> Dict.intersect m1 |> Seq.toList

  areEqual r1 r2

[<Test>]
let ``Dict.intersect returns same results independent of the grouping (associative)`` () = 
  let m1 = dict [1, "1"; 2,"2"; 3,"3"]
  let m2 = dict [3, "3"; 4,"4"; 5,"5"]
  let m3 = dict [5, "5"; 6,"6"; 7,"7"]

  let r1 = (Dict.intersect (Dict.intersect m1 m2) m3) |> Seq.toList
  let r2 = (Dict.intersect m1 (Dict.intersect m2 m3)) |> Seq.toList

  areEqual r1 r2

[<Test>]
let ``Dict.intersect provides same end result as Dict.intersectWith picking the first source value for dupes`` () =
  let m1 = dict [1, "2"; 2,"4"; 4,"8"]
  let m2 = dict [1, "4"; 2,"8"; 4,"16"]

  let r1 = m1 |> Dict.intersect m2 |> Seq.toList
  let r2 = m1 |> Dict.intersectWith konst m2 |> Seq.toList

  areEqual r1 r2


[<Test>]
let ``IReadOnlyDictionary.intersect returns any dictionary when intersected with the empty dictionary (identity)`` () = 
  let m1 = Dictionary(dict [1, "2"; 2,"4"; 4,"8"]) :> IReadOnlyDictionary<int, string>
  let m2 = Dictionary<int,string>() :>  IReadOnlyDictionary<int, string>

  let r1 = m1 |> IReadOnlyDictionary.intersect m2 |> Seq.toList

  areEqual [] r1

[<Test>]
let ``IReadOnlyDictionary.intersect returns same results independent of the order (commutative)`` () = 
  let m1 = Dictionary(dict [1, "1"; 2,"2"; 3,"3"]) :> IReadOnlyDictionary<int, string>
  let m2 = Dictionary(dict [3, "3"; 4,"4"; 5,"5"]) :> IReadOnlyDictionary<int, string>

  let r1 = m1 |> IReadOnlyDictionary.intersect m2 |> Seq.toList
  let r2 = m2 |> IReadOnlyDictionary.intersect m1 |> Seq.toList

  areEqual r1 r2


[<Test>]
let ``IReadOnlyDictionary.intersect returns same results independent of the grouping (distributive)`` () = 
  let m1 = Dictionary(dict [1, "1"; 2,"2"; 3,"3"]) :> IReadOnlyDictionary<int, string>
  let m2 = Dictionary(dict [3, "3"; 4,"4"; 5,"5"]) :> IReadOnlyDictionary<int, string>
  let m3 = Dictionary(dict [5, "5"; 6,"6"; 7,"7"]) :> IReadOnlyDictionary<int, string>

  let r1 = (IReadOnlyDictionary.intersect (IReadOnlyDictionary.intersect m1 m2) m3) |> Seq.toList
  let r2 = (IReadOnlyDictionary.intersect m1 (IReadOnlyDictionary.intersect m2 m3)) |> Seq.toList

  areEqual r1 r2  

[<Test>]
let ``IReadOnlyDictionary.union gives same dictionary when joined with an empty ditionary (identity)`` () =
  let m1 = Dictionary(dict [1, "2"; 2,"4"; 4,"8"]) :> IReadOnlyDictionary<int,string>
  let m2 = Dictionary(dict []) :> IReadOnlyDictionary<int,string>
  let r1 = m1 |> IReadOnlyDictionary.union m2 |> Seq.toList

  areEqual (m1 |> Seq.toList) r1

[<Test>]
let ``IReadOnlyDictionary.union returns same results independent of the order (commutative)`` () = 
  let m1 = Dictionary(dict [1, "1"; 2,"2"; 3,"3"]) :> IReadOnlyDictionary<int, string>
  let m2 = Dictionary(dict [3, "3"; 4,"4"; 5,"6"]) :> IReadOnlyDictionary<int, string>

  let r1 = m1 |> IReadOnlyDictionary.union m2 |> Seq.toList |> List.sortBy (fun x -> x.Key)
  let r2 = m2 |> IReadOnlyDictionary.union m1 |> Seq.toList |> List.sortBy (fun x -> x.Key)

  areEqual r1 r2

[<Test>]
let ``IReadOnlyDictionary.union returns same results independent of the groupping (associative)`` () = 
  let m1 = Dictionary(dict [1, "1"; 2,"2"; 3,"3"]) :> IReadOnlyDictionary<int, string>
  let m2 =  Dictionary(dict [3, "3"; 4,"4"; 5,"6"]) :> IReadOnlyDictionary<int, string>
  let m3 =  Dictionary(dict [6, "6"; 6,"6"; 8,"8"]) :> IReadOnlyDictionary<int, string>

  let r1 = IReadOnlyDictionary.union (IReadOnlyDictionary.union m1 m2) m3 |> Seq.toList  
  let r2 = IReadOnlyDictionary.union m1 (IReadOnlyDictionary.union m2 m3) |> Seq.toList  

  areEqual r1 r2
  
[<Test>]
let ``IReadOnlyDictionary.intersect provides same end result as IReadOnlyDictionary.intersectWith picking the first source value for dupes`` () =
  let m1 = Dictionary(dict [1, "2"; 2,"4"; 4,"8"]) :> IReadOnlyDictionary<int, string>
  let m2 = Dictionary(dict [1, "4"; 2,"8"; 4,"16"]) :> IReadOnlyDictionary<int, string>

  let r1 = m1 |> IReadOnlyDictionary.intersect m2 |> Seq.toList
  let r2 = m1 |> IReadOnlyDictionary.intersectWith konst m2 |> Seq.toList

  areEqual r1 r2

[<Test>]
let ``IReadOnlyDictionary.union provides same end result as Dict.unionWith picking the first source value for dupes`` () =
  let m1 = Dictionary(dict [1, "2"; 2,"4"; 4,"8"]) :> IReadOnlyDictionary<int, string>
  let m2 = Dictionary(dict [1, "4"; 2,"8"; 4,"16"]) :> IReadOnlyDictionary<int, string>

  let r1 = m1 |> IReadOnlyDictionary.union m2 |> Seq.toList
  let r2 = m1 |> IReadOnlyDictionary.unionWith konst m2 |> Seq.toList

  areEqual r1 r2