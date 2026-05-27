namespace DMIslandClient.World

open System
open DMIslandClient.Resources
open DmIslandClient.Utils
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common.SpriteGroup
open LadaEngine.Engine.Renderables.GroupRendering

type World () =
    let textures = [| Resources.SAND; Resources.SANDSTONE |]
    let atlas = TextureAtlas(textures)
    let group = SpriteGroup(atlas)

    let addTile (pos: Pos) =
        let texture = GameRandom.choice(textures)
        let sprite = Sprite(pos, atlas, texture)
        sprite.Width <- 1f
        sprite.Height <- 1f
        group.AddSprite(sprite)
    
    let () =
        let positions = Seq.init 100 (fun x -> Seq.init 100 (fun y -> Pos(x, y))) |> Seq.concat
        Seq.iter addTile positions
        group.Update()
        
    member this.Render(camera) =
        group.Render(camera)

