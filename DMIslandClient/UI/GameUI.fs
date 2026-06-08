namespace DMIslandClient.UI

open DMIslandClient.UI.Components
open DMIslandClient.UI.Text
open LadaEngine.Engine.Base
open LadaEngine.Engine.Common
open LadaEngine.Engine.Global

type GameUI() =
    let uiCamera = Camera()
    let healthBar = HealthBar()
    let mutable frame = 0
    
    member x.SetHealth(health) =
        healthBar.UpdateHealth(health)
    
    member x.Update(dt: float32) =
        ()
    
    member x.Render() =
        frame <- frame + 1
        healthBar.Render(uiCamera)
    
    member x.Load() =
        ()
    
    member x.Resize(window: Window) =
        let top = float32 window.ClientSize.Y / float32 window.ClientSize.X
        let left = -1f
        let right = 1f
        let bottom = - float32 window.ClientSize.Y / float32 window.ClientSize.X
        let ratio = top / right
        let scaling = top
        healthBar.SetScale(0.1f * scaling)
        healthBar.SetPosition(Pos(left + 0.075f * scaling, top - ratio * 0.075f))