local string loc_sceneName = "mainscene"
local string loc_packageName = "prefabs.ab"
local string loc_assetName = "UI_Notice.prefab"

launchABFW={

}
local this = launchABFW

local yie_return = require("CouLaunchABFW").yield_return

local co = coroutine.create(function ()
        yie_return(loc_sceneName,loc_packageName,loc_assetName)
        end
)

function LaunchABFW:startABFW()
    asset(coroutine.resume(co))
end
