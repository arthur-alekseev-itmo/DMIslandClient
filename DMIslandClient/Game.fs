namespace DMIslandClient

open System
open DMIslandClient.Connection
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
    let camera = ElasticCamera(Camera())
    let ui = UISystem()
    let sync = SynchroQueue()

    let controller = PlayerController(GameConnection("http://localhost:5229"))

    let updateRoom (data: Dto.GameStateResponse) =
        let room = Room(data.ViewWidth, data.ViewWidth, MonadicBeach)
        let entityTypeOfString = function
            | "wall" -> EtWall
            | "lambda" -> EtLambda
            | "modus_ponens" -> EtModusPonens
            | m -> failwith $"Unknown mob type: {m}"
        for entity in data.Objects do
            let typ = entityTypeOfString entity.Type
            let pos = Pos(entity.Position.X, entity.Position.Y)
            entities.CreateOrUpdate(entity.Id, typ, pos)
        let pos = Pos(data.Player.Position.X, data.Player.Position.Y)
        entities.CreateOrUpdatePlayer(data.Player.Id, pos)
        currentRoom <- Some room

    let load () =
        controller.SubscribeToUpdate(printfn "%A")
        controller.SubscribeToUpdate(fun event -> sync.AddEvent(fun () -> updateRoom event))
        controller.SendInitial()
        camera.GetCamera().Zoom <- 5f
        ui.Load()

    let render () =
        currentRoom |> Option.iter _.Render(camera.GetCamera())
        entities.Render(camera.GetCamera())
        ui.Render()

    let fixedUpdate (_: float) =
        ()
        
    let trySnapToPlayer () =
        match entities.GetPlayer() with
        | Some player -> camera.SetPosition(player.Position.GetPosition())
        | None -> ()
        
    let update (dt: float) =
        controller.Update()
        trySnapToPlayer()
        sync.ExecuteAll()
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
                 