namespace DMIslandClient.Utils

open LadaEngine.Engine.Base

module MathUtils =
    let lerp (c: float32) (p: Pos) (t: Pos) =
        t * c + p * (1f - c)
    
    let clamp (i: float32) (lower: float32) (upper: float32) =
        max lower (min upper i)