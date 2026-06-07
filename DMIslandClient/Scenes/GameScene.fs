namespace DMIslandClient.Scenes

open DMIslandClient
open DMIslandClient.Connection
open DMIslandClient.Effect
open DMIslandClient.Entity
open DMIslandClient.UI
open DMIslandClient.World
open LadaEngine.Engine.Common
open LadaEngine.Engine.Global
open LadaEngine.Engine.Scene


type GameScene(connection: GameConnection, window: Window) =
    let mutable currentRoom: Room option = Some (Room(20, 20, RoomType.MonadicBeach))
    let entities = EntityGroup()
    let effects = EffectGroup()
    let camera = ElasticCamera(Camera())
    let ui = UISystem()
    let sync = SynchroQueue()

    let controller = PlayerController(connection)
    let dispatcher = EventDispatcher(entities, effects)
            
    let trySnapToPlayer () =
        match entities.GetPlayer() with
        | Some player -> camera.SetPosition(player.Position.GetValue())
        | None -> ()

    interface IScene with
        member this.FixedUpdate() = ()
        member this.GetName() = "Gaming"

        member this.Load() =
            controller.SubscribeToUpdate(fun event -> sync.AddEvent(fun () -> dispatcher.ProcessUpdate(event)))
            controller.SendInitial()
            camera.GetCamera().Zoom <- 6f
            ui.Load()
                
        member this.Render() =
            currentRoom |> Option.iter _.Render(camera.GetCamera())
            entities.Render(camera.GetCamera())
            effects.Render(camera.GetCamera())
            ui.Render()
            
        member this.Resize() =
            ui.Resize(window)
            
        member this.Update(dt: float32) =         
            controller.Update()
            trySnapToPlayer()
            sync.ExecuteAll()
            entities.Update(dt)
            effects.Update(dt)
            camera.Update(dt)
            ui.Update()