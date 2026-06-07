namespace DMIslandClient.UI

open DMIslandClient.UI.Components
open DMIslandClient.UI.Text
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common
open LadaEngine.Engine.Global

type UISystem () =
    let uiCamera = Camera()
    let fpsCounter = FpsCounter(Pos(0f, 0f))
    let mutable frame = 0
    
    member x.Update(dt: float32) =
        fpsCounter.Update(dt)
    
    member x.Render() =
        frame <- frame + 1
        fpsCounter.Render(uiCamera)
    
    member x.Load() =
        ()
    
    member x.Resize(window: Window) =
        let top = float32 window.ClientSize.Y / float32 window.ClientSize.X
        let left = -1f
        let right = 1f
        let bottom = - float32 window.ClientSize.Y / float32 window.ClientSize.X
        let ratio = top / right
        let scaling = top
        fpsCounter.Text().SetScale(0.1f * scaling)
        fpsCounter.Text().SetPosition(Pos(left + 0.05f * scaling, top - ratio * 0.1f))