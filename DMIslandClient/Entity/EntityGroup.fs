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
    
    member x.AddEntity(e: Entity) = entities.Add(e)
    
    member x.UpdateEntities() = failwith "TODO"
    
    member x.Render(camera: Camera) =
        spriteGroup.Render(camera)
        
    member x.Update(dt) =
        Seq.iter (fun (e: Entity) -> e.Update(dt)) entities
        spriteGroup.Update()
        
    member x.GetPlayer() : Entity option =
        None