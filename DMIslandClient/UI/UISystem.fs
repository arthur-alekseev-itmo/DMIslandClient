namespace DMIslandClient.UI

open DMIslandClient.UI.Text
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common
open LadaEngine.Engine.Global

type UISystem () =
    let uiCamera = Camera()
    let textTest = Text("0", Pos(0f, 0f))
    let mutable frame = 0
    
    member x.Update() =
        textTest.SetText(frame.ToString())
        textTest.Update()
    
    member x.Render() =
        frame <- frame + 1
        textTest.Render(uiCamera)
    
    member x.Load() =
        ()
    
    member x.Resize(window: Window) =
        let top = float32 window.ClientSize.Y / float32 window.ClientSize.X
        let left = -1f
        let right = 1f
        let bottom = - float32 window.ClientSize.Y / float32 window.ClientSize.X
        let ratio = top / right
        let scaling = top
        textTest.SetScale(0.1f * scaling)
        textTest.SetPosition(Pos(left + 0.05f * scaling, top - ratio * 0.1f))