namespace DMIslandClient.Entity

open DMIslandClient.Utils
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common.SpriteGroup

module EntityAnimation =
    type IAnimatablePos =
        abstract GetTarget: unit -> Pos
        abstract SetPosition: x: Pos -> unit
        abstract GetPosition: unit -> Pos
        abstract Update: float32 -> unit

    type SmoothAnimatablePos(speed, initial: Pos) =
        let mutable current: Pos = initial
        let mutable target: Pos = initial
        
        interface IAnimatablePos with
            member this.GetPosition() = current
            member this.SetPosition(x) = target <- x
            member this.Update(dt) =
                let delta = dt * speed
                current <- current * (1f - delta) + target * delta
            member this.GetTarget() = target

    type FunctionAnimatablePos(f: float32 -> Pos -> Pos -> Pos, speed: float32, initial: Pos) =
        let mutable initial: Pos = initial
        let mutable target: Pos = initial
        let mutable timeLeft: float32 = 0f
        
        interface IAnimatablePos with
            member this.GetPosition() =
                let clamped = MathUtils.clamp timeLeft 0f 1f
                f clamped initial target
                
            member this.SetPosition(x) =
                let t : IAnimatablePos = this
                initial <- t.GetPosition()
                timeLeft <- 1f
                target <- x
            
            member this.Update(dt) =
                if (timeLeft <= 0f) then
                    timeLeft <- 0f
                    initial <- target
                else timeLeft <- timeLeft - speed * dt

            member this.GetTarget() = target

    let linear c target initial =
        MathUtils.lerp c initial target
    
    type LinearAnimatablePos(speed: float32, initial: Pos) =
        inherit FunctionAnimatablePos(linear, speed, initial)


    let easeOut c target initial =
        MathUtils.lerp (c * c) initial target
    
    type EaseOutAnimatablePos(speed: float32, initial: Pos) =
        inherit FunctionAnimatablePos(easeOut, speed, initial)

    
    let easeBounceOut height c target initial =
        MathUtils.lerp (c * c) initial target + Pos(0f, height * c * (1f - c))
    
    type EaseOutAndBounceAnimatablePos(height: float32, speed: float32, initial: Pos) =
        inherit FunctionAnimatablePos(easeBounceOut height, speed, initial)

    
    type SpriteFlipper(sprite: Sprite) =
        let mutable looksRight = true
        let mutable current = Pos(0, 0)
        
        member x.SetTarget(pos: Pos) =
            let delta = current.X - pos.X
            let looksRightNow = delta < 0f
            if abs delta > 0.01f && (looksRight <> looksRightNow) then
                looksRight <- looksRightNow
                sprite.Width <- -sprite.Width
            current <- pos
        
        