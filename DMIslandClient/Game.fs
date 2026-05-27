namespace DMIslandClient

open System
open DMIslandClient.Entity
open DMIslandClient.UI
open DMIslandClient.UI.Text
open DMIslandClient.World
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common
open LadaEngine.Engine.Global
open OpenTK.Windowing.Common

type Game () =
    let window = Window.Create(800, 600, "DM Island")
    let world = World()
    let entities = EntityGroup()
    let camera = ElasticCamera(Camera())
    let ui = UISystem()
    
    let load () =
        camera.GetCamera().Zoom <- 5f
        ui.Load()
    
    let render () =
        world.Render(camera.GetCamera())
        entities.Render(camera.GetCamera())
        ui.Render()
    
    let fixedUpdate (_: float) =
        ()
        
    let update (dt: float) =
        let playerPos = entities.GetPlayer().Position.GetPosition()
        camera.SetPosition(playerPos)
        
        entities.Update(float32 dt)
        camera.Update(float32 dt)
        ui.Update()

    let resize () =
        ui.Resize(window)
        
    let () =
        window.add_Load(load: Action)
        window.add_Render(render)
        window.add_FixedUpdate(fixedUpdate)
        window.add_Update(update)
        window.add_Resize(resize)
    
    member x.Run() = window.Run()
                 