namespace DMIslandClient.Entity

open DMIslandClient.Entity.EntityAnimation
open DMIslandClient.Resources
open DmIslandClient.Utils
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common
open LadaEngine.Engine.Common.SpriteGroup
open LadaEngine.Engine.Global
open LadaEngine.Engine.Renderables.GroupRendering
open OpenTK.Windowing.GraphicsLibraryFramework

type EntityGroup() =
    let textures = [| Resources.STEVE; Resources.LAMBDA; Resources.MODUS_PONENS |]
    let atlas = TextureAtlas(textures)
    let spriteGroup = SpriteGroup(atlas)
    let entities = ResizeArray<Entity>()

    let createMp() =
        let pos = Pos(GameRandom.random.Next(1, 100), GameRandom.random.Next(1, 100))
        let sprite = Sprite(pos, atlas, Resources.MODUS_PONENS)
        sprite.Width <- 1f
        sprite.Height <- 0.3f
        let mp = Entity(sprite, LinearAnimatablePos(3f, pos))
        spriteGroup.AddSprite(sprite)
        entities.Add(mp)

    let createLambda() =
        let pos = Pos(GameRandom.random.Next(1, 100), GameRandom.random.Next(1, 100))
        let sprite = Sprite(pos, atlas, Resources.LAMBDA)
        sprite.Width <- 0.6f
        sprite.Height <- 0.8f
        let lam = Entity(sprite, EaseOutAnimatablePos(3f, pos))
        spriteGroup.AddSprite(sprite)
        entities.Add(lam)
        
    let () =
        let pos = Pos(1f, 1f)
        let sprite = Sprite(pos, atlas, Resources.STEVE)
        sprite.Width <- 1f
        sprite.Height <- 1f
        let steve = Entity(sprite, EaseOutAndBounceAnimatablePos(1f, 5f, pos))
        spriteGroup.AddSprite(sprite)
        entities.Add(steve)
        steve.SetFlip(true)
        for i = 0 to 100 do createMp()
        for i = 0 to 100 do createLambda()
        
    let moveEnemies() =
        let moveEnemy (e: Entity) =
            let rDir = [|Pos(1f, 0f); Pos(-1f, 0f); Pos(0f, 1f); Pos(0f, -1f)|]
            e.SetTarget(e.Position.GetTarget() + GameRandom.choice rDir)
        Seq.skip 1 entities |> Seq.iter moveEnemy
            
        
    let controlPlayer() =
        if Controls.ButtonPressedOnce(Keys.D) then
            entities[0].SetTarget(entities[0].Position.GetTarget() + Pos(1f, 0f))
            moveEnemies()
        if Controls.ButtonPressedOnce(Keys.A) then
            entities[0].SetTarget(entities[0].Position.GetTarget() + Pos(-1f, 0f))
            moveEnemies()
        if Controls.ButtonPressedOnce(Keys.W) then
            entities[0].SetTarget(entities[0].Position.GetTarget() + Pos(0f, 1f))
            moveEnemies()
        if Controls.ButtonPressedOnce(Keys.S) then
            entities[0].SetTarget(entities[0].Position.GetTarget() + Pos(0f, -1f))
            moveEnemies()
    
    member x.AddEntity(e: Entity) = entities.Add(e)
    
    member x.Render(camera: Camera) =
        spriteGroup.Render(camera)
        
    member x.Update(dt) =
        // TODO: Debug code
        controlPlayer()
        Seq.iter (fun (e: Entity) -> e.Update(dt)) entities
        spriteGroup.Update()
        
    member x.GetPlayer() =
        entities[0]