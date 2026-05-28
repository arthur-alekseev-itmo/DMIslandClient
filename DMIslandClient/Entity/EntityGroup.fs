namespace DMIslandClient.Entity

open System
open System.Collections.Generic
open DMIslandClient.Entity.EntityAnimation
open DMIslandClient.Resources
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common
open LadaEngine.Engine.Common.SpriteGroup
open LadaEngine.Engine.Renderables.GroupRendering

type EntityType =
    | EtLambda
    | EtModusPonens
    | EtWall

type EntityGroup() =
    let textures = [| Resources.STEVE; Resources.LAMBDA; Resources.MODUS_PONENS; Resources.DIRT |]
    let atlas = TextureAtlas(textures)
    let spriteGroup = SpriteGroup(atlas)
    let entities = Dictionary<Guid, Entity>()
    let mutable player : Entity option = None
    
    let createLambda pos =
        let sprite = Sprite(pos, atlas, Resources.LAMBDA)
        let entity = Entity(sprite, EaseOutAnimatablePos(4f, pos))
        spriteGroup.AddSprite(sprite)
        entity.SetFlip(false)
        entity
    
    let createMp pos =
        let sprite = Sprite(pos, atlas, Resources.MODUS_PONENS)
        let entity = Entity(sprite, SmoothAnimatablePos(4f, pos))
        spriteGroup.AddSprite(sprite)
        entity.SetFlip(false)
        entity

    let createWall pos =
        let sprite = Sprite(pos, atlas, Resources.DIRT)
        spriteGroup.AddSprite(sprite)
        Entity(sprite, SmoothAnimatablePos(1f, pos))

    let createPlayer pos =
        let sprite = Sprite(pos, atlas, Resources.STEVE)
        spriteGroup.AddSprite(sprite)
        Entity(sprite, EaseOutAndBounceAnimatablePos(0.5f, 4f, pos))
    
    let createNewEntity id t pos=
        match t with
        | EtLambda -> entities.Add(id, createLambda pos)
        | EtModusPonens -> entities.Add(id, createMp pos)
        | EtWall -> entities.Add(id, createWall pos)
    
    member x.CreateOrUpdate(id: Guid, t: EntityType, pos: Pos)=
        match entities.TryGetValue(id) with
        | true, v -> v.SetTarget(pos)
        | false,_ -> createNewEntity id t pos

    member x.CreateOrUpdatePlayer(id: Guid, pos: Pos) =
        match player with
        | None ->
            let playerEntity = createPlayer pos
            entities.Add(id, playerEntity)
            player <- Some playerEntity
        | Some x -> x.Position.SetPosition(pos)
    
    member x.Render(camera: Camera) =
        spriteGroup.Render(camera)
        
    member x.Update(dt) =
        Seq.iter (fun (e: Entity) -> e.Update(dt)) entities.Values
        spriteGroup.Update()
        
    member x.GetPlayer() : Entity option = player