namespace DMIslandClient.Connection

open System
open System.Collections.Generic
open System.Text.Json.Serialization

module Dto =
    [<CLIMutable>]
    type PlayerActionRequest = {
        [<JsonPropertyName("action")>]
        Action: string
        
        [<JsonPropertyName("direction")>]
        Direction: string option
    }

    [<CLIMutable>]
    type PositionDto = {
        [<JsonPropertyName("x")>]
        X: int
        
        [<JsonPropertyName("y")>]
        Y: int
    }

    [<CLIMutable>]
    type PlayerViewDto = {
        [<JsonPropertyName("id")>]
        Id: Guid
        
        [<JsonPropertyName("name")>]
        Name: string
        
        [<JsonPropertyName("hp")>]
        Hp: int
        
        [<JsonPropertyName("maxHp")>]
        MaxHp: int
        
        [<JsonPropertyName("position")>]
        Position: PositionDto
    }

    [<CLIMutable>]
    type ObjectViewDto = {
        [<JsonPropertyName("id")>]
        Id: Guid
        
        [<JsonPropertyName("type")>]
        Type: string
        
        [<JsonPropertyName("name")>]
        Name: string
        
        [<JsonPropertyName("hp")>]
        Hp: int
        
        [<JsonPropertyName("maxHp")>]
        MaxHp: int
        
        [<JsonPropertyName("position")>]
        Position: PositionDto
        
        [<JsonPropertyName("relativePosition")>]
        RelativePosition: PositionDto
    }

    [<CLIMutable>]
    type GameStateResponse = {
        [<JsonPropertyName("turn")>]
        Turn: int
        
        [<JsonPropertyName("player")>]
        Player: PlayerViewDto
        
        [<JsonPropertyName("objects")>]
        Objects: List<ObjectViewDto>
        
        [<JsonPropertyName("viewWidth")>]
        ViewWidth: int
        
        [<JsonPropertyName("viewHeight")>]
        ViewHeight: int
    }