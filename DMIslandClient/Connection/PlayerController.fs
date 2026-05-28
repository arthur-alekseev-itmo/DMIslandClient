namespace DMIslandClient.Connection

open DMIslandClient.Connection.Dto
open DMIslandClient.Connection
open LadaEngine.Engine.Global
open OpenTK.Windowing.GraphicsLibraryFramework

type PlayerController(connection: GameConnection) =
    let subscriptions = ResizeArray()
    
    let getMoveDirection () =
        if Controls.ButtonPressedOnce(Keys.D) then Some "right"
        else if Controls.ButtonPressedOnce(Keys.A) then Some "left"
        else if Controls.ButtonPressedOnce(Keys.W) then Some "up"
        else if Controls.ButtonPressedOnce(Keys.S) then Some "down"
        else None
        
    let notifyAll result =
        Seq.iter (fun x -> x result) subscriptions
        
    member _.Update() =
        match getMoveDirection () with
        | Some dir -> connection.MoveCallback(dir, notifyAll)
        | None -> ()

    member _.SubscribeToUpdate(callback: GameStateResponse -> unit) =
        subscriptions.Add(callback)

    member _.SendInitial() = connection.SkibCallback(notifyAll)