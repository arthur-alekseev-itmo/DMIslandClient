namespace DMIslandClient

open System
open DMIslandClient.Connection
open DMIslandClient.Connection.Dto
open DMIslandClient.Effect
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
    let mutable currentRoom: Room option = None
    let entities = EntityGroup()
    let effects = EffectGroup()
    let camera = ElasticCamera(Camera())
    let ui = UISystem()
    let sync = SynchroQueue()

    let controller = PlayerController(GameConnection("http://localhost:5229"))
    let dispatcher = EventDispatcher(entities, effects)


    let load () =
        controller.SubscribeToUpdate(fun event -> sync.AddEvent(fun () -> dispatcher.ProcessUpdate(event)))
        controller.SendInitial()
        camera.GetCamera().Zoom <- 6f
        ui.Load()

    let render () =
        currentRoom |> Option.iter _.Render(camera.GetCamera())
        entities.Render(camera.GetCamera())
        effects.Render(camera.GetCamera())
        ui.Render()

    let fixedUpdate (_: float) =
        ()
        
    let trySnapToPlayer () =
        match entities.GetPlayer() with
        | Some player -> camera.SetPosition(player.Position.GetValue())
        | None -> ()
        
    let update (dt: float) =
        controller.Update()
        trySnapToPlayer()
        sync.ExecuteAll()
        entities.Update(float32 dt)
        effects.Update(float32 dt)
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
                 