namespace DMIslandClient.Entity

open DMIslandClient.Entity.EntityAnimation
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common
open LadaEngine.Engine.Common.SpriteGroup

type Entity (sprite: Sprite, position: IAnimatablePos) =
    let spriteFlipper = SpriteFlipper(sprite)
    let mutable flipping = false
    
    member x.Sprite = sprite
    member x.Position = position
    
    member public x.Update(dt) =
        x.Position.Update(dt)
        x.Sprite.Position <- x.Position.GetPosition()

    member public x.SetTarget(t: Pos) =
        x.Position.SetPosition(t)
        if flipping then spriteFlipper.SetTarget(t)
    
    member public x.SetFlip(b) = flipping <- b
